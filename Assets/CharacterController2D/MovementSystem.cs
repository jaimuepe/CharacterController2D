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
        private Vector2 mAccumulatedVelocity;
        public Vector2 AccumulatedVelocity { get { return mAccumulatedVelocity; } }

        [SerializeField]
        private bool jump;
        public bool Jump { get { return jump; } set { jump = value; } }

        public Vector2 DirectionVector { get; set; }

        private float Deceleration { get { return cc.movementParameters.movementDeceleration; } }
        private float Acceleration { get { return cc.movementParameters.movementAcceleration; } }
        private float MaxSpeed { get { return cc.movementParameters.maxMovementSpeed; } }
        private float JumpMagnitude { get { return cc.movementParameters.jumpMagnitude; } }
        private AnimationCurve JumpCurve { get { return cc.movementParameters.jumpCurve; } }
        private float JumpTime { get { return cc.movementParameters.jumpTime; } }

        private float airborneTime = 0.0f;

        CharacterController2D cc;

        private void Start()
        {
            cc = GetComponent<CharacterController2D>();
            CollisionSystem cs = GetComponent<CollisionSystem>();
            cs.OnCollision += OnCollision;
        }

        public void Calculate()
        {
            if (DirectionVector.x != 0.0f)
            {
                mAccumulatedVelocity.x += Acceleration * Time.deltaTime * Mathf.Sign(DirectionVector.x);
            }

            if (Jump || cc.Airborne)
            {
                mAccumulatedVelocity.y = JumpMagnitude * Time.deltaTime * JumpCurve.Evaluate(airborneTime / JumpTime);
            }

            mAccumulatedVelocity.x = Mathf.Clamp(mAccumulatedVelocity.x, -MaxSpeed, MaxSpeed);
            mAccumulatedVelocity.y = Mathf.Max(mAccumulatedVelocity.y, 0.0f);

            ApplyDeceleration();

            if (cc.Airborne)
            {
                airborneTime += Time.deltaTime;
            }

            Debug.Log(airborneTime);
        }

        private void ApplyDeceleration()
        {
            if (DirectionVector.x == 0.0f && mAccumulatedVelocity.x != 0.0f)
            {
                // decelerate

                float deceleration = -Mathf.Sign(mAccumulatedVelocity.x) * Deceleration;

                bool overshot = (mAccumulatedVelocity.x > 0.0f
                        && mAccumulatedVelocity.x + deceleration * Time.deltaTime < 0.0f)
                    || (mAccumulatedVelocity.x < 0.0f
                        && mAccumulatedVelocity.x + deceleration * Time.deltaTime > 0.0f);

                if (overshot)
                {
                    mAccumulatedVelocity.x = 0.0f;
                }
                else
                {
                    mAccumulatedVelocity.x += deceleration * Time.deltaTime;
                }
            }
        }

        public void ClearVariablesEndFrame()
        {
            DirectionVector = Vector2.zero;
            Jump = false;
        }

        private void OnCollision(CollisionData data)
        {
            int sides = data.collisionSides;
            if ((sides & CollisionData.COLLIDE_TOP) > 0 || (sides & CollisionData.COLLIDE_BOTTOM) > 0)
            {
                if ((sides & CollisionData.COLLIDE_BOTTOM) > 0)
                {
                    airborneTime = 0.0f;
                }
                mAccumulatedVelocity.y = 0.0f;
            }
            if ((sides & CollisionData.COLLIDE_RIGHT) > 0 && mAccumulatedVelocity.x > 0.0f)
            {
                mAccumulatedVelocity.x = 0.0f;
            }
            if ((sides & CollisionData.COLLIDE_LEFT) > 0 && mAccumulatedVelocity.x < 0.0f)
            {
                mAccumulatedVelocity.x = 0.0f;
            }
        }
    }
}
