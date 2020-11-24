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

            // Set to false to allow manual control over the transform
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }

        private void Update()
        {
            States.ForEach(s => 
            {
                bool isActive = (s.ActivationLevel() > 0.0f);

                UpdateStateStatus(s, isActive);

                if (isActive)
                    s.Update();
            });
        }

        private void UpdateStateStatus(FuzzyState state, bool status)
        {
            if (status)
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

        /*   
        public enum Priority
        {
            Low,
            Medium, 
            High
        } 

        public Priority GetPriority(float fuzzyValue)
        {
            float low = TriangularFuzzyNumber(fuzzyValue, 0.0f, 0.0f, 0.3f);
            float medium = TriangularFuzzyNumber(fuzzyValue, 0.3f, 0.5f, 0.3f);
            float high = TriangularFuzzyNumber(fuzzyValue, 0.3f, 1.0f, 0.0f);

            float max = Mathf.Max(low, Mathf.Max(medium, high)); // Select the one of highest value (highest priority)

            if (low == max)
                return Priority.Low;
            if (medium == max)
                return Priority.Medium;
            if (high == max)
                return Priority.High;

            return Priority.Low;
        }

        private float TriangularFuzzyNumber(float x, float lhs, float med, float rhs)
        {
            // Formula from https://ijfs.usb.ac.ir/article_359_0038d4fb0f550de224041cbbbd77caf6.pdf
            // Where x = input, lhs = left-length, med = median, rhs = right-length

            if (x > (med - lhs) && x < med)
                return 1.0f - ((med - x) / lhs);

            if (x == med)
                return 1.0f;

            if (x > med && x < (med + rhs))
                return 1.0f - ((x - med) / rhs);         

            return 0.0f;
        }
        */
    }
}
