﻿using UnityEngine;

namespace CharacterControllerTest
{
    public class PlayerInput : MonoBehaviour
    {
        // Uses old Input class
        [Header("Controls")] [SerializeField] private KeyCode forward = KeyCode.W;
        [SerializeField] private KeyCode back = KeyCode.S;
        [SerializeField] private KeyCode left = KeyCode.A;
        [SerializeField] private KeyCode right = KeyCode.D;
        [SerializeField] private KeyCode jump = KeyCode.Space;

        public Vector3 InputVector => inputVector;
        public float mouseX, mouseY;

        public bool IsJumping
        {
            get => isJumping;
            set => isJumping = value;
        }

        private Vector3 inputVector;
        private bool isJumping;
        private float xInput;
        private float zInput;
        private float yInput;

        private void HandleInput()
        {
            // Reset input
            xInput = 0;
            yInput = 0;
            zInput = 0;

            if (Input.GetKey(forward))
            {
                zInput++;
            }

            if (Input.GetKey(back))
            {
                zInput--;
            }

            if (Input.GetKey(left))
            {
                xInput--;
            }

            if (Input.GetKey(right))
            {
                xInput++;
            }

            inputVector = new Vector3(xInput, yInput, zInput);

            isJumping = Input.GetKeyDown(jump);
        }

        private void HandleAxis()
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y"); 
        }

        private void Update()
        {
            HandleInput();
            HandleAxis();
        }
    }
}