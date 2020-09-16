using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OFF_Loader : MonoBehaviour
{

    public Material mat;


    void Start()
    {
        ReadOFF("Assets/OFFMeshes/cube.off");
    }

    void ReadOFF(string fileName)
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();


        /** Reading file **/
        try
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line = sr.ReadLine();
                if (!line.Equals("OFF"))
                {
                    Debug.Log("Not a OFF file !");
                    return;
                }

                line = sr.ReadLine();

                uint[] fileData = Array.ConvertAll(line.Split(' '), uint.Parse);

                if(fileData.Length != 3)
                {
                    Debug.Log("Cannot read file correctly (vertices, triangles & edge are incorrect)");
                    return;
                }

                uint verticesCount = fileData[0];
                uint trianglesCount = fileData[1];
                uint edgeCount = fileData[2];

                Debug.Log("Vertices : " + verticesCount);
                Debug.Log("Triangles : " + trianglesCount);
                Debug.Log("Edges : " + edgeCount);

                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    //Debug.Log(line);

                    

                }
            }
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
        }

        Vector3[] vertices = new Vector3[3];
        int[] triangles = new int[3];



        Mesh msh = new Mesh();


        msh.vertices = vertices;
        msh.triangles = triangles;


        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du matériel
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }
}