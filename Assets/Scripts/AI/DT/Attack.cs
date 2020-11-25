using UnityEngine;
using UnityEngine.AI;

namespace DT
{
    public class Attack : Action
    {
        private readonly NavMeshAgent m_Agent;
        private float 
            m_AttackDelay,
            m_AttackTimer;

        public Attack(DecisionTree_AI context) : base(context)
        {
            m_Agent = context.Agent;
            m_AttackTimer = m_AttackDelay = context.AttackRate;
        }

        public override bool Evaluate()
        {
            if (!m_Agent.isStopped)
            {
                m_Agent.isStopped = true;
                m_Agent.ResetPath();
            }

            RotateTowardsTarget();
            FireAtTarget();

            return true;
        }

        private void RotateTowardsTarget()
        {
            Transform target = Context.Target.transform;
            Transform obj = Context.gameObject.transform;

            Vector3 lookDir = (target.position - obj.position);
            lookDir.y = 0;

            Quaternion rotate = Quaternion.LookRotation(lookDir);
            obj.rotation = Quaternion.Slerp(obj.rotation, rotate, Context.RotationSpeed * Time.deltaTime);
        }

        private void FireAtTarget()
        {
            m_AttackTimer -= Time.deltaTime;

            if (m_AttackTimer <= 0)
            {
                Context.CreateBullet(Context.Target);
                m_AttackTimer = m_AttackDelay;
            }
        }
    }
}