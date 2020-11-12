using System.Collections;
using System.Collections.Generic;

namespace Finite_SM
{
    // Attack enemy until out of range or no longer visible

    public class Attack : State
    {
        public override void Enter(FSM_Context context)
        {
            if (context.Target == null)
                context.TransitionTo(context.PatrolState);
        }

        public override void Update(FSM_Context context)
        {

        }

        public override void Exit(FSM_Context context)
        {

        }
    }
}
