//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_PLANE3D_H
#define EMGU_PLANE3D_H

#include "opencv2/core/core.hpp"
#include "pointUtil.h"

//Define the plane in the Hessian Normal form such that: unitNormal dotproduct (x, y, z) + p = 0
typedef struct Plane3D
{
   ///The unit normal of the plane
   CvPoint3D64f unitNormal;
   ///The distance of the plane from the origin
   double p;
} Plane3D;

//Define a 3D Edge such that [x y z]^T = startPoint + t delta where t = 0..1
//Essentially delta is (end point - starting point)
typedef struct Edge3D
{
   ///starting point
   CvPoint3D64f startPoint;
   ///end point = starting point + delta
   CvPoint3D64f delta;
} Edge3D;

//Set the parameters of the 3D plane given the normal and a point on the plane
CVAPI(void) setPlane3D(Plane3D* plane, const CvPoint3D64f* unitNormal, const CvPoint3D64f* pointInPlane);

//Compute the signed distance from the point to the plane
//If the point is on the side of the plane where the normal is pointed to, the distance is positive.
//Otherwise the distance is negative.
CVAPI(double) pointToPlane3DSignedDistance(const CvPoint3D64f* point, const Plane3D* plane);

//Compute the intersection of three Plane3D
CVAPI(bool) computePlane3DIntersection(Plane3D* plane0, Plane3D* plane1, Plane3D* plane2, CvPoint3D64f* intersectionPoint);

CVAPI(void) setEdge3D(Edge3D* edge, const CvPoint3D64f* start, const CvPoint3D64f* end);

//Check if the 3D edge intersections with the 3D plane.
//Return true if there is an intersection, false otherwise
CVAPI(bool) computeEdge3DPlane3DIntersection(Edge3D* edge, Plane3D* plane, CvPoint3D64f* intersection);

#endif