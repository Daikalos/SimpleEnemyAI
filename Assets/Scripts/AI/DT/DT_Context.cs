using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT
{
    public class DT_Context : Enemy
    {
        private DecisionTree m_DT;

        protected override void Awake()
        {
            base.Awake();

            m_DT = new DecisionTree();
        }

        private void Start()
        {

        }


        private void Update()
        {
            m_DT.ParseTree();
        }
    }
}