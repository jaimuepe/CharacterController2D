using System;
using UnityEngine;

namespace CC2D
{
    [Serializable]
    public class Movementparameters
    {
        [Header("Movement")]
        public float maxMovementSpeed = 5.0f;
        public float movementAcceleration = 10.0f;
        public float movementDeceleration = 1.0f;

        [Header("Jump")]
        public float jumpLeeway;
        public float jumpMagnitude;
        public AnimationCurve jumpCurveUp;
        public AnimationCurve jumpCurveDown;
    }

    [Serializable]
    public class CollisionParameters
    {
        public float groundForgivingDistance = 0.1f;
        public int numberOfVerticalRays = 5;
        public int numberOfHorizontalRays = 5;
        public float skinWidth = 0.02f;
    }
}
