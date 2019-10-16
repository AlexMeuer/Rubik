using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public interface IOptionsScreenCallbacks
    {
        void OnChangeTimerVisibilityRequested(bool visible);
        void OnContinueRequested();
        void OnQuitRequested();
    }
    public class OptionsScreen : ScreenBase
    {
        private const float AnimDurationSeconds = 0.75f;
        private readonly IOptionsScreenCallbacks callbacks;
        private readonly bool timerIsVisible;

        private GameObject panel;
        private float offscreenY;

        public OptionsScreen(IOptionsScreenCallbacks callbacks, bool timerIsVisible)
        {
            this.callbacks = callbacks;
            this.timerIsVisible = timerIsVisible;
        }

        public override void Build()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/OptionsPanel");

            panel = Object.Instantiate(prefab, CanvasObject.transform);
            
            offscreenY = CanvasRect.width * 0.6f + panel.GetComponent<RectTransform>().rect.height;

            var toggle = panel.transform.Find("Toggle").GetComponent<Toggle>();
            toggle.isOn = timerIsVisible;
            toggle.onValueChanged.AddListener(callbacks.OnChangeTimerVisibilityRequested);

            var continueButton = panel.transform.Find("ContinueButton").GetComponent<Button>();
            continueButton.onClick.AddListener(callbacks.OnContinueRequested);

            var quitButton = panel.transform.Find("QuitButton").GetComponent<Button>();
            quitButton.onClick.AddListener(callbacks.OnQuitRequested);
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