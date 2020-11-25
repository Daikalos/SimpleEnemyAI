using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace FSM
{
    // Attack enemy until out of range or no longer visible

    public class Attack : State
    {
        private FSM_AI m_Context;
        private NavMeshAgent m_Agent;

        private Coroutine m_AttackCoroutine;

        public override void Init(FSM_AI context)
        {
            m_Context = context;
            m_Agent = context.Agent;

            m_AttackCoroutine = null;
        }

        public override void Enter()
        {
            m_Agent.isStopped = true;
            m_Agent.ResetPath();

            m_AttackCoroutine = m_Context.StartCoroutine(FireAtTarget());
        }

        public override void Update()
        {
            GameObject target = m_Context.Target;

            if (target == null)
            {
                if (m_Context.TransitionTo(m_Context.PatrolState))
                    return;
            }
            else if (m_Context.Flee())
            {
                if (m_Context.TransitionTo(m_Context.FleeState))
                    return;
            }
            else if (!m_Context.WithinAttackRange(target))
            {
                if (m_Context.TransitionTo(m_Context.ChaseState))
                    return;
            }

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
            Transform obj = m_Context.gameObject.transform;

            Vector3 lookDir = (target.position - obj.position);

            lookDir.y = 0;

            Quaternion rotate = Quaternion.LookRotation(lookDir);
            obj.rotation = Quaternion.Slerp(obj.rotation, rotate, m_Context.RotationSpeed * Time.deltaTime);
        }
    }
}
