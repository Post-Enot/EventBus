using PostEnot.Toolkits.Editor.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace PostEnot.Toolkits.EventManagement.Editor
{
    [CustomEditor(typeof(InputEventsWrapperGeneratorAsset))]
    internal sealed class InputEventsWrapperGeneratorAssetEditor : UnityEditor.Editor
    {
        #region Inspector
        [SerializeField] private VisualTreeAsset visualTreeAsset;
        [SerializeField] private StyleSheet styleSheetAsset;
        #endregion

        private ObjectField _inputAssetField;
        private FileField _eventsFileField;
        private TypeNameField _eventsTypeNameField;
        private FileField _wrapperFileField;
        private TypeNameField _wrapperTypeNameField;
        private Button _buttonGenerateCode;

        public override VisualElement CreateInspectorGUI()
        {
            InputEventsWrapperGeneratorAsset config = target as InputEventsWrapperGeneratorAsset;

            SerializedProperty inputActionAssetProperty = serializedObject.FindProperty("inputActionAsset");
            SerializedProperty eventsFilePathProperty = serializedObject.FindProperty("eventsFilePath");
            SerializedProperty eventsTypeNameProperty = serializedObject.FindProperty("eventsTypeName");
            SerializedProperty wrapperFilePathProperty = serializedObject.FindProperty("wrapperFilePath");
            SerializedProperty wrapperTypeNameProperty = serializedObject.FindProperty("wrapperTypeName");

            InputActionAsset inputActionAsset = inputActionAssetProperty.objectReferenceValue as InputActionAsset;

            VisualElement root = new();
            visualTreeAsset.CloneTree(root);
            root.styleSheets.Add(styleSheetAsset);

            _inputAssetField = root.Q<ObjectField>("inputAssetField");
            _eventsFileField = root.Q<FileField>("eventsFileField");
            _eventsTypeNameField = root.Q<TypeNameField>("eventsTypeNameField");
            _wrapperFileField = root.Q<FileField>("wrapperFileField");
            _wrapperTypeNameField = root.Q<TypeNameField>("wrapperTypeNameField");
            _buttonGenerateCode = root.Q<Button>("generateCodeButton");

            _eventsFileField.ValueChanged += OnValueChanged;
            _eventsTypeNameField.ValueChanged += OnValueChanged;
            _wrapperFileField.ValueChanged += OnValueChanged;
            _wrapperTypeNameField.ValueChanged += OnValueChanged;
            _buttonGenerateCode.clicked += OnGenerateCodeButtonClicked;
            _inputAssetField.RegisterValueChangedCallback(OnInputAssetFieldValueChanged);

            _inputAssetField.BindProperty(inputActionAssetProperty);
            _eventsFileField.BindProperty(eventsFilePathProperty);
            _eventsTypeNameField.BindProperty(wrapperFilePathProperty);
            _wrapperFileField.BindProperty(eventsTypeNameProperty);
            _wrapperTypeNameField.BindProperty(wrapperTypeNameProperty);
            return root;
        }

        private void OnInputAssetFieldValueChanged(ChangeEvent<Object> context) => UpdateButtonState();

        private void OnValueChanged(string value) => UpdateButtonState();

        private void UpdateButtonState()
            => _buttonGenerateCode.enabledSelf = (_inputAssetField.value != null)
            && _eventsFileField.IsValueValid()
            && _eventsTypeNameField.IsValueValid()
            && _wrapperFileField.IsValueValid()
            && _wrapperTypeNameField.IsValueValid();
        private void OnGenerateCodeButtonClicked()
        {
            InputActionAsset asset = _inputAssetField.value as InputActionAsset;
            InputWrapperGenerator.GenerateInputEvents(
                asset,
                _eventsFileField.Value,
                _eventsTypeNameField.Value);
            InputWrapperGenerator.GenerateInputWrapper(
                asset,
                _wrapperFileField.Value,
                _wrapperTypeNameField.Value,
                _eventsTypeNameField.Value);
            AssetDatabase.Refresh();
        }
    }
}
