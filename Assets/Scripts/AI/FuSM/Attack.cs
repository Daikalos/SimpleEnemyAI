using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace FuSM
{
    public class Attack : FuzzyState
    {
        private FuSM_Context m_Context;
        private Coroutine m_AttackCoroutine;

        public override float ActivationLevel()
        {
            if (m_Context.Target != null)
            {
                GameObject target = m_Context.Target;
                GameObject obj = m_Context.gameObject;

                float distToTarget = (target.transform.position - obj.transform.position).magnitude;

                return m_ActivationLevel = (distToTarget <= m_Context.AttackRange) ? 1.0f : 0.0f;
            }

            return 0.0f;
        }

        public override void Init(FuSM_Context context)
        {
            m_Context = context;
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

                GameObject target = m_Context.Target;
                GameObject muzzle = m_Context.Muzzle;
                GameObject bullet = m_Context.Bullet;

                Object.Instantiate(bullet, muzzle.transform.position,
                    Quaternion.LookRotation(target.transform.position - muzzle.transform.position));
            }
        }

        private void RotateTowardsTarget()
        {
            Transform target = m_Context.Target.transform;
            Transform obj = m_Context.gameObject.transform;

            Vector3 lookDir = (target.position - obj.position);

            lookDir.y = 0;

            Quaternion rotate = Quaternion.LookRotation(lookDir);
            obj.rotation = Quaternion.Slerp(obj.rotation, rotate, m_Context.RotationSpeed * Time.deltaTime);
        }
    }
}
