using System;
using UnityEngine;

namespace Game.UI
{
    public interface IScreen : IDisposable
    {
        void Build();

        void AnimateIn(Action onComplete = null);
        void AnimateOut(Action onComplete = null);

        void SetEnabled(bool enabled);
    }

    public abstract class ScreenBase : IScreen
    {
        protected GameObject CanvasObject { get; }
        protected Rect CanvasRect { get; }

        protected ScreenBase()
        {
            CanvasObject = GameObject.Find("Canvas");
            CanvasRect = CanvasObject.GetComponent<RectTransform>().rect;
        }
        
        public abstract void Build();
        public abstract void AnimateIn(Action onComplete = null);
        public abstract void AnimateOut(Action onComplete = null);
        public abstract void Dispose();


        public virtual void SetEnabled(bool enabled)
        {
        }
    }
}