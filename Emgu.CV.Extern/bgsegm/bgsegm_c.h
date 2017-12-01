//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_BGSEGM_C_H
#define EMGU_BGSEGM_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/bgsegm.hpp"

//BackgroundSubtractorMOG
CVAPI(cv::bgsegm::BackgroundSubtractorMOG*) cveBackgroundSubtractorMOGCreate(int history, int nmixtures, double backgroundRatio, double noiseSigma, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm);
CVAPI(void) cveBackgroundSubtractorMOGRelease(cv::bgsegm::BackgroundSubtractorMOG** bgSubtractor);

//BackgroundSubtractorGMG
CVAPI(cv::bgsegm::BackgroundSubtractorGMG*) cveBackgroundSubtractorGMGCreate(int initializationFrames, double decisionThreshold, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm);
CVAPI(void) cveBackgroundSubtractorGMGRelease(cv::bgsegm::BackgroundSubtractorGMG** bgSubtractor);

//BackgroundSubtractorCNT
CVAPI(cv::bgsegm::BackgroundSubtractorCNT*) cveBackgroundSubtractorCNTCreate(
	int minPixelStability,
	bool useHistory,
	int maxPixelStability,
	bool isParallel,
	cv::BackgroundSubtractor** bgSubtractor, 
	cv::Algorithm** algorithm);
CVAPI(void) cveBackgroundSubtractorCNTRelease(cv::bgsegm::BackgroundSubtractorCNT** bgSubtractor);

#endif