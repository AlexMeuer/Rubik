using Core.IoC;
using Core.Messages;
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
            messengerHub.Publish(new UpdateMessage(this, Time.deltaTime));
        }

        private void OnDestroy()
        {
            IoC.Dispose();
        }
    }
}