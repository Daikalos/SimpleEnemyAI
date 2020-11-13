using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    // Patrol as idle state until another enemy is found

    public class Patrol : State
    {
        public override void Init(FSM_Context context)
        {

        }

        public override void Enter(FSM_Context context)
        {
            context.Agent.isStopped = false;

            context.Agent.SetDestination(FSM_Context.RandomPoint(Vector3.zero, 25.0f, -1));
        }

        public override void Update(FSM_Context context) // Whilst patrolling, wander and search
        {
            Wander(context);
            SearchForTarget(context);
        }

        public override void Exit(FSM_Context context)
        {
            context.Agent.isStopped = true;
        }

        private void Wander(FSM_Context context)
        {
            if (context.Agent.remainingDistance > float.Epsilon)
                return;

            context.Agent.SetDestination(FSM_Context.RandomPoint(Vector3.zero, 25.0f, -1));
        }

        private void SearchForTarget(FSM_Context context)
        {
            GameObject closestTarget = ClosestTarget(context, context.VisibleTargets());

            if (closestTarget == null)
                return;

            context.SetTarget(closestTarget);      
            context.TransitionTo(context.ChaseState);
        }

        private GameObject ClosestTarget(FSM_Context context, List<GameObject> targets)
        {
            if (targets.Count == 0)
                return null;

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
    }
}
