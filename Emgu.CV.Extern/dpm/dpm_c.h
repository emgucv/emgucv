//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_DPM_C_H
#define EMGU_DPM_C_H

#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_DPM
#include "opencv2/dpm.hpp"
#else
static inline CV_NORETURN void throw_no_dpm() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without dpm support"); }

namespace  cv {
	namespace dpm {
		class DPMDetector {};
	}
}
#endif

using cv::dpm::DPMDetector;

//constructor
CVAPI(DPMDetector*) cveDPMDetectorCreate(std::vector<cv::String>* filenames, std::vector<cv::String>* classNames, cv::Ptr<cv::dpm::DPMDetector>** sharedPtr);

CVAPI(void) cveDPMDetectorDetect(DPMDetector* dpm, cv::Mat* image, std::vector<CvRect>* rects, std::vector<float>* scores, std::vector<int>* classIds);

CVAPI(size_t) cveDPMDetectorGetClassCount(DPMDetector* dpm);

CVAPI(void) cveDPMDetectorGetClassNames(DPMDetector* dpm, std::vector<cv::String>* names);

CVAPI(bool) cveDPMDetectorIsEmpty(DPMDetector* dpm);

CVAPI(void) cveDPMDetectorRelease(cv::Ptr<cv::dpm::DPMDetector>** sharedPtr);
#endif