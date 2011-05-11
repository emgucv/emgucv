//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "contrib_c.h"

//Octtree
cv::Octree* CvOctreeCreate() { return new cv::Octree(); }
void CvOctreeBuildTree(cv::Octree* tree, cv::Point3f* points, int numberOfPoints, int maxLevels, int minPoints ) 
{ 
   std::vector<cv::Point3f> pts = std::vector<cv::Point3f>(numberOfPoints); 
   memcpy(&pts[0], points, numberOfPoints * sizeof(cv::Point3f));  
   tree->buildTree(pts, maxLevels, minPoints); 
}
void CvOctreeGetPointsWithinSphere(cv::Octree* tree, cv::Point3f* center, float radius, CvSeq* pointSeq )
{
   std::vector<cv::Point3f> points; 
   tree->getPointsWithinSphere(*center, radius, points);
   cvClearSeq(pointSeq);
   if (points.size() > 0)
      cvSeqPushMulti(pointSeq, &points.front(), points.size());
}
void CvOctreeRelease(cv::Octree* tree) { delete tree; } 

//CvAdaptiveSkinDetector
CvAdaptiveSkinDetector* CvAdaptiveSkinDetectorCreate(int samplingDivider, int morphingMethod) { return new CvAdaptiveSkinDetector(samplingDivider, morphingMethod); }
void CvAdaptiveSkinDetectorRelease(CvAdaptiveSkinDetector* detector) { delete detector; }
void CvAdaptiveSkinDetectorProcess(CvAdaptiveSkinDetector* detector, IplImage *inputBGRImage, IplImage *outputHueMask) { detector->process(inputBGRImage, outputHueMask); }
