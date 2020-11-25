using System;
using UnityEngine;

namespace DT
{
    public class DecisionTree
    {
        private Node m_Root;

        public void Evaluate()
        {
            if (m_Root == null)
                Debug.LogError("No root assigned");
            else if (!m_Root.Evaluate())
                Debug.LogError("Decision Tree is not correctly constructed");
        }

        public void AssignRoot(Node root)
        {
            m_Root = root;
        }

        public void AssignTrueNode(Decision parent, Node node)
        {
            parent.TrueNode = node;
        }

        public void AssignFalseNode(Decision parent, Node node)
        {
            parent.FalseNode = node;
        }
    }
}