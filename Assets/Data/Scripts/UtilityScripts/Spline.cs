using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline
{
    public static Vector3 GetPoint(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * oneMinusT * p1 +
            3f * oneMinusT * oneMinusT * t * p2 +
            3f * oneMinusT * t * t * p3 +
            t * t * t * p4;
    }
}
