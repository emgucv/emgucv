//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_SUPERRES_C_H
#define EMGU_SUPERRES_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_SUPERRES
#include "opencv2/superres.hpp"
#else

static inline CV_NORETURN void throw_no_superres() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without Superres support"); }

namespace cv
{
	namespace superres
	{
		class FrameSource
		{
			
		};

		class SuperResolution
		{
			
		};
	}
}
#endif

CVAPI(cv::superres::FrameSource*) cveSuperresCreateFrameSourceVideo(cv::String* fileName, bool useGpu, cv::Ptr<cv::superres::FrameSource>** sharedPtr);
CVAPI(cv::superres::FrameSource*) cveSuperresCreateFrameSourceCamera(int deviceId, cv::Ptr<cv::superres::FrameSource>** sharedPtr);
CVAPI(void) cveSuperresFrameSourceNextFrame(cv::superres::FrameSource* frameSource, cv::_OutputArray* frame);
CVAPI(void) cveSuperresFrameSourceRelease(cv::Ptr<cv::superres::FrameSource>** sharedPtr);

CVAPI(cv::superres::SuperResolution*) cveSuperResolutionCreate(int type, cv::superres::FrameSource* frameSource, cv::superres::FrameSource** frameSourceOut, cv::Ptr<cv::superres::SuperResolution>**);
CVAPI(void) cveSuperResolutionRelease(cv::Ptr<cv::superres::SuperResolution>** sharedPtr);
#endif