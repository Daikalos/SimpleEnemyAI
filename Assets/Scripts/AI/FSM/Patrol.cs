using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Finite_SM
{
    // Patrol as idle state until another enemy is found

    public class Patrol : State
    {
        private Vector3 m_Destination = Vector3.zero;

        public override void Enter(FSM_Context context)
        {
            // Empty
        }

        public override void Update(FSM_Context context) // Whilst patrolling, wander and search
        {
            Wander(context);
            SearchForTargets(context);
        }

        public override void Exit(FSM_Context context)
        {
            context.Agent.isStopped = true;
        }

        /// <summary>
        /// Wander randomly around on map
        /// </summary>
        private void Wander(FSM_Context context)
        {
            if (context.Agent.remainingDistance > float.Epsilon)
                return;

            m_Destination = context.RandomPoint(Vector3.zero, 25.0f, -1);
            context.Agent.SetDestination(m_Destination);
        }

        private void SearchForTargets(FSM_Context context)
        {
            Vector3 position = context.transform.position;

            List<GameObject> visibleTargets = new List<GameObject>(); // Keep a list to store all enemies that is visible

            foreach (GameObject obj in context.Targets) // Get all enemies within sight (not behind walls)
            {
                Vector3 direction = obj.transform.position - position;
                Physics.Raycast(position, direction, out RaycastHit hit);

                if (hit.collider == null || !hit.collider.gameObject.CompareTag("Enemy"))
                    continue;

                visibleTargets.Add(obj);
            }

            if (visibleTargets.Count == 0)
                return;

            List<GameObject> filterTargets = new List<GameObject>(); // Filter all visible targets based on range and angle
            foreach (GameObject target in visibleTargets)
            {
                if (!WithinRange(context, target))
                    continue;

                if (!WithinAngle(context, target))
                    continue;

                filterTargets.Add(target);
            }

            if (filterTargets.Count == 0)
                return;

            GameObject closestTarget = ClosestTarget(context, filterTargets);

            context.SetTarget(closestTarget);
            context.TransitionTo(context.AttackState);
        }

        private GameObject ClosestTarget(FSM_Context context, List<GameObject> targets)
        {
            GameObject result = null;
            float minDistance = float.MaxValue;

            foreach (GameObject target in targets)
            {
                float distance = (target.transform.position - context.transform.position).magnitude;

                if (distance < minDistance)
                {
                    result = target;
                    minDistance = distance;
                }
            }

            return result;
        }

        private bool WithinRange(FSM_Context context, GameObject target)
        {
            float distanceTo = (target.transform.position - context.transform.position).magnitude;

            if (distanceTo > context.ViewRange) // Return if not within range
                return false;

            return true;
        }

        private bool WithinAngle(FSM_Context context, GameObject target)
        {
            Vector3 dir = (target.transform.position - context.transform.position).normalized;
            float withinAngle = Vector3.Dot(dir, context.transform.forward); // 1 = Looking at, -1 = Opposite direction

            if (withinAngle < context.ViewAngle) // Return if not within angle
                return false;

            return true;
        }
    }
}
