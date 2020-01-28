//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_DNN_SUPERRES_C_H
#define EMGU_DNN_SUPERRES_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_DNN_SUPERRES
#include "opencv2/dnn_superres.hpp"
#else
static inline CV_NORETURN void throw_no_dnn_superres() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without Dnn Superres support"); }

namespace cv
{
	namespace dnn_superres
	{
		class DnnSuperResImpl
		{

		};
	}
}

#endif

CVAPI(cv::dnn_superres::DnnSuperResImpl*) cveDnnSuperResImplCreate();
CVAPI(void) cveDnnSuperResImplSetModel(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, const cv::String* algo, int scale);
CVAPI(void) cveDnnSuperResImplReadModel1(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, const cv::String* path);
CVAPI(void) cveDnnSuperResImplReadModel2(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, const cv::String* weights, cv::String* definition);
CVAPI(void) cveDnnSuperResImplUpsample(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, cv::_InputArray* img, cv::_OutputArray* result);
CVAPI(int) cveDnnSuperResImplGetScale(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes);
CVAPI(void) cveDnnSuperResImplGetAlgorithm(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, cv::String* algorithm);
CVAPI(void) cveDnnSuperResImplRelease(cv::dnn_superres::DnnSuperResImpl** dnnSuperRes);

#endif