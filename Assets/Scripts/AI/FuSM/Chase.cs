using UnityEngine;
using UnityEngine.AI;

namespace FuSM
{
    public class Chase : FuzzyState
    {
        private FuSM_AI m_Context;
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

                float healthState = m_Context.FleeState.ActivationLevel();

                m_ActivationLevel = ((distToTarget > appRange) ? 1.0f : 0.0f) - healthState;

                BoundsCheck();

                return m_ActivationLevel;
            }

            return 0.0f;
        }

        public override void Init(FuSM_AI context)
        {
            m_Context = context;
            m_Agent = context.Agent;
            m_RB = context.RB;
        }

        public override void Enter()
        {

        }

        public override void Update()
        {
            m_Agent.destination = m_Context.Target.transform.position;

            if (m_Agent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                m_RB.rotation = Quaternion.Slerp(m_RB.rotation,
                    Quaternion.LookRotation(m_Agent.velocity.normalized),
                    m_Context.RotationSpeed * m_ActivationLevel * Time.deltaTime);
            }

            m_RB.velocity += m_Agent.velocity * m_ActivationLevel;
            m_Agent.nextPosition = m_Context.transform.position;
        }

        public override void Exit()
        {

        }
    }
}
