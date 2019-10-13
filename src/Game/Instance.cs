using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game
{
    public interface IInstance : IDisposable
    {
        Vector3 Position { get; set; }
        void RotateAbout(Vector3 axis, float angle);
    }
    
    public abstract class Instance : IInstance
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

        public void RotateAbout(Vector3 axis, float angle)
        {
            Self.transform.RotateAround(Vector3.zero, axis, angle);
        }
        
        public void Dispose()
        {
            OnDispose();
            
            Object.Destroy(Self);
        }

        protected virtual void OnDispose()
        {
        }

        protected void SetAsChild(GameObject gameObject)
        {
            gameObject.transform.parent = Self.transform;
        }
        
        protected void SetAsChild(Instance instance) => SetAsChild(instance.Self);
    }
}