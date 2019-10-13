using System;
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