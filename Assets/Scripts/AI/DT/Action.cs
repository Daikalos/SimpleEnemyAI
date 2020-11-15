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
            return Task();
        }

        public virtual bool Task()
        {
            return true;
        }

        public delegate bool ExecuteTask();
    }
}