//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_PTCLOUD_C_H
#define EMGU_PTCLOUD_C_H

#include "opencv2/core.hpp"
#include "cvapi_compat.h"

#ifdef HAVE_OPENCV_PTCLOUD
#include "opencv2/ptcloud.hpp"
#else
static inline CV_NORETURN void throw_no_ptcloud() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without ptcloud support. To use this module, please switch to the full Emgu CV runtime."); }

namespace cv {
	class Odometry {};
	class RgbdNormals {};
	class Octree {};
}
#endif

CVAPI(cv::Odometry*) cveOdometryCreate(int odometryType);
CVAPI(void) cveOdometryRelease(cv::Odometry** ptr);
CVAPI(bool) cveOdometryCompute1(cv::Odometry* odometry, cv::_InputArray* srcFrame, cv::_InputArray* dstFrame, cv::_OutputArray* rt);
CVAPI(bool) cveOdometryCompute2(cv::Odometry* odometry, cv::_InputArray* srcDepthFrame, cv::_InputArray* srcRGBFrame, cv::_InputArray* dstDepthFrame, cv::_InputArray* dstRGBFrame, cv::_OutputArray* rt);

CVAPI(cv::RgbdNormals*) cveRgbdNormalsCreate(int rows, int cols, int depth, cv::_InputArray* K, int window_size, int method, cv::Algorithm** algorithm, cv::Ptr<cv::RgbdNormals>** sharedPtr);
CVAPI(void) cveRgbdNormalsRelease(cv::Ptr<cv::RgbdNormals>** sharedPtr);
CVAPI(void) cveRgbdNormalsApply(cv::RgbdNormals* rgbdNormals, cv::_InputArray* points, cv::_OutputArray* normals);

CVAPI(cv::Octree*) cveOctreeCreate(std::vector<cv::Point3f>* pointCloud, int maxDepth, cv::Ptr<cv::Octree>** sharedPtr);
CVAPI(void) cveOctreeRelease(cv::Ptr<cv::Octree>** sharedPtr);

CVAPI(void) cveLoadPointCloud(
	cv::String* filename,
	cv::_OutputArray* vertices,
	cv::_OutputArray* normals,
	cv::_OutputArray* rgb);
CVAPI(void) cveSavePointCloud(
	cv::String* filename,
	cv::_InputArray* vertices,
	cv::_InputArray* normals,
	cv::_InputArray* rgb);
CVAPI(void) cveLoadMesh(
	cv::String* filename,
	cv::_OutputArray* vertices,
	cv::_OutputArray* indices,
	cv::_OutputArray* normals,
	cv::_OutputArray* colors,
	cv::_OutputArray* texCoords);
CVAPI(void) cveSaveMesh(
	cv::String* filename,
	cv::_InputArray* vertices,
	cv::_InputArray* indices,
	cv::_InputArray* normals,
	cv::_InputArray* colors,
	cv::_InputArray* texCoords);

#endif
