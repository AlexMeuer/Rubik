using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public class MainMenuScreen : ScreenBase
    {
        private const float animDurationSeconds = 1f;
            
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
            startButton.transform.localPosition = new Vector3(0f, -40f, 0f);
            startButton.GetComponent<Button>().onClick.AddListener(onStartPressed);
            
            quitButton = Object.Instantiate(textButton, CanvasObject.transform);
            quitButton.name = "QuitButton";
            quitButton.GetComponentInChildren<Text>().text = "quit";
            quitButton.transform.localPosition = new Vector3(0f, -100f, 0f);
            quitButton.GetComponent<Button>().onClick.AddListener(onQuitPressed);

            cubeSizeSlider = Object.Instantiate(intSlider, CanvasObject.transform);
            cubeSizeSlider.name = "CubeSizeSlider";
            cubeSizeSlider.transform.localPosition = new Vector3(0f, 0f, 0f);
        }

        public override void AnimateIn(Action onComplete = null)
        {
            title.transform.DOScale(Vector3.zero, animDurationSeconds)
                .From()
                .SetEase(Ease.OutBounce);
            
            startButton.transform.DOScale(Vector3.zero, animDurationSeconds)
                .From()
                .SetEase(Ease.OutBounce);

            quitButton.transform.DOScale(Vector3.zero, animDurationSeconds)
                .From()
                .SetEase(Ease.OutBounce)
                .SetDelay(0.2f);
            
            cubeSizeSlider.transform.DOScale(Vector3.zero, animDurationSeconds)
                .From()
                .SetEase(Ease.OutBounce)
                .SetDelay(0.4f)
                .OnComplete(() => onComplete?.Invoke());
        }

        public override void AnimateOut(Action onComplete = null)
        {
            title.transform.DOScale(Vector3.zero, animDurationSeconds);

            startButton.transform.DOScale(Vector3.zero, animDurationSeconds);

            quitButton.transform.DOScale(Vector3.zero, animDurationSeconds);
            
            cubeSizeSlider.transform.DOScale(Vector3.zero, animDurationSeconds)
                .OnComplete(() => onComplete?.Invoke());;
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