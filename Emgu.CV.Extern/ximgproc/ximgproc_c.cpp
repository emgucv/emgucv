//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "ximgproc_c.h"

void cveDtFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, double sigmaSpatial, double sigmaColor, int mode, int numIters)
{
   cv::ximgproc::dtFilter(*guide, *src, *dst, sigmaSpatial, sigmaColor, mode, numIters);
}

void cveGuidedFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, int radius, double eps, int dDepth)
{
   cv::ximgproc::guidedFilter(*guide, *src, *dst, radius, eps, dDepth);
}

void cveAmFilter(cv::_InputArray* joint, cv::_InputArray* src, cv::_OutputArray* dst, double sigmaS, double sigmaR, bool adjustOutliers)
{
   cv::ximgproc::amFilter(*joint, *src, *dst, sigmaS, sigmaR, adjustOutliers);
}

void cveJointBilateralFilter(cv::_InputArray* joint, cv::_InputArray* src, cv::_OutputArray* dst, int d, double sigmaColor, double sigmaSpace, int borderType)
{
   cv::ximgproc::jointBilateralFilter(*joint, *src, *dst, d, sigmaColor, sigmaSpace, borderType);
}

void cveFastGlobalSmootherFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, double lambda, double sigmaColor, double lambdaAttenuation, int numIter)
{
   cv::ximgproc::fastGlobalSmootherFilter(*guide, *src, *dst, lambda, sigmaColor, lambdaAttenuation, numIter);
}