using System;
using UnityEngine;

namespace PostEnot.Toolkits.EventManagement
{
    /// <summary>
    /// Предоставляет сериализуемый адаптер, позволяющий через инспектор выбрать любой тип события,
    /// помеченный атрибутом <see cref="TypedEventAttribute"/> и перенаправлять его вызов в
    /// <see cref="UnityEngine.Events.UnityEvent"/>.
    /// </summary>
    [Serializable]
    public sealed class TypedEvent : ITypedEvent
    {
        #region Inspector
        [SerializeReference] private ITypedEvent typedEvent;
        #endregion

        /// <summary>
        /// Возвращает <see langword="true"/>, если тип события был выбран в инспекторе и адаптер готов к использованию.
        /// </summary>
        public bool IsInitialized => typedEvent != null;

        public void Register(IEventBus eventBus) => typedEvent.Register(eventBus);

        public void Unregister(IEventBus eventBus) => typedEvent.Unregister(eventBus);

        public bool IsRegistered(IEventBus eventBus) => typedEvent.IsRegistered(eventBus);

        public void ClearCallbacks() => typedEvent.ClearCallbacks();
    }
}
