//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_HFS_C_H
#define EMGU_HFS_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_HFS
#include "opencv2/hfs.hpp"
#else
static inline CV_NORETURN void throw_no_hfs() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without hfs module support"); }

namespace cv {
	namespace hfs {
		class HfsSegment {};
	}
}
#endif

CVAPI(cv::hfs::HfsSegment*) cveHfsSegmentCreate(
	int height, 
	int width,
	float segEgbThresholdI, 
	int minRegionSizeI,
	float segEgbThresholdII, 
	int minRegionSizeII,
	float spatialWeight, 
	int slicSpixelSize, 
	int numSlicIter,
	cv::Algorithm** algorithmPtr, 
	cv::Ptr<cv::hfs::HfsSegment>** sharedPtr);

CVAPI(void) cveHfsSegmentRelease(cv::Ptr<cv::hfs::HfsSegment>** hfsSegmentPtr);

CVAPI(void) cveHfsPerformSegment(cv::hfs::HfsSegment* hfsSegment, cv::_InputArray* src, cv::Mat* dst, bool ifDraw, bool useGpu);

#endif