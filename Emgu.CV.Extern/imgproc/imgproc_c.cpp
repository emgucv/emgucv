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
void cveGrabCut(cv::_InputArray* img, cv::_InputOutputArray* mask, cv::Rect* rect, cv::_InputOutputArray* bgdModel, cv::_InputOutputArray* fgdModel, int iterCount, int flag)
{
   cv::grabCut(*img, mask? *mask : cv::noArray(), *rect, *bgdModel, *fgdModel, iterCount, flag);
}



void cveFilter2D( cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* kernel, CvPoint* anchor, double delta, int borderType )
{
    CV_Assert( src->size() == dst->size() && src->channels() == dst->channels() );
    cv::filter2D( *src, *dst, dst->depth(), *kernel, *anchor, delta, borderType );
}

void cveCLAHE(cv::_InputArray* src, double clipLimit, emgu::size* tileGridSize, cv::_OutputArray* dst)
{
   cv::Size s(tileGridSize->width, tileGridSize->height);
   cv::Ptr<cv::CLAHE> clahe = cv::createCLAHE(clipLimit, s);
   clahe->apply(*src, *dst);
}

void cveAdaptiveBilateralFilter(cv::_InputArray* src, cv::_OutputArray* dst, emgu::size* ksize, double sigmaSpace, double maxSigmaColor, CvPoint* anchor, int borderType)
{
   cv::Size s(ksize->width, ksize->height);
   cv::adaptiveBilateralFilter(*src, *dst, s, sigmaSpace, maxSigmaColor, *anchor, borderType);
}

void cveErode(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* kernel, CvPoint* anchor, int iterations, int borderType, CvScalar* borderValue)
{
   cv::erode(*src, *dst, kernel ? * kernel : cv::noArray(), *anchor, iterations, borderType, *borderValue);
}

void cveDilate(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* kernel, CvPoint* anchor, int iterations, int borderType, CvScalar* borderValue)
{
   cv::dilate(*src, *dst, kernel ? * kernel : cv::noArray(), *anchor, iterations, borderType, *borderValue);
}
void cveGetStructuringElement(cv::Mat* mat, int shape, emgu::size* ksize, CvPoint* anchor)
{
   cv::Size s(ksize->width, ksize->height);
   cv::Mat res = cv::getStructuringElement(shape, s, *anchor);
   cv::swap(*mat, res);
}
void cveMorphologyEx(cv::_InputArray* src, cv::_OutputArray* dst, int op, cv::_InputArray* kernel, CvPoint* anchor, int iterations, int borderType, CvScalar* borderValue)
{
   cv::morphologyEx(*src, *dst, op, *kernel, *anchor, iterations, borderType, *borderValue);
}

void cveSobel(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, int dx, int dy, int ksize, double scale, double delta, int borderType)
{
   cv::Sobel(*src, *dst, ddepth, dx, dy, ksize, scale, delta, borderType);
}

void cveLaplacian(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, int ksize, double scale, double delta, int borderType)
{
   cv::Laplacian(*src, *dst, ddepth, ksize, scale, delta, borderType);
}

void cvePyrUp(cv::_InputArray* src, cv::_OutputArray* dst, emgu::size* size, int borderType)
{
   cv::Size s(size->width, size->height);
   cv::pyrUp(*src, *dst, s, borderType);
}
void cvePyrDown(cv::_InputArray* src, cv::_OutputArray* dst, emgu::size* size, int borderType)
{
   cv::Size s(size->width, size->height);
   cv::pyrDown(*src, *dst, s, borderType);
}

void cveCanny(cv::_InputArray* image, cv::_OutputArray* edges, double threshold1, double threshold2, int apertureSize, bool L2gradient)
{
   cv::Canny(*image, *edges, threshold1, threshold2, apertureSize, L2gradient);
}

void cveCornerHarris(cv::_InputArray* src, cv::_OutputArray* dst, int blockSize, int ksize, double k, int borderType)
{
   cv::cornerHarris(*src, *dst, blockSize, ksize, k, borderType);
}

double cveThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double thresh, double maxval, int type)
{
   return cv::threshold(*src, *dst, thresh, maxval, type);
}
void cveWatershed(cv::_InputArray* image, cv::_InputOutputArray* markers)
{
   cv::watershed(*image, *markers);
}
void cveAdaptiveThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double maxValue, int adaptiveMethod, int thresholdType, int blockSize, double c)
{
   cv::adaptiveThreshold(*src, *dst, maxValue, adaptiveMethod, thresholdType, blockSize, c);
}
void cveCvtColor(cv::_InputArray* src, cv::_OutputArray* dst, int code, int dstCn)
{
   cv::cvtColor(*src, *dst, code, dstCn);
}
void cveCopyMakeBorder(cv::_InputArray* src, cv::_OutputArray* dst, int top, int bottom, int left, int right, int borderType, CvScalar* value)
{
   cv::copyMakeBorder(*src, *dst, top, bottom, left, right, borderType, *value);
}

void cveIntegral(cv::_InputArray* src, cv::_OutputArray* sum, cv::_OutputArray* sqsum, cv::_OutputArray* tilted, int sdepth)
{
   if (tilted)
   {
      cv::integral(*src, *sum, *sqsum, *tilted, sdepth);
   } else if (sqsum)
   {
      cv::integral(*src, *sum, *sqsum, sdepth);
   } else 
   {
      cv::integral(*src, *sum, sdepth);
   }
}

int cveFloodFill(cv::_InputOutputArray* image, cv::_InputOutputArray* mask, CvPoint* seedPoint, CvScalar* newVal, CvRect* rect, CvScalar* loDiff, CvScalar* upDiff, int flags)
{
   cv::Rect r = *rect;
   if (mask)
      return cv::floodFill(*image, *mask, *seedPoint, *newVal, &r, *loDiff, *upDiff, flags);
   else
      return cv::floodFill(*image, *seedPoint, *newVal, &r, *loDiff, *upDiff, flags);

   rect->x = r.x;
   rect->y = r.y;
   rect->width = r.width;
   rect->height = r.height;
}

void cvePyrMeanShiftFiltering(cv::_InputArray* src, cv::_OutputArray* dst, double sp, double sr, int maxLevel, CvTermCriteria* termCrit)
{
   cv::pyrMeanShiftFiltering(*src, *dst, sp, sr, maxLevel, *termCrit);
}

void cveMoments(cv::_InputArray* arr, bool binaryImage, CvMoments* moments)
{
   CvMoments mm(cv::moments(*arr, binaryImage));
   memcpy(moments, &mm, sizeof(CvMoments));
}
void cveEqualizeHist(cv::_InputArray* src, cv::_OutputArray* dst)
{
   cv::equalizeHist(*src, *dst);
}