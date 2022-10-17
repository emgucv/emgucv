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
static inline CV_NORETURN void throw_no_rgbd() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without rgbd support. To use this module, please switch to the full Emgu CV runtime."); }

namespace cv
{
	namespace rgbd
	{
		class Odometry {};
		class RgbdNormals {};
	}

	namespace linemod
	{
		class Detector {};
		class Modality {};
		struct Match {};
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


CVAPI(cv::linemod::Detector*) cveLinemodLineDetectorCreate(cv::Ptr<cv::linemod::Detector>** sharedPtr);
CVAPI(cv::linemod::Detector*) cveLinemodLinemodDetectorCreate(cv::Ptr<cv::linemod::Detector>** sharedPtr);
CVAPI(void) cveLinemodDetectorRead(cv::linemod::Detector* detector, cv::FileNode* fn);
CVAPI(void) cveLinemodDetectorWrite(cv::linemod::Detector* detector, cv::FileStorage* fs);
CVAPI(int) cveLinemodDetectorAddTemplate(
	cv::linemod::Detector* detector, 
	std::vector< cv::Mat >* sources, 
	cv::String* classId,
	cv::Mat* objectMask, 
	CvRect* boundingBox);
CVAPI(void) cveLinemodDetectorGetClassIds(cv::linemod::Detector* detector, std::vector< cv::String >* classIds);
CVAPI(void) cveLinemodDetectorMatch(
	cv::linemod::Detector* detector, 
	std::vector< cv::Mat >* sources, 
	float threshold, 
	std::vector< cv::linemod::Match >* matches,
	std::vector< cv::String >* classIds,
	cv::_OutputArray* quantizedImages,
	std::vector< cv::Mat >* masks);

CVAPI(int) cveLinemodDetectorGetT(cv::linemod::Detector* detector, int pyramidLevel);

CVAPI(void) cveLinemodDetectorGetModalities(cv::linemod::Detector* detector, std::vector< void* >* vectorOfPtrs);

CVAPI(void) cveLinemodDetectorRelease(cv::Ptr<cv::linemod::Detector>** sharedPtr);

CVAPI(cv::linemod::Match*) cveLinemodMatchCreate();
CVAPI(void) cveLinemodMatchRelease(cv::linemod::Match** match);

CVAPI(cv::linemod::Modality*) cveLinemodModalityCreate(cv::String* modalityType, cv::Ptr<cv::linemod::Modality>** sharedPtr);
CVAPI(void) cveLinemodModalityRelease(cv::Ptr<cv::linemod::Modality>** sharedPtr);

#endif