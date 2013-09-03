//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CORE_C_H
#define EMGU_CORE_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/imgproc/imgproc.hpp"
#include "emgu_c.h"

CVAPI(cv::Mat*) cvMatCreate();
CVAPI(void) cvMatRelease(cv::Mat** mat);
CVAPI(emgu::size) cvMatGetSize(cv::Mat* mat);
CVAPI(void) cvMatCopyToCvArr(cv::Mat* mat, CvArr* cvArray);
CVAPI(void) cvMatFromCvArr(cv::Mat* mat, CvArr* cvArray);
CVAPI(int) cvMatGetElementSize(cv::Mat* mat);

CVAPI(void) CvMinMaxIdx(CvArr* src, double* minVal, double* maxVal, int* minIdx, int* maxIdx, CvArr* mask);

#endif