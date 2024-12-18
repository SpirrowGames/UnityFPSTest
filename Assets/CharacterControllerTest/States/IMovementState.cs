using UnityEngine;

namespace CharacterControllerTest.States
{
    public interface IMovementState
    {
        public void Enter();

        public void Execute(float deltaTime);

        public void Exit();
    }

    public abstract class BaseMovementState : IMovementState
    {
        protected readonly CharacterMovement Movement;
        protected readonly MovementStateMachine SM;

        protected BaseMovementState(MovementStateMachine sm, CharacterMovement movement)
        {
            Movement = movement;
            SM = sm;
        }

        public abstract void Enter();

        public abstract void Execute(float deltaTime);

        public abstract void Exit();

        public string GetStateName() => GetType().Name;
    }

    public class IdleMovementState : BaseMovementState
    {
        public IdleMovementState(MovementStateMachine sm, CharacterMovement movement) : base(sm, movement)
        {
        }

        public override void Enter()
        {
            Debug.Log("Idle state enter");
        }

        public override void Execute(float deltaTime)
        {
            var velocity = Movement.MovementInput * Movement.speed * deltaTime;
            velocity.y = Movement.gravity * deltaTime;
            Movement.SetVelocity(velocity);

            if (Mathf.Abs(Movement.GetCurrentVelocity().x) > 0.01f ||
                Mathf.Abs(Movement.GetCurrentVelocity().z) > 0.01f)
            {
                SM.TransitionTo(SM.WalkMovementState);
            }

            if (Movement.IsJumping)
            {
                SM.TransitionTo(SM.JumpMovementState);
            }

            if (Movement.IsKnockbacking)
            {
                SM.TransitionTo(SM.KnockbackMovementState);
            }
        }

        public override void Exit()
        {
            Debug.Log("Idle state exit");
        }
    }

    public class WalkMovementState : BaseMovementState
    {
        public WalkMovementState(MovementStateMachine sm, CharacterMovement movement) : base(sm, movement)
        {
        }

        public override void Enter()
        {
            Debug.Log("Walk state enter");
        }

        public override void Execute(float deltaTime)
        {
            var velocity = Movement.MovementInput * Movement.speed;
            velocity.y = Movement.gravity;

            Movement.SetVelocity(velocity * deltaTime);

            if (Movement.IsJumping)
            {
                SM.TransitionTo(SM.JumpMovementState);
            }

            if (Movement.IsKnockbacking)
            {
                SM.TransitionTo(SM.KnockbackMovementState);
            }
        }

        public override void Exit()
        {
            Debug.Log("Walk state exit");
        }
    }

    public class JumpMovementState : BaseMovementState
    {
        private float _jumpTime;

        public JumpMovementState(MovementStateMachine sm, CharacterMovement movement) : base(sm, movement)
        {
        }

        public override void Enter()
        {
            Debug.Log("Jump state enter");
            Movement.IsJumping = false;
        }

        public override void Execute(float deltaTime)
        {
            // var velocity = Movement.OldVelocity;
            var velocity = Vector3.zero;
            if (_jumpTime >= Movement.jumpDuration)
            {
                if (Movement.IsGrounded)
                {
                    _jumpTime = 0;
                    SM.TransitionTo(SM.IdleMovementState);
                }

                velocity.y = Movement.gravity;
            }
            else
            {
                _jumpTime += deltaTime;

                var upSpeed = Movement.jumpFallOff.Evaluate(_jumpTime);
                velocity.y = upSpeed * Movement.jumpHeight;
            }

            // 前フレームの移動速度に対して Y 方向速度を上書きする。つまり、XZ 方向の速度は慣性として保持する。
            var newVelocity = Movement.OldVelocity; 
            newVelocity.y = velocity.y * deltaTime;
            
            Movement.AddVelocity(newVelocity);
        }

        public override void Exit()
        {
            Debug.Log("Jump state exit");
        }
    }

    public class KnockbackMovementState : BaseMovementState
    {
        public float knockbackForce = 10f;
        public float knockbackDuration = 0.5f;

        private Vector3 knockbackVelocity = Vector3.zero;
        private float currentKnockbackTime = 0f;

        public KnockbackMovementState(MovementStateMachine sm, CharacterMovement movement) : base(sm, movement)
        {
        }

        public override void Enter()
        {
            knockbackVelocity = Movement.KnockbackDirection * knockbackForce;
            currentKnockbackTime = knockbackDuration;
        }

        public override void Execute(float deltaTime)
        {
            if (currentKnockbackTime > 0)
            {
                currentKnockbackTime -= deltaTime;

                // ノックバックの減衰処理
                float timeRate = 1.0f - (currentKnockbackTime / knockbackDuration);
                knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, timeRate);
                
                // 重力
                knockbackVelocity.y = Movement.gravity;
            }
            else
            {
                knockbackVelocity = Vector3.zero;
                SM.TransitionTo(SM.IdleMovementState);
            }

            Movement.AddVelocity(knockbackVelocity * deltaTime);
        }

        public override void Exit()
        {
            Debug.Log("Knockback state exit");
            Movement.IsKnockbacking = false;
        }
    }
}