using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DT
{
    public class Flee : Action
    {
        private readonly NavMeshAgent m_Agent;
        private readonly Transform[] m_Waypoints;

        private Transform m_Waypoint = null;

        public Flee(DT_Context context) : base(context)
        {
            m_Agent = context.Agent;
            m_Waypoints = GameObject.FindGameObjectsWithTag("Waypoint").Select(w => w.transform).ToArray();
        }

        public override bool Evaluate()
        {
            if (m_Agent.isStopped)
            {
                m_Agent.isStopped = false;
                m_Agent.ResetPath();
            }

            if (m_Waypoint == null)
            {
                m_Waypoint = RandomWaypoint();
                m_Agent.SetDestination(m_Waypoint.position);
            }

            if (m_Agent.pathPending)
                return true;

            if (m_Agent.remainingDistance > float.Epsilon) return true;
            else
            {
                m_Waypoint = null;

                Context.StartHealth = Context.Health;
                Context.SetTarget(null);
            }

            return true;
        }

        private Transform RandomWaypoint()
        {
            return m_Waypoints[Random.Range(0, m_Waypoints.Length)];
        }
    }
}
