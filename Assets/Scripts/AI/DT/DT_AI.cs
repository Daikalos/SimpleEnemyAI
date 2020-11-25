using System;
using UnityEngine;

namespace DT
{
    public class DT_AI : Enemy
    {
        private DecisionTree m_DecisionTree;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            // Construct Decision Tree

            m_DecisionTree = new DecisionTree();

            Decision isFound = new Decision(IsTargetFound);

            Decision shouldFlee = new Decision(Flee);
            Decision isVisible = new Decision(IsTargetVisible);
            Decision inRange = new Decision(WithinAttackRange);

            Patrol patrol = new Patrol(this);
            Attack attack = new Attack(this);
            Chase chase = new Chase(this);
            Flee flee = new Flee(this);

            m_DecisionTree.AssignRoot(isFound);

            m_DecisionTree.AssignTrueNode (isFound, shouldFlee);
            m_DecisionTree.AssignFalseNode(isFound, patrol);

            m_DecisionTree.AssignTrueNode (shouldFlee, flee);
            m_DecisionTree.AssignFalseNode(shouldFlee, isVisible);

            m_DecisionTree.AssignTrueNode (isVisible, inRange);
            m_DecisionTree.AssignFalseNode(isVisible, chase);

            m_DecisionTree.AssignTrueNode (inRange, attack);
            m_DecisionTree.AssignFalseNode(inRange, chase);
        }


        private void Update()
        {
            m_DecisionTree.Evaluate();
        }

        private bool IsTargetFound()     => (Target != null);
        private bool Flee()              => (Health < (StartHealth * FleeBoundary));
        private bool IsTargetVisible()   => (WithinViewRange(Target) && WithinViewAngle(Target) && !BehindWall(Target));
        private bool WithinAttackRange()
        {
            float distanceTo = (Target.transform.position - transform.position).magnitude;
            return (distanceTo < AttackRange);
        }

        public bool IsTargetVisible(GameObject target)
        {
            return WithinViewRange(target) && WithinViewAngle(target) && !BehindWall(target);
        }
        public bool WithinViewRange(GameObject target)
        {
            float distanceTo = (target.transform.position - transform.position).magnitude;
            return (distanceTo < ViewRange);
        }
        public bool WithinViewAngle(GameObject target)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            float withinAngle = Vector3.Dot(dir, transform.forward); // 1 = Looking at, -1 = Opposite direction

            return (withinAngle > ViewAngle);
        }
        public bool BehindWall(GameObject target)
        {
            Vector3 direction = (target.transform.position - transform.position);
            return Physics.Raycast(transform.position, direction, ViewRange, LayerMask.GetMask("Environment"));
        }
    }
}