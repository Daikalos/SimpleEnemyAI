namespace FuSM
{
    public abstract class FuzzyState
    {
        public abstract float FuzzyValue();

        public abstract void Init(FuSM_Context context);
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
