using UnityEngine;
using UnityEngine.AI;
using System.Linq;

namespace FuSM
{
    // Flee when low health

    public class Flee : FuzzyState
    {
        private FuSM_AI m_Context;
        private NavMeshAgent m_Agent;
        private Rigidbody m_RB;

        private Transform[] m_Waypoints;

        public override float ActivationLevel()
        {
            float health = m_Context.Health;
            float startHealth = m_Context.StartHealth;

            float minHealth = startHealth * m_Context.FleeBoundary;
            float upperHealth = startHealth - (minHealth * 0.75f);

            m_ActivationLevel = 1.0f - ((health - minHealth) / (upperHealth - minHealth));

            BoundsCheck();

            return m_ActivationLevel;
        }

        public override void Init(FuSM_AI context)
        {
            m_Context = context;
            m_Agent = context.Agent;
            m_RB = context.RB;

            m_Waypoints = GameObject.FindGameObjectsWithTag("Waypoint").Select(w => w.transform).ToArray();
        }

        public override void Enter()
        {
            m_Agent.destination = RandomWaypoint().position;
        }

        public override void Update()
        {
            if (m_Context.Target == null || !m_Context.IsWithinViewRange)
            {
                if (m_Agent.velocity.sqrMagnitude > Mathf.Epsilon)
                {
                    m_RB.rotation = Quaternion.Slerp(m_RB.rotation,
                        Quaternion.LookRotation(m_Agent.velocity.normalized),
                        m_Context.RotationSpeed * m_ActivationLevel * Time.deltaTime);
                }
            }

            m_RB.velocity = m_Agent.velocity * m_ActivationLevel;
            m_Agent.nextPosition = m_Context.transform.position;

            if (m_Agent.remainingDistance < 0.5f)
            {
                m_Context.StartHealth = m_Context.Health;
            }
        }

        public override void Exit()
        {
            m_Context.SetTarget(null);
        }

        private Transform RandomWaypoint()
        {
            return m_Waypoints[Random.Range(0, m_Waypoints.Length)];
        }
    }
}
