using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMesh : MonoBehaviour
{

    public Material mat;



    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().material = mat;

        //gameObject.GetComponent<MeshFilter>().mesh = CustomMeshCreator.createSurface(10, 5, new Vector3(0, 0, 0), new Vector3(1, 1, 1));
        //gameObject.GetComponent<MeshFilter>().mesh = CustomMeshCreator.createCylinder(10, 2, 2, 4);
        //gameObject.GetComponent<MeshFilter>().mesh = CustomMeshCreator.createSphere(4, 8, 3);
        //gameObject.GetComponent<MeshFilter>().mesh = CustomMeshCreator.CreateCone(10, 2, 3, 0.8f);

        gameObject.GetComponent<MeshFilter>().mesh = OFFLoader.ReadOFF("Assets/OFFMeshes/bunny.off");
        OFFLoader.WriteOFF(gameObject.GetComponent<MeshFilter>().mesh, "Assets/OFFMeshesResults/bunny.off");

    }

}
