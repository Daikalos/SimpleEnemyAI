using UnityEngine;
using UnityEngine.AI;

namespace DT
{
    public class Chase : Action
    {
        private readonly NavMeshAgent m_Agent;

        public Chase(DT_AI context) : base(context)
        {
            m_Agent = context.Agent;
        }

        public override bool Evaluate()
        {
            if (m_Agent.isStopped) // If currently stopped, set to false so that we can move
                m_Agent.isStopped = false;

            // Set current destination as Target to chase
            m_Agent.destination = Context.Target.transform.position; 

            return true;
        }
    }
}
