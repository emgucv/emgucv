#ifndef PLANAR_SUBDIVISION_H
#define PLANAR_SUBDIVISION_H
#include "cxcore.h"
#include "cv.h"
#include <set>

using namespace std;

typedef struct
{
   CvPoint2D32f V0;
   CvPoint2D32f V1;
   CvPoint2D32f V2;
} Triangle2DF;

CVAPI(void) PlanarSubdivisionGetSubdiv2DPoints(CvSubdiv2D* subdiv, CvPoint2D32f* points, CvSubdiv2DEdge* edges, int* pointCount);

CVAPI(void) PlanarSubdivisionGetTriangles(CvSubdiv2D* subdiv, Triangle2DF* storage, int* triangleCount, int includeVirtualPoint);
CVAPI(void) PlanarSubdivisionInsertPoints(CvSubdiv2D* subdiv, CvPoint2D32f* points, int count);
CVAPI(void) PlanarSubdivisionEdgeToPoly(CvSubdiv2DEdge edge, CvPoint2D32f* buffer, int* count);
#endif
