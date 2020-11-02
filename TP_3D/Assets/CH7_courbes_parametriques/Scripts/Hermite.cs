using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hermite : MonoBehaviour
{

    public GameObject P0;
    public GameObject P1;

    public Vector3 V0;
    public Vector3 V1;

    [Range(0, 1)]
    public float u;

    [Range(2, 100)]
    public int precision = 2;

    void OnValidate() {

        // Test point position
        Vector3 PU = HermitePoint(u);
        transform.position = PU;

        ComputeLineRenderer();

    }

    private void ComputeLineRenderer() {

        LineRenderer lr = GetComponent<LineRenderer>();

        List<Vector3> consecutivePosition = new List<Vector3>(precision + 1);

        float Uoffset = 1.0f / (precision);

        for (int i = 0; i < precision + 1; i++) {
            consecutivePosition.Add(HermitePoint(Uoffset * i));
        }


        lr.positionCount = precision + 1;
        lr.SetPositions(consecutivePosition.ToArray());
    }

    private Vector3 HermitePoint(float u) {
        return F1(u) * P0.transform.position + F2(u) * P1.transform.position + F3(u) * V0 + F4(u) * V1;
    }

    private float F1(float u) {
        return 2 * Mathf.Pow(u, 3) - 3 * Mathf.Pow(u, 2) + 1;
    }

    private float F2(float u) {
        return -2 * Mathf.Pow(u, 3) + 3 * Mathf.Pow(u, 2);
    }

    private float F3(float u) {
        return Mathf.Pow(u, 3) - 2 * Mathf.Pow(u, 2) + u;
    }

    private float F4(float u) {
        return Mathf.Pow(u, 3) - Mathf.Pow(u, 2);
    }

}
