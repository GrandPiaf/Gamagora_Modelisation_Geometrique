using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMeshCreator
{
    static public Mesh CreateSurface(int nbColumns, int nbLines, Vector3 to, Vector3 from) {

        int nbVertexColumn = nbColumns + 1;
        int nbVertexLine = nbLines + 1;

        float offsetX = (to.x - from.x) / nbColumns;
        float offsetY = (to.y - from.y) / nbLines;

        Vector3[] vertices = new Vector3[nbVertexColumn * nbVertexLine];
        int[] triangles = new int[nbColumns * nbLines * 2 * 3];


        //Generating vertices
        for (int j = 0; j < nbVertexLine; ++j) {
            for (int i = 0; i < nbVertexColumn; ++i) {
                int index = j * nbVertexColumn + i;
                float x = i * offsetX;
                float y = j * offsetY;
                float z = 0;
                vertices[index] = new Vector3(x, y, z);
            }
        }

        //Generating triangles
        for (int j = 0; j < nbLines; ++j) {
            for (int i = 0; i < nbColumns; ++i) {
                int index = (j * nbColumns * 6) + (i * 6);

                int A = j * nbVertexColumn + i;
                int B = A + 1;
                int C = A + nbVertexColumn;
                int D = C + 1;

                triangles[index] = A;
                triangles[index + 1] = D;
                triangles[index + 2] = C;

                triangles[index + 3] = D;
                triangles[index + 4] = A;
                triangles[index + 5] = B;
            }
        }

        Mesh msh = new Mesh();

        msh.vertices = vertices;
        msh.triangles = triangles;

        return msh;
    }

    static public Mesh CreateCylinder(int meridiens, int width, int height, int rayon) {

        int nbVertices = 2 * meridiens + 2;
        int nbTriangles = 2 * meridiens * 3 + 2 * meridiens * 3;

        Vector3[] vertices = new Vector3[nbVertices]; //Central + border vertices
        int[] triangles = new int[nbTriangles]; //Central + border triangles

        //Compute teta angle offset
        double tetaOffset = (2 * Math.PI) / meridiens;

        //For lower vertices
        float zL = -height / 2;

        //For upper vertices
        float zU = height / 2;


        //Generating central vertices
        for (int m = 0; m < meridiens; ++m) {
            int index = 2 * m;

            //Compute real angle
            double angle = tetaOffset * m;

            //Coordinates
            float x = rayon * Convert.ToSingle(Math.Cos(angle));
            float y = rayon * Convert.ToSingle(Math.Sin(angle));

            //Lower vertex
            vertices[index] = new Vector3(x, y, zL);

            //Upper vertex
            vertices[index + 1] = new Vector3(x, y, zU);

        }

        //Adding border vertices
        int centralLowerindex = 2 * meridiens;
        int centralUpperindex = 2 * meridiens + 1;
        vertices[centralLowerindex] = new Vector3(0, 0, zL);
        vertices[centralUpperindex] = new Vector3(0, 0, zU);



        //Generating central triangles
        int indexTri = 0;
        for (int m = 0; m < meridiens - 1; ++m)  // Minus 1 to snap the cylinder correctly
        {
            int index = 2 * m;

            int A = index;
            int B = index + 2;
            int C = index + 1;
            int D = index + 3;

            triangles[indexTri] = A;
            triangles[indexTri + 1] = D;
            triangles[indexTri + 2] = C;

            triangles[indexTri + 3] = D;
            triangles[indexTri + 4] = A;
            triangles[indexTri + 5] = B;

            indexTri += 6;
        }

        // To snap the cylinder around
        {
            int A = 2 * meridiens - 2;
            int B = 0;
            int C = 2 * meridiens - 1;
            int D = 1;

            triangles[indexTri] = A;
            triangles[indexTri + 1] = D;
            triangles[indexTri + 2] = C;

            triangles[indexTri + 3] = D;
            triangles[indexTri + 4] = A;
            triangles[indexTri + 5] = B;

            indexTri += 6;
        }

        //Adding border triangles
        {

            //Lower
            for (int m = 0; m < meridiens - 1; ++m) {
                int B = 2 * m;
                int C = B + 2;

                triangles[indexTri] = centralLowerindex;
                triangles[indexTri + 1] = C;
                triangles[indexTri + 2] = B;

                indexTri += +3;
            }
            //Wrap around
            {
                int B = 2 * meridiens - 2;
                int C = 0;

                triangles[indexTri] = centralLowerindex;
                triangles[indexTri + 1] = C;
                triangles[indexTri + 2] = B;

                indexTri += +3;
            }

            //Upper
            for (int m = 0; m < meridiens - 1; ++m) {
                int B = 2 * m + 1;
                int C = B + 2;

                triangles[indexTri] = centralUpperindex;
                triangles[indexTri + 1] = B;
                triangles[indexTri + 2] = C;

                indexTri += +3;
            }
            //Wrap around
            {
                int B = 2 * meridiens - 1;
                int C = 1;

                triangles[indexTri] = centralUpperindex;
                triangles[indexTri + 1] = B;
                triangles[indexTri + 2] = C;

                indexTri += +3;
            }
        }


        Mesh msh = new Mesh();

        msh.vertices = vertices;
        msh.triangles = triangles;

        return msh;
    }


    static public Mesh CreateSphere(int meridiens, int paralleles, int rayon) {

        int nbVertices = paralleles * meridiens + 2; //Center + North & South
        int nbTriangles = ((paralleles - 1) * meridiens * 2 + meridiens * 2) * 3; // (Center + Borders) * 3

        Vector3[] vertices = new Vector3[nbVertices]; //Central + border vertices
        int[] triangles = new int[nbTriangles]; //Central + border triangles

        //Compute teta angle offset
        double tetaOffset = (2 * Math.PI) / meridiens;

        //compute phi angle offset
        double phiOffset = (Math.PI) / (paralleles + 1); // +1 to avoid Nortt and South



        //Generating central vertices
        for (int p = 0; p < paralleles; ++p) {
            double phiAngle = phiOffset * (p + 1);

            for (int m = 0; m < meridiens; ++m) {
                double tetaAngle = tetaOffset * m;

                int index = p * meridiens + m;

                float x = rayon * Convert.ToSingle(Math.Sin(phiAngle) * Math.Cos(tetaAngle));
                float y = rayon * Convert.ToSingle(Math.Sin(phiAngle) * Math.Sin(tetaAngle));
                float z = rayon * Convert.ToSingle(Math.Cos(phiAngle));

                vertices[index] = new Vector3(x, y, z);

            }
        }

        //Adding North and South vertices
        int indexNorth = nbVertices - 2;
        int indexSouth = nbVertices - 1;
        vertices[indexNorth] = new Vector3(0, 0, rayon); //North
        vertices[indexSouth] = new Vector3(0, 0, -rayon); //South


        //Generating central triangles
        for (int p = 0; p < paralleles - 1; ++p) {
            int A, B, C, D, index;

            for (int m = 0; m < meridiens - 1; ++m) {
                A = p * meridiens + m;
                B = A + 1;
                C = A + meridiens;
                D = C + 1;

                index = (p * meridiens + m) * 6;

                triangles[index] = A;
                triangles[index + 1] = C;
                triangles[index + 2] = B;

                triangles[index + 3] = D;
                triangles[index + 4] = B;
                triangles[index + 5] = C;
            }

            A = (p + 1) * meridiens - 1;
            B = p * meridiens;
            C = A + meridiens;
            D = B + meridiens;

            index = (p * meridiens + (meridiens - 1)) * 6;

            triangles[index] = A;
            triangles[index + 1] = C;
            triangles[index + 2] = B;

            triangles[index + 3] = D;
            triangles[index + 4] = B;
            triangles[index + 5] = C;
        }

        //Adding North and South triangles
        Debug.Log("NORTH SOUTH");
        for (int m = 0; m < meridiens - 1; ++m) {
            int index = (paralleles - 1) * meridiens * 6 + m * 6;

            int A = m;
            int B = A + 1;

            triangles[index] = indexNorth;
            triangles[index + 1] = A;
            triangles[index + 2] = B;

            int C = (paralleles - 1) * meridiens + m;
            int D = C + 1;

            triangles[index + 3] = indexSouth;
            triangles[index + 4] = D;
            triangles[index + 5] = C;
        }
        {
            int index = (paralleles - 1) * meridiens * 6 + (meridiens - 1) * 6;

            int A = meridiens - 1;
            int B = 0;

            triangles[index] = indexNorth;
            triangles[index + 1] = A;
            triangles[index + 2] = B;

            int C = (paralleles - 1) * meridiens + (meridiens - 1);
            int D = (paralleles - 1) * meridiens;

            triangles[index + 3] = indexSouth;
            triangles[index + 4] = D;
            triangles[index + 5] = C;
        }


        Mesh msh = new Mesh();

        msh.vertices = vertices;
        msh.triangles = triangles;

        return msh;

    }

    static public Mesh CreateCone(int meridiens, float rayon, float height, float heighTruncated) {
        if (heighTruncated <= - 1.0f || heighTruncated >= 1.0f) {
            return CreateFullCone(meridiens, rayon, height);
        } else {
            return CreateTruncatedCone(meridiens, rayon, height, heighTruncated);
        }
    }

    static private Mesh CreateTruncatedCone(int meridiens, float rayon, float height, float heightTruncated) {

        int nbVertices = 2 * meridiens + 2;
        int nbTriangles = 2 * meridiens * 3 + 2 * meridiens * 3;

        Vector3[] vertices = new Vector3[nbVertices]; //Central + border vertices
        int[] triangles = new int[nbTriangles]; //Central + border triangles

        //Compute teta angle offset
        double tetaOffset = (2 * Math.PI) / meridiens;

        //For lower vertices
        float zL = 0;

        //For upper vertices
        float zU = (height * heightTruncated);

        int index = 0;

        //Generating central vertices
        for (int m = 0; m < meridiens; ++m) {

            //Compute real angle
            double angle = tetaOffset * m;

            //Lower vertex
            float xL = rayon * Convert.ToSingle(Math.Cos(angle));
            float yL = rayon * Convert.ToSingle(Math.Sin(angle));
            vertices[index] = new Vector3(xL, yL, zL);
            ++index;

            //Upper vertex
            float xU = rayon * (1 - heightTruncated) * Convert.ToSingle(Math.Cos(angle));
            float yU = rayon * (1 - heightTruncated) * Convert.ToSingle(Math.Sin(angle));
            vertices[index] = new Vector3(xU, yU, zU);
            ++index;

        }

        //Adding border vertices
        int centralLowerindex = index;
        ++index;
        int centralUpperindex = index;
        ++index; //Technically Useless

        vertices[centralLowerindex] = new Vector3(0, 0, zL);
        vertices[centralUpperindex] = new Vector3(0, 0, zU);


        //Generating central triangles
        int indexTri = 0;
        for (int m = 0; m < meridiens - 1; ++m)  // Minus 1 to snap the cone correctly
        {
            index = 2 * m;

            int A = index;
            int B = index + 2;
            int C = index + 1;
            int D = index + 3;

            triangles[indexTri] = A;
            triangles[indexTri + 1] = D;
            triangles[indexTri + 2] = C;

            triangles[indexTri + 3] = D;
            triangles[indexTri + 4] = A;
            triangles[indexTri + 5] = B;

            indexTri += 6;
        }

        // To snap the cylinder around
        {
            int A = 2 * meridiens - 2;
            int B = 0;
            int C = 2 * meridiens - 1;
            int D = 1;

            triangles[indexTri] = A;
            triangles[indexTri + 1] = B;
            triangles[indexTri + 2] = C;

            triangles[indexTri + 3] = D;
            triangles[indexTri + 4] = C;
            triangles[indexTri + 5] = B;

            indexTri += 6;

        }

        //Adding border triangles
        {

            //Lower
            for (int m = 0; m < meridiens - 1; ++m) {
                int B = 2 * m;
                int C = B + 2;

                triangles[indexTri] = centralLowerindex;
                triangles[indexTri + 1] = C;
                triangles[indexTri + 2] = B;

                indexTri += +3;

            }
            //Wrap around
            {
                int B = 2 * meridiens - 2;
                int C = 0;

                triangles[indexTri] = centralLowerindex;
                triangles[indexTri + 1] = C;
                triangles[indexTri + 2] = B;

                indexTri += +3;
            }

            //Upper
            for (int m = 0; m < meridiens - 1; ++m) {
                int B = 2 * m + 1;
                int C = B + 2;

                triangles[indexTri] = centralUpperindex;
                triangles[indexTri + 1] = B;
                triangles[indexTri + 2] = C;

                indexTri += +3;
            }
            //Wrap around
            {
                int B = 2 * meridiens - 1;
                int C = 1;

                triangles[indexTri] = centralUpperindex;
                triangles[indexTri + 1] = B;
                triangles[indexTri + 2] = C;

                indexTri += +3;
            }
        }



        Mesh msh = new Mesh();

        msh.vertices = vertices;
        msh.triangles = triangles;

        return msh;

    }

    static private Mesh CreateFullCone(int meridiens, float rayon, float height) {

        int nbVertices = meridiens + 2;
        int nbTriangles = meridiens * 3 + meridiens * 3;


        Vector3[] vertices = new Vector3[nbVertices]; //Central + border vertices
        int[] triangles = new int[nbTriangles]; //Central + border triangles

        //Compute teta angle offset
        double tetaOffset = (2 * Math.PI) / meridiens;

        //For lower vertices
        float zL = 0;

        //For upper vertices
        float zU = height;

        int index = 0;

        //Generating central vertices
        for (int m = 0; m < meridiens; ++m) {

            //Compute real angle
            double angle = tetaOffset * m;

            //Lower vertex
            float xL = rayon * Convert.ToSingle(Math.Cos(angle));
            float yL = rayon * Convert.ToSingle(Math.Sin(angle));
            vertices[index] = new Vector3(xL, yL, zL);
            ++index;

        }

        //Adding border vertices
        int centralLowerindex = index;
        ++index;
        int centralUpperindex = index;
        ++index; //Technically Useless

        vertices[centralLowerindex] = new Vector3(0, 0, zL);
        vertices[centralUpperindex] = new Vector3(0, 0, zU);


        //Generating central triangles
        int indexTri = 0;
        for (int m = 0; m < meridiens - 1; ++m)  // Minus 1 to snap the cone correctly
        {
            int A = m;
            int B = m + 1;

            triangles[indexTri] = centralUpperindex;
            triangles[indexTri + 1] = A;
            triangles[indexTri + 2] = B;

            indexTri += 3;
        }

        // To snap the cylinder around
        {
            int A = meridiens - 1;
            int B = 0;

            triangles[indexTri] = centralUpperindex;
            triangles[indexTri + 1] = A;
            triangles[indexTri + 2] = B;

            indexTri += 3;
        }

        //Adding border triangles
        {

            //Lower
            for (int m = 0; m < meridiens - 1; ++m) {
                int B = m;
                int C = m + 1;

                triangles[indexTri] = centralLowerindex;
                triangles[indexTri + 1] = C;
                triangles[indexTri + 2] = B;

                indexTri += +3;

            }
            //Wrap around
            {
                int B = meridiens - 1;
                int C = 0;

                triangles[indexTri] = centralLowerindex;
                triangles[indexTri + 1] = C;
                triangles[indexTri + 2] = B;

                indexTri += +3;
            }

        }



        Mesh msh = new Mesh();

        msh.vertices = vertices;
        msh.triangles = triangles;

        return msh;

    }

}
