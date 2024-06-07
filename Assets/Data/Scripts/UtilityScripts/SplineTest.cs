using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SplineTest : MonoBehaviour
{
    public Transform p1, p2, p3, p4;
    public TileManager tileManager;
    public List<Vector3> splinPoints;

    private void OnDrawGizmos()
    {
        int numberOfLines = 40;
        Vector3 previousPoint = p1.position;

        for (int i = 0; i < numberOfLines; i++)
        {
            float t = (float)i / numberOfLines * 2;

            Vector3 newPoint = Spline.GetPoint(p1.position, p2.position, p3.position, p4.position, t);
            Gizmos.DrawLine(previousPoint, newPoint);
            previousPoint = newPoint;
        }
    }
}
