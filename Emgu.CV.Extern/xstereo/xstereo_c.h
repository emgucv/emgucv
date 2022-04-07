//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_STEREO_C_H
#define EMGU_STEREO_C_H

#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_XSTEREO
#include "opencv2/xstereo.hpp"
#else
static inline CV_NORETURN void throw_no_xstereo() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without xstereo support"); }
namespace cv {
	namespace stereo {
		class QuasiDenseStereo {};
		struct PropagationParameters {};
	}
}
#endif

CVAPI(cv::stereo::QuasiDenseStereo*) cveQuasiDenseStereoCreate(
	CvSize* monoImgSize, 
	cv::String* paramFilepath,
	cv::Ptr<cv::stereo::QuasiDenseStereo>** sharedPtr);

CVAPI(void) cveQuasiDenseStereoRelease(cv::Ptr<cv::stereo::QuasiDenseStereo>** sharedPtr);

CVAPI(void) cveQuasiDenseStereoProcess(cv::stereo::QuasiDenseStereo* stereo, cv::Mat* imgLeft, cv::Mat* imgRight);

CVAPI(void) cveQuasiDenseStereoGetDisparity(cv::stereo::QuasiDenseStereo* stereo, cv::Mat* disparity);

#endif