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

//CvCalcOpticalFlowFarneback
CVAPI(void) CvCalcOpticalFlowFarneback( const IplImage* prev0, const IplImage* next0,
                               IplImage* flow0, double pyr_scale, int levels, int winsize,
                               int iterations, int poly_n, double poly_sigma, int flags )
{
   cv::Mat prev0Mat = cv::cvarrToMat(prev0);
   cv::Mat next0Mat = cv::cvarrToMat(next0);
   cv::Mat flow0Mat = cv::cvarrToMat(flow0);
   calcOpticalFlowFarneback(prev0Mat, next0Mat, flow0Mat, pyr_scale, levels, winsize, iterations, poly_n, poly_sigma, flags);
}