using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DT
{
    public class DT_Context : Enemy
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

        private Decision m_Root;

        protected override void Awake()
        {
            base.Awake();

            Agent = GetComponent<NavMeshAgent>();

            GameObject.FindGameObjectsWithTag("Enemy").ToList().ForEach(t =>
            {
                if (!t.Equals(this))
                    Targets.Add(t);
            });
            
        }

        private void Start()
        {
            m_Root = new Decision(this, FindTarget);

            Decision shouldFlee = new Decision(this, Flee);
            Decision isVisible = new Decision(this, IsTargetVisible);
            Decision inRange = new Decision(this, WithinAttackRange);

            Patrol patrol = new Patrol(this);
            Attack attack = new Attack(this);
            Chase chase = new Chase(this);
            Flee flee = new Flee(this);

            m_Root.TrueNode = shouldFlee;
            m_Root.FalseNode = patrol;

            shouldFlee.TrueNode = flee;
            shouldFlee.FalseNode = isVisible;

            isVisible.TrueNode = inRange;
            isVisible.FalseNode = chase;

            inRange.TrueNode = attack;
            inRange.FalseNode = chase;
        }


        private void Update()
        {
            Targets.RemoveAll(t => t == null);

            m_Root.Evaluate();
        }

        public bool IsTargetVisible()
        {
            return WithinViewRange(Target) && WithinViewAngle(Target);
        }

        public bool WithinAttackRange()
        {
            float distanceTo = (Target.transform.position - transform.position).magnitude;
            return (distanceTo < AttackRange);
        }

        public bool Flee() => (Health <= (StartHealth * FleeBoundary));

        private bool FindTarget()
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

            List<GameObject> filterTargets = new List<GameObject>(); // Filter all visible targets based on view range and view angle
            foreach (GameObject target in visibleTargets)
            {
                if (!IsTargetVisible(target))
                    continue;

                filterTargets.Add(target);
            }

            if (visibleTargets.Count <= 0)
                return false;

            Target = ClosestTarget(visibleTargets); // Target found

            return true;
        }

        private GameObject ClosestTarget(List<GameObject> visibleTargets)
        {
            GameObject result = null;
            float minDistance = float.MaxValue;

            foreach (GameObject target in visibleTargets)
            {
                float distance = (target.transform.position - transform.position).magnitude;

                if (distance < minDistance)
                {
                    result = target;
                    minDistance = distance;
                }
            }

            return result;
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
    }
}