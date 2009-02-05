#include "planar_subdivision.h"

void PlanarSubdivisionInsertPoints(CvSubdiv2D* subdiv, const CvPoint2D32f* points, int count)
{
   CvPoint2D32f* point = (CvPoint2D32f*) points;
   for (int i = 0; i < count; i++)
   {
      cvSubdivDelaunay2DInsert(  subdiv, *point);
      point++;
   }
}

void PlanarSubdivisionEdgeToTriangle(const CvSubdiv2DEdge e, Triangle2DF* triangle)
{
   triangle->V0 = cvSubdiv2DEdgeOrg(e)->pt;
   triangle->V1 = cvSubdiv2DEdgeDst(cvSubdiv2DGetEdge(e, CV_NEXT_AROUND_LEFT))->pt;
   triangle->V2 = cvSubdiv2DEdgeDst(e)->pt;
}

inline bool TriangleInRegion (Triangle2DF tri, float left, float right, float top, float bottom)
{
   return
      (
      tri.V0.x >= left && tri.V0.x <= right && tri.V0.y >= top && tri.V0.y <= bottom &&
      tri.V1.x >= left && tri.V1.x <= right && tri.V1.y >= top && tri.V1.y <= bottom &&
      tri.V2.x >= left && tri.V2.x <= right && tri.V2.y >= top && tri.V2.y <= bottom);
}

inline bool PointInRegion(CvPoint2D32f pt, float left, float right, float top, float bottom)
{
   return 
      pt.x >= left && pt.x <= right && pt.y >= top && pt.y <= bottom;
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

inline void TriangleVertexSum(Triangle2DF t, CvPoint2D32f* point)
{
   point->x = t.V0.x +t.V1.x + t.V2.x;
   point->y = t.V0.y + t.V1.y + t.V2.y;
};

void PlanarSubdivisionGetTriangles(const CvSubdiv2D* subdiv, Triangle2DF* triangles, int* triangleCount,  bool includeVirtualPoint)
{
   set<CvPoint2D32f, ltpt> pointSet;

   CvSet* subdivEdges = subdiv->edges;
   CvSeqReader reader;
   cvStartReadSeq((CvSeq*) subdivEdges, &reader);

   int count = 0;
   CvPoint2D32f p;
   Triangle2DF t0 ;
   Triangle2DF t1;

   if (includeVirtualPoint)
   {
      while(CV_IS_SET_ELEM(reader.ptr))
      {
         CvQuadEdge2D* edge = (CvQuadEdge2D*)reader.ptr;
         PlanarSubdivisionEdgeToTriangle( edge->next[0], &t0);
         
         TriangleVertexSum(t0, &p);
         if (pointSet.find(p) == pointSet.end())
         {
            pointSet.insert(p);
            triangles[count++] = t0;
         }
         PlanarSubdivisionEdgeToTriangle( edge->next[2], &t1);
         TriangleVertexSum(t1, &p);
         if (pointSet.find(p) == pointSet.end())
         {
            pointSet.insert(p);
            triangles[count++] = t1;
         }
         CV_NEXT_SEQ_ELEM(subdivEdges->elem_size, reader);
      }
   } else
   {
      float left = subdiv->topleft.x,
         top = subdiv->topleft.y,
         right = subdiv->bottomright.x, 
         bottom = subdiv->bottomright.y;

      while(CV_IS_SET_ELEM(reader.ptr))
      {
         CvQuadEdge2D* edge = (CvQuadEdge2D*)reader.ptr;
         PlanarSubdivisionEdgeToTriangle( edge->next[0], &t0);
         PlanarSubdivisionEdgeToTriangle( edge->next[2], &t1);

         TriangleVertexSum(t0, &p);
         if (pointSet.find(p) == pointSet.end()
            && TriangleInRegion(t0, left, right, top, bottom))
         {
            pointSet.insert(p);
            triangles[count++] = t0;
         }

         TriangleVertexSum(t1, &p);
         if (pointSet.find(p) == pointSet.end()
            && TriangleInRegion(t1, left, right, top, bottom))
         {
            pointSet.insert(p);
            triangles[count++] = t1;
         }
         
         CV_NEXT_SEQ_ELEM(subdivEdges->elem_size, reader);
      }
   }
   
   *triangleCount = count;
}

void PlanarSubdivisionGetSubdiv2DPoints(const CvSubdiv2D* subdiv, CvPoint2D32f* points, CvSubdiv2DEdge* edges, int* pointCount)
{
   set<CvPoint2D32f, ltpt> pointSet;

   float left = subdiv->topleft.x,
      top = subdiv->topleft.y,
      right = subdiv->bottomright.x, 
      bottom = subdiv->bottomright.y;

   CvSet* subdivEdges = subdiv->edges;
   int total = subdivEdges->total;

   int count = 0;

   for (int i = 0; i < total; i++)
   {
      CvQuadEdge2D* qEdge = (CvQuadEdge2D *) cvGetSetElem( subdiv->edges, i );

      if (qEdge && CV_IS_SET_ELEM(qEdge))
      {
         CvSubdiv2DEdge e = (CvSubdiv2DEdge) qEdge;

         if (e && CV_IS_SET_ELEM(e))
         {
            CvPoint2D32f p1 = cvSubdiv2DEdgeOrg(e)->pt;
            if (pointSet.find(p1) == pointSet.end() &&
               PointInRegion(p1, left, right, top, bottom))
            {
               pointSet.insert(p1);
               points[count] = p1;
               edges[count++] = cvSubdiv2DRotateEdge(e, 1);
            }

            CvPoint2D32f p2 = cvSubdiv2DEdgeDst(e)->pt;
            if(pointSet.find(p2) == pointSet.end() &&
               PointInRegion(p2, left, right, top, bottom))
            {
               pointSet.insert(p2);
               points[count] = p2;
               edges[count++] = cvSubdiv2DRotateEdge(e, 3);
            }
         }
      }
   }
   *pointCount = count;
}
