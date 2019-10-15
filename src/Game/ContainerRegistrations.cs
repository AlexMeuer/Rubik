using System;
using Core.Command;
using Core.IoC;
using Game.Camera;
using Game.Cube.Stickers;
using Game.Logging;
using SimpleInjector;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game
{
    public class ContainerRegistrations : ContainerRegistrationsBase
    {
        private readonly ICommandExecutor commandExecutor;

        public ContainerRegistrations(ICommandExecutor commandExecutor)
        {
            this.commandExecutor = commandExecutor;
        }

        public override void RegisterAll(Container container)
        {
            Singleton<ILogger, UnityConsoleLogger>(container);
            Singleton<CameraController, CameraController>(container);
            Singleton<IDragListener, MouseDragListener>(container);
            
            Transient<IStickerDataFactory, StickerDataFactory>(container);
            Transient<IStickerFactory, StickerFactory>(container);
            
            container.RegisterInstance(commandExecutor);
            
            container.RegisterInstance(UnityEngine.Camera.main);
        }
    }
}