using UnityEngine;

namespace FpsTest
{
    /// <summary>
    /// キャラクターの移動の全てを司るクラス
    /// 各 State はそれぞれの状態に応じてこのクラスのメソッドを呼び出す
    /// よって、このクラスのメソッドは組み合わせ可能であったり、パラメータ変更で動きを変えられるように実装する
    /// </summary>
    [RequireComponent(typeof(CharacterController))] // 動きのため
    [RequireComponent(typeof(Rigidbody))] // Collision のため
    public class CharacterMovement : MonoBehaviour
    {
        // References
        public Transform characterCamera;
        public Transform orientation;

        // Movement
        public float speed = 12f;
        public LayerMask groundMask;
        public Transform groundCheck;
        public float groundDistance = 0.4f;

        // Camera
        public float mouseSensitivity = 100f;

        // Jumping
        public float gravity = -3.5f;
        public float jumpHeight = 1f;

        #region Private fields

        // 必須のコンポーネント
        private CharacterController _controller;
        private Rigidbody _rb;

        private Vector3 _velocity;
        public bool IsGrounded => _isGrounded;
        private bool _isGrounded;
        
        private float _xRotation = 0f;
        private float _desiredX;

        public bool ReadyToJump => _readyToJump;
        private bool _readyToJump = true;

        #endregion // Private fields

        public void Translate()
        {
            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            var x = InputManager.Instance.x;
            var z = InputManager.Instance.z;

            // キャラの向きは orientation で別に管理するようにしているため、このスクリプトがアタッチされているオブジェクトの transform ではなく orientation の transform を使う
            // var move = transform.right * x + transform.forward * z;
            var move = orientation.transform.right * x + orientation.transform.forward * z;

            _controller.Move(move * speed * Time.deltaTime);

            // Falling
            if (!_isGrounded)
            {
                _velocity.y += gravity * Time.deltaTime;
            }

            _controller.Move(_velocity * Time.deltaTime);
        }

        public void Look()
        {
            var mouseX = InputManager.Instance.mouseX * InputManager.Instance.sensitivity_x * Time.deltaTime;
            var mouseY = InputManager.Instance.mouseY * InputManager.Instance.sensitivity_y * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -45f, 45f);

            // 回転は実装を変更している
            {
                // Camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                // this.transform.Rotate(Vector3.up * mouseX);

                var rot = characterCamera.transform.localRotation.eulerAngles;
                _desiredX = rot.y + mouseX;

                // カメラとプレイヤーの body の回転をそれぞれ設定する
                // カメラだけ見回し、プレイヤーの body はそれに追随させないということもできるように
                characterCamera.localRotation = Quaternion.Euler(_xRotation, _desiredX, 0f);
                orientation.localRotation = Quaternion.Euler(0f, _desiredX, 0f);
            }
        }

        public void Jump()
        {
            _readyToJump = false;

            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            
            _controller.Move(_velocity * Time.deltaTime);
            
            Invoke(nameof(ResetJump), 0.25f); // クールタイム
        }
        
        public bool IsFalling()
        {
            return _velocity.y <= 0.1;
        }

        #region Event functions

        private void Start()
        {
            GetAllReferences();
        }

        private void Update()
        {
            CheckGrounded();
        }

        #endregion // Event functions

        #region Private methods

        private void GetAllReferences()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // プロトタイピングでマップに InputManager Prefab をすでに配置していた場合を想定してガード
            if (InputManager.Instance == null)
                Instantiate(Resources.Load("InputManager"));

            InputManager.Instance.SetPlayer(this);

            _rb = GetComponent<Rigidbody>();
            _controller = GetComponent<CharacterController>();
        }

        private void CheckGrounded()
        {
            _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
        
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }

        private void ResetJump()
        {
            _readyToJump = true;
        }

        #endregion // Private methods
    }
}