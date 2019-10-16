using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game
{
    public interface IInstance : IDisposable
    {
        Vector3 Position { get; }
        void RotateAbout(Vector3 axis, float angle);
        bool Is(GameObject other);
    }
    
    public abstract class Instance : IInstance
    {
        public static implicit operator GameObject(Instance instance)
        {
            return instance.Self;
        }
        
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
        
        protected Instance(string name, GameObject self, GameObject parent = null)
        {
            Self = self;

            self.name = name;
            
            if (parent != null)
                Self.transform.parent = parent.transform;
        }

        public void RotateAbout(Vector3 axis, float angle)
        {
            Self.transform.RotateAround(Vector3.zero, axis, angle);
        }

        public Vector3 TransformPoint(Vector3 point)
        {
            return Self.transform.TransformPoint(point);
        }

        public Vector3 TransformDirection(Vector3 direction)
        {
            return Self.transform.TransformDirection(direction);
        }

        protected Vector3 InverseTransformPoint(Vector3 point)
        {
            return Self.transform.InverseTransformPoint(point);
        }

        protected Vector3 InverseTransformDirection(Vector3 direction)
        {
            return Self.transform.InverseTransformDirection(direction);
        }

        public bool Is(GameObject other)
        {
            return Self == other;
        }

        public void Dispose()
        {
            OnDispose();
            
            Object.Destroy(Self);
        }

        protected virtual void OnDispose()
        {
        }
    }
}