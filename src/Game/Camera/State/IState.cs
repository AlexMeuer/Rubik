using Core.Logging;
using Core.TinyMessenger;

namespace Game.Camera.State
{
    public interface IState
    {
        void Enter();
        void Exit();
    }

    public abstract class CameraControllerStateBase : IState
    {
        protected readonly StateContext Context;
        protected readonly ITinyMessengerHub MessengerHub;
        protected readonly UnityEngine.Camera Camera;
        protected readonly ILogger Logger;

        protected CameraControllerStateBase(StateContext context, ITinyMessengerHub messengerHub, UnityEngine.Camera camera, ILogger logger)
        {
            Context = context;
            MessengerHub = messengerHub;
            Camera = camera;
            Logger = logger;
        }
        
        public abstract void Enter();
        public abstract void Exit();
    }
}