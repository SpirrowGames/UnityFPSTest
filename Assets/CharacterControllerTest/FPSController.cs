using CharacterControllerTest;
using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(CharacterMovement))]
public class FPSController : MonoBehaviour
{
    public Transform cameraTransform;
    public PlayerInput playerInput;
    public float mouseSensitivity = 100f;

    private CharacterMovement characterMovement;
    private float _xRotation = 0f;

    #region Event functions

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        characterMovement = GetComponent<CharacterMovement>();
        characterMovement.OnHit += OnHit;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleCameraRotation();
    }

    #endregion // Event functions

    #region Private methods

    private void HandleMovement()
    {
        // 移動入力値は現在のキャラクターの状態によらず設定する
        // その入力値をどうするかは CharacterMovement の State に任せる
        {
            var moveDirection = playerInput.InputVector.normalized; // 正規化
            moveDirection = transform.TransformDirection(moveDirection); // 方向をキャラクターの向きに合わせる

            // CharacterMovement に移動入力値を渡す
            characterMovement.AddMovementInput(moveDirection);
        }

        CheckJumpInput();
    }

    private void CheckJumpInput()
    {
        // NOTE 二段ジャンプとか実装するなら、ここで入力回数を数える
        if (playerInput.IsJumping)
        {
            var didJump = characterMovement.DoJump();
        }
    }

    private void HandleCameraRotation()
    {
        var mouseX = playerInput.mouseX * mouseSensitivity * Time.deltaTime;
        var mouseY = playerInput.mouseY * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -45f, 45f);

        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnHit(ControllerColliderHit hit)
    {
        CheckKnockback(hit);
    }

    private void CheckKnockback(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
        {
            var knockbackDir = (transform.position - hit.transform.position).normalized;
            var didKnockback = characterMovement.DoKnockback(knockbackDir);
        }
    }

    #endregion // Private methods
}