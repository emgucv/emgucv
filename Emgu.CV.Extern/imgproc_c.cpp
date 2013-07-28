//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
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
  int minDisparity, int numDisparities, int blockSize,
  int P1, int P2, int disp12MaxDiff,
  int preFilterCap, int uniquenessRatio,
  int speckleWindowSize, int speckleRange,
  int mode)
{
   cv::Ptr<cv::StereoSGBM> ptr =  cv::createStereoSGBM(minDisparity, numDisparities, blockSize, P1, P2, disp12MaxDiff, preFilterCap, uniquenessRatio, speckleWindowSize, speckleRange, mode);
   ptr.addref();
   return ptr.obj;
}
void CvStereoSGBMRelease(cv::StereoSGBM* obj) { delete obj;}
void CvStereoSGBMFindCorrespondence(cv::StereoSGBM* disparitySolver, IplImage* left, IplImage* right, IplImage* disparity)
{
   cv::Mat leftMat = cv::cvarrToMat(left);
   cv::Mat rightMat = cv::cvarrToMat(right);
   cv::Mat dispMat = cv::cvarrToMat(disparity);
   disparitySolver->compute(leftMat, rightMat, dispMat);
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

void cvFilter2D( const CvArr* srcarr, CvArr* dstarr, const CvMat* _kernel, CvPoint anchor, double delta, int borderType )
{
    cv::Mat src = cv::cvarrToMat(srcarr), dst = cv::cvarrToMat(dstarr);
    cv::Mat kernel = cv::cvarrToMat(_kernel);

    CV_Assert( src.size() == dst.size() && src.channels() == dst.channels() );

    cv::filter2D( src, dst, dst.depth(), kernel, anchor, delta, borderType );
}

void cvCLAHE(const CvArr* srcArr, double clipLimit, emgu::size tileGridSize, CvArr* dstArr)
{
   cv::Size s(tileGridSize.width, tileGridSize.height);
   cv::Ptr<cv::CLAHE> clahe = cv::createCLAHE(clipLimit, s);
   cv::Mat srcMat = cv::cvarrToMat(srcArr);
   cv::Mat dstMat = cv::cvarrToMat(dstArr);
   clahe->apply(srcMat, dstMat);
}