using UnityEngine;

namespace FpsTest
{
    /// <summary>
    /// ユーザーの入力をまとめる
    /// 簡易的にシングルトンで扱っている
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public bool jumping;
        public float x, z;
        public float mouseX, mouseY;
        public float sensitivity_x, sensitivity_y;
        
        private CharacterMovement _character;
        
        public void SetPlayer(CharacterMovement character)
        {
            this._character = character;
        }

        private void Awake()
        {
            // シングルトンの処理
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (!_character) return;
            
            sensitivity_x = _character.mouseSensitivity;
            sensitivity_y = _character.mouseSensitivity;
            
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");

            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            
            jumping = Input.GetKeyDown(KeyCode.Space);
        }
        
    }
}