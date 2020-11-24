namespace FuSM
{
    public abstract class FuzzyState
    {
        protected float m_ActivationLevel;
        protected void BoundsCheck()
        {
            if (m_ActivationLevel > 1.0f)
                m_ActivationLevel = 1.0f;
            if (m_ActivationLevel < 0.0f)
                m_ActivationLevel = 0.0f;
        }

        public abstract float ActivationLevel();

        public abstract void Init(FuSM_Context context);
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
