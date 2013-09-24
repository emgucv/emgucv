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
//#include "opencv2/calib3d/calib3d.hpp"
#include "emgu_c.h"
CVAPI(IplImage*) cvGetImageSubRect(IplImage* image, CvRect* rect);

//GrabCut
CVAPI(void) CvGrabCut(IplImage* img, IplImage* mask, cv::Rect* rect, IplImage* bgdModel, IplImage* fgdModel, int iterCount, int flag);

CVAPI(bool) cvCheckRange(CvArr* arr, bool quiet, CvPoint* index, double minVal, double maxVal);

CVAPI(void) cvArrSqrt(CvArr* src, CvArr* dst);

CVAPI(void) CvFilter2D( const CvArr* srcarr, CvArr* dstarr, const CvMat* _kernel, CvPoint anchor, double delta, int borderType );

CVAPI(void) cvCLAHE(const CvArr* srcArr, double clipLimit, emgu::size tileGridSize, CvArr* dstArr);
#endif
