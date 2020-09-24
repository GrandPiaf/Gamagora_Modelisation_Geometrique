using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMesh : MonoBehaviour
{
    // Rendering material
    public Material mat;

    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().material = mat;

        //gameObject.GetComponent<MeshFilter>().mesh = CustomMeshCreator.CreateSphere(4, 8, 3);
    }

}
