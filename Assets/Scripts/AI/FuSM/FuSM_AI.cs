using UnityEngine;

namespace FuSM
{
    public class FuSM_AI : Enemy
    {
        public FuSM_Machine Machine { get; private set; }

        public readonly Patrol PatrolState = new Patrol();
        public readonly Attack AttackState = new Attack();
        public readonly Chase  ChaseState  = new Chase();
        public readonly Flee   FleeState   = new Flee();

        protected override void Awake()
        {
            base.Awake();

            Machine = new FuSM_Machine();

            Machine.AddState(PatrolState);
            Machine.AddState(AttackState);
            Machine.AddState(ChaseState);
            Machine.AddState(FleeState);

            Machine.InitializeStates(this);

            // Set to false to allow manual control over the transform
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }

        private void Update()
        {
            Perception();
            Machine.Update();
        }
    }
}
