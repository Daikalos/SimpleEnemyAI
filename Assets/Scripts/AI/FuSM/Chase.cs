using UnityEngine;
using UnityEngine.AI;

namespace FuSM
{
    public class Chase : FuzzyState
    {
        private FuSM_Context m_Context;
        private NavMeshAgent m_Agent;
        private Rigidbody m_RB;

        public override float ActivationLevel()
        {
            if (m_Context.Target != null)
            {
                GameObject target = m_Context.Target;
                GameObject obj = m_Context.gameObject;

                float distToTarget = (target.transform.position - obj.transform.position).magnitude;
                float appRange = m_Context.ApproachRange;

                m_ActivationLevel = ((distToTarget - appRange) / (m_Context.ViewRange - appRange));

                BoundsCheck();

                return m_ActivationLevel;
            }

            return 0.0f;
        }

        public override void Init(FuSM_Context context)
        {
            m_Context = context;
            m_Agent = context.Agent;
            m_RB = context.RB;
        }

        public override void Enter()
        {
            m_Agent.isStopped = false;
        }

        public override void Update()
        {
            m_Agent.destination = m_Context.Target.transform.position;

            m_RB.velocity = m_Agent.velocity * m_ActivationLevel;
            m_Agent.nextPosition = m_Context.transform.position;
        }

        public override void Exit()
        {

        }
    }
}
