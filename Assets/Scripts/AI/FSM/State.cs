using System.Collections;
using System.Collections.Generic;

namespace Finite_SM
{
    public abstract class State
    {
        public abstract void Enter(FSM_Context context);
        public abstract void Update(FSM_Context context);
        public abstract void Exit(FSM_Context context);
    }
}
