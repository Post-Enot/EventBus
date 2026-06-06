using System;
using UnityEngine.InputSystem;

namespace PostEnot.Toolkits.EventManagement.Input
{
    /// <summary>
    /// Базовый класс для обёрток над <typeparamref name="TInputActions"/>, автоматизирующий
    /// подписку на действия ввода и их отписку при освобождении.
    /// </summary>
    /// <typeparam name="TInputActions">
    /// Тип, содержащий определения карт и действий ввода (обычно сгенерированный из .inputactions).
    /// Должен реализовывать <see cref="IInputActionCollection2"/> и <see cref="IDisposable"/>.
    /// </typeparam>
    public abstract class InputActionsWrapperBase<TInputActions> where TInputActions : class, IInputActionCollection2, IDisposable
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InputActionsWrapperBase{TInputActions}"/>,
        /// сохраняет переданные зависимости и сразу вызывает <see cref="Register"/> для подписки на события ввода.
        /// </summary>
        /// <param name="inputActions">
        /// Экземпляр <typeparamref name="TInputActions"/>, предоставляющий доступ к действиям ввода.
        /// </param>
        /// <param name="invoker">
        /// Интерфейс для вызова событий в системе событий.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="inputActions"/> или <paramref name="invoker"/> равен <see langword="null"/>.
        /// </exception>
        public InputActionsWrapperBase(TInputActions inputActions, IEventInvoker invoker)
        {
            InputActions = inputActions ?? throw new ArgumentNullException(nameof(inputActions));
            Invoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
            Register();
        }

        /// <summary>
        /// Экземпляр <typeparamref name="TInputActions"/>, используемый для доступа к действиям ввода.
        /// </summary>
        public TInputActions InputActions { get; private set; }
        /// <summary>
        /// Интерфейс для вызова событий, через который отправляются сгенерированные структуры событий ввода.
        /// </summary>
        public IEventInvoker Invoker { get; private set; }
        /// <summary>
        /// <see langword="true"/>, если экземпляр уже был освобождён методом <see cref="Dispose"/>; иначе <see langword="false"/>.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Вызывается при создании обёртки для подписки на все необходимые действия ввода.
        /// </summary>
        protected abstract void Register();

        /// <summary>
        /// Вызывается при освобождении обёртки для отписки от всех действий ввода.
        /// </summary>
        protected abstract void Unregister();

        /// <summary>
        /// Освобождает ресурсы, занятые обёрткой: отписывается от событий ввода и обнуляет
        /// ссылки на <see cref="InputActions"/> и <see cref="Invoker"/>.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            IsDisposed = true;
            Unregister();
            InputActions = null;
            Invoker = null;
        }
    }
}
