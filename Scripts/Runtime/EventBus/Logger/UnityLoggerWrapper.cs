#nullable enable

using System;

using IUnityLogger = UnityEngine.ILogger;

namespace PostEnot.Toolkits.EventManagement
{
    public sealed class UnityLoggerWrapper : ILogger
    {
        public UnityLoggerWrapper(IUnityLogger unityLogger) => Logger = unityLogger;

        public IUnityLogger Logger { get; }

        public void LogException(Exception exception) => Logger?.LogException(exception);
    }
}
