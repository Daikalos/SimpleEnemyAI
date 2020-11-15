using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DT
{
    public class Attack : Action
    {
        private float 
            m_AttackDelay,
            m_AttackTimer;

        public Attack(DT_Context context) : base(context)
        {
            m_AttackTimer = m_AttackDelay = context.AttackRate;
        }

        public override void Task()
        {
            NavMeshAgent agent = Context.Agent;

            if (!agent.isStopped)
                agent.isStopped = true;

            RotateTowardsTarget();

            m_AttackTimer -= Time.deltaTime;

            if (m_AttackTimer <= 0)
            {
                GameObject target = Context.Target;
                GameObject muzzle = Context.Muzzle;
                GameObject bullet = Context.Bullet;

                Object.Instantiate(bullet, muzzle.transform.position,
                    Quaternion.LookRotation(target.transform.position - muzzle.transform.position));

                m_AttackTimer = m_AttackDelay;
            }
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
    }
}