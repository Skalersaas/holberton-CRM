namespace Utilities.Events
{
    public interface IEventHandler<in IEvent> where IEvent : IDomainEvent
    {
        Task HandleAsync(IEvent domainEvent);
    }
}
