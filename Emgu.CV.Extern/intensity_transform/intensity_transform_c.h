//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_INTENSITY_TRANSFORM_C_H
#define EMGU_INTENSITY_TRANSFORM_C_H

#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_INTENSITY_TRANSFORM
#include "opencv2/intensity_transform.hpp"
#else
static inline CV_NORETURN void throw_no_intensity_transform() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without intensity transform."); }
#endif

CVAPI(void) cveLogTransform(cv::Mat* input, cv::Mat* output);

CVAPI(void) cveGammaCorrection(cv::Mat* input, cv::Mat* output, float gamma);

CVAPI(void) cveAutoscaling(cv::Mat*  input, cv::Mat*  output);

CVAPI(void) cveContrastStretching(cv::Mat*  input, cv::Mat*  output, int r1, int s1, int r2, int s2);

CVAPI(void) cveBIMEF(cv::_InputArray* input, cv::_OutputArray* output, float mu, float a, float b);

CVAPI(void) cveBIMEF2(cv::_InputArray* input, cv::_OutputArray* output, float k, float mu, float a, float b);

#endif