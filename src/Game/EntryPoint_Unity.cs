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