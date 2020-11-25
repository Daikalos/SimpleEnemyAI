namespace FSM
{
    public abstract class State
    {
        public abstract void Init(FSM_AI context);
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
