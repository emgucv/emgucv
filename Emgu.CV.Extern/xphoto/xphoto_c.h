//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_XPHOTO_C_H
#define EMGU_XPHOTO_C_H

//#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/core/core_c.h"
#include "opencv2/xphoto.hpp"

CVAPI(void) cveBalanceWhite(const cv::Mat* src, cv::Mat* dst, const int algorithmType,
   const float inputMin, const float inputMax,
   const float outputMin, const float outputMax);

CVAPI(void) cveDctDenoising(const cv::Mat* src, cv::Mat* dst, const double sigma, const int psize);
#endif