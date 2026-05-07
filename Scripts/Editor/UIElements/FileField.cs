using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PostEnot.Toolkits.Editor.UIElements
{
    [UxmlElement(libraryPath = "IUP Editor")]
    public partial class FileField : VisualElement
    {
        [UxmlAttribute]
        public string Label
        {
            get => _inputField.label;
            set => _inputField.label = value;
        }
        [UxmlAttribute]
        public string Placeholder
        {
            get => _inputField.textEdition.placeholder;
            set => _inputField.textEdition.placeholder = value;
        }
        [UxmlAttribute]
        public string ButtonText
        {
            get => _browseButton.text;
            set => _browseButton.text = value;
        }
        [UxmlAttribute]
        public string Value
        {
            get => _inputField.value;
            set => _inputField.value = value;
        }
        [UxmlAttribute] public bool SaveInProject { get; set; }
        [UxmlAttribute] public string FilePanelTitle { get; set; }
        [UxmlAttribute] public string FilePanelDefaultName { get; set; }
        [UxmlAttribute] public string FilePanelExtension { get; set; }
        [UxmlAttribute] public string FilePanelMessage { get; set; }
        [UxmlAttribute] public string FilePanelPath { get; set; }

        public static readonly string Uss = "iup-file-field";
        public static readonly string UssInputContainer = "iup-file-field__input-container";
        public static readonly string UssInput = "iup-file-field__input";
        public static readonly string UssButton = "iup-file-field__button";

        public event Action<string> ValueChanged;

        private readonly TextField _inputField;
        private readonly Button _browseButton;
        private readonly VisualElement _inputContainer;
        private readonly HelpBox _helpBox;

        public FileField()
        {
            _inputContainer = new VisualElement()
            {
                name = "input-container"
            };
            _inputField = new TextField();
            _inputField.RegisterValueChangedCallback(OnValueChanged);
            _browseButton = new Button(BrowseFolder)
            {
                text = "..."
            };
            _helpBox = new HelpBox()
            {
                messageType = HelpBoxMessageType.Error
            };
            Add(_inputContainer);
            _inputContainer.Add(_inputField);
            _inputContainer.Add(_browseButton);

            AddToClassList(Uss);
            _inputContainer.AddToClassList(UssInputContainer);
            _inputField.AddToClassList(UssInput);
            _inputField.AddToClassList(TextField.alignedFieldUssClassName);
            _browseButton.AddToClassList(UssButton);
        }

        public void BindProperty(SerializedProperty property) => _inputField.BindProperty(property);

        public bool IsValueValid() => PathUtility.IsValidFilePath(Value, FilePanelExtension);

        public bool IsValueInvalid() => PathUtility.IsInvalidFilePath(Value, FilePanelExtension);

        private void OnValueChanged(ChangeEvent<string> context)
        {
            ValueChanged?.Invoke(Value);
            UpdateHelpBox();
        }

        private void UpdateHelpBox()
        {
            if (PathUtility.IsInvalidFilePath(Value, FilePanelExtension))
            {
                if (Contains(_helpBox))
                {
                    return;
                }
                if (string.IsNullOrEmpty(FilePanelExtension))
                {
                    _helpBox.text = "The path must not contain invalid characters.";
                }
                else
                {
                    _helpBox.text = "The path must not contain invalid characters and " +
                        $"must end with an extension {FilePanelExtension}.";
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

        private void BrowseFolder()
        {
            string filePanelPath = FilePanelPath;
            if (string.IsNullOrEmpty(filePanelPath))
            {
                filePanelPath = Application.dataPath;
            }
            if (SaveInProject)
            {
                Value = EditorUtility.SaveFilePanelInProject(
                    FilePanelTitle,
                    FilePanelDefaultName,
                    FilePanelExtension,
                    FilePanelMessage,
                    filePanelPath);
            }
            else
            {
                Value = EditorUtility.SaveFilePanel(
                    FilePanelTitle,
                    filePanelPath,
                    FilePanelDefaultName,
                    FilePanelExtension);
            }
        }
    }
}
