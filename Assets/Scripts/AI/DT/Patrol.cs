using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DT
{
    public class Patrol : Action
    {
        private readonly NavMeshAgent m_Agent;

        public Patrol(DT_AI context) : base(context)
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

            m_Agent.SetDestination(RandomPoint(Vector3.zero, 25.0f, -1));
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
            GameObject closestTarget = ClosestTarget();

            if (closestTarget == null)
                return;

            Context.SetTarget(closestTarget);
        }

        private List<GameObject> VisibleTargets()
        {
            List<GameObject> visibleTargets = new List<GameObject>(); // Filter all visible targets based on view range and view angle
            foreach (GameObject target in Context.Targets)
            {
                if (!Context.IsTargetVisible(target))
                    continue;

                visibleTargets.Add(target);
            }

            return visibleTargets;
        }

        private GameObject ClosestTarget()
        {
            GameObject result = null;
            float minDistance = float.MaxValue;

            foreach (GameObject target in VisibleTargets())
            {
                float distance = (target.transform.position - Context.transform.position).magnitude;

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
