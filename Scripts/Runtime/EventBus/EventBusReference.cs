#nullable enable

using UnityEngine;

namespace PostEnot.Toolkits.EventManagement
{
    /// <summary>
    /// Предоставляет доступ к шине событий (<see cref="IEventBus"/>) через ассет типа <see cref="ScriptableObject"/>.
    /// Используется для протягивания зависимостей и предоставления доступа к единой шине событий через отображаемые в инспекторе поля.
    /// </summary>
    [CreateAssetMenu(fileName = "EventBusReference", menuName = "PostEnot/Event Bus Reference")]
    public sealed class EventBusReference : ScriptableObject
    {
        #region EditorOnly
#if UNITY_EDITOR
        // Механизм сброса шины при старте Play Mode необходим для корректной работы с выключенным Domain Reload.
        private static int _globalSessionCode;

        private int _sessionCode;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void RuntimeInitCallback()
        {
            unchecked
            {
                _globalSessionCode += 1;
            }
        }

        private bool IsResetRequired()
        {
            if (_sessionCode != _globalSessionCode)
            {
                _sessionCode = _globalSessionCode;
                return true;
            }
            return false;
        }
#endif
        #endregion

        /// <summary>
        /// Возвращает текущую шину событий. При первом обращении создаёт и кеширует экземпляр <see cref="IEventBus"/>.
        /// </summary>
        /// <returns>Готовый к использованию экземпляр <see cref="IEventBus"/>.</returns>
        public IEventBus EventBus
        {
            get
            {
#if UNITY_EDITOR
                if (IsResetRequired())
                {
                    _eventBus = null;
                }
#endif
                if (_eventBus == null)
                {
                    ILogger logger = new UnityLoggerWrapper(Debug.unityLogger);
                    _eventBus = new EventBus(logger);

                }
                return _eventBus;
            }
        }

        private EventBus? _eventBus;
    }
}
