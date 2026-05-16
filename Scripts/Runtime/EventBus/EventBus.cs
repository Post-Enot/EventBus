#nullable enable

using System;
using System.Collections.Generic;

namespace PostEnot.Toolkits.EventManagement
{
    public sealed class EventBus : IEventBus
    {
        public EventBus(ILogger? logger = null) => Logger = logger;

        public ICallbacksMap CallbacksMap => _publicCallbacksMap ??= new CallbacksMap(_callbacksMap);
        public bool IsInvoking => InvokeDepth != 0;
        public bool IsNotInvoking => InvokeDepth == 0;
        public uint InvokeDepth { get; private set; }
        public ILogger? Logger { get; private set; }

        private readonly Dictionary<Type, CallbacksCollection> _callbacksMap = new();

        private ICallbacksMap? _publicCallbacksMap;
        private IEventInvoker? _invoker;

        public void SetLogger(ILogger? logger)
        {
            Logger = logger;
            foreach (CallbacksCollection callbacks in _callbacksMap.Values)
            {
                callbacks.Logger = logger;
            }
        }

        public void UnsetLogger()
        {
            if (Logger == null)
            {
                return;
            }
            foreach (CallbacksCollection callbacks in _callbacksMap.Values)
            {
                callbacks.Logger = null;
            }
        }

        public IEventInvoker CreateInvoker()
        {
            _invoker ??= new EventInvoker(this);
            return _invoker;
        }

        public IEventReceiver CreateReceiver() => new EventReceiver(this);

        public IEventBroker CreateBroker() => new EventBroker(this);

        public void Invoke<TEvent>() where TEvent : new()
        {
            TEvent context = new();
            Invoke(context);
        }

        public void Invoke<TEvent>(TEvent context)
        {
            Type eventType = typeof(TEvent);
            if (_callbacksMap.TryGetValue(eventType, out CallbacksCollection callbacks))
            {
                InvokeDepth += 1;
                try
                {
                    callbacks.Invoke(context);
                }
                finally
                {
                    InvokeDepth -= 1;
                }
                callbacks.TryPerformNullClearing();
                RemoveCallbacksCollectionIfEmpty(eventType, callbacks);
            }
        }

        public void Register<TEvent>(EventCallback<TEvent> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            Type eventType = typeof(TEvent);
            AddCallback(eventType, callback);
        }

        public void Register<TEvent>(EventCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            Type eventType = typeof(TEvent);
            AddCallback(eventType, callback);
        }

        public bool Unregister<TEvent>(EventCallback<TEvent> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            Type eventType = typeof(TEvent);
            return RemoveCallback(eventType, callback);
        }

        public bool Unregister<TEvent>(EventCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            Type eventType = typeof(TEvent);
            return RemoveCallback(eventType, callback);
        }

        public void UnregisterAllFrom<TEvent>()
        {
            Type eventType = typeof(TEvent);
            if (_callbacksMap.TryGetValue(eventType, out CallbacksCollection callbacks))
            {
                callbacks.Clear();
                RemoveCallbacksCollectionIfEmpty(eventType, callbacks);
            }
        }

        public void UnregisterAll()
        {
            Type[] eventTypes = new Type[_callbacksMap.Count];
            _callbacksMap.Keys.CopyTo(eventTypes, 0);
            foreach (Type eventType in eventTypes)
            {
                CallbacksCollection callbacks = _callbacksMap[eventType];
                callbacks.Clear();
                RemoveCallbacksCollectionIfEmpty(eventType, callbacks);
            }
        }

        public bool IsRegistered<TEvent>(EventCallback callback)
        {
            Type eventType = typeof(TEvent);
            return ContainsCallback(eventType, callback);
        }

        public bool IsRegistered<TEvent>(EventCallback<TEvent> callback)
        {
            Type eventType = typeof(TEvent);
            return ContainsCallback(eventType, callback);
        }

        internal bool ContainsCallback(Type eventType, Delegate callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            if (_callbacksMap.TryGetValue(eventType, out CallbacksCollection callbacks))
            {
                return callbacks.Contains(callback);
            }
            return false;
        }

        internal bool RemoveCallback(TypeCallbackPair pair) => RemoveCallback(pair.Type, pair.Callback);

        internal bool RemoveCallback(Type eventType, Delegate callback)
        {
            if (_callbacksMap.TryGetValue(eventType, out CallbacksCollection callbacks))
            {
                bool result = callbacks.Remove(callback);
                RemoveCallbacksCollectionIfEmpty(eventType, callbacks);
                return result;
            }
            return false;
        }

        internal void AddCallback(TypeCallbackPair pair) => AddCallback(pair.Type, pair.Callback);

        internal void AddCallback(Type eventType, Delegate callback)
        {
            if (_callbacksMap.TryGetValue(eventType, out CallbacksCollection callbacks))
            {
                callbacks.Add(callback);
            }
            else
            {
                callbacks = new CallbacksCollection(eventType, Logger);
                callbacks.Add(callback);
                _callbacksMap.Add(eventType, callbacks);
            }
        }

        private void RemoveCallbacksCollectionIfEmpty(Type eventType, CallbacksCollection callbacks)
        {
            if (callbacks.IsEmpty && callbacks.IsNotInvoking)
            {
                _ = _callbacksMap.Remove(eventType);
            }
        }
    }
}
