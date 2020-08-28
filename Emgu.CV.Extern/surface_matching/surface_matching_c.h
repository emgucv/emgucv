//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_SURFACE_MATCHING_C_H
#define EMGU_SURFACE_MATCHING_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_SURFACE_MATCHING
#include "opencv2/surface_matching.hpp"
#else
static inline CV_NORETURN void throw_no_surface_matching() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without surface matching support"); }

namespace cv {
	namespace ppf_match_3d {
		class ICP {};
	}
}

#endif

CVAPI(cv::ppf_match_3d::ICP*) cveICPCreate(
	int iterations, 
	float tolerence, 
	float rejectionScale, 
	int numLevels, 
	int sampleType, 
	int numMaxCorr);

CVAPI(int) cveICPRegisterModelToScene(cv::ppf_match_3d::ICP* icp, cv::Mat* srcPC, cv::Mat* dstPC, double* residual, cv::Mat* pose);

CVAPI(void) cveICPRelease(cv::ppf_match_3d::ICP** icp);

#endif