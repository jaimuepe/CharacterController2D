using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC2D
{
    [RequireComponent(typeof(CharacterController2D))]
    public class MovementSystem : MonoBehaviour
    {
        [Header("Debug")]

        [SerializeField]
        private float mAccumulatedVelocity;
        public float AccumulatedVelocity { get { return mAccumulatedVelocity; } }

        public float Direction { get; set; }

        private float Deceleration { get { return cc.movementParameters.movementDeceleration; } }
        private float Acceleration { get { return cc.movementParameters.movementAcceleration; } }
        private float MaxSpeed { get { return cc.movementParameters.maxMovementSpeed; } }

        private CharacterController2D cc;

        private void Start()
        {
            cc = GetComponent<CharacterController2D>();
        }

        public void Calculate()
        {

            if (Direction != 0.0f)
            {
                mAccumulatedVelocity += Acceleration * Time.deltaTime * Mathf.Sign(Direction);
            }

            mAccumulatedVelocity = Mathf.Clamp(mAccumulatedVelocity, -MaxSpeed, MaxSpeed);

            ApplyDeceleration();
        }

        public void UpdateCollisionData()
        {
            CollisionData cData = cc.collisionSystem.Data;
            int collisionSides = cData.collisionSides;

            if ((collisionSides & CollisionData.COLLIDE_RIGHT) > 0 && mAccumulatedVelocity > 0.0f)
            {
                mAccumulatedVelocity = 0.0f;
            }

            if ((collisionSides & CollisionData.COLLIDE_LEFT) > 0 && mAccumulatedVelocity < 0.0f)
            {
                mAccumulatedVelocity = 0.0f;
            }
        }

        private void ApplyDeceleration()
        {
            if (Direction == 0.0f && mAccumulatedVelocity != 0.0f)
            {
                // decelerate

                float deceleration = -Mathf.Sign(mAccumulatedVelocity) * Deceleration;

                bool overshot = (mAccumulatedVelocity > 0.0f
                        && mAccumulatedVelocity + deceleration * Time.deltaTime < 0.0f)
                    || (mAccumulatedVelocity < 0.0f
                        && mAccumulatedVelocity + deceleration * Time.deltaTime > 0.0f);

                if (overshot)
                {
                    mAccumulatedVelocity = 0.0f;
                }
                else
                {
                    mAccumulatedVelocity += deceleration * Time.deltaTime;
                }
            }
        }

        public void ClearVariablesEndFrame()
        {
            Direction = 0.0f;
        }
    }
}
