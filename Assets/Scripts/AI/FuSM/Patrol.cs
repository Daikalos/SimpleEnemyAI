using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FuSM
{
    public class Patrol : FuzzyState
    {
        private FuSM_AI m_Context;
        private NavMeshAgent m_Agent;
        private Rigidbody m_RB;

        public override float ActivationLevel()
        {
            return m_ActivationLevel = (!m_Context.IsTargetFound) ? 1.0f : 0.0f;
        }

        public override void Init(FuSM_AI context)
        {
            m_Context = context;
            m_Agent = context.Agent;
            m_RB = context.RB;
        }

        public override void Enter()
        {
            m_Agent.isStopped = false; // If agent is stopped, set to false so it can move
            m_Agent.destination = Enemy.RandomPoint(Vector3.zero, 25.0f, -1); // Set destination as random point on mesh
        }

        public override void Update()
        {
            UpdateAgent();

            Wander();
            SearchForTarget();
        }

        private void UpdateAgent()
        {
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

        private void Wander()
        {
            if (m_Agent.remainingDistance > 0.1f)
                return;

            // Set a new destination on mesh when current destination is reached
            m_Agent.destination = Enemy.RandomPoint(Vector3.zero, 25.0f, -1);
        }

        private void SearchForTarget()
        {
            GameObject closestTarget = m_Context.ClosestTarget();

            if (closestTarget == null)
                return;

            m_Context.SetTarget(closestTarget);
        }
    }
}
