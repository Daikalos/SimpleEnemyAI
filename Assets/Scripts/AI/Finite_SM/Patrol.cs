using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Finite_SM
{
    // Patrol as idle state until another enemy is found

    public class Patrol : State
    {
        private Vector3 m_Destination = Vector3.zero;

        public override void Enter(Finite_SM_Context context)
        {
            // Empty
        }

        public override void Update(Finite_SM_Context context)
        {
            Wander(context);

            TargetVisible(context);
        }

        public override void Exit(Finite_SM_Context context)
        {
            context.Agent.isStopped = true;
        }

        /// <summary>
        /// Wander randomly around on map (kind of patrolling)
        /// </summary>
        private void Wander(Finite_SM_Context context)
        {
            if (context.Agent.remainingDistance > float.Epsilon)
                return;

            m_Destination = context.RandomPoint(Vector3.zero, 25.0f, -1);
            context.Agent.SetDestination(m_Destination);
        }

        private void TargetVisible(Finite_SM_Context context)
        {
            foreach (Enemy target in context.Enemies)
            {
                Vector3 direction = target.transform.position - context.transform.position;
                Physics.Raycast(context.transform.position, direction, out RaycastHit hit);

                if (hit.collider == null || !hit.collider.gameObject.CompareTag("Enemy"))
                    continue;

                Debug.Log(target.gameObject.name);
            }
        }
    }
}
