//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_DNN_OBJDETECT_C_H
#define EMGU_DNN_OBJDETECT_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_DNN_OBJDETECT
#include "opencv2/core_detect.hpp"
#else

static inline CV_NORETURN void throw_no_dnn_objdetect() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without Dnn Objdetect support"); }

namespace cv
{
	namespace dnn_objdetect
	{
		class InferBbox
		{
			
		};
	}
}
#endif

CVAPI(cv::dnn_objdetect::InferBbox*) cveInferBboxCreate(cv::Mat* deltaBbox, cv::Mat* classScores, cv::Mat* confScores);
CVAPI(void) cveInferBboxFilter(cv::dnn_objdetect::InferBbox* inferBbox, double thresh);
CVAPI(void) cveInferBboxRelease(cv::dnn_objdetect::InferBbox** inferBbox);

#endif