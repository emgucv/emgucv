//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
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

CVAPI(void) cveAutowbGrayworld(cv::_InputArray* src, cv::_OutputArray* dst, float thresh);

CVAPI(void) cveDctDenoising(const cv::Mat* src, cv::Mat* dst, const double sigma, const int psize);

CVAPI(void) cveXInpaint(const cv::Mat* src, const cv::Mat* mask, cv::Mat* dst, const int algorithmType);
#endif