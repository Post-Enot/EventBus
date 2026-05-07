using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PostEnot.Toolkits.EventManagement.Editor
{
    internal static class InputWrapperGenerator
    {
        internal static void GenerateInputEvents(InputActionAsset asset, string path, string typeName)
        {
            string code = InputEventsGenerator.GenerateCode(asset, typeName);
            SaveToFile(code, path);
        }

        internal static void GenerateInputWrapper(InputActionAsset asset, string path, string typeName, string eventsTypeName)
        {
            string code = InputActionAdapterGenerator.GenerateCode(asset, eventsTypeName, typeName);
            SaveToFile(code, path);
        }

        internal static void SaveToFile(string content, string path)
        {
            path = TrimFirstPathSegment(path);
            string fullPath = Path.Combine(Application.dataPath, path);
            File.WriteAllText(fullPath, content);
        }

        private static string TrimFirstPathSegment(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            int index = path.IndexOf('/');
            if (index == -1)
            {
                index = path.IndexOf('\\');
            }
            int start = index + 1;
            return index >= 0 ? path[start..] : string.Empty;
        }
    }
}
