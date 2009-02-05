#ifndef PLANAR_SUBDIVISION_H
#define PLANAR_SUBDIVISION_H
#include "cxcore.h"
#include "cv.h"

typedef struct
{
   CvPoint2D32f V0;
   CvPoint2D32f V1;
   CvPoint2D32f V2;
} Triangle2DF;

//CVAPI(void) PlanarSubdivisionGetSubdiv2DPoints(const CvSubdiv2D* subdiv, CvMemStorage* storage, CvSeq** points, CvSeq** edges);

CVAPI(CvSeq*) PlanarSubdivisionGetTriangles(const CvSubdiv2D* subdiv, CvMemStorage* storage, bool includeVirtualPoint);
CVAPI(void) PlanarSubdivisionInsertPoints(CvSubdiv2D* subdiv, const CvPoint2D32f* points, int count);

#endif
