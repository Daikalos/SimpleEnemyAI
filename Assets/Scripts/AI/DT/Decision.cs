using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT
{
    public class Decision : Node
    {
        private Node m_TrueNode = null;
        private Node m_FalseNode = null;

        public Node TrueNode => m_TrueNode;
        public Node FalseNode => m_FalseNode;

        public Decision(int id) : base(id)
        {

        }

        public override bool Evaluate()
        {

        }

        public void SetTrueNode(Node trueNode)
        {
            m_TrueNode = trueNode;
        }

        public void SetFalseNode(Node falseNode)
        {
            m_FalseNode = falseNode;
        }
    }
}
