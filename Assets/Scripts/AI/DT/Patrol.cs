using UnityEngine;
using UnityEngine.AI;

namespace DT
{
    public class Patrol : Action
    {
        private readonly NavMeshAgent m_Agent;

        public Patrol(DecisionTree_AI context) : base(context)
        {
            m_Agent = context.Agent;
        }

        public override bool Evaluate()
        {
            Wander();
            SearchForTarget();

            return true;
        }

        private void Wander()
        {
            if (m_Agent.isStopped)
            {
                m_Agent.isStopped = false;
                m_Agent.ResetPath();
            }

            if (m_Agent.remainingDistance > float.Epsilon)
                return;

            m_Agent.destination = RandomPoint(Vector3.zero, 25.0f, -1);
        }

        private Vector3 RandomPoint(Vector3 origin, float distance, int layermask)
        {
            // Returns a random point on navigation mesh

            Vector3 randDirection = (Random.insideUnitSphere * distance) + origin;
            NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, distance, layermask);

            return navHit.position;
        }

        private void SearchForTarget()
        {
            GameObject closestTarget = Context.ClosestTarget();

            if (closestTarget == null)
                return;

            Context.SetTarget(closestTarget);
        }
    }
}
