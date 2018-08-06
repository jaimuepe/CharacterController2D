using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC2D
{
    [System.Serializable]
    public class Movementparameters
    {
        [Header("Gravity")]
        public bool affectedByGravity = true;
        public Vector2 gravityAcceleration = new Vector2(0.0f, -9.8f);
        public float gravityDeceleration = 2.0f;
        public float maxGravitySpeed = 15.0f;

        [Header("Movement")]
        public float maxMovementSpeed = 5.0f;
        public float movementAcceleration = 10.0f;
        public float movementDeceleration = 1.0f;

        public float jumpMagnitude;
        public float jumpTime;
        public AnimationCurve jumpCurve;
    }
    
    [System.Serializable]
    public class CollisionParameters
    {
        public int numberOfVerticalRays = 5;
        public int numberOfHorizontalRays = 5;
        public float skinWidth = 0.02f;
    }
}
