using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC2D
{
    public class JumpGravitySystem : MonoBehaviour
    {
        public bool Jumping { get { return jumping; } set { jumping = value; } }

        public bool Grounded { get { return grounded; } }

        public bool RecentlyGrounded { get { return edgeJumpLeewayTimer > 0.0f; } }

        public bool Airborne { get { return !Grounded; } }

        public float AccumulatedVelocity { get { return mAccumulatedVelocity; } }

        private float JumpMagnitude { get { return cc.movementParameters.jumpMagnitude; } }
        private AnimationCurve JumpCurveUp { get { return cc.movementParameters.jumpCurveUp; } }
        private AnimationCurve JumpCurveDown { get { return cc.movementParameters.jumpCurveDown; } }

        private float jumpCurveUpLength;

        [Header("Debug")]

        [SerializeField]
        private bool grounded;

        [SerializeField]
        private bool falling;

        [SerializeField]
        private bool jumping;

        [SerializeField]
        private float mAccumulatedVelocity;

        [SerializeField]
        private float airborneTime;

        [SerializeField]
        private float edgeJumpLeewayTimer;

        [SerializeField]
        private float jumpActiveTimer;

        private CharacterController2D cc;

        private bool wasJumping;

        private void Awake()
        {
            cc = GetComponent<CharacterController2D>();

            jumpCurveUpLength = JumpCurveUp.keys[JumpCurveUp.length - 1].time;
            airborneTime = jumpCurveUpLength;
        }

        public void Calculate()
        {
            falling = false;

#if UNITY_EDITOR
            jumpCurveUpLength = JumpCurveUp.keys[JumpCurveUp.length - 1].time;
#endif

            if ((Grounded || RecentlyGrounded) && jumpActiveTimer > 0.0f)
            {
                mAccumulatedVelocity = JumpMagnitude * JumpCurveUp.Evaluate(0);
                airborneTime = 0.0f;
                jumpActiveTimer = 0.0f;
                edgeJumpLeewayTimer = 0.0f;

                Jumping = true;
            }
            else if (Airborne)
            {
                if (wasJumping && !Jumping)
                {
                    airborneTime = Mathf.Max(airborneTime, jumpCurveUpLength);
                }

                if (airborneTime == 0.0f && mAccumulatedVelocity == 0.0f)
                {
                    // fall
                    falling = true;
                    airborneTime = jumpCurveUpLength;
                }

                if (airborneTime < jumpCurveUpLength)
                {
                    // going up
                    mAccumulatedVelocity = JumpMagnitude * JumpCurveUp.Evaluate(airborneTime);
                }
                else
                {
                    // falling
                    float t = airborneTime - jumpCurveUpLength;
                    mAccumulatedVelocity = JumpMagnitude * JumpCurveDown.Evaluate(t);
                }
                airborneTime += Time.deltaTime;
            }
            else
            {
                airborneTime = 0.0f;
            }
        }

        public void TryJump()
        {
            jumpActiveTimer = cc.movementParameters.jumpLeeway;
        }

        public void UpdateCollisionData()
        {
            CollisionData cData = cc.collisionSystem.Data;
            int collisionSides = cData.collisionSides;

            if ((collisionSides & CollisionData.COLLIDE_BOTTOM) > 0)
            {
                grounded = true;
                Jumping = false;

                edgeJumpLeewayTimer = 0.0f;
                airborneTime = 0.0f;
                mAccumulatedVelocity = 0.0f;
            }
            else
            {
                grounded = false;
            }

            if ((collisionSides & CollisionData.COLLIDE_TOP) > 0)
            {
                airborneTime = jumpCurveUpLength;
            }

            // leeway
            if (falling)
            {
                edgeJumpLeewayTimer = cc.movementParameters.jumpEdgeLeeway;
            }
            else
            {
                edgeJumpLeewayTimer = Mathf.Max(edgeJumpLeewayTimer - Time.deltaTime, 0.0f);
            }

            if (!grounded)
            {
                jumpActiveTimer = Mathf.Max(jumpActiveTimer - Time.deltaTime, 0.0f);
            }
        }

        public void ClearVariablesEndFrame()
        {
            wasJumping = Jumping;
        }
    }
}
