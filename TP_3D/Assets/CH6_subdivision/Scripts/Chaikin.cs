using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaikin : MonoBehaviour
{

    public List<Vector3> verticesOriginal;

    private List<Vector3> verticesSubdivided;

    [Range(0, 10)]
    public int depth = 3;

    void OnValidate()
    {
        subdivideChaikin();
    }

    private void subdivideChaikin() {

        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        verticesSubdivided = new List<Vector3>(verticesOriginal);

        Vector3 start, end;

        for (int d = 0; d < depth; d++) {

            List<Vector3> nextDepth = new List<Vector3>();

            for (int i = 0; i < verticesSubdivided.Count - 1; i++) {

                start = verticesSubdivided[i];
                end = verticesSubdivided[i + 1];

                nextDepth.Add((start * 3.0f / 4.0f) + (end / 4.0f));
                nextDepth.Add((start / 4.0f) + (end * 3.0f / 4.0f));

            }

            start = verticesSubdivided[verticesSubdivided.Count - 1];
            end = verticesSubdivided[0];

            nextDepth.Add((start * 3.0f / 4.0f) + (end / 4.0f));
            nextDepth.Add((start / 4.0f) + (end * 3.0f / 4.0f));

            verticesSubdivided = nextDepth;

        }
        
        printLines(lineRenderer);
    }

    private void printLines(LineRenderer lineRenderer) {

        /* To print ONLY */
        List<Vector3> verticesRounded = new List<Vector3>();
        verticesRounded.AddRange(verticesSubdivided);
        verticesRounded.Add(verticesRounded[0]);

        lineRenderer.positionCount = verticesRounded.Count;
        lineRenderer.SetPositions(verticesRounded.ToArray());
    }

    void OnDrawGizmos() {
        for (int i = 0; i < verticesOriginal.Count - 1; i++) {
            Gizmos.DrawLine(verticesOriginal[i], verticesOriginal[i+1]);
        }
        Gizmos.DrawLine(verticesOriginal[verticesOriginal.Count-1], verticesOriginal[0]);
    }

}
