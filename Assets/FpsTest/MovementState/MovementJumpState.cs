using UnityEngine;

namespace FpsTest
{
    public class MovementJumpState : MovementBaseState
    {
        public MovementJumpState(MovementStates currentContext, MovementStateFactory movementStateFactory)
            : base(currentContext, movementStateFactory, "Jump")
        {
        }

        private CharacterMovement _character;
        private CharacterStatus _status;

        public override void EnterState()
        {
            _character = Context.GetComponent<CharacterMovement>();
            _status = Context.GetComponent<CharacterStatus>();

            _character.Jump();
        }

        public override void UpdateState()
        {
            ChangeState();
            
            _character.Look();
            // 現状は Translate に Falling の処理も含んでいるため呼び出す必要がある
            _character.Translate();
        }

        public override void FixedUpdateState()
        {
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

            // 地面に付いたら Default に戻す
            if (_character.IsGrounded)
            {
                TransitionTo(Factory.Default());
                return;
            }
        }
    }
}