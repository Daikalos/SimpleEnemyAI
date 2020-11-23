using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT
{
    public abstract class Action : Node
    {
        protected DT_Context Context { get; }

        public Action(DT_Context context)
        {
            Context = context;
        }
    }
}