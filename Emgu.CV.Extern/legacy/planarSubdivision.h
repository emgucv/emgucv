//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_PLANAR_SUBDIVISION_H
#define EMGU_PLANAR_SUBDIVISION_H
#include "opencv2/core/core_c.h"
//#include "opencv2/imgproc/imgproc_c.h"
#include "opencv2/legacy/legacy.hpp"

typedef struct
{
   CvPoint2D32f V0;
   CvPoint2D32f V1;
   CvPoint2D32f V2;
} Triangle2DF;

CVAPI(void) PlanarSubdivisionGetSubdiv2DPoints(CvSubdiv2D* subdiv, CvPoint2D32f* points, CvSubdiv2DEdge* edges, int* pointCount);

CVAPI(void) PlanarSubdivisionGetTriangles(CvSubdiv2D* subdiv, Triangle2DF* storage, int* triangleCount, int includeVirtualPoint);
CVAPI(void) PlanarSubdivisionInsertPoints(CvSubdiv2D* subdiv, CvPoint2D32f* points, int count);
CVAPI(void) PlanarSubdivisionEdgeToPoly(CvSubdiv2DEdge edge, CvSeq* buffer);
#endif
