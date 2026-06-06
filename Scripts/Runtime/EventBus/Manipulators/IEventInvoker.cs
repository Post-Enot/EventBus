#nullable enable

namespace PostEnot.Toolkits.EventManagement
{
    /// <summary>
    /// Представляет инициатора событий, способного вызывать события с контекстом или без.
    /// </summary>
    public interface IEventInvoker
    {
        /// <summary>
        /// Вызывает событие типа <typeparamref name="TEvent"/>, неявно создавая контекст по умолчанию с помощью <see langword="new"/>().
        /// </summary>
        /// <typeparam name="TEvent">Тип события. Должен иметь открытый конструктор без параметров.</typeparam>
        public void Invoke<TEvent>() where TEvent : new();

        /// <summary>
        /// Вызывает событие типа <typeparamref name="TEvent"/> с указанным контекстом.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        /// <param name="context">Контекст, передаваемый подписанным обратным вызовам.</param>
        public void Invoke<TEvent>(TEvent context);
    }
}