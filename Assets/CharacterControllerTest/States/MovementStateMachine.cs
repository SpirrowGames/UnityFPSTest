using System;

namespace CharacterControllerTest.States
{
    [Serializable]
    public class MovementStateMachine
    {
        public IMovementState CurrentMovementState { get; private set; }

        // reference to the state objects
        public WalkMovementState WalkMovementState;
        public JumpMovementState JumpMovementState;
        public IdleMovementState IdleMovementState;
        public KnockbackMovementState KnockbackMovementState;

        // event to notify other objects of the state change
        public event Action<IMovementState> StateChanged;

        // pass in necessary parameters into constructor 
        public MovementStateMachine(CharacterMovement movement)
        {
            // create an instance for each state and pass in PlayerController
            WalkMovementState = new WalkMovementState(this, movement);
            JumpMovementState = new JumpMovementState(this, movement);
            IdleMovementState = new IdleMovementState(this, movement);
            KnockbackMovementState = new KnockbackMovementState(this, movement);
        }

        // set the starting state
        public void Initialize(IMovementState movementState)
        {
            CurrentMovementState = movementState;
            movementState.Enter();

            // notify other objects that state has changed
            StateChanged?.Invoke(movementState);
        }

        // exit this state and enter another
        public void TransitionTo(IMovementState nextMovementState)
        {
            CurrentMovementState.Exit();
            CurrentMovementState = nextMovementState;
            nextMovementState.Enter();

            // notify other objects that state has changed
            StateChanged?.Invoke(nextMovementState);
        }

        // allow the StateMachine to update this state
        public void Execute(float deltaTime)
        {
            CurrentMovementState?.Execute(deltaTime);
        }
    }}