using Core.IoC;
using UnityEngine;

namespace Game
{
    public partial class EntryPoint : MonoBehaviour
    {
        private IDragListener dragListener;
        
        public  void Awake()
        {
            Initialise();
            
            dragListener = IoC.Resolve<IDragListener>();
        }

        public void Update()
        {
            dragListener.Poll();
        }

        private void OnDestroy()
        {
            IoC.Dispose();
        }
    }
}