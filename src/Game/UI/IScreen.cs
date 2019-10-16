using System;
using UnityEngine;

namespace Game.UI
{
    public interface IScreen : IDisposable
    {
        void Build();

        void AnimateIn(Action onComplete);
        void AnimateOut(Action onComplete);
    }

    public abstract class ScreenBase : IScreen
    {
        protected GameObject CanvasObject { get; }

        protected ScreenBase()
        {
            CanvasObject = GameObject.Find("Canvas");
        }
        
        public abstract void Build();
        public abstract void AnimateIn(Action onComplete = null);
        public abstract void AnimateOut(Action onComplete = null);
        public abstract void Dispose();
    }
}