#ifndef MESH_H
#define MESH_H

#include <vector>
#include "math.h"

/************************************************************
 * Structure de sommet de base
 ************************************************************/
struct Point3f
{

    Point3f(float xI, float yI, float zI) : x(xI), y(yI), z(zI) {}
    Point3f() : x(0.), y(0.), z(0.) {}

    union
    {
        struct
        {
            float x;
            float y;
            float z;
        };
        float pos[3];
    };

    Point3f & operator/= (float s) {
        x /= s;
        y /= s;
        z /= s;
        return (*this);
    };

    Point3f & operator+= (Point3f p) {
        x += p.x;
        y += p.y;
        z += p.z;
        return (*this);
    };

    float norm() {
        return sqrt(x*x + y*y + z*z);
    }

    static inline Point3f crossProduct(const Point3f & a, const Point3f & b) {
        Point3f result;
        result.x = a.y * b.z - a.z * b.y;
        result.y = a.z * b.x - a.x * b.z;
        result.z = a.x * b.y - a.y * b.x;
        return(result);
    }

    void normalize(){
        float n = norm();
        x /= n;
        y /= n;
        z /= n;
    }
};


Point3f operator+ (const Point3f & p1, const Point3f & p2);
Point3f operator- (const Point3f & p1, const Point3f & p2);
Point3f operator/ (const Point3f & p, float divisor);


/************************************************************
 * Structure de triangle de base
 ************************************************************/
struct Triangle
{
    Triangle(int xI, int yI, int zI)
        :i1(xI)
        ,i2(yI)
        ,i3(zI)
    {

    }

    union
    {
        struct{
            int i1;
            int i2;
            int i3;
        };
        int index[3];
    };
};




/************************************************************
 * Class de maillage basique
 ************************************************************/
class Mesh {
public:
    Mesh();
    inline Mesh (const std::vector<Point3f> & v, const std::vector<Triangle> & t) : vertices (v), triangles (t)  {}

    bool loadMesh(const char * filename);
    
    void computeVertexNormals ();
    void centerAndScaleToUnit ();
    
    void draw();
    void drawSmooth();
    
    void computeBoundingCube();

    //Bounding box information
    Point3f origin;
    float cubeSize;
    
    std::vector<Point3f> vertices;
    std::vector<Point3f> normals;
    std::vector<Triangle> triangles;
};

#endif // MESH_H
