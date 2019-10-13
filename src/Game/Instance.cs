using System;
using System.Collections.Generic;
using Core.IoC;
using UnityEngine;
using ILogger = Core.Logging.ILogger;
using Object = UnityEngine.Object;

namespace Game
{
    public abstract class Instance : IDisposable
    {
        public Vector3 Position
        {
            get => Self.transform.localPosition;
            set => Self.transform.localPosition = value;
        }
        
        protected GameObject Self { get; }

        protected Instance(string name, GameObject parent = null)
        {
            Self = new GameObject(name);
            
            if (parent != null)
                Self.transform.parent = parent.transform;
        }
        
        public void Dispose()
        {
            Object.Destroy(Self);
        }

        protected void SetAsChild(GameObject gameObject)
        {
            gameObject.transform.parent = Self.transform;
        }
        
        protected void SetAsChild(Instance instance) => SetAsChild(instance.Self);
    }
}