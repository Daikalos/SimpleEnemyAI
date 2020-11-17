using UnityEngine;
using UnityEngine.AI;

namespace FuSM
{
    public class Chase : FuzzyState
    {
        private FuSM_Context m_Context;
        private NavMeshAgent m_Agent;

        public override float FuzzyValue()
        {
            if (m_Context.Target != null)
            {
                GameObject target = m_Context.Target;
                GameObject obj = m_Context.gameObject;

                float distToTarget = (target.transform.position - obj.transform.position).magnitude;

                if ((distToTarget - m_Context.AttackRange) < 0.0f)
                    return 0.0f;

                return ((distToTarget - m_Context.AttackRange) / m_Context.ViewRange);
            }

            return 0.0f;
        }

        public override void Init(FuSM_Context context)
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
            m_Agent.SetDestination(m_Context.Target.transform.position);
        }

        public override void Exit()
        {

        }
    }
}
