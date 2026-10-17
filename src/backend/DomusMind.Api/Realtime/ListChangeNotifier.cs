using System.Collections.Concurrent;
using System.Threading.Channels;

namespace DomusMind.Api.Realtime;

public interface IListChangeNotifier
{
    ListChangeSubscription Subscribe(Guid listId);
    void Publish(Guid listId);
}

public sealed class ListChangeSubscription : IDisposable
{
    private readonly Action _unsubscribe;
    private int _disposed;

    internal ListChangeSubscription(ChannelReader<bool> changes, Action unsubscribe)
    {
        Changes = changes;
        _unsubscribe = unsubscribe;
    }

    public ChannelReader<bool> Changes { get; }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 0)
            _unsubscribe();
    }
}

public sealed class ListChangeNotifier : IListChangeNotifier
{
    private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, Channel<bool>>> _subscribers = new();

    public ListChangeSubscription Subscribe(Guid listId)
    {
        var subscriberId = Guid.NewGuid();
        var channel = Channel.CreateBounded<bool>(new BoundedChannelOptions(1)
        {
            FullMode = BoundedChannelFullMode.DropOldest,
            SingleReader = true,
            SingleWriter = false,
        });

        _subscribers.GetOrAdd(listId, _ => new()).TryAdd(subscriberId, channel);
        return new ListChangeSubscription(channel.Reader, () => Unsubscribe(listId, subscriberId, channel));
    }

    public void Publish(Guid listId)
    {
        if (!_subscribers.TryGetValue(listId, out var subscribers))
            return;

        foreach (var subscriber in subscribers.Values)
            subscriber.Writer.TryWrite(true);
    }

    private void Unsubscribe(Guid listId, Guid subscriberId, Channel<bool> channel)
    {
        channel.Writer.TryComplete();
        if (!_subscribers.TryGetValue(listId, out var subscribers))
            return;

        subscribers.TryRemove(subscriberId, out _);
        if (subscribers.IsEmpty)
            ((ICollection<KeyValuePair<Guid, ConcurrentDictionary<Guid, Channel<bool>>>>)_subscribers)
                .Remove(new KeyValuePair<Guid, ConcurrentDictionary<Guid, Channel<bool>>>(listId, subscribers));
    }
}
