namespace PostEnot.Toolkits.EventManagement
{
    public interface ITypedEvent
    {
        public void Register(IEventBus eventBus);

        public void Unregister(IEventBus eventBus);

        public bool IsRegistered(IEventBus eventBus);

        public void ClearCallbacks();
    }
}
