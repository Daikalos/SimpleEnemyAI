using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Finite_SM
{
    public class FSM_Context : Enemy
    {
        #region Variables

        [SerializeField, Range(0.0f, 50.0f), Tooltip("How far the enemy can spot another")]
        private float m_ViewRange = 10.0f;

        [SerializeField, Range(0.0f, 1.0f), Tooltip("If the enemy is within the specified angle by forward axis")]
        private float m_ViewAngle = 0.5f;

        #endregion

        #region Properties

        public List<GameObject> Targets { get; } = new List<GameObject>();
        public NavMeshAgent Agent { get; private set; } = null;
        public GameObject Target { get; private set; } = null;

        public float ViewRange => m_ViewRange;
        public float ViewAngle => m_ViewAngle;

        #endregion

        private State m_CurrentState = null;

        public readonly Patrol PatrolState = new Patrol();
        public readonly Attack AttackState = new Attack();
        public readonly Flee   FleeState   = new Flee();

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();

            foreach (GameObject target in GameObject.FindGameObjectsWithTag("Enemy")) // Add reference to all other enemies
            {
                if (!target.Equals(this))
                {
                    Targets.Add(target);
                }
            }
        }

        private void Start()
        {
            TransitionTo(PatrolState);
        }

        void Update()
        {
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
            Target = null;

            if (target == gameObject)
                return;

            Target = target;
        }

        /// <summary>
        /// Returns a random point on the navmesh that the AI can walk to
        /// </summary>
        public Vector3 RandomPoint(Vector3 origin, float distance, int layermask)
        {
            Vector3 randDirection = Random.insideUnitSphere * distance;

            randDirection += origin;

            NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, distance, layermask);

            return navHit.position;
        }
    }
}
