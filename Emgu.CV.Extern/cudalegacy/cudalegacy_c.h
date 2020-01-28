//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDALEGACY_C_H
#define EMGU_CUDLEGACY_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_CUDALEGACY
//#include "opencv2/cuda.hpp"
#include "opencv2/cudalegacy.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "emgu_c.h"
#else
static inline CV_NORETURN void throw_no_cudalegacy() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without CUDA Legacy support"); }
namespace cv
{
	namespace cuda
	{
		class BackgroundSubtractorGMG
		{
		};

		class BackgroundSubtractorFGD
		{
			
		};
	}
}
#endif

//----------------------------------------------------------------------------
//
//  Cuda GMG
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::BackgroundSubtractorGMG*) cudaBackgroundSubtractorGMGCreate(
	int initializationFrames,
	double decisionThreshold,
	cv::Ptr<cv::cuda::BackgroundSubtractorGMG>** sharedPtr);
CVAPI(void) cudaBackgroundSubtractorGMGApply(cv::cuda::BackgroundSubtractorGMG* gmg, cv::_InputArray* frame, cv::_OutputArray* fgMask, double learningRate, cv::cuda::Stream* stream);
CVAPI(void) cudaBackgroundSubtractorGMGRelease(cv::Ptr<cv::cuda::BackgroundSubtractorGMG>** gmg);

//----------------------------------------------------------------------------
//
//  Cuda BackgroundSubtractorFGD
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::BackgroundSubtractorFGD*) cudaBackgroundSubtractorFGDCreate(
	int Lc,
	int N1c,
	int N2c,
	int Lcc,
	int N1cc,
	int N2cc,
	bool isObjWithoutHoles,
	int performMorphing,
	float alpha1,
	float alpha2,
	float alpha3,
	float delta,
	float T,
	float minArea,
	cv::Ptr<cv::cuda::BackgroundSubtractorFGD>** sharedPtr);
CVAPI(void) cudaBackgroundSubtractorFGDApply(cv::cuda::BackgroundSubtractorFGD* fgd, cv::_InputArray* frame, cv::_OutputArray* fgMask, double learningRate);
CVAPI(void) cudaBackgroundSubtractorFGDRelease(cv::Ptr<cv::cuda::BackgroundSubtractorFGD>** fgd);

#endif

