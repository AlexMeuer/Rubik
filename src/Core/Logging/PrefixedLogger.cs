namespace Core.Logging
{
    public class PrefixedLogger : ILogger
    {
        public static ILogger ForType<T>(ILogger inner)
        {
            return new PrefixedLogger($"{typeof(T).Name}: ", inner);
        }
        
        private readonly string prefix;
        private readonly ILogger inner;

        public PrefixedLogger(string prefix, ILogger inner)
        {
            this.prefix = prefix;
            this.inner = inner;
        }
        public void Info(object o)
        {
            inner.Info("{0}{1}", prefix, o);
        }

        public void Info(string message, params object[] args)
        {
            inner.Info(prefix + message, args);
        }

        public void Warn(object o)
        {
            inner.Warn("{0}{1}", prefix, o);
        }

        public void Warn(string message, params object[] args)
        {
            inner.Warn(prefix + message, args);
        }

        public void Error(object o)
        {
            inner.Error("{0}{1}", prefix, o);
        }

        public void Error(string message, params object[] args)
        {
            inner.Error(prefix + message, args);
        }
    }
}