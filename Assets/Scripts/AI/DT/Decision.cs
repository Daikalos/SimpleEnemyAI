using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT
{
    public class Decision : Node
    {
        private readonly TestDecision m_Test;

        public Node TrueNode { get; set; } = null;
        public Node FalseNode { get; set; } = null;

        public Decision(TestDecision test)
        {
            m_Test = test;
        }

        public override bool Evaluate()
        {
            bool? result = m_Test?.Invoke();

            if (!result.HasValue)
                return false;

            if (result.Value)
            {
                if (TrueNode == null)
                    return false;

                return TrueNode.Evaluate();
            }
            else
            {
                if (FalseNode == null)
                    return false;

                return FalseNode.Evaluate();
            }
        }

        public delegate bool TestDecision();
    }
}
