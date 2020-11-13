using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class Chase : State
    {
        public override void Init(FSM_Context context)
        {

        }

        public override void Enter(FSM_Context context)
        {
            context.Agent.isStopped = false;
        }

        public override void Update(FSM_Context context)
        {
            if (context.Target == null)
                context.TransitionTo(context.PatrolState);

            if (context.WithinAttackRange(context.Target))
                context.TransitionTo(context.AttackState);

            context.Agent.SetDestination(context.Target.transform.position);
        }

        public override void Exit(FSM_Context context)
        {
            context.Agent.isStopped = true;
        }
    }
}
