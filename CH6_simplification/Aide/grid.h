#ifndef GRID_H
#define GRID_H

#include <Vec3D.h>
#include <Vertex.h>
#include <vector>
#include <map>



class Grid
{
public:
    Grid(){}
    Grid (const Point3f & min, const Point3f & max, unsigned int r): r(r)
    {
        Point3f diff = max-min;
        origin = diff/2;
        //size = ;
        cellsState.resize(r*r*r, false);
    }

    void drawCell(const Point3f & min,const Point3f& Max);
    void drawGrid();
    void normalize();



    void activateCellContaining(const Point3f & pos);
    void addToCell(const Vertex & vertex);
    void putVertices(const std::vector<Vertex> & vertices);
    unsigned int isContainedAt(const Point3f & pos);



};

#endif // GRID_H
