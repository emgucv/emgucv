//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_SURFACE_MATCHING_C_H
#define EMGU_SURFACE_MATCHING_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/surface_matching.hpp"


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