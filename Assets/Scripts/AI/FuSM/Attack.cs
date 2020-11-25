using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace FuSM
{
    public class Attack : FuzzyState
    {
        private FuSM_AI m_Context;
        private Rigidbody m_RB;

        private Coroutine m_AttackCoroutine;

        public override float ActivationLevel()
        {
            if (m_Context.Target != null)
            {
                GameObject target = m_Context.Target;
                GameObject obj = m_Context.gameObject;

                float distToTarget = (target.transform.position - obj.transform.position).magnitude;

                return m_ActivationLevel = ((distToTarget <= m_Context.AttackRange) ? 1.0f : 0.0f);
            }

            return 0.0f;
        }

        public override void Init(FuSM_AI context)
        {
            m_Context = context;
            m_RB = context.RB;

            m_AttackCoroutine = null;
        }

        public override void Enter()
        {
            m_AttackCoroutine = m_Context.StartCoroutine(FireAtTarget());
        }

        public override void Update()
        {
            RotateTowardsTarget();
        }

        public override void Exit()
        {
            m_Context.StopCoroutine(m_AttackCoroutine);
        }

        private IEnumerator FireAtTarget()
        {
            while (true)
            {
                yield return new WaitForSeconds(m_Context.AttackRate);

                m_Context.CreateBullet(m_Context.Target);
            }
        }

        private void RotateTowardsTarget()
        {
            Transform target = m_Context.Target.transform;

            Vector3 lookDir = (target.position - m_Context.transform.position);
            lookDir.y = 0;

            Quaternion rotate = Quaternion.LookRotation(lookDir);
            m_RB.rotation = Quaternion.Slerp(m_RB.rotation, rotate, m_Context.RotationSpeed * Time.deltaTime);
        }
    }
}
