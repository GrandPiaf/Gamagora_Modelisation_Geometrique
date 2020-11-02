using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{

    public List<GameObject> controlPoints;

    [Range(2, 100)]
    public int nbT;


    private void OnValidate() {
        BezierCurve();
    }

    // n + 1 poitns de controles
    // de degré n

    private void BezierCurve() {

        LineRenderer lr = GetComponent<LineRenderer>();
        List<Vector3> consecutivePosition = new List<Vector3>();

        float Toffset = 1.0f / (nbT - 1);

        for (int i = 0; i < nbT; i++) {
            float t = Toffset * i;
            consecutivePosition.Add(BezierPoint(t));
        }


        lr.positionCount = nbT;
        lr.SetPositions(consecutivePosition.ToArray());
    }

    private Vector3 BezierPoint(float t) {
        Vector3 res = Vector3.zero;
        for (int i = 0; i < controlPoints.Count; i++) {
            res += controlPoints[i].transform.position * BernsteinPoly(i, t);
        }
        return res;
    }

    private float BernsteinPoly(int i, float t) {
        int n = controlPoints.Count - 1;
        return Fact(n) / (Fact(i) * Fact(n - i)) * Mathf.Pow(t, i) * Mathf.Pow(1-t, n-i);
    }

    private int Fact(int k) {
        return k == 0 ? 1 : k * Fact(k-1);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        for (int i = 0; i < controlPoints.Count - 1; i++) {
            Gizmos.DrawLine(controlPoints[i].transform.position, controlPoints[i+1].transform.position);
        }
    }
}
