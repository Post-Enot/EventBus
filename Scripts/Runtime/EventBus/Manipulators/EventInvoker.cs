#nullable enable

namespace PostEnot.Toolkits.EventManagement
{
    internal sealed class EventInvoker : IEventInvoker
    {
        public EventInvoker(IEventBus eventBus) => _eventBus = eventBus;

        private readonly IEventBus _eventBus;

        public void Invoke<TEvent>() where TEvent : new() => _eventBus.Invoke<TEvent>();

        public void Invoke<TEvent>(TEvent context) => _eventBus.Invoke(context);
    }
}
