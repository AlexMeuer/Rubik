using System;
using Domain;
using UnityEngine;

namespace Game.Extensions
{
    public static class FaceColorExtensions
    {
        public static Color ToColor(this FaceColor fc)
        {
            switch (fc)
            {
                case FaceColor.White:
                    return Color.white;
                case FaceColor.Yellow:
                    return Color.yellow;
                case FaceColor.Orange:
                    return Color.magenta;
                case FaceColor.Red:
                    return Color.red;
                case FaceColor.Green:
                    return Color.green;
                case FaceColor.Blue:
                    return Color.blue;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fc), fc, null);
            }
        }
    }
}