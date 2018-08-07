using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC2D
{
    public class CharacterController2D : MonoBehaviour
    {
        public Movementparameters movementParameters;
        public CollisionParameters collisionParameters;

        private Transform mTransform;

        [Header("Debug")]

        [SerializeField]
        private Vector2 mTotalVelocity;

        [NonSerialized]
        public MovementSystem movementSystem;
        [NonSerialized]
        public JumpGravitySystem jumpSystem;
        [NonSerialized]
        public CollisionSystem collisionSystem;

        private void Awake()
        {
            mTransform = transform;

            collisionSystem = GetComponent<CollisionSystem>();
            if (!collisionSystem)
            {
                collisionSystem = gameObject.AddComponent<CollisionSystem>();
            }

            movementSystem = GetComponent<MovementSystem>();
            if (!movementSystem)
            {
                movementSystem = gameObject.AddComponent<MovementSystem>();
            }

            jumpSystem = GetComponent<JumpGravitySystem>();
            if (!jumpSystem)
            {
                jumpSystem = gameObject.AddComponent<JumpGravitySystem>();
            }
        }

        public void RequestMove(float moveDirection)
        {
            // either -1, 0 or 1
            if (moveDirection != 0.0f)
            {
                moveDirection = Mathf.Sign(moveDirection);
            }

            movementSystem.Direction = moveDirection;
        }

        public void RequestJump()
        {
            jumpSystem.TryJump();
        }

        public void RequestStopJumping()
        {
            jumpSystem.Jumping = false;
        }

        private void Update()
        {
            movementSystem.Calculate();
            jumpSystem.Calculate();

            mTotalVelocity = new Vector2(movementSystem.AccumulatedVelocity, jumpSystem.AccumulatedVelocity);
            Vector2 deltaMovement = mTotalVelocity * Time.deltaTime;

            deltaMovement = collisionSystem.Calculate(deltaMovement);

            movementSystem.UpdateCollisionData();
            jumpSystem.UpdateCollisionData();

            mTotalVelocity = new Vector2(movementSystem.AccumulatedVelocity, jumpSystem.AccumulatedVelocity);

            ResolvePosition(deltaMovement);

            movementSystem.ClearVariablesEndFrame();
            jumpSystem.ClearVariablesEndFrame();
        }

        private void ResolvePosition(Vector2 deltaMovement)
        {
            mTransform.position += new Vector3(deltaMovement.x, deltaMovement.y);
        }
    }
}
