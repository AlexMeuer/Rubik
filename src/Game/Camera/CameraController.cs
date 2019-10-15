using System;
using Core.Logging;
using Core.TinyMessenger;
using Game.Camera.State;

namespace Game.Camera
{
    public class CameraController : IDisposable
    {
        private readonly StateContext context;
        
        public CameraController(UnityEngine.Camera camera, ITinyMessengerHub messengerHub, ILogger logger)
        {
            var prefixedLogger = PrefixedLogger.ForType<CameraController>(logger);
            
            context = new StateContext(prefixedLogger);
            
            context.TransitionTo(
                new WaitingForDragState(context,
                                        messengerHub,
                                        camera, 
                                        prefixedLogger));
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}