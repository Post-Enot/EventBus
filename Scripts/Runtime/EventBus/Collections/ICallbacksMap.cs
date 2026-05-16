#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PostEnot.Toolkits.EventManagement
{
    /// <summary>
    /// Коллекция, предоставляющая безопасный доступ к информации о подписках и списках обратных вызовов только для чтения.
    /// </summary>
    public interface ICallbacksMap : IReadOnlyCollection<KeyValuePair<Type, ICallbacksCollection>>
    {
        /// <summary>
        /// Индексатор для доступа к коллекции обратных вызовов для события типа <paramref name="eventType"/>.
        /// </summary>
        /// <param name="eventType">Тип события.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventType"/> равен <see langword="null"/>.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Событие типа <paramref name="eventType"/> не имеет активных подписок.
        /// </exception>
        /// <returns>Коллекция обратных вызовов события типа <paramref name="eventType"/>.</returns>
        public ICallbacksCollection this[Type eventType] { get; }

        /// <summary>
        /// Количество коллекций обратных вызовов для различных событий.
        /// </summary>
        public new int Count { get; }
        public IEnumerable<Type> EventTypes { get; }
        public IEnumerable<ICallbacksCollection> CallbacksCollections { get; }

        /// <summary>
        /// Метод доступа к коллекции обратных вызовов для события типа <paramref name="eventType"/>.
        /// </summary>
        /// <param name="eventType">Тип события.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventType"/> равен <see langword="null"/>.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Событие типа <paramref name="eventType"/> не имеет активных подписок.
        /// </exception>
        /// <returns>Коллекция обратных вызовов события типа <paramref name="eventType"/>.</returns>
        public ICallbacksCollection Get(Type eventType);

        /// <summary>
        /// Метод доступа к коллекции обратных вызовов для события типа <typeparamref name="TEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        /// <exception cref="KeyNotFoundException">
        /// Событие типа <typeparamref name="TEvent"/> не имеет активных подписок.
        /// </exception>
        /// <returns>Коллекция обратных вызовов события типа <typeparamref name="TEvent"/>.</returns>
        public ICallbacksCollection Get<TEvent>();

        /// <summary>
        /// Пытается получить коллекцию обратных вызовов для события типа <paramref name="eventType"/>.
        /// </summary>
        /// <param name="eventType">Тип события.</param>
        /// <param name="callbacks">
        /// После возврата содержит коллекцию обратных вызовов для события типа <paramref name="eventType"/>,
        /// если она найдена; иначе <see langword="null"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventType"/> равен <see langword="null"/>.
        /// </exception>
        /// <returns>
        /// <see langword="true"/>, если коллекция обратных вызовов для события типа <paramref name="eventType"/> найдена;
        /// иначе <see langword="false"/>.
        /// </returns>
        public bool TryGetValue(Type eventType, [NotNullWhen(true)] out ICallbacksCollection? callbacks);

        /// <summary>
        /// Пытается получить коллекцию обратных вызовов для события типа <typeparamref name="TEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        /// <param name="callbacks">
        /// После возврата содержит коллекцию обратных вызовов для события типа <typeparamref name="TEvent"/>,
        /// если она найдена; иначе <see langword="null"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/>, если коллекция обратных вызовов для события типа <typeparamref name="TEvent"/> найдена;
        /// иначе <see langword="false"/>.
        /// </returns>
        public bool TryGetValue<TEvent>([NotNullWhen(true)] out ICallbacksCollection? callbacks);

        /// <summary>
        /// Проверяет, содержит ли отображение коллекцию обратных вызовов для события типа <paramref name="eventType"/>.
        /// </summary>
        /// <param name="eventType">Тип события.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventType"/> равен <see langword="null"/>.
        /// </exception>
        /// <returns>
        /// <see langword="true"/>, если отображение содержит коллекцию обратных вызовов для типа <paramref name="eventType"/>;
        /// иначе <see langword="false"/>.
        /// </returns>
        public bool Contains(Type eventType);

        /// <summary>
        /// Проверяет, содержит ли отображение коллекцию обратных вызовов для события типа <typeparamref name="TEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        /// <returns>
        /// <see langword="true"/>, если отображение содержит коллекцию обратных вызовов для типа <typeparamref name="TEvent"/>;
        /// иначе <see langword="false"/>.
        /// </returns>
        public bool Contains<TEvent>();

        /// <summary>
        /// Проверяет, содержится ли указанная коллекция обратных вызовов в данном отображении (обычно сравнение по ссылке).
        /// </summary>
        /// <param name="callbacks">Коллекция обратных вызовов.</param>
        /// <returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="callbacks"/> равен <see langword="null"/>.
        /// </exception>
        /// <see langword="true"/>, если отображение содержит коллекцию обратных вызовов <paramref name="callbacks"/>;
        /// иначе <see langword="false"/>.
        /// </returns>
        public bool Contains(ICallbacksCollection callbacks);
    }
}
