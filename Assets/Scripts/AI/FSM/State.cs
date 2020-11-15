namespace FSM
{
    public abstract class State
    {
        public abstract void Init(FSM_Context context);
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
