using System;
using Core.Command.Messages;
using Core.Logging;
using Core.Messages;
using Core.State;
using Core.TinyMessenger;
using Game.Camera.States;
using Game.Command;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game.Camera
{
    public class CameraController : IDisposable
    {
        private readonly UnityEngine.Camera camera;
        private readonly ITinyMessengerHub messengerHub;
        private readonly ILogger logger;
        private readonly StateContext context;
        private readonly TinyMessageSubscriptionToken spinSubscriptionToken;
        private TinyMessageSubscriptionToken enableDisableSubscriptionToken;
        
        public CameraController(UnityEngine.Camera camera, ITinyMessengerHub messengerHub, ILogger logger)
        {
            this.camera = camera;
            this.messengerHub = messengerHub;
            this.logger = PrefixedLogger.ForType<CameraController>(logger);
            
            context = new StateContext(this.logger);
            
            context.TransitionTo(
                new WaitingForDragState(context,
                                        messengerHub,
                                        this.logger,
                                        camera));
            
            context.Disable();

            spinSubscriptionToken = messengerHub.Subscribe<SpinCamera360Message>(OnSpinRequested);

            enableDisableSubscriptionToken = messengerHub.Subscribe<EnableCameraControlMessage>(EnableControl);
        }
        public void Dispose()
        {
            context.Dispose();
            spinSubscriptionToken.Dispose();
        }

        private void OnSpinRequested(SpinCamera360Message message)
        {
            var cmd = new RotateGameObjectCommand(gameObject: camera.gameObject,
                                                  point: Vector3.zero, 
                                                  axis: Vector3.up, 
                                                  angle: 360f * message.NumberOfSpins, 
                                                  durationSeconds: message.DurationSeconds);
            
            messengerHub.Publish(new EnqueueCommandMessage(this, cmd, transient: true));
        }


        private void DisableControl(DisableCameraControlMessage message)
        {
            if (context.IsDisabled)
                return;
            
            context.Disable();
            
            messengerHub.Unsubscribe<DisableCameraControlMessage>(enableDisableSubscriptionToken);

            enableDisableSubscriptionToken = messengerHub.Subscribe<EnableCameraControlMessage>(EnableControl);
            
            logger.Info("Disabled camera control");
        }

        private void EnableControl(EnableCameraControlMessage message)
        {
            if (!context.IsDisabled)
                return;
            
            context.Enable();
            
            messengerHub.Unsubscribe<DisableCameraControlMessage>(enableDisableSubscriptionToken);

            enableDisableSubscriptionToken = messengerHub.Subscribe<DisableCameraControlMessage>(DisableControl);
            
            logger.Info("Enabled camera control");
        }
    }
}