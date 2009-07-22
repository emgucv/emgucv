#include "cvaux.h"

CVAPI(cv::OctTree*) cvOctTreeCreate() { return new cv::OctTree(); }
CVAPI(void) cvOctTreeBuildTree(cv::OctTree* tree, cv::Point3f* points, int numberOfPoints, int maxLevels, int minPoints ) { cv::Vector<cv::Point3f> pts = cv::Vector<cv::Point3f>(points, numberOfPoints); tree->buildTree(pts, maxLevels, minPoints); }
CVAPI(void) cvOctTreeGetPointsWithinSphere(cv::OctTree* tree, cv::Point3f center, float radius, CvSeq* pointSeq )
{
   cv::Vector<cv::Point3f> points; 
   tree->getPointsWithinSphere(center, radius, points);
   cvClearSeq(pointSeq);
   cvSeqPushMulti(pointSeq, points.begin(), points.size());
}
CVAPI(void) cvOctTreeRelease(cv::OctTree* tree) { tree->~OctTree(); } 