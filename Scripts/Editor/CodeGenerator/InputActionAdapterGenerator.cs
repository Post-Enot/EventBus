using PostEnot.Toolkits.EventManagement.Input;
using System;
using UnityEngine.InputSystem;

namespace PostEnot.Toolkits.EventManagement.Editor
{
    public static class InputActionAdapterGenerator
    {
        public static string GenerateCode(
            InputActionAsset asset,
            ReadOnlySpan<char> eventsTypeName,
            ReadOnlySpan<char> wrapperTypeName)
        {
            CodeGeneratorUtility.TypeNameToClassNameAndNamespace(
                wrapperTypeName,
                out ReadOnlySpan<char> wrapperClassName,
                out ReadOnlySpan<char> wrapperNamespace);
            CodeGenerator gen = new();
            gen.Using($"{nameof(UnityEngine.InputSystem)}")
               .Using($"{nameof(PostEnot.Toolkits.EventManagement.Input)}")
               .Empty()
               .UsingStatic(eventsTypeName)
               .Empty();
            using (gen.BlockNamespace(wrapperNamespace))
            {
                using (gen.BlockBracket($"public partial class {wrapperClassName.ToString()} : {nameof(InputActionEventAdapter)}"))
                {
                    using (gen.BlockBracket("private void Start()"))
                    {
                        foreach (InputActionMap map in asset.actionMaps)
                        {
                            foreach (InputAction action in map.actions)
                            {
                                GenerateSubscribe(gen, action);
                            }
                        }
                    }
                    gen.Empty();
                    using (gen.BlockBracket("private void OnDestroy()"))
                    {
                        foreach (InputActionMap map in asset.actionMaps)
                        {
                            foreach (InputAction action in map.actions)
                            {
                                GenerateUnsubscribe(gen, action);
                            }
                        }
                    }
                    foreach (InputActionMap map in asset.actionMaps)
                    {
                        string mapName = map.name;
                        foreach (InputAction action in map.actions)
                        {
                            GenerateMethod(gen, action, mapName);
                        }
                    }
                }
            }
            return gen.ToString();
        }

        private static void GenerateMethod(CodeGenerator gen, InputAction action, string mapName)
        {
            gen.Empty();
            GenerateMethod(gen, action, mapName, "Started");
            gen.Empty();
            GenerateMethod(gen, action, mapName, "Performed");
            gen.Empty();
            GenerateMethod(gen, action, mapName, "Canceled");
        }

        private static void GenerateMethod(CodeGenerator gen, InputAction action, string mapName, string eventName)
        {
            using (gen.BlockBracket($"private void On{mapName}{action.name}{eventName}(InputAction.CallbackContext inputContext)"))
            {
                gen.AddLines(
                    $"{mapName}.{action.name}{eventName} context = new(inputContext);",
                    $"Invoker.Invoke(context);");
            }
        }

        private static void GenerateSubscribe(CodeGenerator gen, InputAction action)
        {
            string validActionMapID = CodeGeneratorUtility.ToValidID(action.actionMap.name);
            string validActionID = CodeGeneratorUtility.ToValidID(action.name);
            string methodName = $"{validActionMapID}{validActionID}";
            gen.AddLines(
                $"InputActions.FindAction(\"{action.actionMap.name}/{action.name}\").started += On{methodName}Started;",
                $"InputActions.FindAction(\"{action.actionMap.name}/{action.name}\").performed += On{methodName}Performed;",
                $"InputActions.FindAction(\"{action.actionMap.name}/{action.name}\").canceled += On{methodName}Canceled;");
        }

        private static void GenerateUnsubscribe(CodeGenerator gen, InputAction action)
        {
            string validActionMapID = CodeGeneratorUtility.ToValidID(action.actionMap.name);
            string validActionID = CodeGeneratorUtility.ToValidID(action.name);
            string methodName = $"{validActionMapID}{validActionID}";
            gen.AddLines(
                $"InputActions.FindAction(\"{action.actionMap.name}/{action.name}\").started -= On{methodName}Started;",
                $"InputActions.FindAction(\"{action.actionMap.name}/{action.name}\").performed -= On{methodName}Performed;",
                $"InputActions.FindAction(\"{action.actionMap.name}/{action.name}\").canceled -= On{methodName}Canceled;");
        }
    }
}
