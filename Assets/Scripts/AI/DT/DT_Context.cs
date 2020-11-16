﻿using System;
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
            // Construct Decision Tree

            m_Root = new Decision(IsTargetFound);

            Decision shouldFlee = new Decision(Flee);
            Decision isVisible = new Decision(IsTargetVisible);
            Decision inRange = new Decision(WithinAttackRange);

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

            if (!m_Root.Evaluate())
                Debug.LogException(new NullReferenceException("Decision Tree is not correctly constructed"), this);
        }

        private bool IsTargetFound()     => (Target != null);
        private bool Flee()              => (Health <= (StartHealth * FleeBoundary));
        private bool IsTargetVisible()   => (WithinViewRange(Target) && WithinViewAngle(Target));
        private bool WithinAttackRange()
        {
            float distanceTo = (Target.transform.position - transform.position).magnitude;
            return (distanceTo < AttackRange);
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

        public void SetTarget(GameObject target)
        {
            if (target == gameObject)
                return;

            Target = target;
        }
    }
}