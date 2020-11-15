using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DT
{
    public class Patrol : Action
    {
        public Patrol(DT_Context context) : base(context)
        {

        }

        public override bool Task()
        {
            NavMeshAgent agent = Context.Agent;

            if (agent.isStopped)
                agent.isStopped = false;

            if (agent.remainingDistance > float.Epsilon)
                return true;

            agent.SetDestination(RandomPoint(Vector3.zero, 25.0f, -1));

            return true;
        }

        private Vector3 RandomPoint(Vector3 origin, float distance, int layermask)
        {
            // Returns a random point on navigation mesh

            Vector3 randDirection = (Random.insideUnitSphere * distance) + origin;
            NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, distance, layermask);

            return navHit.position;
        }
    }
}
