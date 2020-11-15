using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT
{
    public class Action : Node
    {
        public Action(DT_Context context) : base(context)
        {

        }

        public override bool Evaluate()
        {
            Task();

            return true;
        }

        public virtual void Task()
        {

        }

        public delegate bool ExecuteTask();
    }
}