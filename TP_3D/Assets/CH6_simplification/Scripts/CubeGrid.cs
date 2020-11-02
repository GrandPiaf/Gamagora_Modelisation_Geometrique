using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGrid {

    public Vector3 average;
    public int number;
    public int indexVertex;

    public CubeGrid() {
        average = Vector3.zero;
        number = 0;
        indexVertex = -1;
    }

    internal void Add(Vector3 vertex) {
        average += vertex;
        number++;
    }

    internal void ComputeAverage() {
        if (number > 0) {
            average /= number;
        }
    }
}
