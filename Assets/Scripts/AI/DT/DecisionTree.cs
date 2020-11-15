using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT
{
    public class DecisionTree
    {
        private Decision m_Root;

        public void ParseTree()
        {
            m_Root.Evaluate();
        }

        public void AddTrueDecision(Decision trueDecision, int parentID)
        {
            if (m_Root == null)
                return;

            InsertTrueDecision(m_Root, trueDecision, parentID);
        }

        public void AddFalseDecision(Decision falseDecision, int parentID)
        {
            if (m_Root == null)
                return;

            InsertFalseDecision(m_Root, falseDecision, parentID);
        }

        private bool InsertTrueDecision(Decision currentDecision, Decision trueDecision, int parentID)
        {
            if (currentDecision == null)
                return false;

            if (currentDecision.ID == parentID)
            {
                currentDecision.SetTrueDecision(trueDecision);
                return true;
            }
            else
            {
                if (InsertTrueDecision(currentDecision.TrueNode, trueDecision, parentID))
                    return true;
                else if (InsertTrueDecision(currentDecision.FalseDecision, trueDecision, parentID))
                    return true;
            }
            return false;
        }

        private bool InsertFalseDecision(Decision currentDecision, Decision falseDecision, int parentID)
        {
            if (currentDecision == null)
                return false;

            if (currentDecision.ID == parentID)
            {
                currentDecision.SetFalseDecision(falseDecision);
                return true;
            }
            else
            {
                if (InsertFalseDecision(currentDecision.TrueDecision, falseDecision, parentID))
                    return true;
                else if (InsertFalseDecision(currentDecision.FalseDecision, falseDecision, parentID))
                    return true;
            }
            return false;
        }
    }
}