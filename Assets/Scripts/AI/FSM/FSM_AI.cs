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

        public bool TransitionTo(State state)
        {
            return m_Machine.TransitionTo(state);
        }
    }
}
