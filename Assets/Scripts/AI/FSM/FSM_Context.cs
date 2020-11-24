using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace FSM
{
    public class FSM_Context : Enemy
    {
        private State m_CurrentState = null;

        private readonly List<State> States = new List<State>();

        public readonly Patrol PatrolState = new Patrol();
        public readonly Attack AttackState = new Attack();
        public readonly Chase  ChaseState  = new Chase();
        public readonly Flee   FleeState   = new Flee();

        protected override void Awake()
        {
            base.Awake();

            States.Add(PatrolState);
            States.Add(AttackState);
            States.Add(ChaseState);
            States.Add(FleeState);

            States.ForEach(s => s.Init(this)); // Initialize each state
        }

        private void Start()
        {
            TransitionTo(PatrolState);
        }

        private void Update()
        {
            m_CurrentState?.Update();
        }

        public bool TransitionTo(State state)
        {
            if (state == m_CurrentState || state == null)
                return false;
            
            m_CurrentState?.Exit();
            m_CurrentState = state;
            m_CurrentState.Enter();

            return true;
        }

        public static Vector3 RandomPoint(Vector3 origin, float distance, int layermask)
        {
            // Returns a random point on navigation mesh

            Vector3 randDirection = (Random.insideUnitSphere * distance) + origin;
            NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, distance, layermask);

            return navHit.position;
        }

        public bool Flee() => (Health < (StartHealth * FleeBoundary));
        public bool IsTargetVisible(GameObject target) => (WithinViewRange(target) && WithinViewAngle(target));
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
        public bool WithinApproachRange(GameObject target)
        {
            float distanceTo = (target.transform.position - transform.position).magnitude;
            return (distanceTo < ApproachRange);
        }
        public bool WithinAttackRange(GameObject target)
        {
            float distanceTo = (target.transform.position - transform.position).magnitude;
            return (distanceTo < AttackRange);
        }
    }
}
