using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DT
{
    public class Chase : Action
    {
        public Chase(DT_Context context) : base(context)
        {

        }

        public override void Task()
        {
            NavMeshAgent agent = Context.Agent;

            if (agent.isStopped)
                agent.isStopped = false;

            agent.SetDestination(Context.Target.transform.position);
        }
    }
}
