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
            if (m_Context.Machine.IsActive(m_Context.FleeState)) // If currently fleeing, don't attempt chase
                return (m_ActivationLevel = 0.0f);

            return (m_ActivationLevel = (m_Context.IsTargetFound && !m_Context.IsWithinApproachRange) ? 1.0f : 0.0f);
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
            // Set current destination as Target to chase
            m_Agent.destination = m_Context.Target.transform.position;

            if (m_Agent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                m_RB.rotation = Quaternion.Slerp(m_RB.rotation,
                    Quaternion.LookRotation(m_Agent.velocity.normalized),
                    m_Context.RotationSpeed * Time.deltaTime);
            }

            m_RB.velocity = m_Agent.velocity;
            m_Agent.nextPosition = m_Context.transform.position;
        }

        public override void Exit()
        {

        }
    }
}
