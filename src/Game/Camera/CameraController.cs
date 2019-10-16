using System;
using Core.Command.Messages;
using Core.Logging;
using Core.Messages;
using Core.State;
using Core.TinyMessenger;
using DG.Tweening;
using Game.Camera.States;
using Game.Command;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game.Camera
{
    public class CameraController : IDisposable
    {
        private readonly Transform cameraPivot;
        private readonly UnityEngine.Camera camera;
        private readonly ITinyMessengerHub messengerHub;
        private readonly ILogger logger;
        private readonly StateContext context;
        private readonly Vector3 defaultPosition;
        private readonly Vector3 defaultRotation;
        private readonly TinyMessageSubscriptionToken spinSubscriptionToken;
        private readonly TinyMessageSubscriptionToken resetPositionSubscriptionToken;
        private readonly TinyMessageSubscriptionToken orbitSubscriptionToken;
        private TinyMessageSubscriptionToken enableDisableSubscriptionToken;
        
        public CameraController(UnityEngine.Camera camera, ITinyMessengerHub messengerHub, ILogger logger)
        {
            this.camera = camera;
            this.messengerHub = messengerHub;
            this.logger = PrefixedLogger.ForType<CameraController>(logger);

            var transform = camera.transform;
            
            cameraPivot = transform.parent;
            
            defaultPosition = transform.position;
            defaultRotation = transform.rotation.eulerAngles;
            
            context = new StateContext(this.logger);
            
            context.TransitionTo(
                new WaitingForDragState(context,
                                        messengerHub,
                                        this.logger,
                                        camera));
            
            context.Disable();

            spinSubscriptionToken = messengerHub.Subscribe<SpinCamera360Message>(OnSpinRequested);

            enableDisableSubscriptionToken = messengerHub.Subscribe<EnableCameraControlMessage>(EnableControl);

            resetPositionSubscriptionToken = messengerHub.Subscribe<ResetCameraMessage>(Reset);

            orbitSubscriptionToken = messengerHub.Subscribe<CameraOrbitRequestMessage>(Orbit);
        }
        public void Dispose()
        {
            context.Dispose();
            spinSubscriptionToken.Dispose();
            enableDisableSubscriptionToken.Dispose();
            resetPositionSubscriptionToken.Dispose();
            orbitSubscriptionToken.Dispose();
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
            
            enableDisableSubscriptionToken.Dispose();

            enableDisableSubscriptionToken = messengerHub.Subscribe<EnableCameraControlMessage>(EnableControl);
            
            logger.Info("Disabled camera control");
        }

        private void EnableControl(EnableCameraControlMessage message)
        {
            if (!context.IsDisabled)
                return;
            
            context.Enable();
            
            enableDisableSubscriptionToken.Dispose();

            enableDisableSubscriptionToken = messengerHub.Subscribe<DisableCameraControlMessage>(DisableControl);
            
            logger.Info("Enabled camera control");
        }

        private void Reset(ResetCameraMessage message)
        {
            logger.Info("Resetting camera position.");
            
            camera.transform.DOMove(defaultPosition, 3f)
                .SetEase(Ease.InOutQuad);
            
            camera.transform.DORotate(defaultRotation, 3f)
                .SetEase(Ease.InOutQuad);
        }

        private void Orbit(CameraOrbitRequestMessage message)
        {
            logger.Info("Commencing orbit of camera {0} times.", message.Times);

            cameraPivot.DORotate(endValue: Vector3.up * 360 * message.Times,
                                 duration: 4f * message.Times,
                                 mode: RotateMode.FastBeyond360)
                       .OnComplete(() => message.OnComplete?.Invoke());
        }
    }
}