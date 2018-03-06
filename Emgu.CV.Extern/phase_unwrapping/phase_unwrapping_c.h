//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_PHASE_UNWRAPPING_C_H
#define EMGU_PHASE_UNWRAPPING_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/phase_unwrapping.hpp"

CVAPI(cv::phase_unwrapping::HistogramPhaseUnwrapping*) cveHistogramPhaseUnwrappingCreate(
	int width,
	int height,
	float histThresh,
	int nbrOfSmallBins,
	int nbrOfLargeBins);

CVAPI(void) cveHistogramPhaseUnwrappingRelease(cv::phase_unwrapping::HistogramPhaseUnwrapping** phase_unwrapping);

CVAPI(void) cveHistogramPhaseUnwrappingGetInverseReliabilityMap(cv::phase_unwrapping::HistogramPhaseUnwrapping* phase_unwrapping, cv::_OutputArray* reliabilityMap);

CVAPI(void) cveHistogramPhaseMapUnwrappingUnwrapPhaseMap(
	cv::phase_unwrapping::HistogramPhaseUnwrapping* phase_unwrapping,
	cv::_InputArray* wrappedPhaseMap,
	cv::_OutputArray* unwrappedPhaseMap,
	cv::_InputArray* shadowMask);

#endif