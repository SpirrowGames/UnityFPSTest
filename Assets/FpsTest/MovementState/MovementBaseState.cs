namespace FpsTest
{
    public abstract class MovementBaseState : IMovementState
    {
        private readonly string _name; 
        protected readonly MovementStates Context;
        protected readonly MovementStateFactory Factory;

        protected MovementBaseState(MovementStates currentContext, MovementStateFactory movementStateFactory, string name)
        {
            _name = name;
            Context = currentContext;
            Factory = movementStateFactory;
        }

        public abstract void EnterState();

        public abstract void UpdateState();

        public abstract void FixedUpdateState();

        public abstract void ExitState();

        public abstract void ChangeState();
        
        public string GetStateName() => _name;

        protected void TransitionTo(IMovementState newState)
        {
            ExitState();

            newState.EnterState();
            
            // コンテクストに新しいステートを通知
            Context.OnNewStateChanged(newState);
        }
    }
}