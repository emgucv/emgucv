#include "planar_subdivision.h"

#if CV_SSE2 && defined(__GNUC__)
#define align(x) __attribute__ ((aligned (x)))
#elif CV_SSE2 && (defined(__ICL) || defined _MSC_VER && _MSC_VER >= 1300)
#define align(x) __declspec(align(x))
#else
#define align(x)
#endif

void PlanarSubdivisionInsertPoints(CvSubdiv2D* subdiv, const CvPoint2D32f* points, int count)
{
   CvPoint2D32f* point = (CvPoint2D32f*) points;
   for (int i = 0; i < count; i++)
      cvSubdivDelaunay2DInsert(subdiv, *(point++));
}

void PlanarSubdivisionEdgeToTriangle(const CvSubdiv2DEdge e, Triangle2DF* triangle)
{
   triangle->V0 = cvSubdiv2DEdgeOrg(e)->pt;
   triangle->V1 = cvSubdiv2DEdgeDst(cvSubdiv2DGetEdge(e, CV_NEXT_AROUND_LEFT))->pt;
   triangle->V2 = cvSubdiv2DEdgeDst(e)->pt;
}

bool PointInRegion(CvPoint2D32f pt, CvPoint2D32f topleft, CvPoint2D32f bottomright)
{
   return 
      pt.x >= topleft.x && pt.y >= topleft.y && pt.x <= bottomright.x  && pt.y <= bottomright.y;
}

bool TriangleInRegion (Triangle2DF tri, CvPoint2D32f topleft, CvPoint2D32f bottomright)
{
   return
      PointInRegion(tri.V0, topleft, bottomright)
      && PointInRegion(tri.V1, topleft, bottomright)
      && PointInRegion(tri.V2, topleft, bottomright);
}

struct ltpt
{
  bool operator()(CvPoint2D32f p1, CvPoint2D32f p2) const
  {
     return p1.x == p2.x? 
        p1.y < p2.y :
        p1.x < p2.x;
  }
};

CvPoint2D32f TriangleVertexSum(Triangle2DF t)
{
   CvPoint2D32f point;
   point.x = t.V0.x +t.V1.x + t.V2.x;
   point.y = t.V0.y + t.V1.y + t.V2.y;
   return point;
};

void PlanarSubdivisionGetTriangles(const CvSubdiv2D* subdiv, Triangle2DF* triangles, int* triangleCount,  bool includeVirtualPoint)
{
   set<CvPoint2D32f, ltpt> pointSet;

   CvSet* subdivEdges = subdiv->edges;
   CvSeqReader reader;
   cvStartReadSeq((CvSeq*) subdivEdges, &reader);

   int count = 0;

   Triangle2DF* currentTriangle = triangles;

   Triangle2DF t;
   if (includeVirtualPoint)
   {
      while(CV_IS_SET_ELEM(reader.ptr))
      {
         CvQuadEdge2D* edge = (CvQuadEdge2D*)reader.ptr;
         PlanarSubdivisionEdgeToTriangle( edge->next[0], &t);

         if (pointSet.insert(TriangleVertexSum(t)).second)
         {
            *currentTriangle++ = t;
            count++;
         }

         PlanarSubdivisionEdgeToTriangle( edge->next[2], &t);

         if (pointSet.insert(TriangleVertexSum(t)).second)
         {
            *currentTriangle++ = t;
            count++;
         }
         CV_NEXT_SEQ_ELEM(subdivEdges->elem_size, reader);
      }
   } else
   {
      CvPoint2D32f topleft = subdiv->topleft;
      CvPoint2D32f bottomright = subdiv->bottomright;

      while(CV_IS_SET_ELEM(reader.ptr))
      {
         CvQuadEdge2D* edge = (CvQuadEdge2D*)reader.ptr;
         PlanarSubdivisionEdgeToTriangle( edge->next[0], &t);

         if (pointSet.insert(TriangleVertexSum(t)).second
            && TriangleInRegion(t, topleft, bottomright))
         {
            *currentTriangle++ = t;
            count++;
         }

         PlanarSubdivisionEdgeToTriangle( edge->next[2], &t);

         if (pointSet.insert(TriangleVertexSum(t)).second
            && TriangleInRegion(t, topleft, bottomright))
         {
            *currentTriangle++ = t;
            count++;
         }
         
         CV_NEXT_SEQ_ELEM(subdivEdges->elem_size, reader);
      }
   }
   
   *triangleCount = count;
}

void PlanarSubdivisionGetSubdiv2DPoints(const CvSubdiv2D* subdiv, CvPoint2D32f* points, CvSubdiv2DEdge* edges, int* pointCount)
{
   set<CvPoint2D32f, ltpt> pointSet;

   CvPoint2D32f topleft = subdiv->topleft;
   CvPoint2D32f bottomright = subdiv->bottomright;

   CvSet* subdivEdges = subdiv->edges;
   int total = subdivEdges->total;

   CvPoint2D32f* currentPoint = points;
   CvSubdiv2DEdge* currentEdge = edges;
   int count = 0;

   for (int i = 0; i < total; i++)
   {
      CvQuadEdge2D* qEdge = (CvQuadEdge2D *) cvGetSetElem( subdiv->edges, i );

      if (qEdge && CV_IS_SET_ELEM(qEdge))
      {
         CvSubdiv2DEdge e = (CvSubdiv2DEdge) qEdge;

         if (e && CV_IS_SET_ELEM(e))
         {
            CvSubdiv2DPoint* p1 = cvSubdiv2DEdgeOrg(e);
            if (p1 &&
               pointSet.insert(p1->pt).second &&
               PointInRegion(p1->pt, topleft, bottomright))
            {
               *points++ = p1->pt;
               *edges++ = cvSubdiv2DRotateEdge(e, 1);
               count++;
            }

            CvSubdiv2DPoint* p2 = cvSubdiv2DEdgeDst(e);
            if(p2 &&
               pointSet.insert(p2->pt).second &&
               PointInRegion(p2->pt, topleft, bottomright))
            {
               *points++ = p2->pt;
               *edges++ = cvSubdiv2DRotateEdge(e, 3);
               count++;
            }
         }
      }
   }
   *pointCount = count;
}
