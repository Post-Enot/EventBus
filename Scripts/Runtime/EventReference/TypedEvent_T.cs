using System;
using UnityEngine;
using UnityEngine.Events;

namespace PostEnot.Toolkits.EventManagement
{
    /// <summary>
    /// Предоставляет сериализуемый адаптер, перенаправлять его вызов события типа <typeparamref name="TEvent"/>
    /// из шины событий в <see cref="UnityEvent{TEvent}"/>. В отличие от <see cref="TypedEvent"/>, позволяет
    /// явно задать тип события через <typeparamref name="TEvent"/>.
    /// </summary>
    [Serializable]
    public sealed class TypedEvent<TEvent> : ITypedEvent
    {
        #region Inspector
        [SerializeField] private UnityEvent<TEvent> @event;
        #endregion

        public UnityEvent<TEvent> Event => @event;

        public void Register(IEventBus eventBus) => eventBus.Register<TEvent>(HandleEvent);

        public void Unregister(IEventBus eventBus) => eventBus.Unregister<TEvent>(HandleEvent);

        public bool IsRegistered(IEventBus eventBus) => eventBus.IsRegistered<TEvent>(HandleEvent);

        public void ClearCallbacks()
        {
            @event.RemoveAllListeners();
            int count = @event.GetPersistentEventCount();
            for (int i = 0; i < count; i += 1)
            {
                @event.SetPersistentListenerState(i, UnityEventCallState.Off);
            }
        }

        private void HandleEvent(TEvent context) => @event.Invoke(context);
    }
}
