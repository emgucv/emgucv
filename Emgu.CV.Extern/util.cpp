#pragma warning( disable: 4251 )

#include "opencv2/core/core_c.h"
#include "opencv2/contrib/contrib.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/calib3d/calib3d.hpp"

CVAPI(IplImage*) cvGetImageSubRect(IplImage* image, CvRect* rect) 
{ 
	IplImage* res = cvCreateImageHeader(cvSize(rect->width, rect->height), image->depth, image->nChannels);
	CvMat mat;
	cvGetSubRect(image, &mat, *rect);
	cvGetImage(&mat, res);
	return res;
}

//CvAdaptiveSkinDetector
CVAPI(CvAdaptiveSkinDetector*) CvAdaptiveSkinDetectorCreate(int samplingDivider, int morphingMethod) { return new CvAdaptiveSkinDetector(samplingDivider, morphingMethod); }
CVAPI(void) CvAdaptiveSkinDetectorRelease(CvAdaptiveSkinDetector* detector) { delete detector; }
CVAPI(void) CvAdaptiveSkinDetectorProcess(CvAdaptiveSkinDetector* detector, IplImage *inputBGRImage, IplImage *outputHueMask) { detector->process(inputBGRImage, outputHueMask); }

//GrabCut
CVAPI(void) CvGrabCut(IplImage* img, IplImage* mask, cv::Rect* rect, IplImage* bgdModel, IplImage* fgdModel, int iterCount, int flag)
{
cv::Mat imgMat = cv::cvarrToMat(img);
cv::Mat maskMat = cv::cvarrToMat(mask);
cv::Mat bgdModelMat = cv::cvarrToMat(bgdModel);
cv::Mat fgdModelMat = cv::cvarrToMat(fgdModel);
cv::grabCut(imgMat, maskMat, *rect, bgdModelMat, fgdModelMat, iterCount, flag);
}

//StereoSGBM
CVAPI(cv::StereoSGBM*) CvStereoSGBMCreate(
  int minDisparity, int numDisparities, int SADWindowSize,
  int P1, int P2, int disp12MaxDiff,
  int preFilterCap, int uniquenessRatio,
  int speckleWindowSize, int speckleRange,
  bool fullDP)
{
   return new cv::StereoSGBM(minDisparity, numDisparities, SADWindowSize, P1, P2, disp12MaxDiff, preFilterCap,uniquenessRatio,speckleWindowSize,speckleRange, fullDP);
}
CVAPI(void) CvStereoSGBMRelease(cv::StereoSGBM* obj) { delete obj;}
CVAPI(void) CvStereoSGBMFindCorrespondence(cv::StereoSGBM* disparitySolver, IplImage* left, IplImage* right, IplImage* disparity)
{
   cv::Mat leftMat = cv::cvarrToMat(left);
   cv::Mat rightMat = cv::cvarrToMat(right);
   cv::Mat dispMat = cv::cvarrToMat(disparity);
   (*disparitySolver)(leftMat, rightMat, dispMat);
}

CVAPI(bool) cvCheckRange(CvArr* arr, bool quiet, CvPoint* index, double minVal, double maxVal)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   cv::Point p;
   bool result = cv::checkRange(mat, quiet, &p , minVal, maxVal);
   index->x = p.x;
   index->y = p.y;
   return result;
}

CVAPI(void) cvArrSqrt(CvArr* src, CvArr* dst)
{
   cv::Mat srcMat = cv::cvarrToMat(src);
   cv::Mat dstMat = cv::cvarrToMat(dst);
   cv::sqrt(srcMat, dstMat);
}

CVAPI(bool) getHomographyMatrixFromMatchedFeatures(std::vector<cv::KeyPoint>* model, std::vector<cv::KeyPoint>* observed, CvArr* indices, CvArr* mask, CvMat* homography)
{
   cv::Mat indMat = cv::cvarrToMat(indices);
   cv::Mat maskMat = cv::cvarrToMat(mask);
   int nonZero = cv::countNonZero(maskMat);
   if (nonZero < 4) return false;

   cv::Mat_<float> srcPtMat(nonZero, 2);
   cv::Mat_<float> dstPtMat(nonZero, 2);

   int idx = 0;
   for(int i = 0; i < maskMat.rows; i++)
   {
      if ( *maskMat.ptr(i) != 0)
      {
         int* tmp = (int*) indMat.ptr(i);
         memcpy(srcPtMat.ptr(idx), &(*model)[*tmp], sizeof(float) * 2);
         memcpy(dstPtMat.ptr(idx), &(*observed)[i], sizeof(float) *2);
         idx++;
      }
   }
   cv::Mat result = cv::findHomography(srcPtMat, dstPtMat, CV_RANSAC, 3);
   cv::Mat hMat = cv::cvarrToMat(homography);
   result.copyTo(hMat);
   return true;

}