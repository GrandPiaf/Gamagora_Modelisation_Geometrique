using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{
    [Range(0, 10)]
    public float edgeSize = 1f;

    // Start is called before the first frame update
    void Start()
    {
        List<Sphere> sphereList = new List<Sphere>();
        sphereList.Add(new Sphere( new Vector3(-1, -1, -1), 1) );
        sphereList.Add(new Sphere( new Vector3(1, 2, 0), 2) );

        BBox bbox = new BBox(sphereList);

        bbox.computeBorders();
        computeCells(ref bbox, edgeSize);
    }

    public void computeCells(ref BBox bbox, float edgeSize) {

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

                    if (bbox.ContainsCube(cubeCenter)) {
                        Instantiate(cube, cubeCenter, Quaternion.identity, transform);
                    }
                }
            }
        }

        Destroy(cube);

    }
}
