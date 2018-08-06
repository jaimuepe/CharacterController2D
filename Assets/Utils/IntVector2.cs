using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct IntVector2 : IEquatable<IntVector2>
{
    public int x;
    public int y;

    public bool Equals(IntVector2 other)
    {
        return x == other.x && y == other.y;
    }

    public static IntVector2 zero { get; }
}
