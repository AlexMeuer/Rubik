using System.Collections.Generic;
using Core.Command.Messages;
using Core.Messages;
using Core.State;
using Core.TinyMessenger;
using Game.Command;
using Game.Cube;
using UnityEngine;
using ILogger = Core.Logging.ILogger;
using Random = UnityEngine.Random;

namespace Game.GameState.States
{
    public class ScramblingState : CubeGameStateBase
    {
        private delegate Slice SliceFinder(Vector3 position);
        
        private const int RandomMoveCountMin = 20;
        private const int RandomMoveCountMax = 30;

        private readonly float randomPointVariance;
        private readonly IReadOnlyList<SliceFinder> sliceFinders;
        
        private int remainingRandomMoveCount;
        private TinyMessageSubscriptionToken commandFinishedSubscriptionToken;

        public ScramblingState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger, IRubiksCube cube)
            : base(context, messengerHub, logger, cube)
        {
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
            
            MessengerHub.Publish(new TurnLightsOnOffMessage(this, turnOn: false));
            
            MessengerHub.Publish(new EnableCameraControlMessage(this));

            RotateRandomSlice();
        }

        protected override void OnExit()
        {
            MessengerHub.Unsubscribe<CommandCompleteMessage>(commandFinishedSubscriptionToken);
            
            MessengerHub.Publish(new TurnLightsOnOffMessage(this, turnOn: true));
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
                Context.TransitionTo(new PlayingState(Context, MessengerHub, Logger, RubiksCube));
            }
        }

        private void RotateRandomSlice()
        {
            var position = Random.rotationUniform * Vector3.right * Random.Range(-randomPointVariance, randomPointVariance);

            var axis = Random.Range(0, sliceFinders.Count - 1);

            var slice = sliceFinders[axis](position);
            
            var command = new RotateSliceCommand(slice, Random.value > 0.5f);
            
            MessengerHub.Publish(new EnqueueCommandMessage(this, command, transient: true));
        }
    }
}