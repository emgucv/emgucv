//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDABGSEGM_C_H
#define EMGU_CUDABGSEGM_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_CUDABGSEGM
#include "opencv2/cudabgsegm.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "emgu_c.h"
#else
static inline CV_NORETURN void throw_no_cudabgsegm() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without CUDA BgSegm support"); }


namespace cv
{
	namespace cuda
	{
		class BackgroundSubtractorMOG
		{
		};

		class BackgroundSubtractorMOG2
		{
		};
	}
}

namespace cv
{
	class BackgroundSubtractor
	{
	};
}
#endif

//----------------------------------------------------------------------------
//
//  Cuda MOG
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::BackgroundSubtractorMOG*) cudaBackgroundSubtractorMOGCreate(
	int history, int nmixtures, double backgroundRatio, double noiseSigma, 
	cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm,
	cv::Ptr<cv::cuda::BackgroundSubtractorMOG>** sharedPtr);
CVAPI(void) cudaBackgroundSubtractorMOGApply(cv::cuda::BackgroundSubtractorMOG* mog, cv::_InputArray* frame, cv::_OutputArray* fgMask, double learningRate, cv::cuda::Stream* stream);
CVAPI(void) cudaBackgroundSubtractorMOGRelease(cv::Ptr<cv::cuda::BackgroundSubtractorMOG>** mog);

//----------------------------------------------------------------------------
//
//  Cuda MOG2
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::BackgroundSubtractorMOG2*) cudaBackgroundSubtractorMOG2Create(
	int history, double varThreshold, bool detectShadows, 
	cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm,
	cv::Ptr<cv::cuda::BackgroundSubtractorMOG2>** sharedPtr);
CVAPI(void) cudaBackgroundSubtractorMOG2Apply(cv::cuda::BackgroundSubtractorMOG2* mog, cv::_InputArray* frame, cv::_OutputArray* fgMask, double learningRate, cv::cuda::Stream* stream);
CVAPI(void) cudaBackgroundSubtractorMOG2Release(cv::Ptr<cv::cuda::BackgroundSubtractorMOG2>** mog);

#endif