using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections.Generic;

namespace FuSM
{
    public class FuSM_Context : Enemy
    {
        private readonly List<FuzzyState> ActiveStates = new List<FuzzyState>();
        private readonly List<FuzzyState> States = new List<FuzzyState>();

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

        private void Update()
        {
            float avg = AverageFuzzyValue();
            foreach (FuzzyState state in States)
            {
                float val = state.FuzzyValue();
                if (val > avg)
                {
                    if (!ActiveStates.Contains(state))
                    {
                        ActiveStates.Add(state);
                        state.Enter();
                    }
                }
                else
                {
                    if (ActiveStates.Contains(state))
                    {
                        ActiveStates.Remove(state);
                        state.Exit();
                    }
                }
            }

            ActiveStates.ForEach(s => s.Update());
        }

        private float AverageFuzzyValue()
        {
            return States.Sum(s => s.FuzzyValue()) / States.Count;
        }

        public static Vector3 RandomPoint(Vector3 origin, float distance, int layermask)
        {
            // Returns a random point on navigation mesh

            Vector3 randDirection = (Random.insideUnitSphere * distance) + origin;
            NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, distance, layermask);

            return navHit.position;
        }

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
    }
}
