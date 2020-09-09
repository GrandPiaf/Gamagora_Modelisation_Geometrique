using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cylinder : MonoBehaviour
{

    [Range(0, 100)]
    public float rayon;

    [Range(0, 100)]
    public float height;

    [Range(0, 100)]
    public int meridien;


    public Material mat;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();

        createSurface();

        gameObject.GetComponent<MeshRenderer>().material = mat;
    }


    void createSurface()
    {
        Vector3[] vertices = new Vector3[2 * meridien + 2];     // 2 times each merdiens + 2 central vertices
        int[] triangles = new int[2 * meridien * 6 /*+ 2 * meridien*/]; // 2 times each meridiens for triangle + 2 times merdiens for limit triangles

        //Compute teta angle offset
        double tetaOffset = (2 * Math.PI) / meridien;

        //Generating vertices
        for (int m = 0; m < meridien; ++m)
        {
            int index = 2 * m;

            //Compute real angle
            double angle = tetaOffset * m;

            //Lower vertex
            float x = Convert.ToSingle( rayon * Math.Cos(angle) );
            float y = Convert.ToSingle( rayon * Math.Sin(angle) );
            float zL = -height / 2;

            vertices[index] = new Vector3(x, y, zL);

            //Upper vertex
            float zU = height / 2;

            vertices[index + 1] = new Vector3(x, y, zU);

        }

        // Central vertices
        {
            int index = 2 * meridien;
            vertices[index] = new Vector3(0, 0, -height / 2);
            vertices[index + 1] = new Vector3(0, 0, height / 2);
        }


        //Generating triangles
        for (int m = 0; m < meridien - 1; ++m)  // Minus 1 to snap the cylinder correctly
        {
            int index = 2 * m;

            int A = index;
            int B = index + 2;
            int C = index + 1;
            int D = index + 3;

            int indexTri = 2 * m * 6;

            triangles[indexTri] = A;
            triangles[indexTri + 1] = B;
            triangles[indexTri + 2] = C;

            triangles[indexTri + 3] = D;
            triangles[indexTri + 4] = C;
            triangles[indexTri + 5] = B;
        }

        // To snap the cylinder around
        {
            int A = 2 * meridien - 2;
            int B = 0;
            int C = 2 * meridien - 1;
            int D = 1;

            int indexTri = 2 * (meridien - 1) * 6;

            triangles[indexTri] = A;
            triangles[indexTri + 1] = B;
            triangles[indexTri + 2] = C;

            triangles[indexTri + 3] = D;
            triangles[indexTri + 4] = C;
            triangles[indexTri + 5] = B;
        }

        // Central surfaces
        for (int m = 0; m < meridien - 1; ++m)
        {
            
        }

        // To snap the cylinder around
        {
            
        }

        Mesh msh = new Mesh();                          // Création et remplissage du Mesh

        msh.vertices = vertices;
        msh.triangles = triangles;

        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du matériel

    }

}
