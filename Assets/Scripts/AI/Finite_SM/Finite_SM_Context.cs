using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Finite_SM
{
    public class Finite_SM_Context : Enemy
    {
        #region Variables

        private readonly List<Enemy> m_Enemies = new List<Enemy>();

        private NavMeshAgent m_Agent = null;

        private GameObject m_Target = null; // Current Target

        #endregion

        #region Properties

        public List<Enemy> Enemies => m_Enemies;
        public NavMeshAgent Agent => m_Agent;
        public GameObject Target => m_Target;

        #endregion

        private State m_CurrentState = null;

        public readonly Patrol PatrolState = new Patrol();
        public readonly Chase  ChaseState  = new Chase();
        public readonly Attack AttackState = new Attack();
        public readonly Flee   FleeState   = new Flee();

        private void Awake()
        {
            m_Agent = GetComponent<NavMeshAgent>();

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) // Add reference to all other enemies
            {
                Enemy enemy = obj.GetComponent<Enemy>();

                if (!enemy.Equals(this))
                {
                    m_Enemies.Add(enemy);
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

        private void TransitionTo(State state)
        {
            if (m_CurrentState != null)
                m_CurrentState.Exit(this);

            m_CurrentState = state;
            m_CurrentState.Enter(this);
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
