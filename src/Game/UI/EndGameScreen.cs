using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public interface IEndGameScreenCallbacks
    {
        void OnReturnRequested();
        void OnStayRequested();
    }
    
    public class EndGameScreen : ScreenBase
    {
        private const float AnimDurationSeconds = 0.75f;
        
        private readonly TimeSpan timeToSolve;
        private readonly IEndGameScreenCallbacks callbacks;

        private GameObject panel;
        private float offscreenY;

        public EndGameScreen(TimeSpan timeToSolve, IEndGameScreenCallbacks callbacks)
        {
            this.timeToSolve = timeToSolve;
            this.callbacks = callbacks;
        }

        public override void Build()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/EndDialog");

            panel = Object.Instantiate(prefab, CanvasObject.transform);
            
            offscreenY = CanvasRect.width * 0.6f + panel.GetComponent<RectTransform>().rect.height;

            panel.transform.Find("Minutes_Count").GetComponent<Text>().text = ((int)timeToSolve.TotalMinutes).ToString();

            panel.transform.Find("Seconds_Count").GetComponent<Text>().text = timeToSolve.Seconds.ToString();
            
            panel.transform.Find("ReturnButton").GetComponent<Button>().onClick.AddListener(callbacks.OnReturnRequested);
            
            panel.transform.Find("StayButton").GetComponent<Button>().onClick.AddListener(callbacks.OnStayRequested);
        }

        public override void AnimateIn(Action onComplete = null)
        {
            panel.transform.DOMoveY(offscreenY, AnimDurationSeconds)
                .From()
                .SetEase(Ease.OutQuad)
                .OnComplete(() => onComplete?.Invoke());
        }

        public override void AnimateOut(Action onComplete = null)
        {
            panel.transform.DOMoveY(offscreenY, AnimDurationSeconds)
                .SetEase(Ease.InQuad)
                .OnComplete(() => onComplete?.Invoke());
        }

        public override void Dispose()
        {
            Object.Destroy(panel);
        }
    }
}