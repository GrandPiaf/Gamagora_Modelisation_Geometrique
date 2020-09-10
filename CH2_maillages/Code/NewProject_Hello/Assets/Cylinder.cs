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
        int nbVertices = 2 * meridien + 2;
        int nbTriangles = 2 * meridien * 3 + 2 * meridien * 3;

        Vector3[] vertices = new Vector3[nbVertices]; //Central + border vertices
        int[] triangles = new int[nbTriangles]; //Central + border triangles

        //Compute teta angle offset
        double tetaOffset = (2 * Math.PI) / meridien;

        //For lower vertices
        float zL = -height / 2;

        //For upper vertices
        float zU = height / 2;


        //Generating central vertices
        for (int m = 0; m < meridien; ++m)
        {
            int index = 2 * m;

            //Compute real angle
            double angle = tetaOffset * m;

            //Coordinates
            float x = Convert.ToSingle( rayon * Math.Cos(angle) );
            float y = Convert.ToSingle( rayon * Math.Sin(angle) );

            //Lower vertex
            vertices[index] = new Vector3(x, y, zL);

            //Upper vertex
            vertices[index + 1] = new Vector3(x, y, zU);

        }

        //Adding border vertices
        int centralLowerindex = 2 * meridien;
        int centralUpperindex = 2 * meridien + 1;
        vertices[centralLowerindex] = new Vector3(0, 0, zL);
        vertices[centralUpperindex] = new Vector3(0, 0, zU);



        //Generating central triangles
        int indexTri = 0;
        for (int m = 0; m < meridien - 1; ++m)  // Minus 1 to snap the cylinder correctly
        {
            int index = 2 * m;

            int A = index;
            int B = index + 2;
            int C = index + 1;
            int D = index + 3;

            Debug.Log(indexTri);

            triangles[indexTri] = A;
            triangles[indexTri + 1] = B;
            triangles[indexTri + 2] = C;

            triangles[indexTri + 3] = D;
            triangles[indexTri + 4] = C;
            triangles[indexTri + 5] = B;

            indexTri += 6;
        }

        // To snap the cylinder around
        {
            int A = 2 * meridien - 2;
            int B = 0;
            int C = 2 * meridien - 1;
            int D = 1;

            Debug.Log(indexTri);

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
            for (int m = 0; m < meridien - 1; ++m)
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
                int B = 2 * meridien - 2;
                int C = 0;

                triangles[indexTri] = centralLowerindex;
                triangles[indexTri + 1] = C;
                triangles[indexTri + 2] = B;

                indexTri += +3;
            }

            //Upper
            for (int m = 0; m < meridien - 1; ++m)
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
                int B = 2 * meridien - 1;
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

}
