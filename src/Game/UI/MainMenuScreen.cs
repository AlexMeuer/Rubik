using System;
using Core.State;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public class MainMenuScreen : ScreenBase
    {
        private const float AnimDurationSeconds = 1f;
            
        private readonly UnityAction onStartPressed;
        private readonly UnityAction onQuitPressed;

        private GameObject title;
        private GameObject startButton;
        private GameObject quitButton;
        private GameObject cubeSizeSlider;

        public int SelectedCubeSize => (int) cubeSizeSlider.GetComponent<Slider>().value;

        public MainMenuScreen(UnityAction onStartPressed, UnityAction onQuitPressed)
        {
            this.onStartPressed = onStartPressed;
            this.onQuitPressed = onQuitPressed;
        }
        
        public override void Build()
        {
            var titleText = Resources.Load<GameObject>("Prefabs/TitleText");
            var textButton = Resources.Load<GameObject>("Prefabs/TextButton");
            var intSlider = Resources.Load<GameObject>("Prefabs/IntSlider");

            title = Object.Instantiate(titleText, CanvasObject.transform);
            title.name = "Title";
            title.GetComponent<Text>().text = "Rubik's Cube";

            startButton = Object.Instantiate(textButton, CanvasObject.transform);
            startButton.name = "StartButton";
            startButton.GetComponentInChildren<Text>().text = "start";
            startButton.transform.localPosition = new Vector3(0f, -60f, 0f);
            startButton.GetComponent<Button>().onClick.AddListener(onStartPressed);
            
            quitButton = Object.Instantiate(textButton, CanvasObject.transform);
            quitButton.name = "QuitButton";
            quitButton.GetComponentInChildren<Text>().text = "quit";
            quitButton.transform.localPosition = new Vector3(0f, -140f, 0f);
            quitButton.GetComponent<Button>().onClick.AddListener(onQuitPressed);

            cubeSizeSlider = Object.Instantiate(intSlider, CanvasObject.transform);
            cubeSizeSlider.name = "CubeSizeSlider";
            cubeSizeSlider.transform.localPosition = new Vector3(0f, 0f, 0f);
        }

        public override void AnimateIn(Action onComplete = null)
        {
            title.transform.DOScaleY(0f, AnimDurationSeconds)
                .From()
                .SetEase(Ease.OutQuad);

            var buttonStartX = -CanvasRect.width * 0.5f - startButton.GetComponent<RectTransform>().rect.width * 0.5f;
            
            startButton.transform.DOLocalMoveX(buttonStartX, AnimDurationSeconds)
                .From()
                .SetDelay(0.6f)
                .SetEase(Ease.OutQuad);

            quitButton.transform.DOLocalMoveX(buttonStartX, AnimDurationSeconds)
                .From()
                .SetEase(Ease.OutQuad)
                .SetDelay(0.9f)
                .OnComplete(() => onComplete?.Invoke());
            
            cubeSizeSlider.transform.DOLocalMoveX(buttonStartX, AnimDurationSeconds)
                .From()
                .SetEase(Ease.OutQuad)
                .SetDelay(0.3f);
        }

        public override void AnimateOut(Action onComplete = null)
        {
            title.transform.DOScaleY(0f, AnimDurationSeconds)
                .SetEase(Ease.InQuad);

            var buttonEndX = CanvasRect.width * 0.5f + startButton.GetComponent<RectTransform>().rect.width * 0.5f;
            
            startButton.transform.DOLocalMoveX(buttonEndX, AnimDurationSeconds)
                .SetDelay(0.3f)
                .SetEase(Ease.InQuad);

            quitButton.transform.DOLocalMoveX(buttonEndX, AnimDurationSeconds)
                .SetEase(Ease.InQuad)
                .SetDelay(0.6f)
                .OnComplete(() => onComplete?.Invoke());
            
            cubeSizeSlider.transform.DOLocalMoveX(buttonEndX, AnimDurationSeconds)
                .SetEase(Ease.InQuad);
        }

        public override void Dispose()
        {
            Object.Destroy(title);
            Object.Destroy(cubeSizeSlider);
            Object.Destroy(quitButton);
            Object.Destroy(startButton);
        }
    }
}