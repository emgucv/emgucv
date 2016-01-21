//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_XIMGPROC_C_H
#define EMGU_XIMGPROC_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/ximgproc.hpp"

CVAPI(void) cveDtFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, double sigmaSpatial, double sigmaColor, int mode, int numIters);

CVAPI(void) cveGuidedFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, int radius, double eps, int dDepth);

CVAPI(void) cveAmFilter(cv::_InputArray* joint, cv::_InputArray* src, cv::_OutputArray* dst, double sigmaS, double sigmaR, bool adjustOutliers);

CVAPI(void) cveJointBilateralFilter(cv::_InputArray* joint, cv::_InputArray* src, cv::_OutputArray* dst, int d, double sigmaColor, double sigmaSpace, int borderType);

CVAPI(void) cveFastGlobalSmootherFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, double lambda, double sigmaColor, double lambdaAttenuation, int numIter);

#endif