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

        string filename = "cube.off";

        gameObject.GetComponent<MeshFilter>().mesh = OFFLoader.ReadOFF("Assets/Common/OFFMeshes/" + filename);
        //OFFLoader.WriteOFF(gameObject.GetComponent<MeshFilter>().mesh, "Assets/Common/OFFMeshesResults/new" + filename);

        OFFLoader.traceMaillage(gameObject.GetComponent<MeshFilter>().mesh, false);
    
    }

}
