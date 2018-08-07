using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CollisionData
{
    public static int COLLIDE_TOP = 1;
    public static int COLLIDE_LEFT = 1 << 1;
    public static int COLLIDE_BOTTOM = 1 << 2;
    public static int COLLIDE_RIGHT = 1 << 3;

    public bool colliding;
    public int collisionSides;
    internal bool closeToGround;
}
