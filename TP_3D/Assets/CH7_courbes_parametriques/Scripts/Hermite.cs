using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hermite : MonoBehaviour
{

    public GameObject testPoint;

    public GameObject P0;
    public GameObject P1;

    public Vector3 V0;
    public Vector3 V1;

    [Range(0, 1)]
    public float u;

    void OnValidate() {

        Vector3 PU = F1(u) * P0.transform.position;

        testPoint.transform.position = PU;

    }

    private float F1(float u) {
        return 2 * Mathf.Pow(u, 3) - 3 * Mathf.Pow(u, 2) + 1;
    }

    private float F2(float u) {
        return -2 * Mathf.Pow(u, 3) + 3 * Mathf.Pow(u, 2);
    }

    private float F3(float u) {
        return 2 * Mathf.Pow(u, 3) - 3 * Mathf.Pow(u, 2) + 1;
    }

    private float F4(float u) {
        return 2 * Mathf.Pow(u, 3) - 3 * Mathf.Pow(u, 2) + 1;
    }

}
