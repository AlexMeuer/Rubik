using System;
using Core.Extensions;
using UnityEngine;

namespace Game.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 Map(this Vector3 v, float oldMin, float oldMax, float newMin, float newMax)
        {
            return new Vector3(
                v.x.Map(oldMin, oldMax, newMin, newMax),
                v.y.Map(oldMin, oldMax, newMin, newMax),
                v.z.Map(oldMin, oldMax, newMin, newMax));
        }

        public static Vector3 Abs(this Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        public static Vector3Int ToIntegerAxis(this Vector3 v)
        {
            var abs = v.Abs();

            if (abs.x > abs.y && abs.x > abs.z)
                return new Vector3Int(Math.Sign(v.x), 0, 0);

            if (abs.y > abs.x && abs.y > abs.z)
                return new Vector3Int(0, Math.Sign(v.y), 0);

            return new Vector3Int(0, 0, Math.Sign(v.z));
        }
        
        public static Vector3Int Round(this Vector3 v)
        {
            return new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
        }

        public static Vector3Int Truncate(this Vector3 v)
        {
            return new Vector3Int((int)v.x, (int)v.y, (int)v.z);
        }
        
        public static Vector3Int Floor(this Vector3 v)
        {
            return new Vector3Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y), Mathf.FloorToInt(v.z));
        }

        public static Vector3Int ClampAll(this Vector3Int v, int min, int max)
        {
            return new Vector3Int(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max), Mathf.Clamp(v.z, min, max));
        }
    }
}