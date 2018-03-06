//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "planarSubdivision.h"

//#if _MSC_VER >= 1500
//#define use_unordered_map 1
//#include <unordered_map>
//#else
#include <set>
//#endif

namespace emgu
{
  struct Point2D32f
  {
     float x; 
     float y;
  };
}

union PointKey
{
   emgu::Point2D32f pt;
   double key;
};

#if use_unordered_map
class UniquePointSet 
   : public std::tr1::unordered_map<double, char>
{
public:
   std::pair<std::tr1::unordered_map<double, char>::iterator, bool> insert(const CvPoint2D32f &pt)
   {
      PointKey pk;
      pk.pt = pt;
      return std::tr1::unordered_map<double, char>::insert(std::tr1::unordered_map<double, char>::value_type(pk.key, 0) );
   }
};
#else
class UniquePointSet : public std::set<double>
{
public:
   std::pair<std::set<double>::iterator, bool> insert(const CvPoint2D32f& pt )
   {
      PointKey pk;
      pk.pt.x = pt.x;
      pk.pt.y = pt.y;
      return std::set<double>::insert(pk.key);
   }
};
#endif

void PlanarSubdivisionInsertPoints(CvSubdiv2D* subdiv, CvPoint2D32f* points, int count)
{
   CvPoint2D32f* point = (CvPoint2D32f*) points;
   for (int i = 0; i < count; i++)
      cvSubdivDelaunay2DInsert(subdiv, *(point++));
}

void PlanarSubdivisionEdgeToTriangle(CvSubdiv2DEdge e, Triangle2DF* triangle)
{
   triangle->V0 = cvSubdiv2DEdgeOrg(e)->pt;
   triangle->V1 = cvSubdiv2DEdgeDst(cvSubdiv2DGetEdge(e, CV_NEXT_AROUND_LEFT))->pt;
   triangle->V2 = cvSubdiv2DEdgeDst(e)->pt;
}

inline bool PointInRegion(const CvPoint2D32f pt, const CvSubdiv2D* subdiv)
{
   return 
      pt.x >= subdiv->topleft.x && pt.y >= subdiv->topleft.y && pt.x <= subdiv->bottomright.x  && pt.y <= subdiv->bottomright.y;
}

inline bool TriangleInRegion(const Triangle2DF tri, const CvSubdiv2D* subdiv)
{
   return
      PointInRegion(tri.V0, subdiv)
      && PointInRegion(tri.V1, subdiv)
      && PointInRegion(tri.V2, subdiv);
}

inline CvPoint2D32f TriangleVertexSum(const Triangle2DF* t)
{
   return cvPoint2D32f(t->V0.x + t->V1.x + t->V2.x, t->V0.y + t->V1.y + t->V2.y);
};

void PlanarSubdivisionGetTriangles(CvSubdiv2D* subdiv, Triangle2DF* triangles, int* triangleCount,  int includeVirtualPoint)
{
   UniquePointSet pointSet;

   CvSet* subdivEdges = subdiv->edges;
   CvSeqReader reader;
   cvStartReadSeq((CvSeq*) subdivEdges, &reader);

   schar* start = reader.ptr;
   
   Triangle2DF* currentTriangle = triangles;

   Triangle2DF t;
   if (includeVirtualPoint)
   {
      while(CV_IS_SET_ELEM(reader.ptr))
      {
         CvQuadEdge2D* edge = (CvQuadEdge2D*)reader.ptr;

         PlanarSubdivisionEdgeToTriangle( edge->next[0], &t);
         if (pointSet.insert(TriangleVertexSum(&t)).second)
            *currentTriangle++ = t;

         PlanarSubdivisionEdgeToTriangle( edge->next[2], &t);
         if (pointSet.insert(TriangleVertexSum(&t)).second)
            *currentTriangle++ = t;

         CV_NEXT_SEQ_ELEM(subdivEdges->elem_size, reader);

         // prevent infinite loop
         if(reader.ptr == start)
            break;
      }
   } else
   {
      while(CV_IS_SET_ELEM(reader.ptr))
      {
         CvQuadEdge2D* edge = (CvQuadEdge2D*)reader.ptr;

         PlanarSubdivisionEdgeToTriangle( edge->next[0], &t);
         if (pointSet.insert(TriangleVertexSum(&t)).second
            && TriangleInRegion(t, subdiv))
            *currentTriangle++ = t;

         PlanarSubdivisionEdgeToTriangle( edge->next[2], &t);
         if (pointSet.insert(TriangleVertexSum(&t)).second
            && TriangleInRegion(t, subdiv))
            *currentTriangle++ = t;

         CV_NEXT_SEQ_ELEM(subdivEdges->elem_size, reader);

         // prevent infinite loop
         if(reader.ptr == start)
            break;
      }
   }

   *triangleCount = (currentTriangle - triangles) ;
}

void PlanarSubdivisionEdgeToPoly(CvSubdiv2DEdge edge, CvSeq* buffer)
{
   cvClearSeq(buffer);
   if(!edge) return;

   CvSubdiv2DPoint* v0 = cvSubdiv2DEdgeOrg(edge);
   if (!v0 || v0->flags < 0) { return; }

   for (; ; edge = cvSubdiv2DGetEdge(edge, CV_NEXT_AROUND_LEFT))
   {
      CvSubdiv2DPoint* v = cvSubdiv2DEdgeDst(edge);
      if (!v || v->flags < 0) { cvClearSeq(buffer); return; }
      cvSeqPush(buffer, &v->pt);

      if (0 == memcmp(&v->pt, &v0->pt, sizeof(CvPoint2D32f)))
         break;
   }
   if (buffer->total <= 2) cvClearSeq(buffer);
}

void PlanarSubdivisionGetSubdiv2DPoints(CvSubdiv2D* subdiv, CvPoint2D32f* points, CvSubdiv2DEdge* edges, int* pointCount)
{
   UniquePointSet pointSet;

   CvSet* subdivEdges = subdiv->edges;

   CvPoint2D32f* currentPoint = points;
   CvSubdiv2DEdge* currentEdge = edges;

   CvSeqReader reader;
   cvStartReadSeq((CvSeq*) subdivEdges, &reader);
   
   schar* start = reader.ptr;
   
   while(CV_IS_SET_ELEM(reader.ptr))
   {
      CvQuadEdge2D* qEdge = (CvQuadEdge2D*)reader.ptr;

      if (qEdge && CV_IS_SET_ELEM(qEdge))
      {
         CvSubdiv2DEdge e = (CvSubdiv2DEdge) qEdge;

         if (e && CV_IS_SET_ELEM(e))
         {
            CvSubdiv2DPoint* p1 = cvSubdiv2DEdgeOrg(e);
            if (p1 &&
               pointSet.insert(p1->pt).second &&
               PointInRegion(p1->pt, subdiv))
            {
               *currentPoint++ = p1->pt;
               *currentEdge++ = cvSubdiv2DRotateEdge(e, 1);
            }

            CvSubdiv2DPoint* p2 = cvSubdiv2DEdgeDst(e);
            if(p2 &&
               pointSet.insert(p2->pt).second &&
               PointInRegion(p2->pt, subdiv))
            {
               *currentPoint++ = p2->pt;
               *currentEdge++ = cvSubdiv2DRotateEdge(e, 3);
            }
         }
      }
      CV_NEXT_SEQ_ELEM(subdivEdges->elem_size, reader);
      
      // prevent infinite loop
      if(reader.ptr == start)
         break;
   }
   *pointCount = currentPoint - points;
}
