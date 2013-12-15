//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_IMGPROC_C_H
#define EMGU_IMGPROC_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/imgproc/types_c.h"
//#include "opencv2/calib3d/calib3d.hpp"
#include "emgu_c.h"
CVAPI(IplImage*) cvGetImageSubRect(IplImage* image, CvRect* rect);

//GrabCut
CVAPI(void) cveGrabCut(cv::_InputArray* img, cv::_InputOutputArray* mask, cv::Rect* rect, cv::_InputOutputArray* bgdModel, cv::_InputOutputArray* fgdModel, int iterCount, int flag);

CVAPI(void) cveFilter2D(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* kernel, CvPoint* anchor, double delta, int borderType );

CVAPI(void) cveCLAHE(cv::_InputArray* src, double clipLimit, emgu::size* tileGridSize, cv::_OutputArray* dst);

CVAPI(void) cveAdaptiveBilateralFilter(cv::_InputArray* src, cv::_OutputArray* dst, emgu::size* ksize, double sigmaSpace, double maxSigmaColor, CvPoint* anchor, int borderType);

CVAPI(void) cveErode(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* kernel, CvPoint* anchor, int iterations, int borderType, CvScalar* borderValue);

CVAPI(void) cveDilate(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* kernel, CvPoint* anchor, int iterations, int borderType, CvScalar* borderValue);
CVAPI(void) cveGetStructuringElement(cv::Mat* mat, int shape, emgu::size* ksize, CvPoint* anchor);
CVAPI(void) cveMorphologyEx(cv::_InputArray* src, cv::_OutputArray* dst, int op, cv::_InputArray* kernel, CvPoint* anchor, int iterations, int borderType, CvScalar* borderValue);

CVAPI(void) cveSobel(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, int dx, int dy, int ksize, double scale, double delta, int borderType);
CVAPI(void) cveLaplacian(cv::_InputArray* src, cv::_OutputArray* dst, int ddepth, int ksize, double scale, double delta, int borderType);
CVAPI(void) cvePyrUp(cv::_InputArray* src, cv::_OutputArray* dst, emgu::size* size, int borderType);
CVAPI(void) cvePyrDown(cv::_InputArray* src, cv::_OutputArray* dst, emgu::size* size, int borderType);

CVAPI(void) cveCanny(cv::_InputArray* image, cv::_OutputArray* edges, double threshold1, double threshold2, int apertureSize, bool L2gradient);
CVAPI(void) cveCornerHarris(cv::_InputArray* src, cv::_OutputArray* dst, int blockSize, int ksize, double k, int borderType);
CVAPI(double) cveThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double thresh, double maxval, int type);
CVAPI(void) cveWatershed(cv::_InputArray* image, cv::_InputOutputArray* markers);
CVAPI(void) cveAdaptiveThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double maxValue, int adaptiveMethod, int thresholdType, int blockSize, double c);
CVAPI(void) cveCvtColor(cv::_InputArray* src, cv::_OutputArray* dst, int code, int dstCn);
CVAPI(void) cveCopyMakeBorder(cv::_InputArray* src, cv::_OutputArray* dst, int top, int bottom, int left, int right, int borderType, CvScalar* value);

CVAPI(void) cveIntegral(cv::_InputArray* src, cv::_OutputArray* sum, cv::_OutputArray* sqsum, cv::_OutputArray* tilted, int sdepth);

CVAPI(int) cveFloodFill(cv::_InputOutputArray* image, cv::_InputOutputArray* mask, CvPoint* seedPoint, CvScalar* newVal, CvRect* rect, CvScalar* loDiff, CvScalar* upDiff, int flags);
CVAPI(void) cvePyrMeanShiftFiltering(cv::_InputArray* src, cv::_OutputArray* dst, double sp, double sr, int maxLevel, CvTermCriteria* termCrit);

CVAPI(void) cveMoments(cv::_InputArray* arr, bool binaryImage, CvMoments* moments);

CVAPI(void) cveEqualizeHist(cv::_InputArray* src, cv::_OutputArray* dst);
#endif
