using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections.Generic;

namespace FuSM
{
    public class FuSM_AI : Enemy
    {
        private FuSM_Machine m_Machine;

        public readonly Patrol PatrolState = new Patrol();
        public readonly Attack AttackState = new Attack();
        public readonly Chase  ChaseState  = new Chase();
        public readonly Flee   FleeState   = new Flee();

        protected override void Awake()
        {
            base.Awake();

            m_Machine = new FuSM_Machine();

            m_Machine.AddState(PatrolState);
            m_Machine.AddState(AttackState);
            m_Machine.AddState(ChaseState);
            m_Machine.AddState(FleeState);

            m_Machine.InitializeStates(this);

            // Set to false to allow manual control over the transform
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }

        private void Update()
        {
            RB.velocity = Vector3.zero;

            m_Machine.Update();
        }

        public static Vector3 RandomPoint(Vector3 origin, float distance, int layermask)
        {
            // Returns a random point on navigation mesh

            Vector3 randDirection = (Random.insideUnitSphere * distance) + origin;
            NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, distance, layermask);

            return navHit.position;
        }

        public bool IsTargetVisible(GameObject target) => (WithinViewRange(target) && WithinViewAngle(target) && !BehindWall(target));
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
