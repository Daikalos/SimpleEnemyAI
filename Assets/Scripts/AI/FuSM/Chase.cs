﻿using UnityEngine;
using UnityEngine.AI;

namespace FuSM
{
    public class Chase : State
    {
        private FuSM_Context m_Context;
        private NavMeshAgent m_Agent;

        public override void Init(FuSM_Context context)
        {
            m_Context = context;
            m_Agent = context.Agent;
        }

        public override void Enter()
        {
            m_Agent.isStopped = false;
        }

        public override void Update()
        {
            GameObject target = m_Context.Target;

            if (target == null)
            {
                if (m_Context.TransitionTo(m_Context.PatrolState)) 
                    return;
            }

            if (m_Context.WithinAttackRange(target))
            {
                if (m_Context.TransitionTo(m_Context.AttackState)) 
                    return;
            }

            m_Agent.SetDestination(target.transform.position);
        }

        public override void Exit()
        {

        }
    }
}
