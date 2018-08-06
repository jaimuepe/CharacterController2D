using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC2D
{
    [RequireComponent(typeof(CharacterController2D))]
    public class GravitySystem : MonoBehaviour
    {
        CharacterController2D cc;

        [SerializeField]
        Vector2 accumulatedVelocity;
        public Vector2 AccumulatedVelocity { get { return accumulatedVelocity; } }

        private bool Enabled { get { return cc.movementParameters.affectedByGravity; } }
        private Vector2 Acceleration { get { return cc.movementParameters.gravityAcceleration; } }
        private float Deceleration { get { return cc.movementParameters.gravityDeceleration; } }
        private float MaxSpeed { get { return cc.movementParameters.maxGravitySpeed; } }

        private void Start()
        {
            cc = GetComponent<CharacterController2D>();
            CollisionSystem cs = GetComponent<CollisionSystem>();
            cs.OnCollision += OnCollision;
        }

        public void Calculate()
        {
            if (Enabled)
            {
                accumulatedVelocity += Acceleration * Time.deltaTime;
                accumulatedVelocity = Vector2.ClampMagnitude(accumulatedVelocity, MaxSpeed);
            }
            else
            {
                ApplyDeceleration();
            }
        }

        private void ApplyDeceleration()
        {
            if (accumulatedVelocity.x != 0.0f)
            {
                float deceleration = -Mathf.Sign(accumulatedVelocity.x) * Deceleration;

                bool overshot = (accumulatedVelocity.x > 0.0f
                        && accumulatedVelocity.x + deceleration * Time.deltaTime < 0.0f)
                    || (accumulatedVelocity.x < 0.0f
                        && accumulatedVelocity.x + deceleration * Time.deltaTime > 0.0f);

                if (overshot)
                {
                    accumulatedVelocity.x = 0.0f;
                }
                else
                {
                    accumulatedVelocity.x += deceleration * Time.deltaTime;
                }
            }

            if (accumulatedVelocity.y != 0.0f)
            {
                float deceleration = -Mathf.Sign(accumulatedVelocity.y) * Deceleration;

                bool overshot = (accumulatedVelocity.y > 0.0f
                        && accumulatedVelocity.y + deceleration * Time.deltaTime < 0.0f)
                    || (accumulatedVelocity.y < 0.0f
                        && accumulatedVelocity.y + deceleration * Time.deltaTime > 0.0f);

                if (overshot)
                {
                    accumulatedVelocity.y = 0.0f;
                }
                else
                {
                    accumulatedVelocity.y += deceleration * Time.deltaTime;
                }
            }
        }

        private void OnCollision(CollisionData data)
        {
            int sides = data.collisionSides;
            if ((sides & CollisionData.COLLIDE_TOP) > 0 && accumulatedVelocity.y > 0.0f)
            {
                accumulatedVelocity.y = 0.0f;
            }
            if ((sides & CollisionData.COLLIDE_BOTTOM) > 0 && accumulatedVelocity.y < 0.0f)
            {
                accumulatedVelocity.y = 0.0f;
            }
            if ((sides & CollisionData.COLLIDE_RIGHT) > 0 && accumulatedVelocity.x > 0.0f)
            {
                accumulatedVelocity.x = 0.0f;
            }
            if ((sides & CollisionData.COLLIDE_LEFT) > 0 && accumulatedVelocity.x < 0.0f)
            {
                accumulatedVelocity.x = 0.0f;
            }
        }
    }
}
