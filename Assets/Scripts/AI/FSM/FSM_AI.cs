using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace FSM
{
    public class FSM_AI : Enemy
    {
        private FSM_Machine m_Machine;

        public readonly Patrol PatrolState = new Patrol();
        public readonly Attack AttackState = new Attack();
        public readonly Chase ChaseState = new Chase();
        public readonly Flee FleeState = new Flee();

        public bool ShouldFlee { get; private set; }
        public bool IsTargetFound { get; private set; }
        public bool IsTargetVisible { get; private set; }
        public bool IsWithinViewRange { get; private set; }
        public bool IsWithinViewAngle { get; private set; }
        public bool IsBehindWall { get; private set; }
        public bool IsWithinApproachRange { get; private set; }
        public bool IsWithinAttackRange { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            m_Machine = new FSM_Machine();

            m_Machine.AddState(PatrolState);
            m_Machine.AddState(AttackState);
            m_Machine.AddState(ChaseState);
            m_Machine.AddState(FleeState);

            m_Machine.InitializeStates(this);
        }

        private void Start()
        {
            TransitionTo(PatrolState);
        }

        private void Update()
        {
            Perception();
            m_Machine.Update();
        }

        private void Perception()
        {
            IsTargetFound = (Target != null);

            if (!IsTargetFound)
                return;

            Vector3 dir = (Target.transform.position - transform.position).normalized;

            float distTo = (Target.transform.position - transform.position).magnitude;
            float withinAngle = Vector3.Dot(dir, transform.forward);

            ShouldFlee            = (Health < (StartHealth * FleeBoundary));
            IsWithinViewRange     = (distTo < ViewRange);
            IsWithinViewAngle     = (withinAngle > ViewAngle);
            IsBehindWall          = Physics.Raycast(transform.position, dir, ViewRange, LayerMask.GetMask("Environment"));
            IsWithinApproachRange = (distTo < ApproachRange);
            IsWithinAttackRange   = (distTo < AttackRange);
            IsTargetVisible       = (IsWithinViewRange && IsWithinViewAngle && !IsBehindWall);
        }

        public bool TransitionTo(State state)
        {
            return m_Machine.TransitionTo(state);
        }
    }
}
