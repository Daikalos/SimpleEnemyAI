using UnityEngine;
using UnityEngine.AI;
using System.Linq;

namespace FuSM
{
    // Flee when low health

    public class Flee : FuzzyState
    {
        private FuSM_Context m_Context;
        private NavMeshAgent m_Agent;

        private Transform[] m_Waypoints;

        public override float FuzzyValue()
        {
            float health = m_Context.Health;
            float startHealth = m_Context.StartHealth;

            float minHealth = m_Context.StartHealth * m_Context.FleeBoundary;

            float fuzzyValue = ((health - minHealth) / (startHealth - minHealth));

            return (1.0f - fuzzyValue);
        }

        public override void Init(FuSM_Context context)
        {
            m_Context = context;
            m_Agent = context.Agent;

            m_Waypoints = GameObject.FindGameObjectsWithTag("Waypoint").Select(w => w.transform).ToArray();
        }

        public override void Enter()
        {
            m_Agent.isStopped = false;
            m_Agent.SetDestination(RandomWaypoint().position);
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            m_Context.StartHealth = m_Context.Health;
            m_Context.SetTarget(null);
        }

        private Transform RandomWaypoint()
        {
            return m_Waypoints[Random.Range(0, m_Waypoints.Length)];
        }
    }
}
