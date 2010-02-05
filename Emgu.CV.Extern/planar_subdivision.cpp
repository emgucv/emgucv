#include "planar_subdivision.h"

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

bool PointInRegion(CvPoint2D32f pt, CvSubdiv2D* subdiv)
{
   return 
      pt.x >= subdiv->topleft.x && pt.y >= subdiv->topleft.y && pt.x <= subdiv->bottomright.x  && pt.y <= subdiv->bottomright.y;
}

bool TriangleInRegion (Triangle2DF tri, CvSubdiv2D* subdiv)
{
   return
      PointInRegion(tri.V0, subdiv)
      && PointInRegion(tri.V1, subdiv)
      && PointInRegion(tri.V2, subdiv);
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

CvPoint2D32f TriangleVertexSum(Triangle2DF* t)
{
   CvPoint2D32f point;
   point.x = t->V0.x + t->V1.x + t->V2.x;
   point.y = t->V0.y + t->V1.y + t->V2.y;
   return point;
};

void PlanarSubdivisionGetTriangles(CvSubdiv2D* subdiv, Triangle2DF* triangles, int* triangleCount,  int includeVirtualPoint)
{
   set<CvPoint2D32f, ltpt> pointSet;

   CvSet* subdivEdges = subdiv->edges;
   CvSeqReader reader;
   cvStartReadSeq((CvSeq*) subdivEdges, &reader);

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
      }
   }

   *triangleCount = (currentTriangle - triangles) ;
}

void PlanarSubdivisionEdgeToPoly(CvSubdiv2DEdge edge, CvPoint2D32f* buffer, int* count)
{
   CvSubdiv2DPoint* v0 = cvSubdiv2DEdgeOrg(edge);
   if (v0->flags == -1) { *count = 0; return; }

   CvPoint2D32f* currentBuffer = buffer;

   for (; ; edge = cvSubdiv2DGetEdge(edge, CV_NEXT_AROUND_LEFT))
   {
      CvSubdiv2DPoint* v = cvSubdiv2DEdgeDst(edge);
      if (v->flags == -1) { *count = 0; return; }
      *currentBuffer++ = v->pt;

      if (v->pt.x == v0->pt.x && v->pt.y == v0->pt.y)
         break;
   }
   *count = currentBuffer - buffer;
   if (*count <= 2) *count = 0;
}

void PlanarSubdivisionGetSubdiv2DPoints(CvSubdiv2D* subdiv, CvPoint2D32f* points, CvSubdiv2DEdge* edges, int* pointCount)
{
   set<CvPoint2D32f, ltpt> pointSet;

   CvSet* subdivEdges = subdiv->edges;

   CvPoint2D32f* currentPoint = points;
   CvSubdiv2DEdge* currentEdge = edges;

   CvSeqReader reader;
   cvStartReadSeq((CvSeq*) subdivEdges, &reader);
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

         CV_NEXT_SEQ_ELEM(subdivEdges->elem_size, reader);
      }
   }
   *pointCount = currentPoint - points;
}
