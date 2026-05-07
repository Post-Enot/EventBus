using System;
using UnityEngine.InputSystem;

namespace PostEnot.Toolkits.EventManagement.Editor
{
    public static class InputEventsGenerator
    {
        public static string GenerateCode(InputActionAsset asset, ReadOnlySpan<char> typeName)
        {
            CodeGeneratorUtility.TypeNameToClassNameAndNamespace(
                typeName,
                out ReadOnlySpan<char> className,
                out ReadOnlySpan<char> @namespace);
            CodeGenerator gen = new();
            gen.Using(nameof(UnityEngine))
               .Empty()
               .Using("InputContext", "UnityEngine.InputSystem.InputAction.CallbackContext")
               .Empty();
            using (gen.BlockNamespace(@namespace))
            {
                using (gen.BlockBracket($"public static partial class {className.ToString()}"))
                {
                    foreach (InputActionMap map in asset.actionMaps)
                    {
                        using (gen.BlockBracket($"public static partial class {map.name}"))
                        {
                            foreach (InputAction action in map.actions)
                            {
                                GenerateEventStructs(gen, action);
                            }
                        }
                    }
                }
            }
            return gen.ToString();
        }

        private static void GenerateEventStructs(CodeGenerator gen, InputAction action)
        {
            string valueType = action.expectedControlType switch
            {
                "Integer" => "int",
                "Double" => "double",
                "Axis" => "float",
                "Vector2" => "Vector2",
                "Vector3" => "Vector3",
                "Quaternion" => "Quaternion",
                _ => null
            };
            GenerateEventStruct(gen, $"{action.name}Started", valueType);
            GenerateEventStruct(gen, $"{action.name}Performed", valueType);
            GenerateEventStruct(gen, $"{action.name}Canceled", valueType);
        }

        private static void GenerateEventStruct(CodeGenerator gen, string structName, string valueType)
        {
            using (gen.BlockBracket($"public readonly struct {structName}"))
            {
                gen.AddLine($"public {structName}(InputContext inputContext) => InputContext = inputContext;")
                   .Empty()
                   .AddLine($"public readonly InputContext InputContext {{ get; }}");
                if (valueType != null)
                {
                    gen.Empty()
                       .AddLine($"public readonly {valueType} Value => InputContext.ReadValue<{valueType}>();");
                }
            }
        }
    }
}
