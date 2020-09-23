using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class OFF_Loader : MonoBehaviour
{

    public Material mat;
    public string fileName;

    string oldFileName = "";


    void Update()
    {
        if (oldFileName != fileName)
        {
            ReadOFF("Assets/OFFMeshes/" + fileName);
            oldFileName = fileName;
        }
    }

    void ReadOFF(string path)
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        Mesh msh = new Mesh();

        /** Reading file **/
        try
        {
            // Opening file
            using (StreamReader sr = new StreamReader(path))
            {

                // Read first line "OFF"
                string line = sr.ReadLine();
                if (!line.Equals("OFF"))
                {
                    Debug.Log("Not a OFF file !");
                    return;
                }


                // Read second lines : file data
                line = sr.ReadLine();
                int[] fileData = Array.ConvertAll(line.Split(' '), int.Parse);
                if (fileData.Length != 3)
                {
                    Debug.Log("Cannot read file correctly (wrong parameters number)");
                    return;
                }

                int verticesCount = fileData[0];
                int trianglesCount = fileData[1];
                int edgeCount = fileData[2];

                //Debug.Log("Vertices : " + verticesCount);
                //Debug.Log("Triangles : " + trianglesCount);
                //Debug.Log("Edges : " + edgeCount);


                // Create arrays
                Vector3[] vertices = new Vector3[verticesCount];
                int[] triangles = new int[trianglesCount * 3];


                // Read vertices
                for (int v = 0; v < verticesCount; ++v)
                {
                    line = sr.ReadLine();
                    string[] lineSplit = line.Split(' ');
                    if (lineSplit.Length < 3)
                    {
                        Debug.Log("Cannot read vertex " + v + " correctly (wrong coordinates number)");
                        return;
                    }

                    NumberFormatInfo format = new NumberFormatInfo();
                    format.NumberDecimalSeparator = ".";
                    format.NegativeSign = "-";

                    vertices[v] = new Vector3();
                    float.TryParse(lineSplit[0], NumberStyles.Float, format, out vertices[v].x);
                    float.TryParse(lineSplit[1], NumberStyles.Float, format, out vertices[v].y);
                    float.TryParse(lineSplit[2], NumberStyles.Float, format, out vertices[v].z);
                }


                // Read triangles
                for (int t = 0; t < trianglesCount * 3; t += 3)
                {
                    line = sr.ReadLine();
                    string[] lineSplit = line.Split(' ');

                    if (lineSplit.Length < 4)
                    {
                        Debug.Log("Cannot read triangle " + t + " correctly (wrong index number)");
                        return;
                    }

                    int indexNumber;
                    int.TryParse(lineSplit[0], out indexNumber);

                    if (indexNumber != 3)
                    {
                        Debug.Log("Index numbers triangle " + t + " incorrect");
                        return;
                    }

                    int.TryParse(lineSplit[1], out triangles[t]);
                    int.TryParse(lineSplit[2], out triangles[t + 1]);
                    int.TryParse(lineSplit[3], out triangles[t + 2]);
                }

                msh.vertices = vertices;
                msh.triangles = triangles;

            }
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
        }


        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du matériel
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }

    public void traceMaillage()
    {
        Mesh m = gameObject.GetComponent<MeshFilter>().mesh;

        Debug.Log("Printing mesh data");
        Debug.Log("Name : " + fileName);
        Debug.Log("Vertices count : " + m.vertexCount);
        Debug.Log("Triangles count : " + (m.triangles.Length / 3) );

        Debug.Log("*** Enumerating vertices in format : INDEX | (x, y, z) ***");
        for (int i = 0; i < m.vertexCount; ++i) {
            Debug.Log(i + " | " + m.vertices[i]);
        }

        Debug.Log("*** Enumerating triangles in format : A - B - C ***");
        for (int i = 0; i < m.triangles.Length; i += 3) {
            Debug.Log(m.triangles[i] + " - " + m.triangles[i + 1] + " - " + m.triangles[i + 2]);
        }

    }
}