using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PostEnot.Toolkits.Editor.UIElements
{
    [UxmlElement(libraryPath = "IUP Editor")]
    public partial class TypeNameField : VisualElement
    {
        public TypeNameField()
        {
            _inputField = new TextField();
            _inputField.RegisterValueChangedCallback(OnValueChanged);
            _helpBox = new HelpBox()
            {
                messageType = HelpBoxMessageType.Error
            };
            _helpBox.AddToClassList(UssHelpBox);
            Add(_inputField);
            AddToClassList(Uss);
            _inputField.AddToClassList(UssInput);
            _inputField.AddToClassList(TextField.alignedFieldUssClassName);
        }

        public static readonly string Uss = "iup-type-name-field";
        public static readonly string UssInput = "iup-type-name-field__input";
        public static readonly string UssHelpBox = "iup-type-name-field__help-box";

        [UxmlAttribute] public string Label
        {
            get => _inputField.label;
            set => _inputField.label = value;
        }

        [UxmlAttribute] public string Placeholder
        {
            get => _inputField.textEdition.placeholder;
            set => _inputField.textEdition.placeholder = value;
        }

        [UxmlAttribute] public string Value
        {
            get => _inputField.value;
            set => _inputField.value = value;
        }

        public event Action<string> ValueChanged;

        private readonly TextField _inputField;
        private readonly HelpBox _helpBox;

        public void BindProperty(SerializedProperty property) => _inputField.BindProperty(property);

        public bool IsValueValid() => EventManagement.Editor.CodeGeneratorUtility.IsValidTypeName(Value);

        public bool IsValueInvalid() => EventManagement.Editor.CodeGeneratorUtility.IsInvalidTypeName(Value);

        private void OnValueChanged(ChangeEvent<string> context)
        {
            ValueChanged?.Invoke(Value);
            UpdateHelpBox();
        }

        private void UpdateHelpBox()
        {
            if (GetValidationMessage(Value, out string message))
            {
                _helpBox.text = message;
                if (Contains(_helpBox))
                {
                    return;
                }
                Add(_helpBox);
            }
            else
            {
                if (Contains(_helpBox))
                {
                    Remove(_helpBox);
                }
            }
        }

        private static bool GetValidationMessage(string className, out string message)
        {
            if (EventManagement.Editor.CodeGeneratorUtility.IsInvalidTypeName(className))
            {
                message = "Value should be valid type name.";
                return true;
            }
            message = null;
            return false;
        }
    }
}
