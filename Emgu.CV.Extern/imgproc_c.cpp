//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "imgproc_c.h"

IplImage* cvGetImageSubRect(IplImage* image, CvRect* rect) 
{ 
	IplImage* res = cvCreateImageHeader(cvSize(rect->width, rect->height), image->depth, image->nChannels);
	CvMat mat;
	cvGetSubRect(image, &mat, *rect);
	cvGetImage(&mat, res);
	return res;
}

//GrabCut
void CvGrabCut(IplImage* img, IplImage* mask, cv::Rect* rect, IplImage* bgdModel, IplImage* fgdModel, int iterCount, int flag)
{
cv::Mat imgMat = cv::cvarrToMat(img);
cv::Mat maskMat = cv::cvarrToMat(mask);
cv::Mat bgdModelMat = cv::cvarrToMat(bgdModel);
cv::Mat fgdModelMat = cv::cvarrToMat(fgdModel);
cv::grabCut(imgMat, maskMat, *rect, bgdModelMat, fgdModelMat, iterCount, flag);
}

//StereoSGBM
cv::StereoSGBM* CvStereoSGBMCreate(
  int minDisparity, int numDisparities, int SADWindowSize,
  int P1, int P2, int disp12MaxDiff,
  int preFilterCap, int uniquenessRatio,
  int speckleWindowSize, int speckleRange,
  bool fullDP)
{
   return new cv::StereoSGBM(minDisparity, numDisparities, SADWindowSize, P1, P2, disp12MaxDiff, preFilterCap,uniquenessRatio,speckleWindowSize,speckleRange, fullDP);
}
void CvStereoSGBMRelease(cv::StereoSGBM* obj) { delete obj;}
void CvStereoSGBMFindCorrespondence(cv::StereoSGBM* disparitySolver, IplImage* left, IplImage* right, IplImage* disparity)
{
   cv::Mat leftMat = cv::cvarrToMat(left);
   cv::Mat rightMat = cv::cvarrToMat(right);
   cv::Mat dispMat = cv::cvarrToMat(disparity);
   (*disparitySolver)(leftMat, rightMat, dispMat);
}

bool cvCheckRange(CvArr* arr, bool quiet, CvPoint* index, double minVal, double maxVal)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   cv::Point p;
   bool result = cv::checkRange(mat, quiet, &p , minVal, maxVal);
   index->x = p.x;
   index->y = p.y;
   return result;
}

void cvArrSqrt(CvArr* src, CvArr* dst)
{
   cv::Mat srcMat = cv::cvarrToMat(src);
   cv::Mat dstMat = cv::cvarrToMat(dst);
   cv::sqrt(srcMat, dstMat);
}
