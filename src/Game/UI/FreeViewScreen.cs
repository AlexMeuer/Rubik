using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public class FreeViewScreen : ScreenBase
    {
        private const float AnimDurationSeconds = 1f;
        
        private readonly IEndGameScreenCallbacks callbacks;
        
        private GameObject returnButton;
        private Rect buttonRect;

        public FreeViewScreen(IEndGameScreenCallbacks callbacks)
        {
            this.callbacks = callbacks;
        }

        public override void Build()
        { 
            var buttonPrefab = Resources.Load<GameObject>("Prefabs/BackButton");
            
            buttonRect = buttonPrefab.GetComponent<RectTransform>().rect;

            returnButton = Object.Instantiate(buttonPrefab, CanvasObject.transform);
            
            returnButton.GetComponent<Button>().onClick.AddListener(callbacks.OnReturnRequested);
        }

        public override void AnimateIn(Action onComplete = null)
        {
            returnButton.transform.DOLocalMoveX(-CanvasRect.width - buttonRect.width, AnimDurationSeconds)
                .From()
                .SetEase(Ease.OutBounce)
                .OnComplete(() => onComplete?.Invoke());
        }

        public override void AnimateOut(Action onComplete = null)
        {
            returnButton.transform.DOLocalMoveX(-CanvasRect.width - buttonRect.width, AnimDurationSeconds)
                .SetEase(Ease.OutBounce)
                .OnComplete(() => onComplete?.Invoke());
        }

        public override void Dispose()
        {
            Object.Destroy(returnButton);
        }
    }
}