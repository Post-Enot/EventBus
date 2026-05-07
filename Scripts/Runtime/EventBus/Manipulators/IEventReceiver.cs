#nullable enable

namespace PostEnot.Toolkits.EventManagement
{
    public interface IEventReceiver
    {
        public bool IsEnabled { get; }
        public bool IsDisabled { get; }

        public IEventReceiver Enable();

        public IEventReceiver Disable();

        public IEventReceiver SetEnabled(bool isEnabled);

        public IEventReceiver ToggleEnabled();

        public IEventReceiver Register<TEvent>(EventCallback<TEvent> callback);

        public IEventReceiver Register<TEvent>(EventCallback callback);

        public IEventReceiver Unregister<TEvent>(EventCallback<TEvent> callback);

        public IEventReceiver Unregister<TEvent>(EventCallback callback);

        public IEventReceiver UnregisterAll();
    }
}
