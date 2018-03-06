//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
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

//BackgroundSubtractorGSOC
CVAPI(cv::bgsegm::BackgroundSubtractorGSOC*) cveBackgroundSubtractorGSOCCreate(
	int mc, int nSamples, float replaceRate, float propagationRate, int hitsThreshold, float alpha, float beta, float blinkingSupressionDecay, float blinkingSupressionMultiplier, float noiseRemovalThresholdFacBG, float noiseRemovalThresholdFacFG, 
	cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm);
CVAPI(void) cveBackgroundSubtractorGSOCRelease(cv::bgsegm::BackgroundSubtractorGSOC** bgSubtractor);


//BackgroundSubtractorLSBP
CVAPI(cv::bgsegm::BackgroundSubtractorLSBP*) cveBackgroundSubtractorLSBPCreate(
	int mc, int nSamples, int LSBPRadius, float tlower, float tupper, float tinc, float tdec, float rscale, float rincdec, float noiseRemovalThresholdFacBG, float noiseRemovalThresholdFacFG, int LSBPthreshold, int minCount, 
	cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm);
CVAPI(void) cveBackgroundSubtractorLSBPRelease(cv::bgsegm::BackgroundSubtractorLSBP** bgSubtractor);

#endif