#include <cvaux.h>

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