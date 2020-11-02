using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exo3 : MonoBehaviour {
    [Range(0, 10)]
    public float edgeSize = 1f;

    public int threshold = 200;

    private BBox bbox;

    List<GameObject> volume;

    // Start is called before the first frame update
    void Start() {
        volume = new List<GameObject>();

        List<Sphere> sphereList = new List<Sphere>();
        sphereList.Add(new Sphere(new Vector3(-1, -1, -1), 1, 200));
        sphereList.Add(new Sphere(new Vector3(0, 1, 0), 2, 30));

        bbox = new BBox(sphereList);

        bbox.computeBorders();
        computeCells();
    }

    public void computeCells() {

        int nbCellX = Mathf.CeilToInt((bbox.maxBorder.x - bbox.minBorder.x) / edgeSize);
        int nbCellY = Mathf.CeilToInt((bbox.maxBorder.y - bbox.minBorder.y) / edgeSize);
        int nbCellZ = Mathf.CeilToInt((bbox.maxBorder.z - bbox.minBorder.z) / edgeSize);

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3(edgeSize, edgeSize, edgeSize);

        for (int i = 0; i < nbCellX; i++) {

            float coordX = bbox.minBorder.x + i * edgeSize;

            for (int j = 0; j < nbCellY; j++) {

                float coordY = bbox.minBorder.y + j * edgeSize;

                for (int k = 0; k < nbCellZ; k++) {

                    float coordZ = bbox.minBorder.z + k * edgeSize;

                    Vector3 cubeCenter = new Vector3(coordX, coordY, coordZ);

                    int pot = bbox.Potential(cubeCenter);
                    if (pot >= threshold) {
                        volume.Add(Instantiate(cube, cubeCenter, Quaternion.identity));
                    }
                }
            }
        }

        Destroy(cube);

    }

    private void OnDrawGizmos() {

        if (bbox == null) {
            return;
        }

        foreach (Sphere sphere in bbox.sphereList) {
            Gizmos.DrawWireSphere(sphere.origin, sphere.radius);
        }

        int nbCellX = Mathf.CeilToInt((bbox.maxBorder.x - bbox.minBorder.x) / edgeSize);
        int nbCellY = Mathf.CeilToInt((bbox.maxBorder.y - bbox.minBorder.y) / edgeSize);
        int nbCellZ = Mathf.CeilToInt((bbox.maxBorder.z - bbox.minBorder.z) / edgeSize);

        float coordX = bbox.minBorder.x + (nbCellX * edgeSize);
        float coordY = bbox.minBorder.y + (nbCellY * edgeSize);
        float coordZ = bbox.minBorder.z + (nbCellZ * edgeSize);

        Vector3 minBorderReal = bbox.minBorder;
        Vector3 maxBorderReal = new Vector3(coordX, coordY, coordZ);

        Gizmos.color = Color.magenta;

        Vector3 A = new Vector3(maxBorderReal.x, minBorderReal.y, minBorderReal.z);
        Gizmos.DrawLine(minBorderReal, A);

        Vector3 B = new Vector3(minBorderReal.x, minBorderReal.y, maxBorderReal.z);
        Gizmos.DrawLine(minBorderReal, B);

        Vector3 C = new Vector3(minBorderReal.x, maxBorderReal.y, minBorderReal.z);
        Gizmos.DrawLine(minBorderReal, C);


        Vector3 D = new Vector3(minBorderReal.x, maxBorderReal.y, maxBorderReal.z);
        Gizmos.DrawLine(maxBorderReal, D);

        Vector3 E = new Vector3(maxBorderReal.x, minBorderReal.y, maxBorderReal.z);
        Gizmos.DrawLine(maxBorderReal, E);

        Vector3 F = new Vector3(maxBorderReal.x, maxBorderReal.y, minBorderReal.z);
        Gizmos.DrawLine(maxBorderReal, F);

        Gizmos.DrawLine(A, E);
        Gizmos.DrawLine(A, F);

        Gizmos.DrawLine(B, E);
        Gizmos.DrawLine(C, F);

        Gizmos.DrawLine(B, D);
        Gizmos.DrawLine(C, D);
    }

}
