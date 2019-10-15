using Core.State;

namespace Game.Camera.States
{
    public class DisabledState : IState
    {
        public bool IsActive {get; private set; }
        
        public void Enter()
        {
            IsActive = true;
        }

        public void Exit()
        {
            IsActive = false;
        }
    }
}