using Rewired;
using UnityEngine;

namespace CC2D
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        private CharacterController2D characterController2D;

        private Player player;

        private bool jumpPressed;
        private Vector2 moveVector;

        private void Awake()
        {
            player = ReInput.players.GetPlayer(0);
        }

        private void Update()
        {
            GetInput();
            ProcessInput();
        }

        private void GetInput()
        {
            moveVector.x = player.GetAxis("mHorizontal");
            moveVector.y = player.GetAxis("mVertical");

            jumpPressed = player.GetButtonDown("jump");
        }

        private void ProcessInput()
        {
            if (moveVector.x != 0.0f || moveVector.y != 0.0f)
            {
                characterController2D.RequestMove(moveVector);
            }
            if (jumpPressed)
            {
                characterController2D.RequestJump();
            }
        }
    }
}
