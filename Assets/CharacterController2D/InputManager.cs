using Rewired;
using UnityEngine;

namespace CC2D
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        private CharacterController2D characterController2D;

        private bool jumpPressed;
        private bool jumpReleased;

        private float moveDirection;

        private void Update()
        {
            GetInput();
            ProcessInput();
        }

        private void GetInput()
        {
            moveDirection = Input.GetAxisRaw("Horizontal");
            jumpPressed = Input.GetButtonDown("Jump");
            bool jumpHold = !jumpPressed && Input.GetButton("Jump");
            jumpReleased = !jumpPressed && !jumpHold;

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (Time.timeScale == 1.0f)
                {
                    Time.timeScale = 0.1f;
                }
                else if (Time.timeScale == 0.1f)
                {
                    Time.timeScale = 0.5f;
                }
                else if (Time.timeScale == 0.5f)
                {
                    Time.timeScale = 1.0f;
                }
            }
#endif
        }

        private void ProcessInput()
        {
            if (moveDirection != 0.0f)
            {
                characterController2D.RequestMove(moveDirection);
            }
            if (jumpPressed)
            {
                characterController2D.RequestJump();
            }
            else if (jumpReleased)
            {
                characterController2D.RequestStopJumping();
            }
        }
    }
}
