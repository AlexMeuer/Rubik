using Core.IoC;
using Game.Cube.Factory;
using Game.UI;

namespace Game.GameState.States
{
    public partial class PlayingState : IOptionsScreenCallbacks
    {
        private const string keyTimerVisible = "TimerVisible";
        
        private IScreen optionsScreen;
        
        public void OnChangeTimerVisibilityRequested(bool visible)
        {
            inGameScreen.TimerIsVisible = visible;
            
            Context.Store.SetBool(keyTimerVisible, visible);
        }

        public void OnContinueRequested()
        {
            CloseOptionsScreen();
        }

        public void OnQuitRequested()
        {
            CloseOptionsScreen();

            RubiksCube.AnimateOut(() =>
            {
                RubiksCube.Dispose();
                
                Context.TransitionTo(new MainMenuState(Context, MessengerHub, Logger,
                    IoC.Resolve<IRubiksCubeFactory>()));
            });
        }
    }
}