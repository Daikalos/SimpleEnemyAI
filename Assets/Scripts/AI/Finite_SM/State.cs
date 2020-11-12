using System.Collections;
using System.Collections.Generic;

namespace Finite_SM
{
    public abstract class State
    {
        public abstract void Enter(Finite_SM_Context context);
        public abstract void Update(Finite_SM_Context context);
        public abstract void Exit(Finite_SM_Context context);
    }
}
