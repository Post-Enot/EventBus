using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PostEnot.Toolkits.EventManagement.Editor
{
    [CustomPropertyDrawer(typeof(TypedEvent))]
    internal sealed class TypedEventDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            Foldout foldout = new()
            {
                text = property.displayName
            };
            foldout.BindProperty(property);
            PropertyField typedEventField = new();
            SerializedProperty typedEventProperty = property.FindPropertyRelative("typedEvent");
            Type typedEventValue = TypedEventUtility.GetEventTypeOfTypedEvent(typedEventProperty.managedReferenceValue);
            typedEventField.BindProperty(typedEventProperty);
            typedEventField.AddToClassList(BaseField<int>.alignedFieldUssClassName);
            TypedEventReferenceField typedEventReferenceField = new()
            {
                label = "Event Type",
                value = typedEventValue
            };
            UpdateTypedEventDisplaying(typedEventField, typedEventValue);
            typedEventReferenceField.RegisterValueChangedCallback(context =>
            {
                if (typedEventProperty.managedReferenceValue is ITypedEvent typedEvent)
                {
                    typedEvent.ClearCallbacks();
                }
                typedEventProperty.managedReferenceValue = TypedEventUtility.CreateTypedEventInstance(context.newValue);
                typedEventProperty.serializedObject.ApplyModifiedProperties();
                typedEventField.MarkDirtyRepaint();
                UpdateTypedEventDisplaying(typedEventField, context.newValue);
            });
            foldout.Add(typedEventReferenceField);
            foldout.Add(typedEventField);
            typedEventReferenceField.AddToClassList(TypedEventReferenceField.alignedFieldUssClassName);
            return foldout;
        }

        private static void UpdateTypedEventDisplaying(PropertyField field, Type value)
        {
            if (value == null)
            {
                field.style.display = DisplayStyle.None;
            }
            else
            {
                field.style.display = StyleKeyword.Null;
            }
        }
    }
}
