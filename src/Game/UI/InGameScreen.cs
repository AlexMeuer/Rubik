using System;
using Core.Command.Messages;
using Core.Extensions;
using Core.Timer;
using Core.TinyMessenger;
using DG.Tweening;
using Game.Messages;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public class InGameScreen : ScreenBase
    {
        private const float AnimDurationSeconds = 1f;

        private readonly ITinyMessengerHub messengerHub;
        private readonly ITimer timer;

        private Rect buttonRect;
        private GameObject undoButton;
        private GameObject redoButton;
        private GameObject timerText;
        private GameObject optionsButton;
        private TinyMessageSubscriptionToken cmdCompletedSubscriptionToken;

        private bool canUndo;
        private bool canRedo;

        public bool TimerIsVisible
        {
            get => timerText.activeInHierarchy;
            set => timerText.SetActive(value);
        }

        public InGameScreen(ITinyMessengerHub messengerHub, ITimer timer)
        {
            this.messengerHub = messengerHub;
            this.timer = timer;
        }

        public override void Build()
        {
            var textPrefab = Resources.Load<GameObject>("Prefabs/TitleText");
            var undoButtonPrefab = Resources.Load<GameObject>("Prefabs/UndoButton");
            var menuButtonPrefab = Resources.Load<GameObject>("Prefabs/OptionsButton");

            timerText = Object.Instantiate(textPrefab, CanvasObject.transform);
            timerText.name = "Timer";
            
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
            
            optionsButton = Object.Instantiate(menuButtonPrefab, CanvasObject.transform);
            optionsButton.name = "OptionsButton";
            
            undoButton.GetComponent<Button>().onClick.AddListener(UndoRequested);
            redoButton.GetComponent<Button>().onClick.AddListener(RedoRequested);
            optionsButton.GetComponent<Button>().onClick.AddListener(OptionsRequested);

            cmdCompletedSubscriptionToken = messengerHub.Subscribe<CommandCompleteMessage>(OnCommandCompleted);
            
            timer.AddListener(OnTimerUpdate);
        }

        public override void AnimateIn(Action onComplete = null)
        {
            timerText.SetActive(true);
            
            undoButton.transform.DOLocalMoveX(CanvasRect.width + buttonRect.width, AnimDurationSeconds)
                .From()
                .SetEase(Ease.OutBounce)
                .OnComplete(() => onComplete?.Invoke());
            
            redoButton.transform.DOLocalMoveX(CanvasRect.width + buttonRect.width, AnimDurationSeconds)
                .From()
                .SetEase(Ease.OutBounce);

            optionsButton.transform.DOLocalMoveX(-CanvasRect.width - buttonRect.width, AnimDurationSeconds)
                .From()
                .SetEase(Ease.OutBounce);
        }

        public override void AnimateOut(Action onComplete = null)
        {
            timerText.SetActive(false);
            
            undoButton.transform.DOLocalMoveX(CanvasRect.width + buttonRect.width, AnimDurationSeconds)
                .SetEase(Ease.InQuad)
                .OnComplete(() => onComplete?.Invoke());
            
            redoButton.transform.DOLocalMoveX(CanvasRect.width + buttonRect.width, AnimDurationSeconds)
                .SetEase(Ease.InQuad);

            optionsButton.transform.DOLocalMoveX(-CanvasRect.width - buttonRect.width, AnimDurationSeconds)
                .SetEase(Ease.InQuad);
        }

        public override void Dispose()
        {
            timer.RemoveListener(OnTimerUpdate);
            cmdCompletedSubscriptionToken.Dispose();
            Object.Destroy(timerText);
            Object.Destroy(optionsButton);
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

        private void OptionsRequested() => messengerHub.Publish(new OptionsRequestedMessage(this));

        private void OnTimerUpdate(TimeSpan elapsed)
        {
            timerText.GetComponent<Text>().text = "{0:00}:{1:00}".Format((int)elapsed.TotalMinutes, elapsed.Seconds);
        }

        private static void SetEnabled(Selectable button, bool enabled)
        {
            button.interactable = enabled;
            button.image.color = enabled ? Color.white : Color.gray;
        }
    }
}