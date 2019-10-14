using System;
using Core.IoC;
using Game.Cube.Stickers;
using Game.Logging;
using SimpleInjector;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game
{
    public class ContainerRegistrations : ContainerRegistrationsBase
    {
        public override void RegisterAll(Container container)
        {
            Singleton<ILogger, UnityConsoleLogger>(container);
            Transient<IStickerDataFactory, StickerDataFactory>(container);
            Transient<IStickerFactory, StickerFactory>(container);
            
            if (Application.isEditor)
                Singleton<IDragListener, MouseDragListener>(container);
            else
                throw new NotImplementedException();
            
            container.RegisterInstance(Camera.main);
        }
    }
}