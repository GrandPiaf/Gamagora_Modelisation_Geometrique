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
    public int meridien;

    [Range(2, 100)]
    public int parallele;

    float oldRayon = 0;
    int oldMeridien = 0;
    int oldParallele = 0;

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
        int nbVertices = parallele * meridien + 2;
        int nbTriangles = parallele * meridien + parallele * meridien;

        Vector3[] vertices = new Vector3[nbVertices]; //Central + border vertices
        int[] triangles = new int[nbTriangles]; //Central + border triangles

        //Compute teta angle offset
        double tetaOffset = (2 * Math.PI) / meridien;

        //Compute teta angle offset
        double phiOffset = (Math.PI) / (parallele-1);


        //Generating vertices
        //Le parcours est de "haut en bas en descendant en cercle

        for (int p = 0; p < parallele; ++p)
        {
            double phiAngle = p * phiOffset;
            for(int m = 0; m < meridien; ++m)
            {
                double tetaAngle = m * tetaOffset;

                float x = Convert.ToSingle(rayon * Math.Sin(phiAngle) * Math.Cos(tetaAngle));
                float y = Convert.ToSingle(rayon * Math.Sin(phiAngle) * Math.Sin(tetaAngle));
                float z = Convert.ToSingle(rayon * Math.Sin(tetaAngle));

                int index = p * m;
                vertices[index] = new Vector3(x, y, z);
            }
        }


        //North & South vertices
        int indexNorth = 0; //TODO
        vertices[indexNorth] = new Vector3(0, 0, rayon);

        int indexSouth = 0; //TODO
        vertices[indexSouth] = new Vector3(0, 0, -rayon);


        //Generating triangles


        //North & South triangle fans


        Mesh msh = new Mesh();                          // Création et remplissage du Mesh

        msh.vertices = vertices;
        msh.triangles = triangles;

        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du matériel

    }

    void Update()
    {
        if (oldRayon != rayon || oldParallele != parallele || oldMeridien != meridien)
        {
            createSurface();

            oldRayon = rayon;
            oldParallele = parallele;
            oldMeridien = meridien;
        }
    }

}
