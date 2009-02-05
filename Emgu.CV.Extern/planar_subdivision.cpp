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

CvSeq* PlanarSubdivisionGetTriangles(const CvSubdiv2D* subdiv, CvMemStorage* storage, bool includeVirtualPoint)
{
   CvSet* subdivEdges = subdiv->edges;
   CvSeqReader reader;
   cvStartReadSeq((CvSeq*) subdivEdges, &reader);

   CvSeq* triangles = cvCreateSeq(0, sizeof(CvSeq), sizeof(Triangle2DF), storage);
   Triangle2DF t0 ;
   Triangle2DF t1;

   if (includeVirtualPoint)
   {
      while(CV_IS_SET_ELEM(reader.ptr))
      {
         CvQuadEdge2D* edge = (CvQuadEdge2D*)reader.ptr;
         PlanarSubdivisionEdgeToTriangle( edge->next[0], &t0);
         PlanarSubdivisionEdgeToTriangle( edge->next[2], &t1);
         cvSeqPush(triangles, &t0);
         cvSeqPush(triangles, &t1);
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
         if (TriangleInRegion(t0, left, right, top, bottom))
            cvSeqPush(triangles, &t0);
         if (TriangleInRegion(t1, left, right, top, bottom))
            cvSeqPush(triangles, &t1);
         CV_NEXT_SEQ_ELEM(subdivEdges->elem_size, reader);
      }
   }

   return triangles;
}

/*
void PlanarSubdivisionGetSubdiv2DPoints(const CvSubdiv2D* subdiv, CvMemStorage* storage, CvSeq** points, CvSeq** edges)
{
   float left = subdiv->topleft.x,
      top = subdiv->topleft.y,
      right = subdiv->bottomright.x, 
      bottom = subdiv->bottomright.y;

   CvSet* subdivEdges = subdiv->edges;
   int total = subdivEdges->total;
   int elemSize = subdivEdges->elem_size;

   *points = cvCreateSeq(0, sizeof(CvSeq), sizeof(CvPoint2D32f), storage);
   *edges = cvCreateSeq(0, sizeof(CvSeq), sizeof(CvSubdiv2DEdge), storage);

   CvSeqReader reader;
   cvStartReadSeq((CvSeq*) subdivEdges, &reader);

   CvQuadEdge2D* qEdge;
   CvSubdiv2DEdge e;
   CvPoint2D32f p1;
   CvPoint2D32f p2;
   CvSubdiv2DEdge divEdge;

   for (int i = 0; i < total; i++)
   {
      if(CV_IS_SET_ELEM(reader.ptr))
      {
         qEdge = (CvQuadEdge2D*)reader.ptr;

         if (qEdge->flags != -1)
         {
            e = qEdge->next[0];

            p1 = qEdge->pt[0]->pt;
            p2 = cvSubdiv2DEdgeDst(e)->pt;
            if (PointInRegion(p1, left, right, top, bottom)
               && PointInRegion(p2, left, right, top, bottom))
            {
               cvSeqPush(*points, &p1);
               divEdge = cvSubdiv2DRotateEdge(e, 1);
               cvSeqPush(*edges, &divEdge);
               cvSeqPush(*points, &p2);
               divEdge = cvSubdiv2DRotateEdge(e, 3);
               cvSeqPush(*edges, &divEdge);
            }
         }
      }
      CV_NEXT_SEQ_ELEM(elemSize, reader);
   }
}*/
