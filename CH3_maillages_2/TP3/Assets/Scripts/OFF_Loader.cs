using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OFF_Loader : MonoBehaviour
{

    public Material mat;

    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();


        /** Reading file **/

        Vector3[] vertices = new Vector3[3];
        int[] triangles = new int[3];



        Mesh msh = new Mesh();


        msh.vertices = vertices;
        msh.triangles = triangles;


        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du matériel
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }
}