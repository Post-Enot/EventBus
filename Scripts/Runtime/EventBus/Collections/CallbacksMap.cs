#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PostEnot.Toolkits.EventManagement
{
    internal sealed class CallbacksMap : ICallbacksMap
    {
        public CallbacksMap(Dictionary<Type, CallbacksCollection> callbacksByType) => _callbacksByType = callbacksByType;

        public ICallbacksCollection this[Type eventType] => _callbacksByType[eventType];

        public int Count => _callbacksByType.Count;
        public IEnumerable<Type> EventTypes => _callbacksByType.Keys;
        public IEnumerable<ICallbacksCollection> CallbacksCollections => _callbacksByType.Values;

        private readonly Dictionary<Type, CallbacksCollection> _callbacksByType;

        public ICallbacksCollection Get(Type eventType) => _callbacksByType[eventType];

        public ICallbacksCollection Get<TEvent>()
        {
            Type eventType = typeof(TEvent);
            return _callbacksByType[eventType];
        }

        public bool TryGetValue(Type eventType, [NotNullWhen(true)] out ICallbacksCollection? callbacks)
        {
            if (_callbacksByType.TryGetValue(eventType, out CallbacksCollection callbacksBase))
            {
                callbacks = callbacksBase;
                return true;
            }
            callbacks = null;
            return false;
        }

        public bool TryGetValue<TEvent>([NotNullWhen(true)] out ICallbacksCollection? callbacks)
        {
            Type eventType = typeof(TEvent);
            if (_callbacksByType.TryGetValue(eventType, out CallbacksCollection callbacksBase))
            {
                callbacks = callbacksBase;
                return true;
            }
            callbacks = null;
            return false;
        }

        public bool Contains(Type eventType) => _callbacksByType.ContainsKey(eventType);

        public bool Contains<TEvent>()
        {
            Type eventType = typeof(TEvent);
            return _callbacksByType.ContainsKey(eventType);
        }

        public bool Contains(ICallbacksCollection callbacks)
        {
            if (callbacks == null)
            {
                throw new ArgumentNullException(nameof(callbacks));
            }
            return callbacks is CallbacksCollection callbacksOther && _callbacksByType.ContainsValue(callbacksOther);
        }

        public IEnumerator<KeyValuePair<Type, ICallbacksCollection>> GetEnumerator()
        {
            foreach (KeyValuePair<Type, CallbacksCollection> pair in _callbacksByType)
            {
                yield return new KeyValuePair<Type, ICallbacksCollection>(pair.Key, pair.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
