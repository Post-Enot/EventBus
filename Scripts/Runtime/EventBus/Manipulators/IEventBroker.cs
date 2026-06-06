#nullable enable

namespace PostEnot.Toolkits.EventManagement
{
    /// <summary>
    /// Объединяет возможности получателя событий (<see cref="IEventReceiver"/>) и инициатора событий (<see cref="IEventInvoker"/>),
    /// позволяя одновременно подписываться на события и вызывать их.
    /// </summary>
    public interface IEventBroker : IEventReceiver, IEventInvoker { }
}
