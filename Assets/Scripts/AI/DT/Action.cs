using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT
{
    public class Action : Node
    {
        ExecuteAction m_Action;

        public Action(ExecuteAction action, int id) : base(id)
        {
            this.m_Action = action;
        }

        public override bool Evaluate()
        {
            return m_Action();
        }

        public delegate bool ExecuteAction();
    }
}