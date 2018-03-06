//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "plane3D.h"

void setPlane3D(Plane3D* plane, const CvPoint3D64f* unitNormal, const CvPoint3D64f* pointInPlane)
{
   memcpy(&plane->unitNormal, unitNormal, sizeof(CvPoint3D64f));
   plane->p = - cvPoint3D64fDotProduct(unitNormal, pointInPlane);
}

double pointToPlane3DSignedDistance(const CvPoint3D64f* point, const Plane3D* plane)
{
   return cvPoint3D64fDotProduct(point, &plane->unitNormal) + plane->p;
}

bool computePlane3DIntersection(Plane3D* plane0, Plane3D* plane1, Plane3D* plane2, CvPoint3D64f* intersectionPoint)
{
   cv::Mat_<double> left(3, 3);
   memcpy(left.ptr<double>(0), plane0, 3 * sizeof(double));
   memcpy(left.ptr<double>(1), plane1, 3 * sizeof(double));
   memcpy(left.ptr<double>(2), plane2, 3 * sizeof(double));

   cv::Mat_<double> right(3, 1);
   double* rightPtr = right.ptr<double>();
   *rightPtr++ = -plane0->p;
   *rightPtr++ = -plane1->p;
   *rightPtr++ = -plane2->p;

   cv::Mat_<double> intersection(3, 1);

   if (cv::solve(left, right, intersection)) 
   {
      memcpy( intersectionPoint, intersection.ptr<double>(), 3 * sizeof(double));
      return true;
   } else
   return false;
}

void setEdge3D(Edge3D* edge, const CvPoint3D64f* start, const CvPoint3D64f* end)
{
   memcpy(&edge->startPoint, start, sizeof(CvPoint3D64f));
   cvPoint3D64fSub(end, start, &edge->delta);
}

bool computeEdge3DPlane3DIntersection(Edge3D* edge, Plane3D* plane, CvPoint3D64f* intersection)
{
   double denominator = cvPoint3D64fDotProduct(&plane->unitNormal, &edge->delta);
   if (denominator == 0) return false;
   double numerator = plane->p - cvPoint3D64fDotProduct(&plane->unitNormal, &edge->startPoint);
   double fraction = numerator / denominator;
   if ( fraction < 0 || fraction > 1) return false;
   
   intersection->x = edge->startPoint.x + fraction*edge->delta.x;
   intersection->y = edge->startPoint.y + fraction*edge->delta.y;
   intersection->z = edge->startPoint.z + fraction*edge->delta.z;
   return true;
}

void computePlane3DCuboidIntersection(Plane3D* plane, CvPoint3D64f* center, CvPoint3D64f* size, std::vector<CvPoint3D64f>& intersections)
{  
   std::vector<CvPoint3D64f> vertices;
   CvPoint3D64f halfSize;
   cvPoint3D64fMul(size, 0.5, &halfSize);
   vertices.push_back(cvPoint3D64f(center->x - halfSize.x, center->y - halfSize.y, center->z - halfSize.z));
   vertices.push_back(cvPoint3D64f(center->x - halfSize.x, center->y - halfSize.y, center->z + halfSize.z));
   vertices.push_back(cvPoint3D64f(center->x - halfSize.x, center->y + halfSize.y, center->z - halfSize.z));
   vertices.push_back(cvPoint3D64f(center->x - halfSize.x, center->y + halfSize.y, center->z + halfSize.z));
   vertices.push_back(cvPoint3D64f(center->x + halfSize.x, center->y - halfSize.y, center->z - halfSize.z));
   vertices.push_back(cvPoint3D64f(center->x + halfSize.x, center->y - halfSize.y, center->z + halfSize.z));
   vertices.push_back(cvPoint3D64f(center->x + halfSize.x, center->y + halfSize.y, center->z - halfSize.z));
   vertices.push_back(cvPoint3D64f(center->x + halfSize.x, center->y + halfSize.y, center->z + halfSize.z));
   
   std::vector<Edge3D> edges;
   Edge3D edge;
   setEdge3D(&edge, &vertices[0], &vertices[1]); edges.push_back(edge);
   setEdge3D(&edge, &vertices[1], &vertices[3]); edges.push_back(edge);
   setEdge3D(&edge, &vertices[3], &vertices[2]); edges.push_back(edge);
   setEdge3D(&edge, &vertices[2], &vertices[0]); edges.push_back(edge);
   setEdge3D(&edge, &vertices[0], &vertices[4]); edges.push_back(edge);
   setEdge3D(&edge, &vertices[1], &vertices[5]); edges.push_back(edge);
   setEdge3D(&edge, &vertices[3], &vertices[7]); edges.push_back(edge);
   setEdge3D(&edge, &vertices[2], &vertices[6]); edges.push_back(edge);
   setEdge3D(&edge, &vertices[4], &vertices[5]); edges.push_back(edge);
   setEdge3D(&edge, &vertices[5], &vertices[7]); edges.push_back(edge);
   setEdge3D(&edge, &vertices[7], &vertices[6]); edges.push_back(edge);
   setEdge3D(&edge, &vertices[6], &vertices[4]); edges.push_back(edge);

   //std::vector<CvPoint3D64f> intersections;
   CvPoint3D64f c = cvPoint3D64f(0, 0, 0);
   for(unsigned int i = 0; i < edges.size(); i++)
   {
      CvPoint3D64f intersection;
      if(computeEdge3DPlane3DIntersection(&edges[i], plane, &intersection))
      {
         intersections.push_back(intersection);
         c.x += intersection.x;
         c.y += intersection.y;
         c.z += intersection.z;
      }
   }

   if (intersections.size() <= 1) return;
   c.x /= intersections.size();
   c.y /= intersections.size();
   c.z /= intersections.size();

   CvPoint3D64f nor0;
   CvPoint3D64f nor1;
   CvPoint3D64f crossProduct;
   for (unsigned int i = 1; i < intersections.size(); i++)
   {
      for (int j = i; j > 0; j--)
      {
         cvPoint3D64fSub(&intersections[j-1], &c, &nor0);
         cvPoint3D64fSub(&intersections[j], &c, &nor1);
         cvPoint3D64fCrossProduct(&nor0, &nor1, &crossProduct);
         bool sameSide = cvPoint3D64fDotProduct(&crossProduct, &plane->unitNormal) >= 0;
         if (sameSide) break;
         std::swap(intersections[j-1], intersections[j]);
      }
   }  
}