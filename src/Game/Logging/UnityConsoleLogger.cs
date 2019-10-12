using Core.Logging;

namespace Game.Logging
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class UnityConsoleLogger : ILogger
    {
        public void Info(object o)
        {
            UnityEngine.Debug.Log(o);
        }

        public void Info(string message, params object[] args)
        {
            UnityEngine.Debug.LogFormat(message, args);
        }

        public void Warn(object o)
        {
            UnityEngine.Debug.LogWarning(o);
        }

        public void Warn(string message, params object[] args)
        {
            UnityEngine.Debug.LogWarningFormat(message, args);
        }

        public void Error(object o)
        {
            UnityEngine.Debug.LogError(o);
        }

        public void Error(string message, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(message, args);
        }
    }
}