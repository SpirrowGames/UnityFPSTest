using System;
using CharacterControllerTest.States;
using UnityEngine;

namespace CharacterControllerTest
{
    /// <summary>
    /// キャラクターの移動の全てを司るクラス
    /// 各 State はそれぞれの状態に応じてこのクラスのメソッドを呼び出す
    /// よって、このクラスのメソッドは組み合わせ可能であったり、パラメータ変更で動きを変えられるように実装する
    /// </summary>
    [RequireComponent(typeof(CharacterController))] // 動きのため
    public class CharacterMovement : MonoBehaviour
    {
        // Movement
        public float speed = 12f;
        public float sprintSpeed = 20f;
        public float crouchSpeed = 4f;

        // Jumping
        public float gravity = -3.5f;
        public float jumpHeight = 1f;
        public float jumpDuration = 0.4f;
        public AnimationCurve jumpFallOff;
        
        public Action<ControllerColliderHit> OnHit;

        // 必須のコンポーネント
        private CharacterController _controller;
        public MovementStateMachine movementStateMachine;

        private Vector3 _velocity;
        public Vector3 OldVelocity { get; private set; }
        public bool IsGrounded => _controller.isGrounded;

        public bool ReadyToJump => _readyToJump;
        private bool _readyToJump = true;

        public Vector3 MovementInput { get; private set; }
        public bool IsJumping { get; set; }
        public bool IsKnockbacking { get; set; }
        public Vector3 KnockbackDirection { get; private set; }

        public void AddMovementInput(Vector3 movement)
        {
            MovementInput += movement;
        }

        public bool DoJump()
        {
            if (CanJump())
            {
                _readyToJump = false;
                IsJumping = true;
                Invoke(nameof(ResetJump), 0.25f); // クールタイム
                return true;
            }

            return false;
        }

        public bool DoKnockback(Vector3 knockbackDirection)
        {
            // ノックバック中はノックバックを受け付けないということにしておく
            if (IsKnockbacking) return false;
            
            IsKnockbacking = true;
            KnockbackDirection = knockbackDirection;
            
            return true;
        }

        public Vector3 GetCurrentVelocity()
        {
            return _velocity;
        }

        public void SetVelocity(Vector3 velocity)
        {
            _velocity = velocity;
        }

        public void AddVelocity(Vector3 velocity)
        {
            _velocity += velocity;
        }

        #region Event functions

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            movementStateMachine = new MovementStateMachine(this);
        }

        private void Start()
        {
            // 初期は Idle
            movementStateMachine.Initialize(movementStateMachine.IdleMovementState);
        }

        private void Update()
        {
            CharacterMove();
            
            OldVelocity = _velocity;
            _velocity = Vector3.zero;
        }

        private void CharacterMove()
        {
            // 各 State で velocity が計算される
            movementStateMachine.Execute(deltaTime: Time.deltaTime);

            // ===============================================
            // CharacterController.Move を呼ぶのはこの一度だけ
            _controller.Move(_velocity);
            // ===============================================

            // 入力された移動量をクリアする
            MovementInput = Vector3.zero;
        }
        
        // CharacterController の衝突イベント
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            OnHit?.Invoke(hit);
        }

        #endregion // Event functions

        #region Private methods

        private void ResetJump()
        {
            _readyToJump = true;
        }

        private bool CanJump()
        {
            return _controller.isGrounded;
        }

        #endregion // Private methods
    }
}