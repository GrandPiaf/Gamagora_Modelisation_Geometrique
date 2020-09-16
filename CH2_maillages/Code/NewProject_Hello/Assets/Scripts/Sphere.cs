using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class Sphere : MonoBehaviour
{

    [Range(1, 100)]
    public float rayon;

    [Range(3, 100)]
    public int meridiens;

    [Range(2, 100)]
    public int paralleles;

    [Range(0, 360)]
    public float cutAngle;

    float oldRayon = 0;
    int oldMeridiens = 0;
    int oldParalleles = 0;

    public Material mat;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();

        //createSurface();

        gameObject.GetComponent<MeshRenderer>().material = mat;
    }


    void createSurface()
    {

        int nbVertices = paralleles * meridiens     +   2; //Center + North & South
        int nbTriangles = ( (paralleles - 1) * meridiens * 2   +   meridiens * 2 ) * 3; // (Center + Borders) * 3

        Vector3[] vertices = new Vector3[nbVertices]; //Central + border vertices
        int[] triangles = new int[nbTriangles]; //Central + border triangles

        //Compute teta angle offset
        double tetaOffset = (2 * Math.PI) / meridiens;

        //compute phi angle offset
        double phiOffset = (Math.PI) / (paralleles + 1); // +1 to avoid Nortt and South



        //Generating central vertices
        for (int p = 0; p < paralleles; ++p)
        {
            double phiAngle = phiOffset * (p+1);

            for (int m = 0; m < meridiens; ++m)
            {
                double tetaAngle = tetaOffset * m;

                int index = p * meridiens + m;

                float x = rayon *  Convert.ToSingle(Math.Sin(phiAngle) * Math.Cos(tetaAngle));
                float y = rayon *  Convert.ToSingle(Math.Sin(phiAngle) * Math.Sin(tetaAngle));
                float z = rayon *  Convert.ToSingle(Math.Cos(phiAngle));

                vertices[index] = new Vector3(x, y, z);

            }
        }

        //Adding North and South vertices
        int indexNorth = nbVertices - 2;
        int indexSouth = nbVertices - 1;
        vertices[indexNorth] = new Vector3(0, 0, rayon); //North
        vertices[indexSouth] = new Vector3(0, 0, -rayon); //South


        //Generating central triangles
        for (int p = 0; p < paralleles - 1; ++p)
        {
            int A, B, C, D, index;

            for (int m = 0; m < meridiens - 1; ++m)
            {
                A = p * meridiens + m;
                B = A + 1;
                C = A + meridiens;
                D = C + 1;

                index = (p * meridiens + m) * 6;

                triangles[index] = A;
                triangles[index + 1] = C;
                triangles[index + 2] = B;

                triangles[index + 3] = D;
                triangles[index + 4] = B;
                triangles[index + 5] = C;
            }

            A = (p+1) * meridiens - 1;
            B = p * meridiens;
            C = A + meridiens;
            D = B + meridiens;

            index = (p * meridiens + (meridiens - 1)) * 6;

            triangles[index] = A;
            triangles[index + 1] = C;
            triangles[index + 2] = B;

            triangles[index + 3] = D;
            triangles[index + 4] = B;
            triangles[index + 5] = C;
        }

        //Adding North and South triangles
        Debug.Log("NORTH SOUTH");
        for (int m = 0; m < meridiens - 1; ++m)
        {
            int index = (paralleles - 1) * meridiens * 6 + m * 6;

            int A = m;
            int B = A + 1;

            triangles[index] = indexNorth;
            triangles[index + 1] = A;
            triangles[index + 2] = B;

            int C = (paralleles - 1) * meridiens + m;
            int D = C + 1;

            triangles[index + 3] = indexSouth;
            triangles[index + 4] = D;
            triangles[index + 5] = C;
        }
        {
            int index = (paralleles - 1) * meridiens * 6 + (meridiens - 1) * 6;

            int A = meridiens - 1;
            int B = 0;

            triangles[index] = indexNorth;
            triangles[index + 1] = A;
            triangles[index + 2] = B;

            int C = (paralleles - 1) * meridiens + (meridiens - 1);
            int D = (paralleles - 1) * meridiens;

            triangles[index + 3] = indexSouth;
            triangles[index + 4] = D;
            triangles[index + 5] = C;
        }


        Mesh msh = new Mesh();                          // Création et remplissage du Mesh

        msh.vertices = vertices;
        msh.triangles = triangles;

        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du matériel

    }

    void Update()
    {
        if (oldRayon != rayon || oldParalleles != paralleles || oldMeridiens != meridiens)
        {
            createSurface();

            oldRayon = rayon;
            oldParalleles = paralleles;
            oldMeridiens = meridiens;
        }
    }

}
