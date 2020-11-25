using System.Collections.Generic;
using UnityEngine;

namespace DT
{
    public class DecisionTree_AI : Enemy
    {
        private DecisionTree m_DecisionTree;

        protected override void Awake()
        {
            base.Awake();

            m_DecisionTree = new DecisionTree();

            Decision isFound = new Decision(IsTargetFound);
            Decision shouldFlee = new Decision(Flee);
            Decision inRange = new Decision(WithinAttackRange);

            Patrol patrol = new Patrol(this);
            Attack attack = new Attack(this);
            Chase chase = new Chase(this);
            Flee flee = new Flee(this);

            m_DecisionTree.AssignRoot(isFound);

            m_DecisionTree.AssignTrueNode(isFound, shouldFlee);
            m_DecisionTree.AssignFalseNode(isFound, patrol);

            m_DecisionTree.AssignTrueNode(shouldFlee, flee);
            m_DecisionTree.AssignFalseNode(shouldFlee, inRange);

            m_DecisionTree.AssignTrueNode(inRange, attack);
            m_DecisionTree.AssignFalseNode(inRange, chase);
        }

        private void Update()
        {
            if (!m_DecisionTree.Evaluate())
                Debug.LogError("Decision Tree is not correctly constructed");
        }

        private bool IsTargetFound()     => (Target != null);
        private bool Flee()              => (Health < (StartHealth * FleeBoundary));
        private bool WithinAttackRange() => (WithinApproachRange(Target));
    }
}