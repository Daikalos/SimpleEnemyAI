using System.Collections.Generic;
using UnityEngine;

namespace DT
{
    public class DT_AI : Enemy
    {
        private DecisionTree m_DecisionTree;

        protected override void Awake()
        {
            base.Awake();

            m_DecisionTree = new DecisionTree();

            Decision isFound = new Decision(TargetFound);
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
            Perception();
            m_DecisionTree.Evaluate();
        }

        // Wrap booleans in methods to pass as delegates for decision tree to evaluate
        private bool TargetFound()       => IsTargetFound;
        private bool Flee()              => ShouldFlee;
        private bool WithinAttackRange() => IsWithinApproachRange;
    }
}