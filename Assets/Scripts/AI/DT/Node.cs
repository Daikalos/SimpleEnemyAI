using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT
{
    public abstract class Node
    {
        protected DT_Context Context { get; }

        public Node(DT_Context context)
        {
            Context = context;
        }

        public abstract bool Evaluate();
    }
}
