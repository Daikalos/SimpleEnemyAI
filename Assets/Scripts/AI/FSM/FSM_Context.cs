using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FSM
{
    public class FSM_Context : Enemy
    {
        #region Variables

        [SerializeField, Range(0.0f, 50.0f)]
        private float m_ViewRange = 10.0f;
        [SerializeField, Range(0.0f, 1.0f)]
        private float m_ViewAngle = 0.75f;
        [SerializeField, Range(0.0f, 25.0f)]
        private float m_AttackRange = 5.0f;
        [SerializeField, Range(0.0f, 5.0f)]
        private float m_AttackRate = 2.5f;

        [SerializeField]
        private GameObject m_Muzzle = null;
        [SerializeField]
        private GameObject m_Bullet = null;

        #endregion

        #region Properties

        public List<GameObject> Targets { get; } = new List<GameObject>(); // CHECK FOR NULL, IF NULL, Transition TO PATROL
        public NavMeshAgent Agent { get; private set; } = null;
        public GameObject Target { get; private set; } = null;
        public GameObject Muzzle => m_Muzzle;
        public GameObject Bullet => m_Bullet;
 
        public float ViewRange => m_ViewRange;
        public float ViewAngle => m_ViewAngle;
        public float AttackRange => m_AttackRange;
        public float AttackRate => m_AttackRate;

        #endregion

        private State m_CurrentState = null;

        private readonly List<State> States = new List<State>();

        public readonly Patrol PatrolState = new Patrol();
        public readonly Attack AttackState = new Attack();
        public readonly Chase  ChaseState  = new Chase();
        public readonly Flee   FleeState   = new Flee();
        public readonly Dead   DeadState   = new Dead();

        private void Awake()
        {
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
            States.Add(DeadState);

            States.ForEach(s => s.Init(this)); // Initialize each state
        }

        private void Start()
        {
            TransitionTo(PatrolState);
        }

        private void Update()
        {
            Targets.RemoveAll(t => t == null);

            if (m_CurrentState != null)
                m_CurrentState.Update(this);
        }

        public void TransitionTo(State state)
        {
            if (m_CurrentState != null)
                m_CurrentState.Exit(this);

            m_CurrentState = state;
            m_CurrentState.Enter(this);
        }

        public void SetTarget(GameObject target)
        {
            if (target == gameObject)
                return;

            Target = target;
        }

        public override void TakeDamage()
        {
            if ((--m_Health) <= 0)
            {
                TransitionTo(DeadState);
            }
        }

        public static Vector3 RandomPoint(Vector3 origin, float distance, int layermask)
        {
            Vector3 randDirection = (Random.insideUnitSphere * distance) + origin;
            NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, distance, layermask);

            return navHit.position;
        }

        public List<GameObject> VisibleTargets()
        {
            Vector3 position = transform.position;

            List<GameObject> visibleTargets = new List<GameObject>(); // Keep a list to store all enemies that is visible
            foreach (GameObject target in Targets) // Get all enemies within sight (not behind walls)
            {
                Vector3 direction = (target.transform.position - position);
                Physics.Raycast(position, direction, out RaycastHit hit);

                if (hit.collider == null || !hit.collider.gameObject.CompareTag("Enemy"))
                    continue;

                visibleTargets.Add(target);
            }

            List<GameObject> filterTargets = new List<GameObject>(); // Filter all visible targets based on range and angle
            foreach (GameObject target in visibleTargets)
            {
                if (!TargetVisible(target))
                    continue;

                filterTargets.Add(target);
            }
            
            return filterTargets;
        }
        
        public bool TargetVisible(GameObject target)
        {
            if (target == null)
                return false;

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
            if (target == null)
                return false;

            float distanceTo = (target.transform.position - transform.position).magnitude;
            return (distanceTo < AttackRange);
        }
    }
}
