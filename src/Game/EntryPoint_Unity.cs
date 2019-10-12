using Core.IoC;
using UnityEngine;

namespace Game
{
    public partial class EntryPoint : MonoBehaviour
    {
        public  void Awake()
        {
            Initialise();
        }

        private void Update()
        {
            
        }

        public void FixedUpdate()
        {
            
        }

        public void LateUpdate()
        {
            
        }

        private void OnDestroy()
        {
            IoC.Dispose();
        }
    }
}