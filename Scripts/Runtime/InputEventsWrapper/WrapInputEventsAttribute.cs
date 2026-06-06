#nullable enable

using System;

namespace PostEnot.Toolkits.EventManagement.Input
{
    /// <summary>
    /// Указывает, что для данного класса, реализующего <see cref="UnityEngine.InputSystem.IInputActionCollection2"/>,
    /// необходимо сгенерировать класс-обёртку и класс событий ввода.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class WrapInputEventsAttribute : Attribute
    {
        /// <summary>
        /// Пространство имён генерируемого класса событий. Если не задано, используется пространство имён исходного класса.
        /// </summary>
        public string? EventsNamespace { get; set; }
        /// <summary>
        /// Имя генерируемого класса событий. Если не задано, используется имя исходного класса с суффиксом "_Events".
        /// </summary>
        public string? EventsClassName { get; set; }
        /// <summary>
        /// Пространство имён генерируемого класса-обёртки. Если не задано, используется пространство имён исходного класса.
        /// </summary>
        public string? WrapperNamespace { get; set; }
        /// <summary>
        /// Имя генерируемого класса-обёртки. Если не задано, используется имя исходного класса с суффиксом "_Wrapper".
        /// </summary>
        public string? WrapperClassName { get; set; }
    }
}
