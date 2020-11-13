using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    // Attack enemy until out of range or no longer visible

    public class Attack : State
    {
        private Coroutine m_AttackCoroutine = null;

        public override void Init(FSM_Context context)
        {

        }

        public override void Enter(FSM_Context context)
        {
            m_AttackCoroutine = context.StartCoroutine(FireAtTarget(context));
        }

        public override void Update(FSM_Context context)
        {
            if (context.Target == null)
                context.TransitionTo(context.PatrolState);

            if (!context.TargetVisible(context.Target))
                context.TransitionTo(context.ChaseState);
        }

        public override void Exit(FSM_Context context)
        {
            context.StopCoroutine(m_AttackCoroutine);
        }

        private IEnumerator FireAtTarget(FSM_Context context)
        {
            while (true)
            {
                yield return new WaitForSeconds(context.AttackRate);

                GameObject target = context.Target;
                GameObject muzzle = context.Muzzle;
                GameObject bullet = context.Bullet;

                Object.Instantiate(bullet, muzzle.transform.position,
                    Quaternion.LookRotation(target.transform.position - muzzle.transform.position));
            }
        }
    }
}
