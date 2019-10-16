using System;

namespace Core.Extensions
{
    public static class FloatExtensions
    {
        public static float Map(this float oldVal, float oldMin, float oldMax, float newMin, float newMax)
        {
            return newMin + (newMax - newMin) * ((oldVal-oldMin) / (oldMax-oldMin));
        }

        public static bool NearlyEqual(this float a, float b, float tolerance = 0.000001f)
        {
            return a.Equals(b) || Math.Abs(a - b) < tolerance;
        }
    }
}