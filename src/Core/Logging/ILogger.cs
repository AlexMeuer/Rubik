using Core.JetBrains;

namespace Core.Logging
{
    public interface ILogger
    {
        void Info(object o);
        [StringFormatMethod("message")]
        void Info(string message, params object[] args);
        
        void Warn(object o);
        [StringFormatMethod("message")]
        void Warn(string message, params object[] args);
        
        void Error(object o);
        [StringFormatMethod("message")]
        void Error(string message, params object[] args);
    }
}