#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PostEnot.Toolkits.EventManagement
{
    public interface ICallbacksMap : IReadOnlyCollection<KeyValuePair<Type, ICallbacksCollection>>
    {
        /// <summary>
        /// Индексатор для доступа к коллекции обратных вызовов для события указанного типа.
        /// </summary>
        /// <param name="eventType">Тип события.</param>
        /// <returns>Коллекция обратных вызовов события указанного типа.</returns>
        public ICallbacksCollection this[Type eventType] { get; }

        public ICallbacksCollection Get(Type eventType);
        public ICallbacksCollection Get<TEvent>();
        public bool TryGetValue(Type eventType, [NotNullWhen(true)] out ICallbacksCollection? callbacks);
        public bool TryGetValue<TEvent>([NotNullWhen(true)] out ICallbacksCollection? callbacks);
        public bool Contains(Type eventType);
        public bool Contains<TEvent>();
        public bool Contains(ICallbacksCollection callbacks);
    }
}
