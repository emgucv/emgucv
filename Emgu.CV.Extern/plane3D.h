#include "opencv2/core/core.hpp"
#include "pointUtil.h"

//Define the plane ax + by + cz + d = 0
//The vector [a b c] defines the normal of the plane
typedef struct Plane3D
{
   double a;
   double b;
   double c;
   double d;
} Plane3D;

//Define a 3D Edge such that [x y z]^T = startPoint + t delta where t = 0..1
//Essentially delta is (end point - starting point)
typedef struct Edge3D
{
   //starting point
   CvPoint3D64f startPoint;
   //end point - starting point
   CvPoint3D64f delta;
} Edge3D;

//Set the parameters of the 3D plane given the normal and a point on the plane
CVAPI(void) setPlane3D(Plane3D* plane, CvPoint3D64f* normal, CvPoint3D64f* pointInPlane);

//Compute the signed distance from the point to the plane
//If the point is on the side of the plane where the normal is pointed to, the distance is positive.
//Otherwise the distance is negative.
CVAPI(double) pointToPlane3DSignedDistance(CvPoint3D64f* point, Plane3D* plane);

//Compute the intersection of 3 Plane3D
CVAPI(bool) computePlane3DIntersection(Plane3D* plane0, Plane3D* plane1, Plane3D* plane2, CvPoint3D64f* intersectionPoint);

CVAPI(void) setEdge3D(Edge3D* edge, CvPoint3D64f* start, CvPoint3D64f* end);

//Check if the 3D edge intersections with the 3D plane.
//Return true if there is an intersection, false otherwise
CVAPI(bool) computeEdge3DPlane3DIntersection(Edge3D* edge, Plane3D* plane, CvPoint3D64f* intersection);

