namespace Core.Lighting
{
    public interface ILightLevelController
    {
        void TurnOn(float delay = 0f, float duration = 1f);
        void TurnOff(float delay = 0f, float duration = 1f);
    }
}