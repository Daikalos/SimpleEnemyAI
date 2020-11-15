using UnityEngine;
using UnityEngine.AI;

namespace FSM
{
    // Patrol as idle state until another enemy is found

    public class Patrol : State
    {
        private FSM_Context m_Context;
        private NavMeshAgent m_Agent;

        public override void Init(FSM_Context context)
        {
            m_Context = context;
            m_Agent = context.Agent;
        }

        public override void Enter()
        {
            m_Agent.isStopped = false;
            m_Agent.SetDestination(FSM_Context.RandomPoint(Vector3.zero, 25.0f, -1));
        }

        public override void Update() // Whilst patrolling, wander and search
        {
            Wander();

            if (SearchForTarget()) // If target found
                return;
        }

        public override void Exit()
        {
            m_Agent.isStopped = true;
        }

        private void Wander()
        {
            if (m_Agent.remainingDistance > float.Epsilon)
                return;

            m_Agent.SetDestination(FSM_Context.RandomPoint(Vector3.zero, 25.0f, -1));
        }

        private bool SearchForTarget()
        {
            GameObject closestTarget = ClosestTarget();

            if (closestTarget == null)
                return false;

            m_Context.SetTarget(closestTarget);      
            m_Context.TransitionTo(m_Context.ChaseState);

            return true;
        }

        private GameObject ClosestTarget()
        {
            GameObject result = null;
            float minDistance = float.MaxValue;

            foreach (GameObject target in m_Context.VisibleTargets())
            {
                float distance = (target.transform.position - m_Context.transform.position).magnitude;

                if (distance < minDistance)
                {
                    result = target;
                    minDistance = distance;
                }
            }

            return result;
        }
    }
}
