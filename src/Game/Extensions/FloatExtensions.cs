namespace Game.Extensions
{
    public static class FloatExtensions
    {
        public static float Map(this float oldVal, float oldMin, float oldMax, float newMin, float newMax)
        {
            return newMin + (newMax - newMin) * ((oldVal-oldMin) / (oldMax-oldMin));
        }
    }
}