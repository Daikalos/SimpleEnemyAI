using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DT
{
    public class DT_Context : Enemy
    {
        private Decision m_Root;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            // Construct Decision Tree

            m_Root = new Decision(IsTargetFound);

            Decision shouldFlee = new Decision(Flee);
            Decision isVisible = new Decision(IsTargetVisible);
            Decision inRange = new Decision(WithinAttackRange);

            Patrol patrol = new Patrol(this);
            Attack attack = new Attack(this);
            Chase chase = new Chase(this);
            Flee flee = new Flee(this);

            m_Root.TrueNode = shouldFlee;
            m_Root.FalseNode = patrol;

            shouldFlee.TrueNode = flee;
            shouldFlee.FalseNode = isVisible;

            isVisible.TrueNode = inRange;
            isVisible.FalseNode = chase;

            inRange.TrueNode = attack;
            inRange.FalseNode = chase;
        }


        private void Update()
        {
            if (!m_Root.Evaluate())
                Debug.LogException(new NullReferenceException("Decision Tree is not correctly constructed"), this);
        }

        private bool IsTargetFound()     => (Target != null);
        private bool Flee()              => (Health <= (StartHealth * FleeBoundary));
        private bool IsTargetVisible()   => (WithinViewRange(Target) && WithinViewAngle(Target));
        private bool WithinAttackRange()
        {
            float distanceTo = (Target.transform.position - transform.position).magnitude;
            return (distanceTo < AttackRange);
        }

        public bool IsTargetVisible(GameObject target)
        {
            return WithinViewRange(target) && WithinViewAngle(target);
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
    }
}