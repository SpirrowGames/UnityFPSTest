namespace FpsTest
{
    public class MovementDefaultState : MovementBaseState
    {
        public MovementDefaultState(MovementStates currentContext, MovementStateFactory movementStateFactory)
            : base(currentContext, movementStateFactory, "Default")
        {
        }

        private CharacterMovement _character;
        private CharacterStatus _status;

        public override void EnterState()
        {
            _character = Context.GetComponent<CharacterMovement>();
            _status = Context.GetComponent<CharacterStatus>();
        }

        public override void UpdateState()
        {
            // Default では、視点変更と移動を行う
            _character.Look();
            _character.Translate();
            
            ChangeState();
        }

        public override void FixedUpdateState()
        {
            // RigidBody など物理挙動を使うときに利用する
        }

        public override void ExitState()
        {
        }

        public override void ChangeState()
        {
            if (_status.health <= 0)
            {
                TransitionTo(Factory.Die());
                return;
            }

            // ジャンプ入力があった場合
            if (InputManager.Instance.jumping)
            {
                if (_character.ReadyToJump && _character.IsGrounded)
                    TransitionTo(Factory.Jump());

                return;
            }
        }
    }
}