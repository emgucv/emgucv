#include <cvaux.h>

CVAPI(IplImage*) cvGetImageSubRect(IplImage* image, CvRect* rect) 
{ 
	IplImage* res = cvCreateImageHeader(cvSize(rect->width, rect->height), image->depth, image->nChannels);
	CvMat mat;
	cvGetSubRect(image, &mat, *rect);
	cvGetImage(&mat, res);
	return res;
}