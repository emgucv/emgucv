//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_RGBD_C_H
#define EMGU_RGBD_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_RGBD
#include "opencv2/rgbd.hpp"
#else
static inline CV_NORETURN void throw_no_rgbd() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without rgbd support"); }

namespace cv
{
	namespace rgbd
	{
		class Odometry {};
		class RgbdNormals {};
	}
}

#endif


CVAPI(cv::rgbd::Odometry*) cveOdometryCreate(
    cv::String* odometryType,
    cv::Algorithm** algorithm, 
    cv::Ptr<cv::rgbd::Odometry>** sharedPtr
);
CVAPI(void) cveOdometryRelease(cv::Ptr<cv::rgbd::Odometry>** sharedPtr);
CVAPI(bool) cveOdometryCompute(
	cv::rgbd::Odometry* odometry,
	cv::Mat* srcImage,
	cv::Mat* srcDepth, 
	cv::Mat* srcMask, 
	cv::Mat* dstImage,
	cv::Mat* dstDepth,
	cv::Mat* dstMask,
	cv::_OutputArray* rt,
	cv::Mat* initRt);

CVAPI(cv::rgbd::RgbdNormals*) cveRgbdNormalsCreate(
	int rows,
	int cols,
	int depth,
	cv::_InputArray* K,
	int window_size,
	int method,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::rgbd::RgbdNormals>** sharedPtr);
CVAPI(void) cveRgbdNormalsRelease(cv::Ptr<cv::rgbd::RgbdNormals>** sharedPtr);
CVAPI(void) cveRgbdNormalsApply(
	cv::rgbd::RgbdNormals* rgbdNormals,
	cv::_InputArray* points,
	cv::_OutputArray* normals);
#endif