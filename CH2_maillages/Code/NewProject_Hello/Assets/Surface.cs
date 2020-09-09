using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Surface : MonoBehaviour
{

    [Range(0, 100)]
    public int nbColumns;

    [Range(0, 100)]
    public int nbLines;

    public Vector3 from;
    public Vector3 to;

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

        int nbVertexColumn = nbColumns + 1;
        int nbVertexLine = nbLines + 1;

        float offsetX = (to.x - from.x) / nbColumns;
        float offsetY = (to.y - from.y) / nbLines;

        Vector3[] vertices = new Vector3[nbVertexColumn * nbVertexLine];            // Création des structures de données qui accueilleront sommets et  triangles
        int[] triangles = new int[nbColumns * nbLines * 2 * 3];


        //Generating vertices
        for (int j = 0; j < nbVertexLine; ++j)
        {
            for (int i = 0; i < nbVertexColumn; ++i)
            {
                int index = j * nbVertexColumn + i;
                float x = i * offsetX;
                float y = j * offsetY;
                float z = 0;
                vertices[index] = new Vector3(x, y, z);
                Debug.Log("(" + x + ", " + y + ", " + z + ")");
            }
        }

        //Generating triangles
        for (int j = 0; j < nbLines; ++j)
        {
            for (int i = 0; i < nbColumns; ++i)
            {
                int index = (j * nbColumns * 6) + (i * 6);

                int A = j * nbVertexColumn + i;
                int B = A + 1;
                int C = A + nbVertexColumn;
                int D = C + 1;

                triangles[index    ] = A;
                triangles[index + 1] = C;
                triangles[index + 2] = B;

                triangles[index + 3] = D;
                triangles[index + 4] = B;
                triangles[index + 5] = C;
            }
        }

        Mesh msh = new Mesh();                          // Création et remplissage du Mesh

        msh.vertices = vertices;
        msh.triangles = triangles;

        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du matériel

    }

}
