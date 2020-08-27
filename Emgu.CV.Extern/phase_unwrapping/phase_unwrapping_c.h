//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_PHASE_UNWRAPPING_C_H
#define EMGU_PHASE_UNWRAPPING_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_PHASE_UNWRAPPING
#include "opencv2/phase_unwrapping.hpp"
#else
static inline CV_NORETURN void throw_no_phase_unwrapping() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without phase unwrapping support"); }
namespace cv {
namespace phase_unwrapping {
class HistogramPhaseUnwrapping {};
}
}

#endif

CVAPI(cv::phase_unwrapping::HistogramPhaseUnwrapping*) cveHistogramPhaseUnwrappingCreate(
	int width,
	int height,
	float histThresh,
	int nbrOfSmallBins,
	int nbrOfLargeBins,
	cv::Ptr<cv::phase_unwrapping::HistogramPhaseUnwrapping>** sharedPtr);

CVAPI(void) cveHistogramPhaseUnwrappingRelease(cv::phase_unwrapping::HistogramPhaseUnwrapping** phase_unwrapping, cv::Ptr<cv::phase_unwrapping::HistogramPhaseUnwrapping>** sharedPtr);

CVAPI(void) cveHistogramPhaseUnwrappingGetInverseReliabilityMap(cv::phase_unwrapping::HistogramPhaseUnwrapping* phase_unwrapping, cv::_OutputArray* reliabilityMap);

CVAPI(void) cveHistogramPhaseMapUnwrappingUnwrapPhaseMap(
	cv::phase_unwrapping::HistogramPhaseUnwrapping* phase_unwrapping,
	cv::_InputArray* wrappedPhaseMap,
	cv::_OutputArray* unwrappedPhaseMap,
	cv::_InputArray* shadowMask);

#endif