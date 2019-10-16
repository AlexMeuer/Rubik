using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using UnityEngine;

namespace Game.Cube
{
    public interface ICubeSolvedChecker
    {
        bool IsSolved(IRubiksCube cube);
    }

    public class CubeSolvedChecker : ICubeSolvedChecker
    {
        private static readonly Func<StickerData, FaceColor> SelectX = sd => sd.X;
        private static readonly Func<StickerData, FaceColor> SelectY = sd => sd.Y;
        private static readonly Func<StickerData, FaceColor> SelectZ = sd => sd.Z;

        public bool IsSolved(IRubiksCube cube)
        {
            var data = cube.GetStickerData().ToList();

            return DirectionIsAllOneColor(data, v => v.x == 1, SelectX) &&
                   DirectionIsAllOneColor(data, v => v.x == -1, SelectX) &&
                   DirectionIsAllOneColor(data, v => v.y == 1, SelectY) &&
                   DirectionIsAllOneColor(data, v => v.y == -1, SelectY) &&
                   DirectionIsAllOneColor(data, v => v.z == 1, SelectZ) &&
                   DirectionIsAllOneColor(data, v => v.z == -1, SelectZ);
        }

        private static bool DirectionIsAllOneColor(IEnumerable<StickerData> data, Func<Vector3Int, bool> axisFilter,
            Func<StickerData, FaceColor> selector)
        {
            return data.Where(sd => axisFilter(sd.Directions))
                       .Select(selector)
                       .Distinct()
                       .Count() == 1;
        }
    }
}