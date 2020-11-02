using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere
{
    public Vector3 origin;
    public float radius;
    public int potential;

    public Sphere(Vector3 origin, float radius, int potential) {
        this.origin = origin;
        this.radius = radius;
        this.potential = potential;
    }
}
