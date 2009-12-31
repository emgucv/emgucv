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
