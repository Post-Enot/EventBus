#nullable enable

namespace PostEnot.Toolkits.EventManagement
{
    public interface IEventInvoker
    {
        public void Invoke<TEvent>() where TEvent : new();

        public void Invoke<TEvent>(TEvent context);
    }
}
