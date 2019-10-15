using Core.IoC;
using Core.Messages;
using UnityEngine;

namespace Game
{
    public partial class EntryPoint : MonoBehaviour
    {
        private GameObjectEnableDisableToggler<TurnLightsOnOffMessage> lightToggler;
        
        public  void Awake()
        {
            Initialise();
            
            // A quick and dirty way of turning the single directional light on and off.
            lightToggler = new GameObjectEnableDisableToggler<TurnLightsOnOffMessage>(messengerHub,
                                                                                      GameObject.FindWithTag("MainLight"),
                                                                                      (m) => m.TurnOn);
        }

        public void Update()
        {
            dragListener.Poll();
        }

        private void OnDestroy()
        {
            lightToggler?.Dispose();
            
            IoC.Dispose();
        }
    }
}