using System;

namespace Core.Timer
{
    public interface ITimer
    {
        TimeSpan Elapsed { get; }
        
        void AddListener(Action<TimeSpan> onUpdate, bool invokeNow = true);
        void RemoveListener(Action<TimeSpan> onUpdate);
        void Start();
        void Restart();
        void Stop();
    }
}