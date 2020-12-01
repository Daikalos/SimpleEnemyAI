using UnityEngine;

namespace DT
{
    public class DecisionTree
    {
        private Node m_Root;

        public bool Evaluate()
        {
            if (m_Root == null)
            {
                Debug.LogError("no root assigned");
                return false;
            }
            else
            {
                if (!m_Root.Evaluate())
                {
                    Debug.LogError("something went wrong with evaluating the tree");
                    return false;
                }

                return true;
            }
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