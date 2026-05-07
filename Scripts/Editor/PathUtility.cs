using System;
using System.IO;
using System.Linq;

namespace PostEnot.Toolkits
{
    internal static class PathUtility
    {
        internal static bool IsInvalidFilePath(string path, string extension) => !IsValidFilePath(path, extension);

        internal static bool IsValidFilePath(string path, string extension) => IsValidPath(path) && path.EndsWith(extension);

        internal static bool IsInvalidPath(string path) => !IsValidPath(path);

        internal static bool IsValidPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            char[] invalidPathChars = Path.GetInvalidPathChars();
            foreach (char ch in path)
            {
                if (invalidPathChars.Contains(ch))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
