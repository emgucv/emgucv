//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_BGSEGM_C_H
#define EMGU_BGSEGM_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_BGSEGM
#include "opencv2/bgsegm.hpp"
#else
static inline CV_NORETURN void throw_no_bgsegm() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without bgsegm module support"); }
namespace cv {
	class BackgroundSubtractor {};
	namespace bgsegm {
		class BackgroundSubtractorMOG {};
		class BackgroundSubtractorGMG {};
		class BackgroundSubtractorCNT {};
		class BackgroundSubtractorGSOC {};
		class BackgroundSubtractorLSBP {};
	}
}
#endif

//BackgroundSubtractorMOG
CVAPI(cv::bgsegm::BackgroundSubtractorMOG*) cveBackgroundSubtractorMOGCreate(int history, int nmixtures, double backgroundRatio, double noiseSigma, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm, cv::Ptr<cv::bgsegm::BackgroundSubtractorMOG>** sharedPtr);
CVAPI(void) cveBackgroundSubtractorMOGRelease(cv::bgsegm::BackgroundSubtractorMOG** bgSubtractor, cv::Ptr<cv::bgsegm::BackgroundSubtractorMOG>** sharedPtr);

//BackgroundSubtractorGMG
CVAPI(cv::bgsegm::BackgroundSubtractorGMG*) cveBackgroundSubtractorGMGCreate(int initializationFrames, double decisionThreshold, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm, cv::Ptr<cv::bgsegm::BackgroundSubtractorGMG>** sharedPtr);
CVAPI(void) cveBackgroundSubtractorGMGRelease(cv::bgsegm::BackgroundSubtractorGMG** bgSubtractor, cv::Ptr<cv::bgsegm::BackgroundSubtractorGMG>** sharedPtr);

//BackgroundSubtractorCNT
CVAPI(cv::bgsegm::BackgroundSubtractorCNT*) cveBackgroundSubtractorCNTCreate(
	int minPixelStability,
	bool useHistory,
	int maxPixelStability,
	bool isParallel,
	cv::BackgroundSubtractor** bgSubtractor, 
	cv::Algorithm** algorithm,
	cv::Ptr<cv::bgsegm::BackgroundSubtractorCNT>** sharedPtr);
CVAPI(void) cveBackgroundSubtractorCNTRelease(cv::bgsegm::BackgroundSubtractorCNT** bgSubtractor, cv::Ptr<cv::bgsegm::BackgroundSubtractorCNT>** sharedPtr);

//BackgroundSubtractorGSOC
CVAPI(cv::bgsegm::BackgroundSubtractorGSOC*) cveBackgroundSubtractorGSOCCreate(
	int mc, int nSamples, float replaceRate, float propagationRate, int hitsThreshold, float alpha, float beta, float blinkingSupressionDecay, float blinkingSupressionMultiplier, float noiseRemovalThresholdFacBG, float noiseRemovalThresholdFacFG, 
	cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm, cv::Ptr<cv::bgsegm::BackgroundSubtractorGSOC>** sharedPtr);
CVAPI(void) cveBackgroundSubtractorGSOCRelease(cv::bgsegm::BackgroundSubtractorGSOC** bgSubtractor, cv::Ptr<cv::bgsegm::BackgroundSubtractorGSOC>** sharedPtr);


//BackgroundSubtractorLSBP
CVAPI(cv::bgsegm::BackgroundSubtractorLSBP*) cveBackgroundSubtractorLSBPCreate(
	int mc, int nSamples, int LSBPRadius, float tlower, float tupper, float tinc, float tdec, float rscale, float rincdec, float noiseRemovalThresholdFacBG, float noiseRemovalThresholdFacFG, int LSBPthreshold, int minCount, 
	cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm, cv::Ptr<cv::bgsegm::BackgroundSubtractorLSBP>** sharedPtr);
CVAPI(void) cveBackgroundSubtractorLSBPRelease(cv::bgsegm::BackgroundSubtractorLSBP** bgSubtractor, cv::Ptr<cv::bgsegm::BackgroundSubtractorLSBP>** sharedPtr);

#endif