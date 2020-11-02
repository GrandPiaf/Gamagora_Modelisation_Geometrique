using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBox
{

    public List<Sphere> sphereList;

    public Vector3 minBorder;
    public Vector3 maxBorder;

    public BBox(List<Sphere> sphereList) {
        this.sphereList = sphereList;
    }

    public void computeBorders() {

        if(sphereList.Count == 0) {
            Debug.LogError("EMPTY BBOX LIST");
            return;
        }

        // Just setting a simple value but NOT Vector3.zero to avoid some undefined behaviors
        minBorder = sphereList[0].origin;
        maxBorder = sphereList[0].origin;

        foreach (Sphere sphere in sphereList) {

            minBorder = Vector3.Min(minBorder, sphere.origin - sphere.radius * 1.2f * Vector3.one);
            maxBorder = Vector3.Max(maxBorder, sphere.origin + sphere.radius * 1.2f * Vector3.one);

        }

    }

    public bool Union(Vector3 cubeCenter) {
        foreach (Sphere sphere in sphereList) {

            if(Vector3.Distance(cubeCenter, sphere.origin) <= sphere.radius) {
                return true;
            }

        }
        return false;
    }


    public bool Intersection(Vector3 cubeCenter) {

        foreach (Sphere sphere in sphereList) {

            if (Vector3.Distance(cubeCenter, sphere.origin) > sphere.radius) {
                return false;
            }

        }

        return true;
    }

    public int Potential(Vector3 cubeCenter) {

        int potentialSum = 0;

        foreach (Sphere sphere in sphereList) {
            if (Vector3.Distance(cubeCenter, sphere.origin) <= sphere.radius) {
                potentialSum += sphere.potential;
            }
        }

        return potentialSum;
    }


}
