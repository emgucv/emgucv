//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_FUZZY_C_H
#define EMGU_FUZZY_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/fuzzy.hpp"

CVAPI(void) cveFtCreateKernel(cv::_InputArray* A, cv::_InputArray* B, cv::_OutputArray* kernel, int chn);
CVAPI(void) cveFtcreateKernelFromFunction(int function, int radius, cv::_OutputArray* kernel, int chn);

CVAPI(void) cveFtInpaint(cv::Mat* image, cv::Mat* mask, cv::Mat* output, int radius, int function, int algorithm);
CVAPI(void) cveFtFilter(cv::Mat* image, cv::Mat* kernel, cv::Mat* output);
#endif