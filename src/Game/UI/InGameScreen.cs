using System;
using Core.Command;
using Core.Command.Messages;
using Core.TinyMessenger;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public class InGameScreen : ScreenBase
    {
        private const float AnimDurationSeconds = 1f;

        private readonly ITinyMessengerHub messengerHub;
            
        private Rect buttonRect;
        private GameObject undoButton;
        private GameObject redoButton;
        private TinyMessageSubscriptionToken cmdCompletedSubscriptionToken;

        private bool canUndo;
        private bool canRedo;

        public InGameScreen(ITinyMessengerHub messengerHub)
        {
            this.messengerHub = messengerHub;
        }

        public override void Build()
        {
            var undoButtonPrefab = Resources.Load<GameObject>("Prefabs/UndoButton");
            buttonRect = undoButtonPrefab.GetComponent<RectTransform>().rect;

            const float margin = 20f;

            undoButton = Object.Instantiate(undoButtonPrefab, CanvasObject.transform);
            undoButton.name = "UndoButton";
            undoButton.transform.localPosition = new Vector3(
                x: CanvasRect.width * 0.5f - buttonRect.width * 0.5f - margin, 
                y: CanvasRect.height * 0.5f - buttonRect.height * 0.5f - margin, 
                z: 0);
            
            redoButton = Object.Instantiate(undoButtonPrefab, CanvasObject.transform);
            redoButton.name = "RedoButton";
            redoButton.transform.localScale = Vector3.one;
            redoButton.transform.localPosition = new Vector3(
                x: CanvasRect.width * 0.5f - buttonRect.width * 0.5f - margin, 
                y: CanvasRect.height * 0.5f - buttonRect.height * 1.5f - margin, 
                z: 0);
            
            undoButton.GetComponent<Button>().onClick.AddListener(UndoRequested);
            redoButton.GetComponent<Button>().onClick.AddListener(RedoRequested);

            cmdCompletedSubscriptionToken = messengerHub.Subscribe<CommandCompleteMessage>(OnCommandCompleted);
        }

        public override void AnimateIn(Action onComplete = null)
        {
            undoButton.transform.DOMoveX(CanvasRect.width + buttonRect.width, AnimDurationSeconds)
                .From()
                .SetEase(Ease.OutBounce)
                .OnComplete(() => onComplete?.Invoke());
            
            redoButton.transform.DOMoveX(CanvasRect.width + buttonRect.width, AnimDurationSeconds)
                .From()
                .SetEase(Ease.OutBounce)
                .OnComplete(() => onComplete?.Invoke());
        }

        public override void AnimateOut(Action onComplete = null)
        {
            undoButton.transform.DOMoveX(CanvasRect.width + buttonRect.width, AnimDurationSeconds)
                .SetEase(Ease.OutBounce)
                .OnComplete(() => onComplete?.Invoke());
            
            redoButton.transform.DOMoveX(CanvasRect.width + buttonRect.width, AnimDurationSeconds)
                .SetEase(Ease.OutBounce)
                .OnComplete(() => onComplete?.Invoke());
        }

        public override void Dispose()
        {
            cmdCompletedSubscriptionToken.Dispose();
            Object.Destroy(redoButton);
            Object.Destroy(undoButton);
        }

        public override void SetEnabled(bool enabled)
        {
            SetEnabled(undoButton.GetComponent<Button>(),canUndo && enabled);
            SetEnabled(redoButton.GetComponent<Button>(), canRedo && enabled);
        }

        private void OnCommandCompleted(CommandCompleteMessage message)
        {
            canUndo = message.Handler.CanUndo;
            canRedo = message.Handler.CanRedo;

            var btnComponent = undoButton.GetComponent<Button>();
            SetEnabled(btnComponent, canUndo && btnComponent.enabled);
            
            btnComponent = redoButton.GetComponent<Button>();
            SetEnabled(btnComponent, canRedo && btnComponent.enabled);
        }

        private void UndoRequested() => messengerHub.Publish(new UndoCommandMessage(this));
        
        private void RedoRequested() => messengerHub.Publish(new RedoCommandMessage(this));

        private void SetEnabled(Button button, bool enabled)
        {
            button.interactable = enabled;
            button.image.color = enabled ? Color.white : Color.gray;
        }
    }
}