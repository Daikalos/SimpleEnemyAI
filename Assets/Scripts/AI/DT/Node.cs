using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT
{
    public abstract class Node
    {
        private DT_Context m_Context;

        protected DT_Context Context => m_Context;

        public Node(DT_Context context)
        {
            m_Context = context;
        }

        public abstract bool Evaluate();
    }
}
