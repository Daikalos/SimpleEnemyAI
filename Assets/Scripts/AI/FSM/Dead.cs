using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class Dead : State
    {
        public override void Init(FSM_Context context)
        {

        }

        public override void Enter(FSM_Context context)
        {
            GameObject.Destroy(context.gameObject);
        }

        public override void Update(FSM_Context context)
        {

        }

        public override void Exit(FSM_Context context)
        {

        }
    }
}
