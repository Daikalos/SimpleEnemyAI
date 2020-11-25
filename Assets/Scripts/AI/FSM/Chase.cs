using UnityEngine.AI;

namespace FSM
{
    public class Chase : State
    {
        private FSM_AI m_Context;
        private NavMeshAgent m_Agent;

        public override void Init(FSM_AI context)
        {
            m_Context = context;
            m_Agent = context.Agent;
        }

        public override void Enter()
        {
            m_Agent.isStopped = false;
        }

        public override void Update()
        {
            if (!m_Context.IsTargetFound)
            {
                if (m_Context.TransitionTo(m_Context.PatrolState)) 
                    return;
            }
            else if (m_Context.IsWithinApproachRange)
            {
                if (m_Context.TransitionTo(m_Context.AttackState)) 
                    return;
            }

            m_Agent.destination = m_Context.Target.transform.position;
        }

        public override void Exit()
        {

        }
    }
}
