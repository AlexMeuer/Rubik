using Core.Lighting;
using DG.Tweening;
using UnityEngine;

namespace Game.Lighting
{
    public class UnityLightLevelController : ILightLevelController
    {
        public void TurnOn(float delay = 0f, float duration = 1F) =>
            ChangeIntensityOfAllLightsTo(1f, delay, duration);

        public void TurnOff(float delay = 0f, float duration = 1f) =>
            ChangeIntensityOfAllLightsTo(0f, delay, duration);

        private void ChangeIntensityOfAllLightsTo(float intensity, float delay, float durationSeconds)
        {
            foreach (var light in Object.FindObjectsOfType<Light>())
            {
                light.DOIntensity(intensity, durationSeconds)
                    .SetEase(Ease.OutQuad)
                    .SetDelay(delay);
            }
        }
    }
}