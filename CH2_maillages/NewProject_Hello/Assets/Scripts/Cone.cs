﻿using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cone : MonoBehaviour
{

    [Range(1, 100)]
    public float rayon;

    [Range(1, 100)]
    public float height;

    [Range(0, 1)]
    // 1 = not truncated
    //between 0 & 0.9999.. it is truncated
    // This is a percentage : it means we trunc the cone at 0.8 percent of the height defined
    public float heightTruncated;

    [Range(3, 100)]
    public int meridiens;

    float oldRayon = 0;
    float oldHeight = 0;
    float oldHeightTruncated = 0;
    int oldMeridiens = 0;

    public Material mat;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();

        //createSurface();

        gameObject.GetComponent<MeshRenderer>().material = mat;
    }


    void createTruncatedCone()
    {

        int nbVertices = 2 * meridiens + 2;
        int nbTriangles = 2 * meridiens * 3 + 2 * meridiens * 3;

        Vector3[] vertices = new Vector3[nbVertices]; //Central + border vertices
        int[] triangles = new int[nbTriangles]; //Central + border triangles

        //Compute teta angle offset
        double tetaOffset = (2 * Math.PI) / meridiens;

        //For lower vertices
        float zL = 0;

        //For upper vertices
        float zU = (height * heightTruncated);

        int index = 0;

        //Generating central vertices
        for (int m = 0; m < meridiens; ++m)
        {

            //Compute real angle
            double angle = tetaOffset * m;

            //Lower vertex
            float xL = rayon * Convert.ToSingle(Math.Cos(angle));
            float yL = rayon * Convert.ToSingle(Math.Sin(angle));
            vertices[index] = new Vector3(xL, yL, zL);
            ++index;

            //Upper vertex
            float xU = rayon * (1 - heightTruncated) * Convert.ToSingle(Math.Cos(angle));
            float yU = rayon * (1 - heightTruncated) * Convert.ToSingle(Math.Sin(angle));
            vertices[index] = new Vector3(xU, yU, zU);
            ++index;

        }

        //Adding border vertices
        int centralLowerindex = index;
        ++index;
        int centralUpperindex = index;
        ++index; //Technically Useless

        vertices[centralLowerindex] = new Vector3(0, 0, zL);
        vertices[centralUpperindex] = new Vector3(0, 0, zU);


        //Generating central triangles
        int indexTri = 0;
        for (int m = 0; m < meridiens - 1; ++m)  // Minus 1 to snap the cone correctly
        {
            index = 2 * m;

            int A = index;
            int B = index + 2;
            int C = index + 1;
            int D = index + 3;

            triangles[indexTri] = A;
            triangles[indexTri + 1] = D;
            triangles[indexTri + 2] = C;

            triangles[indexTri + 3] = D;
            triangles[indexTri + 4] = A;
            triangles[indexTri + 5] = B;

            indexTri += 6;
        }

        // To snap the cylinder around
        {
            int A = 2 * meridiens - 2;
            int B = 0;
            int C = 2 * meridiens - 1;
            int D = 1;

            triangles[indexTri] = A;
            triangles[indexTri + 1] = B;
            triangles[indexTri + 2] = C;

            triangles[indexTri + 3] = D;
            triangles[indexTri + 4] = C;
            triangles[indexTri + 5] = B;

            indexTri += 6;

        }

        //Adding border triangles
        {

            //Lower
            for (int m = 0; m < meridiens - 1; ++m)
            {
                int B = 2 * m;
                int C = B + 2;

                triangles[indexTri] = centralLowerindex;
                triangles[indexTri + 1] = C;
                triangles[indexTri + 2] = B;

                indexTri += +3;
                
            }
            //Wrap around
            {
                int B = 2 * meridiens - 2;
                int C = 0;

                triangles[indexTri] = centralLowerindex;
                triangles[indexTri + 1] = C;
                triangles[indexTri + 2] = B;

                indexTri += +3;
            }

            //Upper
            for (int m = 0; m < meridiens - 1; ++m)
            {
                int B = 2 * m + 1;
                int C = B + 2;

                triangles[indexTri] = centralUpperindex;
                triangles[indexTri + 1] = B;
                triangles[indexTri + 2] = C;

                indexTri += +3;
            }
            //Wrap around
            {
                int B = 2 * meridiens - 1;
                int C = 1;

                triangles[indexTri] = centralUpperindex;
                triangles[indexTri + 1] = B;
                triangles[indexTri + 2] = C;

                indexTri += +3;
            }
        }

        

        Mesh msh = new Mesh();                          // Création et remplissage du Mesh

        msh.vertices = vertices;
        msh.triangles = triangles;

        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du matériel

    }

    void createCone()
    {

        int nbVertices = meridiens + 2;
        int nbTriangles = meridiens * 3 + meridiens * 3;


        Vector3[] vertices = new Vector3[nbVertices]; //Central + border vertices
        int[] triangles = new int[nbTriangles]; //Central + border triangles

        //Compute teta angle offset
        double tetaOffset = (2 * Math.PI) / meridiens;

        //For lower vertices
        float zL = 0;

        //For upper vertices
        float zU = (height * heightTruncated);

        int index = 0;

        //Generating central vertices
        for (int m = 0; m < meridiens; ++m)
        {

            //Compute real angle
            double angle = tetaOffset * m;

            //Lower vertex
            float xL = rayon * Convert.ToSingle(Math.Cos(angle));
            float yL = rayon * Convert.ToSingle(Math.Sin(angle));
            vertices[index] = new Vector3(xL, yL, zL);
            ++index;

        }

        //Adding border vertices
        int centralLowerindex = index;
        ++index;
        int centralUpperindex = index;
        ++index; //Technically Useless

        vertices[centralLowerindex] = new Vector3(0, 0, zL);
        vertices[centralUpperindex] = new Vector3(0, 0, zU);


        //Generating central triangles
        int indexTri = 0;
        for (int m = 0; m < meridiens - 1; ++m)  // Minus 1 to snap the cone correctly
        {
            int A = m;
            int B = m + 1;

            triangles[indexTri] = centralUpperindex;
            triangles[indexTri + 1] = A;
            triangles[indexTri + 2] = B;

            indexTri += 3;
        }

        // To snap the cylinder around
        {
            int A = meridiens - 1;
            int B = 0;

            triangles[indexTri] = centralUpperindex;
            triangles[indexTri + 1] = A;
            triangles[indexTri + 2] = B;

            indexTri += 3;
        }

        //Adding border triangles
        {

            //Lower
            for (int m = 0; m < meridiens - 1; ++m)
            {
                int B = m;
                int C = m + 1;

                triangles[indexTri] = centralLowerindex;
                triangles[indexTri + 1] = C;
                triangles[indexTri + 2] = B;

                indexTri += +3;

            }
            //Wrap around
            {
                int B = meridiens - 1;
                int C = 0;

                triangles[indexTri] = centralLowerindex;
                triangles[indexTri + 1] = C;
                triangles[indexTri + 2] = B;

                indexTri += +3;
            }

        }



        Mesh msh = new Mesh();                          // Création et remplissage du Mesh

        msh.vertices = vertices;
        msh.triangles = triangles;

        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du matériel

    }

    void Update()
    {
        if (oldRayon != rayon || oldHeight != height || oldMeridiens != meridiens || oldHeightTruncated != heightTruncated)
        {
            if (heightTruncated != 1.0f) // Full cone    // Can optimize a bit
            {
                createTruncatedCone();
            }
            else
            {
                createCone();
            }

            Debug.Log(gameObject.GetComponent<MeshFilter>().mesh.vertexCount);
            Debug.Log(gameObject.GetComponent<MeshFilter>().mesh.triangles.Length);

            oldRayon = rayon;
            oldHeight = height;
            oldMeridiens = meridiens;
            oldHeightTruncated = heightTruncated;
        }
    }

}