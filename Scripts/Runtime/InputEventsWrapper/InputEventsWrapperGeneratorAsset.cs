using UnityEngine;
using UnityEngine.InputSystem;

namespace PostEnot.Toolkits.EventManagement
{
    [CreateAssetMenu(fileName = "InputEventsWrapperGenerator", menuName = "PostEnot/System/Input Events Wrapper Generator")]
    public sealed class InputEventsWrapperGeneratorAsset : ScriptableObject
    {
        #region Inspector
        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private string eventsFilePath;
        [SerializeField] private string eventsTypeName;
        [SerializeField] private string wrapperFilePath;
        [SerializeField] private string wrapperTypeName;
        #endregion

        public InputActionAsset InputActionAsset => inputActionAsset;
        public string EventsFilePath => eventsFilePath;
        public string EventsTypeName => eventsTypeName;
        public string WrapperFilePath => wrapperFilePath;
        public string WrapperTypeName => wrapperTypeName;
    }
}
