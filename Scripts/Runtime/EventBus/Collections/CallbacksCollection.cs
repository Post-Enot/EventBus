#nullable enable

using System;
using System.Collections.Generic;

namespace PostEnot.Toolkits.EventManagement
{
    internal sealed class CallbacksCollection : ICallbacksCollection
    {
        public CallbacksCollection(Type eventType, ILogger? logger)
        {
            EventType = eventType;
            Logger = logger;
        }

        public int Count => _callbacks.Count - RemovedElementsCount;
        public uint InvokeDepth { get; private set; }
        public bool IsInvoking => InvokeDepth != 0;
        public bool IsNotInvoking => InvokeDepth == 0;
        public Type EventType { get; }
        public ILogger? Logger { get; set; }

        internal int RemovedElementsCount { get; private set; }
        internal bool IsEmpty => Count == 0;

        private static readonly Predicate<Delegate?> _isNull = obj => obj == null;

        private readonly List<Delegate?> _callbacks = new();

        public bool IsRegistered(Delegate callback) => _callbacks.Contains(callback);

        public bool IsRegistered(EventCallback callback) => _callbacks.Contains(callback);

        public int IndexOf(Delegate callback)
        {
            int removedElementsCount = 0;
            for (int i = 0; i < _callbacks.Count; i += 1)
            {
                if (_callbacks[i] == null)
                {
                    removedElementsCount += 1;
                }
                else if (callback.Equals(_callbacks[i]))
                {
                    return i - removedElementsCount;
                }
            }
            return -1;
        }

        public int IndexOf(EventCallback callback)
        {
            int removedElementsCount = 0;
            for (int i = 0; i < _callbacks.Count; i += 1)
            {
                if (_callbacks[i] == null)
                {
                    removedElementsCount += 1;
                }
                else if (callback.Equals(_callbacks[i]))
                {
                    return i - removedElementsCount;
                }
            }
            return -1;
        }

        internal void Add(Delegate callback) => _callbacks.Add(callback);

        internal bool Remove(Delegate callback)
        {
            if (IsNotInvoking)
            {
                return _callbacks.Remove(callback);
            }
            int index = _callbacks.IndexOf(callback);
            if (index == -1)
            {
                return false;
            }
            _callbacks[index] = null;
            RemovedElementsCount += 1;
            return true;
        }

        internal bool Contains(Delegate callback) => _callbacks.Contains(callback);

        internal void TryPerformNullClearing()
        {
            if (IsInvoking)
            {
                return;
            }
            if (RemovedElementsCount == 0)
            {
                return;
            }
            _ = _callbacks.RemoveAll(_isNull);
            RemovedElementsCount = 0;
        }

        internal void Clear()
        {
            if (IsNotInvoking)
            {
                _callbacks.Clear();
                return;
            }
            for (int i = 0; i < _callbacks.Count; i += 1)
            {
                if (_callbacks[i] != null)
                {
                    _callbacks[i] = null;
                    RemovedElementsCount += 1;
                }
            }
        }

        internal void Invoke<TEvent>(TEvent context)
        {
            InvokeDepth += 1;
            int callbacksCount = _callbacks.Count;
            for (int i = 0; i < callbacksCount; i += 1)
            {
                Delegate? specificCallback = _callbacks[i];
                if (specificCallback == null)
                {
                    continue;
                }
                try
                {
                    if (specificCallback is EventCallback<TEvent> callbackWithContext)
                    {
                        callbackWithContext.Invoke(context);
                    }
                    else
                    {
                        EventCallback callback = (EventCallback)specificCallback;
                        callback.Invoke();
                    }
                }
                catch (Exception exception)
                {
                    Logger?.LogException(exception);
                }
            }
            InvokeDepth -= 1;
        }
    }
}
