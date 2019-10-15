using System;
using System.Collections.Generic;
using System.Linq;
using Core.State;
using Core.TinyMessenger;
using Domain;
using Game.Cube;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game.GameState.States
{
    public class CheckingSolvedState : CubeGameStateBase
    {
        private static readonly Func<StickerData, FaceColor> SelectX = sd => sd.X;
        private static readonly Func<StickerData, FaceColor> SelectY = sd => sd.Y;
        private static readonly Func<StickerData, FaceColor> SelectZ = sd => sd.Z;

        public CheckingSolvedState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger,
            IRubiksCube rubiksCube) : base(context, messengerHub, logger, rubiksCube)
        {
        }

        private bool IsSolved
        {
            get
            {
                var data = RubiksCube.GetStickerData().ToList();

                var forward = new Vector3Int(0, 0, 1);
                var backward = new Vector3Int(0, 0, -1);

                return DirectionIsAllOneColor(data, Vector3Int.right, SelectX) &&
                       DirectionIsAllOneColor(data, Vector3Int.left, SelectX) &&
                       DirectionIsAllOneColor(data, Vector3Int.up, SelectY) &&
                       DirectionIsAllOneColor(data, Vector3Int.down, SelectY) &&
                       DirectionIsAllOneColor(data, forward, SelectZ) &&
                       DirectionIsAllOneColor(data, backward, SelectZ);
            }
        }

        protected override void OnEnter()
        {
            if (IsSolved)
            {
                Context.TransitionTo(new SolvedState(Context, MessengerHub, Logger, RubiksCube));
            }
            else
            {
                Context.TransitionTo(new PlayingState(Context, MessengerHub, Logger, RubiksCube));
            }
        }

        protected override void OnExit()
        {
        }

        private static bool DirectionIsAllOneColor(IEnumerable<StickerData> data, Vector3Int direction,
            Func<StickerData, FaceColor> selector)
        {
            return data.Where(sd => sd.Directions == direction)
                       .Select(selector)
                       .Distinct()
                       .Count() == 1;
        }
    }
}