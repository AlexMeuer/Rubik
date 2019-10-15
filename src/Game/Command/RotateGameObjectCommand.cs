using System.Collections;
using Core.Command;
using UnityEngine;

namespace Game.Command
{
    public class RotateGameObjectCommand : IInvokableCommand
    {
        private readonly Transform transform;
        private readonly Vector3 point;
        private readonly Vector3 axis;
        private readonly float angle;
        private readonly float waitTime;

        public RotateGameObjectCommand(GameObject gameObject, Vector3 point, Vector3 axis, float angle, float durationSeconds)
        {
            transform = gameObject.transform;
            
            this.point = point;
            this.axis = axis;
            this.angle = angle;

            waitTime = Mathf.Abs(durationSeconds / angle);
        }

        public IEnumerator Execute()
        {
            for (var i = 0; i < angle; ++i)
            {
                transform.RotateAround(point, axis, 1f);
                
                yield return new WaitForSeconds(waitTime);
            }
        }

        public IEnumerator Undo()
        {
            throw new System.NotImplementedException();
        }
    }
}