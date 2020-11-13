using System.Collections;
using System.Collections.Generic;

namespace FSM
{
    public abstract class State
    {
        public abstract void Init(FSM_Context context);
        public abstract void Enter(FSM_Context context);
        public abstract void Update(FSM_Context context);
        public abstract void Exit(FSM_Context context);
    }
}
