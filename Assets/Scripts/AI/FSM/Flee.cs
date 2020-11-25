using UnityEngine;
using UnityEngine.AI;
using System.Linq;

namespace FSM
{
    // Flee when low health

    public class Flee : State
    {
        private FSM_AI m_Context;
        private NavMeshAgent m_Agent;

        private Transform[] m_Waypoints;

        public override void Init(FSM_AI context)
        {
            m_Context = context;
            m_Agent = context.Agent;

            m_Waypoints = GameObject.FindGameObjectsWithTag("Waypoint").Select(w => w.transform).ToArray();
        }

        public override void Enter()
        {
            m_Agent.isStopped = false;
            m_Agent.destination = RandomWaypoint().position;

            m_Context.SetTarget(null);
        }

        public override void Update()
        {
            if (m_Agent.remainingDistance <= float.Epsilon)
            {
                if (m_Context.TransitionTo(m_Context.PatrolState))
                    return;
            }
        }

        public override void Exit()
        {
            m_Context.StartHealth = m_Context.Health;
        }

        private Transform RandomWaypoint()
        {
            return m_Waypoints[Random.Range(0, m_Waypoints.Length)];
        }
    }
}
