using System.Collections.Generic;

namespace FSM
{
    public class FSM_Machine
    {
        private State m_CurrentState = null;

        private readonly List<State> States;

        public FSM_Machine()
        {
            States = new List<State>();
        }

        public void Update()
        {
            m_CurrentState?.Update();
        }

        public void InitializeStates(FSM_AI context)
        {
            States.ForEach(s => s.Init(context));
        }

        public void AddState(State state)
        {
            States.Add(state);
        }

        public bool TransitionTo(State state)
        {
            if (state == m_CurrentState || state == null)
                return false;

            m_CurrentState?.Exit();
            m_CurrentState = state;
            m_CurrentState.Enter();

            return true;
        }
    }
}
