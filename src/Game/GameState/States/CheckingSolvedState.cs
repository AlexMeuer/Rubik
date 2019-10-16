using System;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;
using Core.State;
using Core.TinyMessenger;
using Domain;
using Game.Cube;
using Game.UI;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game.GameState.States
{
    public class CheckingSolvedState : CubeGameStateBase
    {
        private static readonly Func<StickerData, FaceColor> SelectX = sd => sd.X;
        private static readonly Func<StickerData, FaceColor> SelectY = sd => sd.Y;
        private static readonly Func<StickerData, FaceColor> SelectZ = sd => sd.Z;

        private readonly IScreen screen;

        public CheckingSolvedState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger,
            IRubiksCube rubiksCube, IScreen screen) : base(context, messengerHub, logger, rubiksCube)
        {
            this.screen = screen;
        }

        private bool IsSolved
        {
            get
            {
                var data = RubiksCube.GetStickerData().ToList();

                return DirectionIsAllOneColor(data, v => v.x == 1,  SelectX) &&
                       DirectionIsAllOneColor(data, v => v.x == -1, SelectX) &&
                       DirectionIsAllOneColor(data, v => v.y == 1,  SelectY) &&
                       DirectionIsAllOneColor(data, v => v.y == -1, SelectY) &&
                       DirectionIsAllOneColor(data, v => v.z == 1,  SelectZ) &&
                       DirectionIsAllOneColor(data, v => v.z == -1, SelectZ);
            }
        }

        protected override void OnEnter()
        {
            if (IsSolved)
            {
                screen.AnimateOut(screen.Dispose);
                
                Context.TransitionTo(new SolvedState(Context, MessengerHub, Logger, RubiksCube));
            }
            else
            {
                Context.TransitionTo(new PlayingState(Context, MessengerHub, Logger, RubiksCube, screen));
            }
        }

        protected override void OnExit()
        {
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