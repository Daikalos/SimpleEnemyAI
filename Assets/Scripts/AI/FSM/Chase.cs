﻿using UnityEngine;
using UnityEngine.AI;

namespace FSM
{
    public class Chase : State
    {
        private FSM_AI m_Context;
        private NavMeshAgent m_Agent;

        public override void Init(FSM_AI context)
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
            else if (m_Context.WithinApproachRange(target))
            {
                if (m_Context.TransitionTo(m_Context.AttackState)) 
                    return;
            }

            m_Agent.destination = target.transform.position;
        }

        public override void Exit()
        {

        }
    }
}
