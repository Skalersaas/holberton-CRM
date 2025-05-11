namespace Utilities.Events
{
    public class EventDispatcher
    {
        private readonly Dictionary<Type, List<object>> _handlers = [];

        public void Register<TEvent>(IEventHandler<TEvent> handler) where TEvent : IDomainEvent
        {
            var type = typeof(TEvent);
            if (!_handlers.ContainsKey(type))
                _handlers[type] = [];

            _handlers[type].Add(handler);
        }

        public async Task DispatchAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
        {
            var type = typeof(TEvent);
            if (_handlers.TryGetValue(type, out var handlers))
            {
                foreach (var handler in handlers.Cast<IEventHandler<TEvent>>())
                {
                    await handler.HandleAsync(domainEvent);
                }
            }
        }
    }
}
