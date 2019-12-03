
namespace AI.StateMachine
{
    public struct State
    {
        public delegate void SubState(StateMachine stateMachine);
        public SubState OnEnter;
        public SubState Entering;

        public SubState OnActive;
        public SubState Active;

        public SubState OnExit;
        public SubState Exiting;
    }
}