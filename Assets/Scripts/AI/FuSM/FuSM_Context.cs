using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections.Generic;

namespace FuSM
{
    public class FuSM_Context : Enemy
    {
        #region Variables

        [SerializeField, Range(0.0f, 50.0f)]
        private float m_ViewRange = 10.0f;
        [SerializeField, Range(0.0f, 1.0f)]
        private float m_ViewAngle = 0.6f;
        [SerializeField, Range(0.0f, 25.0f)]
        private float m_AttackRange = 5.0f;
        [SerializeField, Range(0.0f, 5.0f)]
        private float m_AttackRate = 2.5f;
        [SerializeField, Range(0.0f, 8.0f), Tooltip("How fast this rotates towards target when attacking")]
        private float m_RotationSpeed = 4.5f;
        [SerializeField, Range(0.0f, 1.0f), Tooltip("the limit on health before the enemy attempts to flee")]
        private float m_FleeBoundary = 0.4f;

        [SerializeField]
        private GameObject m_Muzzle = null;
        [SerializeField]
        private GameObject m_Bullet = null;

        #endregion

        #region Properties

        public List<GameObject> Targets { get; } = new List<GameObject>();
        public NavMeshAgent Agent { get; private set; } = null;
        public GameObject Target { get; private set; } = null;
        public GameObject Muzzle => m_Muzzle;
        public GameObject Bullet => m_Bullet;
 
        public float ViewRange => m_ViewRange;
        public float ViewAngle => m_ViewAngle;
        public float AttackRange => m_AttackRange;
        public float AttackRate => m_AttackRate;
        public float RotationSpeed => m_RotationSpeed;
        public float FleeBoundary => m_FleeBoundary;

        #endregion

        private State m_CurrentState = null;

        private readonly List<State> States = new List<State>();

        public readonly Patrol PatrolState = new Patrol();
        public readonly Attack AttackState = new Attack();
        public readonly Chase  ChaseState  = new Chase();
        public readonly Flee   FleeState   = new Flee();

        protected override void Awake()
        {
            base.Awake();

            Agent = GetComponent<NavMeshAgent>();
            GameObject.FindGameObjectsWithTag("Enemy").ToList().ForEach(t => 
            {
                if (!t.Equals(this))
                    Targets.Add(t);
            });

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
            Targets.RemoveAll(t => t == null);
            
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

        public void SetTarget(GameObject target)
        {
            if (target == gameObject)
                return;

            Target = target;
        }

        public static Vector3 RandomPoint(Vector3 origin, float distance, int layermask)
        {
            // Returns a random point on navigation mesh

            Vector3 randDirection = (Random.insideUnitSphere * distance) + origin;
            NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, distance, layermask);

            return navHit.position;
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

        public bool WithinAttackRange(GameObject target)
        {
            float distanceTo = (target.transform.position - transform.position).magnitude;
            return (distanceTo < AttackRange);
        }

        public bool FuzzyFlee()
        {


            return false;
        }

        public bool Flee() => (Health <= (StartHealth * FleeBoundary));
    }
}
