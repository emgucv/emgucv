//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_DPM_C_H
#define EMGU_DPM_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/dpm.hpp"

using cv::dpm::DPMDetector;

//constructor
CVAPI(DPMDetector*) cveDPMDetectorCreate(std::vector<cv::String>* filenames, std::vector<cv::String>* classNames);

CVAPI(void) cveDPMDetectorDetect(DPMDetector* dpm, cv::Mat* image, std::vector<CvRect>* rects, std::vector<float>* scores, std::vector<int>* classIds);

CVAPI(size_t) cveDPMDetectorGetClassCount(DPMDetector* dpm);

CVAPI(void) cveDPMDetectorGetClassNames(DPMDetector* dpm, std::vector<cv::String>* names);

CVAPI(bool) cveDPMDetectorIsEmpty(DPMDetector* dpm);

CVAPI(void) cveDPMDetectorRelease(DPMDetector** dpm);
#endif