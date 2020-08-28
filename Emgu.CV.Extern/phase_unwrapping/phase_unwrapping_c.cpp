//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "phase_unwrapping_c.h"

cv::phase_unwrapping::HistogramPhaseUnwrapping* cveHistogramPhaseUnwrappingCreate(
	int width,
	int height,
	float histThresh,
	int nbrOfSmallBins,
	int nbrOfLargeBins,
	cv::Ptr<cv::phase_unwrapping::HistogramPhaseUnwrapping>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHASE_UNWRAPPING
	cv::phase_unwrapping::HistogramPhaseUnwrapping::Params p;
	p.width = width;
	p.height = height;
	p.histThresh = histThresh;
	p.nbrOfSmallBins = nbrOfSmallBins;
	p.nbrOfLargeBins = nbrOfLargeBins;
	cv::Ptr<cv::phase_unwrapping::HistogramPhaseUnwrapping> unwrapping = cv::phase_unwrapping::HistogramPhaseUnwrapping::create(p);
	*sharedPtr = new cv::Ptr<cv::phase_unwrapping::HistogramPhaseUnwrapping>(unwrapping);
	return unwrapping.get();
#else
	throw_no_phase_unwrapping();
#endif
}

void cveHistogramPhaseUnwrappingRelease(cv::phase_unwrapping::HistogramPhaseUnwrapping** phase_unwrapping, cv::Ptr<cv::phase_unwrapping::HistogramPhaseUnwrapping>** sharedPtr)
{
#ifdef HAVE_OPENCV_PHASE_UNWRAPPING
	delete *sharedPtr;
	*phase_unwrapping = 0;
	*sharedPtr = 0;
#else
	throw_no_phase_unwrapping();
#endif
}

void cveHistogramPhaseUnwrappingGetInverseReliabilityMap(cv::phase_unwrapping::HistogramPhaseUnwrapping* phase_unwrapping, cv::_OutputArray* reliabilityMap)
{
#ifdef HAVE_OPENCV_PHASE_UNWRAPPING
	phase_unwrapping->getInverseReliabilityMap(*reliabilityMap);
#else
	throw_no_phase_unwrapping();
#endif
}

void cveHistogramPhaseMapUnwrappingUnwrapPhaseMap(
	cv::phase_unwrapping::HistogramPhaseUnwrapping* phase_unwrapping,
	cv::_InputArray* wrappedPhaseMap,
	cv::_OutputArray* unwrappedPhaseMap,
	cv::_InputArray* shadowMask)
{
#ifdef HAVE_OPENCV_PHASE_UNWRAPPING
	phase_unwrapping->unwrapPhaseMap(*wrappedPhaseMap, *unwrappedPhaseMap, shadowMask ? *shadowMask : dynamic_cast<cv::InputArray>(cv::noArray()));
#else
	throw_no_phase_unwrapping();
#endif
}