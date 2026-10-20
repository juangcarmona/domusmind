using DomusMind.Application.Abstractions.Notifications;
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

public sealed class ListChangeNotifier : IListChangeNotifier, IListChangePublisher
{
    private readonly object _gate = new();
    private readonly Dictionary<Guid, Dictionary<Guid, Channel<bool>>> _subscribers = [];

    public ListChangeSubscription Subscribe(Guid listId)
    {
        var subscriberId = Guid.NewGuid();
        var channel = Channel.CreateBounded<bool>(new BoundedChannelOptions(1)
        {
            FullMode = BoundedChannelFullMode.DropOldest,
            SingleReader = true,
            SingleWriter = false,
        });

        lock (_gate)
        {
            if (!_subscribers.TryGetValue(listId, out var subscribers))
            {
                subscribers = [];
                _subscribers.Add(listId, subscribers);
            }

            subscribers.Add(subscriberId, channel);
        }

        return new ListChangeSubscription(channel.Reader, () => Unsubscribe(listId, subscriberId, channel));
    }

    public void Publish(Guid listId)
    {
        Channel<bool>[] channels;
        lock (_gate)
        {
            if (!_subscribers.TryGetValue(listId, out var subscribers))
                return;

            channels = subscribers.Values.ToArray();
        }

        foreach (var channel in channels)
            channel.Writer.TryWrite(true);
    }

    private void Unsubscribe(Guid listId, Guid subscriberId, Channel<bool> channel)
    {
        channel.Writer.TryComplete();
        lock (_gate)
        {
            if (!_subscribers.TryGetValue(listId, out var subscribers))
                return;

            subscribers.Remove(subscriberId);
            if (subscribers.Count == 0)
                _subscribers.Remove(listId);
        }
    }
}
