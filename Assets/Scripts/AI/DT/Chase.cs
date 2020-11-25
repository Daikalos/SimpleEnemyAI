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
            if (m_Agent.isStopped)
            {
                m_Agent.isStopped = false;
                m_Agent.ResetPath();
            }

            m_Agent.SetDestination(Context.Target.transform.position);

            return true;
        }
    }
}
