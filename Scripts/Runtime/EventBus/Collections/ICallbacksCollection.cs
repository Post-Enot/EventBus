#nullable enable

using System;

namespace PostEnot.Toolkits.EventManagement
{
    public interface ICallbacksCollection
    {
        public int Count { get; }
        public Type EventType { get; }
        public bool IsInvoking { get; }
        public bool IsNotInvoking { get; }
        public uint InvokeDepth { get; }
        public ILogger? Logger { get; }

        public bool IsRegistered(Delegate callback);
        public bool IsRegistered(EventCallback callback);
        public int IndexOf(Delegate callback);
        public int IndexOf(EventCallback callback);
    }
}
