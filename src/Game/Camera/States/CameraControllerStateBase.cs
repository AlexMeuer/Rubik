using Core.Logging;
using Core.State;
using Core.TinyMessenger;

namespace Game.Camera.States
{
    public abstract class CameraControllerStateBase : StateBase
    {
        protected readonly UnityEngine.Camera Camera;

        protected CameraControllerStateBase(StateContext context, ITinyMessengerHub messengerHub, ILogger logger, UnityEngine.Camera camera)
            : base(context, messengerHub, logger)
        {
            Camera = camera;
        }
    }
}