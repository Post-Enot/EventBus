using System;

namespace PostEnot.Toolkits.EventManagement
{
    /// <summary>
    /// Представляет интерфейс для адаптации всевозможных систем логирования и их использования шиной событий.
    /// </summary>
    public interface ILogger
    {
        public void LogException(Exception exception);
    }
}
