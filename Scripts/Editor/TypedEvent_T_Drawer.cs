using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PostEnot.Toolkits.EventManagement.Editor
{
    [CustomPropertyDrawer(typeof(TypedEvent<>))]
    internal sealed class TypedEvent_T_Drawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty eventProperty = property.FindPropertyRelative("event");
            return new PropertyField(eventProperty)
            {
                label = property.displayName
            };
        }
    }
}
