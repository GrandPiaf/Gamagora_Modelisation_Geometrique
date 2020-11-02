using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class OFFLoader
{
    // Normalizing mesh
    static private void normalizeMesh(ref Vector3[] vertices, ref float squaredMagnitudePoint) {

        // Normalizing with real magnitude
        // 1 sqrt is better than vertices.Lenght sqrt !
        float realMagnitude = Mathf.Sqrt(squaredMagnitudePoint);

        for (int i = 0; i < vertices.Length; i++) {
            vertices[i] /= realMagnitude;
        }

    }

    // To center the mesh around it's own gravity point
    static private void centeringMesh(ref Vector3[] vertices, ref Vector3 gravityCenterPoint) {
        // Gravity point is computed in ReadOFF : it avoids reading every point each time

        // Get translation vector
        Vector3 translate = Vector3.zero - gravityCenterPoint;

        // Translate every vertex
        for (int i = 0; i < vertices.Length; i++) {
            vertices[i] += translate;
        }

    }


    static private void computeNormals(ref Mesh msh) {

        Vector3[] normmals = new Vector3[msh.vertexCount];

        // Triangle normals
        Vector3[] triangleNormals = new Vector3[msh.triangles.Length / 3]; // Divided by 3 to get the real number of triangles, otherwise it represents the number of indices
        
        // List of triangles linked to vertices
        List<int>[] verticesTriangle = new List<int>[msh.vertexCount];

        for (int i = 0; i < verticesTriangle.Length; i++) {
            verticesTriangle[i] = new List<int>();
        }


        /** Compute triangles normals **/
        // Simple cross product
        for (int t = 0; t < triangleNormals.Length; ++t) {

            // Triangle index in Mesh object
            int triangleIndex = t * 3;

            // Retrieving points
            Vector3 A = msh.vertices[ msh.triangles[triangleIndex] ];
            Vector3 B = msh.vertices[ msh.triangles[triangleIndex+1] ];
            Vector3 C = msh.vertices[ msh.triangles[triangleIndex+2] ];

            // Computing vectors
            Vector3 AB = B - A;
            Vector3 AC = C - A;

            // Computing triangle normal
            triangleNormals[t] = Vector3.Cross(AB, AC);
                
            // Adding triangle to vertex lists
            verticesTriangle[ msh.triangles[triangleIndex]   ].Add(t);
            verticesTriangle[ msh.triangles[triangleIndex+1] ].Add(t);
            verticesTriangle[ msh.triangles[triangleIndex+2] ].Add(t);

        }

        /** Compute vertices normals **/
        // Simple average of connected triangle normals
        for (int v = 0; v < verticesTriangle.Length; v++) {

            normmals[v] = Vector3.zero;

            for (int t = 0; t < verticesTriangle[v].Count; t++) {
                normmals[v] += triangleNormals[ verticesTriangle[v][t] ];
            }

            normmals[v] /= verticesTriangle[v].Count;
        }

        msh.SetNormals(normmals);

    }


    // Read file at "path" and compute it's own gravity point
    static public Mesh ReadOFF(string path) {

        Mesh msh = new Mesh();

        /** Reading file **/
        // Opening file
        using (StreamReader sr = new StreamReader(path)) {

            // Read first line "OFF"
            string line = sr.ReadLine();
            if (!line.Equals("OFF")) {
                Debug.Log("Not a OFF file !");
                return null;
            }


            // Read second lines : file data
            line = sr.ReadLine();
            int[] fileData = Array.ConvertAll(line.Split(' '), int.Parse);
            if (fileData.Length != 3) {
                Debug.Log("Cannot read file correctly (wrong parameters number)");
                return null;
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

            Vector3 gravityCenterPoint = Vector3.zero;
            float squaredMagnitudePoint = 0;

            // Read vertices
            for (int v = 0; v < verticesCount; ++v) {
                line = sr.ReadLine();
                string[] lineSplit = line.Split(' ');
                if (lineSplit.Length < 3) {
                    Debug.Log("Cannot read vertex " + v + " correctly (wrong coordinates number)");
                    return null;
                }

                NumberFormatInfo format = new NumberFormatInfo();
                format.NumberDecimalSeparator = ".";
                format.NegativeSign = "-";

                vertices[v] = new Vector3();
                float.TryParse(lineSplit[0], NumberStyles.Float, format, out vertices[v].x);
                float.TryParse(lineSplit[1], NumberStyles.Float, format, out vertices[v].y);
                float.TryParse(lineSplit[2], NumberStyles.Float, format, out vertices[v].z);

                // Sum of points for further centering
                gravityCenterPoint += vertices[v];

                // Getting the furthest point
                if (vertices[v].sqrMagnitude > squaredMagnitudePoint) {
                    squaredMagnitudePoint = vertices[v].sqrMagnitude;
                }

            }

            // Getting real coordinates of gravity center point
            gravityCenterPoint /= verticesCount;

            // Centering the mesh around Vector3.zero
            centeringMesh(ref vertices, ref gravityCenterPoint);

            //Normlaizing mesh in range [-1;1]
            normalizeMesh(ref vertices, ref squaredMagnitudePoint);


            // Read triangles
            for (int t = 0; t < trianglesCount * 3; t += 3) {
                line = sr.ReadLine();
                string[] lineSplit = line.Split(' ');

                if (lineSplit.Length < 4) {
                    Debug.Log("Cannot read triangle " + t + " correctly (wrong index number)");
                    return null;
                }

                int indexNumber;
                int.TryParse(lineSplit[0], out indexNumber);

                if (indexNumber != 3) {
                    Debug.Log("Index numbers triangle " + t + " incorrect");
                    return null;
                }

                int.TryParse(lineSplit[1], out triangles[t]);
                int.TryParse(lineSplit[2], out triangles[t + 1]);
                int.TryParse(lineSplit[3], out triangles[t + 2]);
            }

            msh.vertices = vertices;
            msh.triangles = triangles;

            computeNormals(ref msh);

        }

        return msh;

    }



    // Writing mesh file
    static public void WriteOFF(Mesh msh, string path) {

        using (StreamWriter sw = new StreamWriter(path)) {

            sw.WriteLine("OFF");
            sw.WriteLine(msh.vertexCount + " " + (msh.triangles.Length / 3) + " " + msh.triangles.Length);

            for (int v = 0; v < msh.vertexCount; v++) {
                //sw.WriteLine(msh.vertices[v].x + " " + msh.vertices[v].y + " " + msh.vertices[v].z);

                sw.Write(msh.vertices[v].x.ToString(System.Globalization.CultureInfo.InvariantCulture));
                sw.Write(" ");
                sw.Write(msh.vertices[v].y.ToString(System.Globalization.CultureInfo.InvariantCulture));
                sw.Write(" ");
                sw.Write(msh.vertices[v].z.ToString(System.Globalization.CultureInfo.InvariantCulture));
                sw.Write("\n");
            }

            for (int t = 0; t < msh.triangles.Length; t+=3) {
                sw.WriteLine("3 " + msh.triangles[t] + " " + msh.triangles[t+1] + " " + msh.triangles[t+2]);
            }

            sw.Close();

        }

    }


    // Taking a boolean parameter to print detailed vertices and triangles
    static public void traceMaillage(Mesh msh, bool detailed)
    {

        Debug.Log("Printing mesh data");
        //Debug.Log("Name : " + fileName);
        Debug.Log("Vertices count : " + msh.vertexCount);
        Debug.Log("Triangles count : " + (msh.triangles.Length / 3) );

        /*
         * Count edges
         */
        Counter edges = new Counter(msh);

        Debug.Log("Total Edge count : " + edges.getEdgeNumber().ToString());
        Debug.Log("Edge count per face 3 ");
        Debug.Log("Edge count per vertex / min : " + edges.getMinEdgeCountPerVertex().ToString() + " / max : " + edges.getMaxEdgeCountPerVertex().ToString());
        Debug.Log("Edge count shared by none or one face : " + edges.getEdgeCountSharedNoneOrOneFace().ToString());


        //Debug.Log("Gravity center point: " + gravityCenterPoint);

        if (detailed) {

            Debug.Log("*** Enumerating vertices in format : INDEX | (x, y, z) ***");
            for (int i = 0; i < msh.vertexCount; ++i) {
                Debug.Log(i + " | " + msh.vertices[i]);
            }

            Debug.Log("*** Enumerating triangles in format : A - B - C ***");
            for (int i = 0; i < msh.triangles.Length; i += 3) {
                Debug.Log(msh.triangles[i] + " - " + msh.triangles[i + 1] + " - " + msh.triangles[i + 2]);
            }

        }

    }

    private class Counter {

        private class Edge {
            private int item1;
            private int item2;

            public Edge(int i1, int i2) {
                this.item1 = i1;
                this.item2 = i2;
            }

            public override int GetHashCode() {
                return this.item1.GetHashCode() ^ this.item2.GetHashCode();
            }

            public override bool Equals(object obj) {
                return Equals(obj as Edge);
            }

            public bool Equals(Edge obj) {
                return (this.item1 == obj.item1 && this.item2 == obj.item2)
                    || (this.item1 == obj.item2 && this.item2 == obj.item1);
            }
        }

        // Mesh
        private Mesh mesh;
        
        // Edge count
        private Dictionary<Edge, int> edgeCount;

        // Edge count per vertex
        private int[] edgeCountPerVertex;
        private int minEdgeCountVertex;
        private int maxEdgeCountVertex;

        public Counter(Mesh mesh) {

            this.mesh = mesh;
            this.edgeCount = new Dictionary<Edge, int>();

            this.edgeCountPerVertex = new int[mesh.vertexCount];
            this.minEdgeCountVertex = int.MaxValue;
            this.maxEdgeCountVertex = int.MinValue;

            BuildLists(this.mesh);

        }

        private void BuildLists(Mesh mesh) {

            // Edge count
            for (int i = 0; i < mesh.triangles.Length; i += 3) {
                Edge edgeA = new Edge(mesh.triangles[i], mesh.triangles[i + 1]);
                Edge edgeB = new Edge(mesh.triangles[i + 1], mesh.triangles[i + 2]);
                Edge edgeC = new Edge(mesh.triangles[i + 2], mesh.triangles[i]);

                AddAndCount(edgeA);
                AddAndCount(edgeB);
                AddAndCount(edgeC);


                this.edgeCountPerVertex[mesh.triangles[i]]++;
                this.edgeCountPerVertex[mesh.triangles[i + 1]]++;
                this.edgeCountPerVertex[mesh.triangles[i + 2]]++;
            }

            //Edge count per vertex
            foreach (int v in this.edgeCountPerVertex) {
                this.minEdgeCountVertex = Math.Min(this.minEdgeCountVertex, v);
                this.maxEdgeCountVertex = Math.Max(this.maxEdgeCountVertex, v);
            }

        }

        private void AddAndCount(Edge edge) {
            if (!edgeCount.ContainsKey(edge)) {
                edgeCount.Add(edge, 0);
            }
            edgeCount[edge]++;
        }

        internal int getEdgeCountSharedNoneOrOneFace() {
            int count = 0;
            
            foreach (KeyValuePair<Edge, int> kvp in this.edgeCount) {
                if (kvp.Value != 2) {
                    count++;
                }
            }

            return count;
        }

        internal int getMinEdgeCountPerVertex() {
            return this.minEdgeCountVertex;
        }

        internal int getMaxEdgeCountPerVertex() {
            return this.maxEdgeCountVertex;
        }

        // Return total edge number
        internal int getEdgeNumber() {
            return edgeCount.Count;
        }

    }
}