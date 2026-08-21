#nullable enable

namespace PostEnot.Toolkits.EventManagement
{
    /// <summary>
    /// Представляет типизированную шину событий, позволяющую подписываться на события, отписываться от них и вызывать их.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Коллекция пар ключ-значение, предоставляющая безопасный доступ для чтения данных о текущих регистрациях обратных вызовов.
        /// </summary>
        public ICallbacksMap CallbacksMap { get; }
        /// <summary>
        /// Происходит ли в данный момент вызов события.
        /// </summary>
        public bool IsInvoking { get; }
        /// <summary>
        /// Не происходит ли в данный момент вызов события.
        /// </summary>
        public bool IsNotInvoking { get; }
        /// <summary>
        /// Количество вложенных вызовов, она же глубина вызовов - счётчик увеличивается, когда в результате одного вызова события
        /// происходит вызов иных событий.
        /// </summary>
        public uint InvokeDepth { get; }
        /// <summary>
        /// Логгер, используемый для логирования исключений обратных вызовов; может иметь значение <see langword="null"/>.
        /// </summary>
        public ILogger? Logger { get; }

        /// <summary>
        /// Устанавливает логгер, используемый для логирования исключений обратных вызовов.
        /// </summary>
        /// <param name="logger">Устанавливаемый логгер. Может иметь значение <see langword="null"/>.</param>
        public void SetLogger(ILogger? logger);

        /// <summary>
        /// Сбрасывает логгер, используемый для логгирования исключений обратных вызовов.
        /// </summary>
        public void UnsetLogger();

        /// <summary>
        /// Создаёт инициатора событий (<see cref="IEventInvoker"/>), позволяющего вызывать события
        /// без возможности изменения подписок. Это полезно для передачи компонентам, которые должны
        /// только инициировать события, не имея доступа к управлению подписками.
        /// </summary>
        /// <returns>Экземпляр <see cref="IEventInvoker"/>, связанный с данной шиной.</returns>
        public IEventInvoker CreateInvoker();

        /// <summary>
        /// Создаёт получателя событий (<see cref="IEventReceiver"/>), предоставляющего удобное
        /// управление подписками: регистрацию, отмену регистрации, временное отключение и полную очистку.
        /// Полезно для группировки связанных подписок и безопасного управления их жизненным циклом.
        /// </summary>
        /// <returns>Новый экземпляр <see cref="IEventReceiver"/>, связанный с данной шиной.</returns>
        public IEventReceiver CreateReceiver();

        /// <summary>
        /// Создаёт получателя событий (<see cref="IEventReceiver"/>), предоставляющего удобное
        /// управление подписками: регистрацию, отмену регистрации, временное отключение и полную очистку.
        /// Полезно для группировки связанных подписок и безопасного управления их жизненным циклом.
        /// </summary>
        /// <param name="isEnabled">Будет ли получатель включён.</param>
        /// <returns>Новый экземпляр <see cref="IEventReceiver"/>, связанный с данной шиной.</returns>
        public IEventReceiver CreateReceiver(bool isEnabled);

        /// <summary>
        /// Создаёт брокера событий (<see cref="IEventBroker"/>), объединяющего возможности получения
        /// и вызова событий. Это позволяет одному компоненту и подписываться на события, и инициировать их,
        /// используя единый объект.
        /// </summary>
        /// <returns>Новый экземпляр <see cref="IEventBroker"/>, связанный с данной шиной.</returns>
        public IEventBroker CreateBroker();

        /// <summary>
        /// Создаёт брокера событий (<see cref="IEventBroker"/>), объединяющего возможности получения
        /// и вызова событий. Это позволяет одному компоненту и подписываться на события, и инициировать их,
        /// используя единый объект.
        /// </summary>
        /// <param name="isEnabled">Будет ли брокер включён.</param>
        /// <returns>Новый экземпляр <see cref="IEventBroker"/>, связанный с данной шиной.</returns>
        public IEventBroker CreateBroker(bool isEnabled);

        /// <summary>
        /// Регистрирует обратный вызов, принимающий контекст, для события типа <typeparamref name="TEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        /// <param name="callback">Обратный вызов.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="callback"/> равен <see langword="null"/>.
        /// </exception>
        public void Register<TEvent>(EventCallback<TEvent> callback);

        /// <summary>
        /// Регистрирует обратный вызов без параметров для события типа <typeparamref name="TEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        /// <param name="callback">Обратный вызов.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="callback"/> равен <see langword="null"/>.
        /// </exception>
        public void Register<TEvent>(EventCallback callback);
        
        /// <summary>
        /// Отменяет регистрацию обратного вызова принимающего контекст для события типа <typeparamref name="TEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        /// <param name="callback">Ранее зарегистрированный обратный вызов.</param>
        /// <returns>
        /// <see langword="true"/>, если обратный вызов был найден и удалён; иначе <see langword="false"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="callback"/> равен <see langword="null"/>.
        /// </exception>
        public bool Unregister<TEvent>(EventCallback<TEvent> callback);

        /// <summary>
        /// Отменяет регистрацию обратного вызова без параметров для события типа <typeparamref name="TEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        /// <param name="callback">Ранее зарегистрированный обратный вызов.</param>
        /// <returns>
        /// <see langword="true"/>, если обратный вызов был найден и удалён; иначе <see langword="false"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="callback"/> равен <see langword="null"/>.
        /// </exception>
        public bool Unregister<TEvent>(EventCallback callback);

        /// <summary>
        /// Отменяет регистрацию всех обратных вызовов для <typeparamref name="TEvent"/> события.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        public void UnregisterAllFrom<TEvent>();

        /// <summary>
        /// Отменяет регистрацию всех обратных вызовов для всех типов событий; равносилен полному сбросу состояния.
        /// </summary>
        public void UnregisterAll();

        /// <summary>
        /// Проверяет существование регистрации обратного вызова без параметров к событию типа <typeparamref name="TEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        /// <param name="callback">Проверяемый обратный вызов.</param>
        /// <returns>
        /// <see langword="true"/>, если обратный вызов зарегистрирован на событие <typeparamref name="TEvent"/>;
        /// иначе <see langword="false"/>.
        /// </returns>
        public bool IsRegistered<TEvent>(EventCallback callback);

        /// <summary>
        /// Проверяет существование регистрации обратного вызова принимающего контекст к событию типа <typeparamref name="TEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        /// <param name="callback">Проверяемый обратный вызов.</param>
        /// <returns>
        /// <see langword="true"/>, если обратный вызов <paramref name="callback"/> зарегистрирован на
        /// событие <typeparamref name="TEvent"/>; иначе <see langword="false"/>.
        /// </returns>
        public bool IsRegistered<TEvent>(EventCallback<TEvent> callback);

        /// <summary>
        /// Вызывает событие типа <typeparamref name="TEvent"/>, неявно создавая контекст по умолчанию
        /// с помощью <see langword="new"/>().
        /// </summary>
        /// <typeparam name="TEvent">
        /// Тип события. Должен иметь открытый конструктор без параметров.
        /// </typeparam>
        public void Invoke<TEvent>() where TEvent : new();

        /// <summary>
        /// Вызывает событие типа <typeparamref name="TEvent"/> с указанным контекстом.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        /// <param name="context">Контекст, передаваемый подписанным обратным вызовам.</param>
        public void Invoke<TEvent>(TEvent context);
    }
}
