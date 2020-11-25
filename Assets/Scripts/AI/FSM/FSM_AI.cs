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
            m_Machine.Update();
        }

        public bool TransitionTo(State state)
        {
            return m_Machine.TransitionTo(state);
        }

        public static Vector3 RandomPoint(Vector3 origin, float distance, int layermask)
        {
            // Returns a random point on navigation mesh

            Vector3 randDirection = (Random.insideUnitSphere * distance) + origin;
            NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, distance, layermask);

            return navHit.position;
        }

        public bool Flee() 
        {
            return (Health < (StartHealth * FleeBoundary));
        }
        public bool IsTargetVisible(GameObject target) 
        { 
            return (WithinViewRange(target) && WithinViewAngle(target) && !BehindWall(target)); 
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

        public GameObject ClosestTarget()
        {
            GameObject result = null;
            float minDistance = float.MaxValue;

            foreach (GameObject target in VisibleTargets())
            {
                float distance = (target.transform.position - transform.position).magnitude;
                if (distance < minDistance)
                {
                    result = target;
                    minDistance = distance;
                }
            }
            return result; // Return target of closest distance
        }
        public List<GameObject> VisibleTargets()
        {
            List<GameObject> visibleTargets = new List<GameObject>(); // Get all the current visible targets
            foreach (GameObject target in Targets)
            {
                if (!IsTargetVisible(target))
                    continue;

                visibleTargets.Add(target);
            }

            return visibleTargets;
        }
    }
}
