#nullable enable

namespace PostEnot.Toolkits.EventManagement
{
    /// <summary>
    /// Представляет получателя событий.
    /// Обеспечивает удобное управление подписками: регистрацию, отмену регистрации, временное отключение всех подписок
    /// и полную очистку. Это полезно для группировки связанных подписок в одном месте,
    /// позволяя включать/отключать их одной командой (например, при активации/деактивации игрового объекта)
    /// и гарантированно отписываться от всего разом, избегая утечек.
    /// </summary>
    public interface IEventReceiver
    {
        /// <summary>
        /// Указывает, включён ли получатель событий.
        /// </summary>
        public bool IsEnabled { get; }
        /// <summary>
        /// Указывает, отключён ли получатель событий.
        /// </summary>
        public bool IsDisabled { get; }

        /// <summary>
        /// Включает получателя, активируя все ранее зарегистрированные подписки.
        /// </summary>
        /// <returns>Текущий экземпляр <see cref="IEventReceiver"/> для цепочки вызовов.</returns>
        public IEventReceiver Enable();

        /// <summary>
        /// Отключает получателя, временно деактивируя все активные подписки.
        /// </summary>
        /// <returns>Текущий экземпляр <see cref="IEventReceiver"/> для цепочки вызовов.</returns>
        public IEventReceiver Disable();

        /// <summary>
        /// Устанавливает состояние получателя в соответствии с параметром <paramref name="isEnabled"/>.
        /// </summary>
        /// <param name="isEnabled">Если <see langword="true"/>, получатель будет включён; иначе отключён.</param>
        /// <returns>Текущий экземпляр <see cref="IEventReceiver"/> для цепочки вызовов.</returns>
        public IEventReceiver SetEnabled(bool isEnabled);

        /// <summary>
        /// Переключает состояние получателя: включает, если был отключён, и отключает, если был включён.
        /// </summary>
        /// <returns>Текущий экземпляр <see cref="IEventReceiver"/> для цепочки вызовов.</returns>
        public IEventReceiver ToggleEnabled();

        /// <summary>
        /// Регистрирует обратный вызов, принимающий контекст, для события типа <typeparamref name="TEvent"/>.
        /// Если получатель включён, подписка вступает в силу немедленно.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        /// <param name="callback">Обратный вызов.</param>
        /// <returns>Текущий экземпляр <see cref="IEventReceiver"/> для цепочки вызовов.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="callback"/> равен <see langword="null"/>.
        /// </exception>
        public IEventReceiver Register<TEvent>(EventCallback<TEvent> callback);

        /// <summary>
        /// Регистрирует обратный вызов без параметров для события типа <typeparamref name="TEvent"/>.
        /// Если получатель включён, подписка вступает в силу немедленно.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        /// <param name="callback">Обратный вызов.</param>
        /// <returns>Текущий экземпляр <see cref="IEventReceiver"/> для цепочки вызовов.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="callback"/> равен <see langword="null"/>.
        /// </exception>
        public IEventReceiver Register<TEvent>(EventCallback callback);

        /// <summary>
        /// Отменяет регистрацию обратного вызова, принимающего контекст, для события типа <typeparamref name="TEvent"/>.
        /// Если получатель включён, обратный вызов немедленно удаляется из шины.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        /// <param name="callback">Ранее зарегистрированный обратный вызов.</param>
        /// <returns>Текущий экземпляр <see cref="IEventReceiver"/> для цепочки вызовов.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="callback"/> равен <see langword="null"/>.
        /// </exception>
        public IEventReceiver Unregister<TEvent>(EventCallback<TEvent> callback);

        /// <summary>
        /// Отменяет регистрацию обратного вызова без параметров для события типа <typeparamref name="TEvent"/>.
        /// Если получатель включён, обратный вызов немедленно удаляется из шины.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        /// <param name="callback">Ранее зарегистрированный обратный вызов.</param>
        /// <returns>Текущий экземпляр <see cref="IEventReceiver"/> для цепочки вызовов.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="callback"/> равен <see langword="null"/>.
        /// </exception>
        public IEventReceiver Unregister<TEvent>(EventCallback callback);

        /// <summary>
        /// Отменяет регистрацию всех обратных вызовов, связанных с этим получателем.
        /// Если получатель включён, все подписки немедленно удаляются из шины.
        /// </summary>
        /// <returns>Текущий экземпляр <see cref="IEventReceiver"/> для цепочки вызовов.</returns>
        public IEventReceiver UnregisterAll();
    }
}
