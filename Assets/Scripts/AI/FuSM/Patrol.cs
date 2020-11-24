﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FuSM
{
    // Patrol as idle state until another enemy is found

    public class Patrol : FuzzyState
    {
        private FuSM_Context m_Context;
        private NavMeshAgent m_Agent;
        private Rigidbody m_RB;

        public override float ActivationLevel()
        {
            return m_ActivationLevel = (m_Context.Target == null) ? 1.0f : 0.0f;
        }

        public override void Init(FuSM_Context context)
        {
            m_Context = context;
            m_Agent = context.Agent;
            m_RB = context.RB;
        }

        public override void Enter()
        {
            m_Agent.isStopped = false;
            m_Agent.SetDestination(FuSM_Context.RandomPoint(Vector3.zero, 25.0f, -1));
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
                    4.0f * Time.deltaTime);
            }

            m_RB.velocity = m_Agent.velocity;
            m_Agent.nextPosition = m_Context.transform.position;
        }

        public override void Exit()
        {

        }

        private void Wander()
        {
            if (m_Agent.pathPending)
                return;

            if (m_Agent.remainingDistance > 0.1f)
                return;

            m_Agent.SetDestination(FuSM_Context.RandomPoint(Vector3.zero, 25.0f, -1));
        }

        private void SearchForTarget()
        {
            GameObject closestTarget = ClosestTarget();

            if (closestTarget == null)
                return;

            m_Context.SetTarget(closestTarget);
        }

        private List<GameObject> VisibleTargets()
        {
            Vector3 position = m_Context.transform.position;

            List<GameObject> visibleTargets = new List<GameObject>(); // Keep a list to store all enemies that is visible
            foreach (GameObject target in m_Context.Targets) // Get all enemies within sight (not behind walls)
            {
                Vector3 direction = (target.transform.position - position);
                Physics.Raycast(position, direction, out RaycastHit hit);

                if (hit.collider == null || !hit.collider.gameObject.CompareTag("Enemy"))
                    continue;

                visibleTargets.Add(target);
            }

            List<GameObject> filterTargets = new List<GameObject>(); // Filter all visible targets based on view range and view angle
            foreach (GameObject target in visibleTargets)
            {
                if (!m_Context.IsTargetVisible(target))
                    continue;

                filterTargets.Add(target);
            }

            return filterTargets;
        }

        private GameObject ClosestTarget()
        {
            GameObject result = null;
            float minDistance = float.MaxValue;

            foreach (GameObject target in VisibleTargets())
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
