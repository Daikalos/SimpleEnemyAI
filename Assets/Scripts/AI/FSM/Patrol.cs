using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FSM
{
    public class Patrol : State
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
            m_Agent.destination = Enemy.RandomPoint(Vector3.zero, 25.0f, -1);
        }

        public override void Update() // Whilst patrolling, wander and search
        {
            if (m_Context.Target != null)
            {
                if (m_Context.TransitionTo(m_Context.ChaseState))
                    return;
            }

            Wander();
            SearchForTarget(); // If target found
        }

        public override void Exit()
        {

        }

        private void Wander()
        {
            if (m_Agent.remainingDistance > 0.1f)
                return;

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
