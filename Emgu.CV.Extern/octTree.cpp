#include "cvaux.h"

CVAPI(cv::OctTree*) cvOctTreeCreate() { return new cv::OctTree(); }
CVAPI(void) cvOctTreeBuildTree(cv::OctTree* tree, cv::Point3f* points, int numberOfPoints, int maxLevels, int minPoints ) { std::vector<cv::Point3f> pts = std::vector<cv::Point3f>(numberOfPoints); memcpy(&pts[0], points, numberOfPoints * sizeof(cv::Point3f));  tree->buildTree(pts, maxLevels, minPoints); }
CVAPI(void) cvOctTreeGetPointsWithinSphere(cv::OctTree* tree, cv::Point3f center, float radius, CvSeq* pointSeq )
{
   cv::vector<cv::Point3f> points; 
   tree->getPointsWithinSphere(center, radius, points);
   cvClearSeq(pointSeq);
   cvSeqPushMulti(pointSeq, &points[0], points.size());
}
CVAPI(void) cvOctTreeRelease(cv::OctTree* tree) { tree->~OctTree(); } 