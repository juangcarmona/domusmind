namespace DomusMind.Application.Abstractions.Notifications;

public interface IListChangePublisher
{
    void Publish(Guid listId);
}
