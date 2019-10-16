using System.Collections.Generic;
using Core.Command;
using Core.Command.Messages;
using Core.IoC;
using Core.Lighting;
using Core.Messages;
using Core.State;
using Core.TinyMessenger;
using Game.Command;
using Game.Cube;
using Game.UI;
using UnityEngine;
using ILogger = Core.Logging.ILogger;
using Random = UnityEngine.Random;

namespace Game.GameState.States
{
    public class ScramblingState : CubeGameStateBase
    {
        private delegate Slice SliceFinder(Vector3 position);

        private const int RandomMoveCountMin = 3;
        private const int RandomMoveCountMax = 4;

        private readonly ILightLevelController lightLevelController;
        private readonly float randomPointVariance;
        private readonly IReadOnlyList<SliceFinder> sliceFinders;

        private int remainingRandomMoveCount;
        private TinyMessageSubscriptionToken commandFinishedSubscriptionToken;

        public ScramblingState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger, IRubiksCube cube,
            ILightLevelController lightLevelController) : base(context, messengerHub, logger, cube)
        {
            this.lightLevelController = lightLevelController;

            randomPointVariance = RubiksCube.PiecesPerRow * 0.5f;

            sliceFinders = new List<SliceFinder>(3)
            {
                RubiksCube.FindXAxisSlice,
                RubiksCube.FindYAxisSlice,
                RubiksCube.FindZAxisSlice,
            };
        }

        protected override void OnEnter()
        {
            remainingRandomMoveCount = Random.Range(RandomMoveCountMin, RandomMoveCountMax);

            commandFinishedSubscriptionToken = MessengerHub.Subscribe<CommandCompleteMessage>(OnCommandFinished);

            MessengerHub.Publish(new EnableCameraControlMessage(this));

            lightLevelController.TurnOff(delay: 1f);

            RotateRandomSlice();
        }

        protected override void OnExit()
        {
            commandFinishedSubscriptionToken.Dispose();

            lightLevelController.TurnOn();
        }

        private void OnCommandFinished(CommandCompleteMessage message)
        {
            if (message.Command is RotateSliceCommand)
            {
                OnRotateSliceFinished();
            }
        }

        private void OnRotateSliceFinished()
        {
            if (remainingRandomMoveCount-- > 0)
            {
                RotateRandomSlice();
            }
            else
            {
                var screen = new InGameScreen(MessengerHub);
                
                screen.Build();
                
                screen.AnimateIn();
                
                Context.TransitionTo(new PlayingState(Context, MessengerHub, Logger, RubiksCube, screen));
            }
        }

        private void RotateRandomSlice()
        {
            var position = Random.rotationUniform * Vector3.right *
                           Random.Range(-randomPointVariance, randomPointVariance);

            var axis = Random.Range(0, sliceFinders.Count - 1);

            var slice = sliceFinders[axis](position);

            var command = new RotateSliceCommand(slice, Random.value > 0.5f);

            MessengerHub.Publish(new EnqueueCommandMessage(this, command, transient: true));
        }
    }
}