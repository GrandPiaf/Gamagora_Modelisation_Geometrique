using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Simplification : MonoBehaviour
{
    // Rendering material
    public Material mat;

    [Range(1, 100)]
    public int nbCell = 10;
    private int oldNbCell = 0;

    private CubeGrid[, ,] grid;
    private Vector3 lowerBorder = new Vector3(-1, -1, -1);
    private Vector3 upperBorder = new Vector3(1, 1, 1);
    private float offset;

    private Mesh originalMesh;
    private Mesh simplifiedMesh;

    void Start() {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        gameObject.GetComponent<MeshRenderer>().material = mat;
        originalMesh = OFFLoader.ReadOFF("Assets/Common/OFFMeshes/max.off");
        gameObject.GetComponent<MeshFilter>().mesh = originalMesh;
    }


    void Update() {

        if(oldNbCell != nbCell) {
            oldNbCell = nbCell;
            simplifiedMesh = new Mesh();
            simplifiedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            Simplify();
        }
    }


    private void Simplify() {
        // Setting lower & upper borders
        SetBorders();

        // CreateGrid
        CreateGrid();

        // Add every vertices in their correct cube
        // Also compute average position in cube
        SortVertices();

        // This is a middle step to check if everythings works fine
        DrawCubes(true);

        // Simplify triangles
        SimplifyTriangles();

        simplifiedMesh.RecalculateNormals();

        gameObject.GetComponent<MeshFilter>().mesh = simplifiedMesh;
    }

    private void SimplifyTriangles() {

        List<int> newTriangles = new List<int>();

        for (int i = 0; i < originalMesh.triangles.Length; i+=3) {

            int indexA = Getindex(GetIndexesVertex(originalMesh.vertices[originalMesh.triangles[i]]));
            int indexB = Getindex(GetIndexesVertex(originalMesh.vertices[originalMesh.triangles[i+1]]));
            int indexC = Getindex(GetIndexesVertex(originalMesh.vertices[originalMesh.triangles[i+2]]));

            if (!IsSameCell(indexA, indexB, indexC)) {

                newTriangles.Add(indexA);
                newTriangles.Add(indexB);
                newTriangles.Add(indexC);
            
            }

        }

        simplifiedMesh.triangles = newTriangles.ToArray();

    }

    private bool IsSameCell(int indexA, int indexB, int indexC) {

        if (indexA == indexB || indexB == indexC || indexC == indexA) {
            return true;
        }

        return false;
    }

    private int Getindex(Vector3 vertex) {
        return grid[Mathf.FloorToInt(vertex.x), Mathf.FloorToInt(vertex.y), Mathf.FloorToInt(vertex.z)].indexVertex;
    }

    private void DrawCubes(bool withAverage = false) {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        float offset = (upperBorder.x - lowerBorder.x) / nbCell;
        cube.transform.localScale = new Vector3(offset, offset, offset);
        
        for (int i = 0; i < nbCell; i++) {
            float coordX = lowerBorder.x + i * offset + (offset/2);

            for (int j = 0; j < nbCell; j++) {
                float coordY = lowerBorder.y + j * offset + (offset / 2);

                for (int k = 0; k < nbCell; k++) {
                    float coordZ = lowerBorder.z + k * offset + (offset / 2);

                    if (grid[i, j, k].number > 0) {

                        Vector3 cubeCenter;
                        if (withAverage) {
                            cubeCenter = grid[i, j, k].average;
                        } else {
                            cubeCenter = new Vector3(coordX, coordY, coordZ);
                        }
                        Instantiate(cube, cubeCenter, Quaternion.identity);

                    }
                }
            }
        }

        Destroy(cube);
    }

    private void SortVertices() {

        // Sort vertices in corresponding cubes
        foreach (Vector3 vertex in originalMesh.vertices) {

            Vector3 indexes = GetIndexesVertex(vertex);

            // Add vertex to found CubeGrid
            grid[Mathf.FloorToInt(indexes.x), Mathf.FloorToInt(indexes.y), Mathf.FloorToInt(indexes.z)].Add(vertex);

        }


        // Compute average position in cube containing vertices
        // Also set the vertex index
        // And fill the new vertices list

        List<Vector3> newVertices = new List<Vector3>();
        int index = 0;

        for (int i = 0; i < nbCell; i++) {
            for (int j = 0; j < nbCell; j++) {
                for (int k = 0; k < nbCell; k++) {

                    grid[i, j, k].ComputeAverage();
                    newVertices.Add(grid[i, j, k].average);
                    grid[i, j, k].indexVertex = index;
                    index++;

                }
            }
        }

        simplifiedMesh.vertices = newVertices.ToArray();

    }

    private Vector3 GetIndexesVertex(Vector3 vertex) {
        // Get CubeGrid index from vertex coordinates
        Vector3 temp = vertex - lowerBorder; // Simplified from vertex + (Vector3.zero - lowerBorder);
        return (temp / offset);
    }

    private void CreateGrid() {
        grid = new CubeGrid[nbCell, nbCell, nbCell];
        for (int i = 0; i < nbCell; i++) {
            for (int j = 0; j < nbCell; j++) {
                for (int k = 0; k < nbCell; k++) {
                    grid[i, j, k] = new CubeGrid();
                }
            }
        }
    }


    private void SetBorders() {
        // Now just a simple version
        // In a way it works for every mesh read from my custom OFFLoader script
        lowerBorder = new Vector3(-1, -1, -1);
        upperBorder = new Vector3(1, 1, 1);
        offset = (upperBorder.x - lowerBorder.x) / nbCell;
    }



    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;

        Vector3 A = new Vector3(upperBorder.x, lowerBorder.y, lowerBorder.z);
        Gizmos.DrawLine(lowerBorder, A);

        Vector3 B = new Vector3(lowerBorder.x, lowerBorder.y, upperBorder.z);
        Gizmos.DrawLine(lowerBorder, B);

        Vector3 C = new Vector3(lowerBorder.x, upperBorder.y, lowerBorder.z);
        Gizmos.DrawLine(lowerBorder, C);


        Vector3 D = new Vector3(lowerBorder.x, upperBorder.y, upperBorder.z);
        Gizmos.DrawLine(upperBorder, D);

        Vector3 E = new Vector3(upperBorder.x, lowerBorder.y, upperBorder.z);
        Gizmos.DrawLine(upperBorder, E);

        Vector3 F = new Vector3(upperBorder.x, upperBorder.y, lowerBorder.z);
        Gizmos.DrawLine(upperBorder, F);

        Gizmos.DrawLine(A, E);
        Gizmos.DrawLine(A, F);

        Gizmos.DrawLine(B, E);
        Gizmos.DrawLine(C, F);

        Gizmos.DrawLine(B, D);
        Gizmos.DrawLine(C, D);
    }

}
