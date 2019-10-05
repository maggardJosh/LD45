using UnityEngine;
using System.Collections;

public static class VectorExtensions 
{
    public static Vector3 ToVector3(this Vector2 vect)
    {
        return new Vector3(vect.x, vect.y);
    }
}
