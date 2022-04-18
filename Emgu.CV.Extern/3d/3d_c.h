//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_3D_C_H
#define EMGU_3D_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/3d.hpp"



CVAPI(cv::Odometry*) cveOdometryCreate(cv::OdometryType odometryType);
CVAPI(void) cveOdometryRelease(cv::Odometry** sharedPtr);
CVAPI(bool) cveOdometryCompute1(
	cv::Odometry* odometry,
	cv::_InputArray* srcFrame, 
	cv::_InputArray* dstFrame, 
	cv::_OutputArray* rt);

CVAPI(bool) cveOdometryCompute2(
	cv::Odometry* odometry,
	cv::_InputArray* srcDepthFrame, 
	cv::_InputArray* srcRGBFrame, 
	cv::_InputArray* dstDepthFrame, 
	cv::_InputArray* dstRGBFrame, 
	cv::_OutputArray* rt);


CVAPI(cv::RgbdNormals*) cveRgbdNormalsCreate(
	int rows,
	int cols,
	int depth,
	cv::_InputArray* K,
	int window_size,
	int method,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::RgbdNormals>** sharedPtr);
CVAPI(void) cveRgbdNormalsRelease(cv::Ptr<cv::RgbdNormals>** sharedPtr);
CVAPI(void) cveRgbdNormalsApply(
	cv::RgbdNormals* rgbdNormals,
	cv::_InputArray* points,
	cv::_OutputArray* normals);

CVAPI(void) cveDecomposeEssentialMat(cv::_InputArray* e, cv::_OutputArray* r1, cv::_OutputArray* r2, cv::_OutputArray* t);

CVAPI(int) cveDecomposeHomographyMat(
	cv::_InputArray* h,
	cv::_InputArray* k,
	cv::_OutputArray* rotations,
	cv::_OutputArray* translations,
	cv::_OutputArray* normals);

#endif