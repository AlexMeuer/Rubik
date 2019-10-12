using System;

namespace Core.Logging
{
    public static class PLog
    {
        public static void Info(object o)
        {
            UnityEngine.Debug.Log(o);
        }

        public static void Warn(object o)
        {
            UnityEngine.Debug.LogWarning(o);
        }

        public static void Error(object o)
        {
            UnityEngine.Debug.LogError(o);
        }

        public static void Error(object o, Exception e)
        {
            UnityEngine.Debug.LogErrorFormat("{0}\n{1}", o, e);
        }
    }
}