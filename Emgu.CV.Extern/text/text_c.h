//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_OBJDETECT_C_H
#define EMGU_OBJDETECT_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/text/erfilter.hpp"
#include "vectors_c.h"

//ERFilter
CVAPI(cv::text::ERFilter*) cveERFilterNM1Create(
   cv::String* classifier,
   int thresholdDelta,
   float minArea,
   float maxArea,
   float minProbability,
   bool nonMaxSuppression,
   float minProbabilityDiff);
CVAPI(cv::text::ERFilter*) cveERFilterNM2Create(cv::String* classifier, float minProbability);
CVAPI(void) cveERFilterRelease(cv::text::ERFilter** filter);
CVAPI(void) cveERFilterRun(cv::text::ERFilter* filter, cv::_InputArray* image, std::vector<cv::text::ERStat>* regions);

CVAPI(void) cveERGrouping(
   cv::_InputArray* image, cv::_InputArray* channels, 
   std::vector<cv::text::ERStat>** regions, int count, 
   std::vector< std::vector<cv::Vec2i> >* groups, std::vector<cv::Rect>* group_rects, 
   int method, cv::String* fileName, float minProbability );

CVAPI(void) cveMSERsToERStats(
	cv::_InputArray* image, 
	std::vector< std::vector< cv::Point > >* contours,
	std::vector< std::vector< cv::text::ERStat> >* regions);

CVAPI(void) cveComputeNMChannels(cv::_InputArray* src, cv::_OutputArray* channels, int mode);
#endif