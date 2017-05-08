//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_SALIENCY_C_H
#define EMGU_SALIENCY_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/saliency.hpp"

CVAPI(cv::saliency::Saliency*) cveSaliencyCreate(cv::String* saliencyType);
CVAPI(void) cveSaliencyRelease(cv::saliency::Saliency** saliency);

CVAPI(bool) cveSaliencyComputeSaliency(cv::saliency::Saliency* saliency, cv::_InputArray* image, cv::_OutputArray* saliencyMap);

CVAPI(bool) cveSaliencyStaticComputeBinaryMap(cv::saliency::Saliency* saliency, cv::Mat* saliencyMap, cv::Mat* binaryMap);

CVAPI(bool) cveSaliencyMotionInit(cv::saliency::Saliency* saliency);
CVAPI(void) cveSaliencyMotionSetImageSize(cv::saliency::Saliency* saliency, int width, int height);

CVAPI(void) cveSaliencySetTrainingPath(cv::saliency::Saliency* saliency, cv::String* trainingPath);
CVAPI(void) cveSaliencyGetObjectnessValues(cv::saliency::Saliency* saliency, std::vector<float>* values);

CVAPI(cv::Algorithm*) cveSaliencyGetAlgorithm(cv::saliency::Saliency* saliency);
#endif