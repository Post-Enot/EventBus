#nullable enable

using System;
using System.Collections.Generic;

namespace PostEnot.Toolkits.EventManagement
{
    internal sealed class EventReceiver : IEventReceiver
    {
        public EventReceiver(EventBus eventBus) => _eventBus = eventBus;

        public bool IsEnabled { get; private set; }
        public bool IsDisabled => !IsEnabled;

        private readonly List<TypeCallbackPair> _callbacks = new();
        private readonly EventBus _eventBus;

        public IEventReceiver Enable()
        {
            if (IsDisabled)
            {
                IsEnabled = true;
                foreach (TypeCallbackPair pair in _callbacks)
                {
                    _eventBus.AddCallback(pair);
                }
            }
            return this;
        }

        public IEventReceiver Disable()
        {
            if (IsEnabled)
            {
                IsEnabled = false;
                foreach (TypeCallbackPair pair in _callbacks)
                {
                    _ = _eventBus.RemoveCallback(pair);
                }
            }
            return this;
        }

        public IEventReceiver SetEnabled(bool isEnabled) => isEnabled ? Enable() : Disable();

        public IEventReceiver ToggleEnabled() => IsEnabled ? Disable() : Enable();

        public IEventReceiver Register<TEvent>(EventCallback<TEvent> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            Type eventType = typeof(TEvent);
            TypeCallbackPair pair = new(eventType, callback);
            _callbacks.Add(pair);
            if (IsEnabled)
            {
                _eventBus.AddCallback(pair);
            }
            return this;
        }

        public IEventReceiver Register<TEvent>(EventCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            Type eventType = typeof(TEvent);
            TypeCallbackPair pair = new(eventType, callback);
            _callbacks.Add(pair);
            if (IsEnabled)
            {
                _eventBus.AddCallback(pair);
            }
            return this;
        }

        public IEventReceiver Unregister<TEvent>(EventCallback<TEvent> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            Type eventType = typeof(TEvent);
            TypeCallbackPair pair = new(eventType, callback);
            bool isRemove = _callbacks.Remove(pair);
            if (isRemove && IsEnabled)
            {
                _ = _eventBus.RemoveCallback(pair);
            }
            return this;
        }

        public IEventReceiver Unregister<TEvent>(EventCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            Type eventType = typeof(TEvent);
            TypeCallbackPair pair = new(eventType, callback);
            bool isRemove = _callbacks.Remove(pair);
            if (isRemove && IsEnabled)
            {
                _ = _eventBus.RemoveCallback(pair);
            }
            return this;
        }

        public IEventReceiver UnregisterAll()
        {
            foreach (TypeCallbackPair pair in _callbacks)
            {
                _ = _eventBus.RemoveCallback(pair);
            }
            _callbacks.Clear();
            return this;
        }
    }
}
