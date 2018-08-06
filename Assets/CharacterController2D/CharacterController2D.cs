using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC2D
{
    public class CharacterController2D : MonoBehaviour
    {
        public Movementparameters movementParameters;
        public CollisionParameters collisionParameters;

        public bool Grounded { get { return collision.Grounded; } }
        public bool Airborne { get { return collision.Airborne; } }

        private Transform mTransform;

        [Header("Debug")]

        [SerializeField]
        private Vector2 mTotalVelocity;

        private MovementSystem movement;
        private CollisionSystem collision;

        private void Awake()
        {
            mTransform = transform;

            collision = GetComponent<CollisionSystem>();
            if (!collision)
            {
                collision = gameObject.AddComponent<CollisionSystem>();
            }

            movement = GetComponent<MovementSystem>();
            if (!movement)
            {
                movement = gameObject.AddComponent<MovementSystem>();
            }
        }

        public void RequestMove(Vector2 moveVector)
        {
            Vector2 dirVector = movement.DirectionVector;
            if (moveVector.x != 0.0f)
            {
                dirVector.x = Mathf.Sign(moveVector.x);
            }

            if (moveVector.y != 0.0f)
            {
                dirVector.y = Mathf.Sign(moveVector.y);
            }

            movement.DirectionVector = dirVector;
        }

        public void RequestJump()
        {
            if (collision.Grounded)
            {
                movement.Jump = true;
            }
        }

        private void Update()
        {
            collision.ClearVariablesStartFrame();

            movement.Calculate();

            Vector2 deltaMovement = mTotalVelocity * Time.deltaTime;

            deltaMovement = collision.Calculate(deltaMovement);

            ResolvePosition(deltaMovement);

            movement.ClearVariablesEndFrame();
        }

        private void ResolvePosition(Vector2 deltaMovement)
        {
            mTransform.position += new Vector3(deltaMovement.x, deltaMovement.y);
        }
    }
}
