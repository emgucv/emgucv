//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDASTEREO_C_H
#define EMGU_CUDASTEREO_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_CUDASTEREO
#include "opencv2/cudastereo.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "emgu_c.h"
#else
static inline CV_NORETURN void throw_no_cudastereo() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without CUDA Stereo support"); }

namespace cv
{
	namespace cuda
	{
		class StereoBM
		{
		};

		class StereoConstantSpaceBP
		{
		};

		class DisparityBilateralFilter
		{
		};
	}
}

#endif

//----------------------------------------------------------------------------
//
//  Cuda Stereo
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::StereoBM*) cudaStereoBMCreate(int numDisparities, int blockSize, cv::Ptr<cv::cuda::StereoBM>** sharedPtr);

CVAPI(void) cudaStereoBMFindStereoCorrespondence(cv::cuda::StereoBM* stereo, cv::_InputArray* left, cv::_InputArray* right, cv::_OutputArray* disparity, cv::cuda::Stream* stream);

CVAPI(void) cudaStereoBMRelease(cv::Ptr<cv::cuda::StereoBM>** stereoBM);

CVAPI(cv::cuda::StereoConstantSpaceBP*) cudaStereoConstantSpaceBPCreate(int ndisp, int iters, int levels, int nr_plane, cv::Ptr<cv::cuda::StereoConstantSpaceBP>** sharedPtr);

CVAPI(void) cudaStereoConstantSpaceBPFindStereoCorrespondence(cv::cuda::StereoConstantSpaceBP* stereo, cv::_InputArray* left, cv::_InputArray* right, cv::_OutputArray* disparity, cv::cuda::Stream* stream);

CVAPI(void) cudaStereoConstantSpaceBPRelease(cv::Ptr<cv::cuda::StereoConstantSpaceBP>** stereo);

CVAPI(cv::cuda::DisparityBilateralFilter*) cudaDisparityBilateralFilterCreate(int ndisp, int radius, int iters, cv::Ptr<cv::cuda::DisparityBilateralFilter>** sharedPtr);

CVAPI(void) cudaDisparityBilateralFilterApply(cv::cuda::DisparityBilateralFilter* filter, cv::_InputArray* disparity, cv::_InputArray* image, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaDisparityBilateralFilterRelease(cv::Ptr<cv::cuda::DisparityBilateralFilter>** filter);

CVAPI(void) cudaDrawColorDisp(cv::_InputArray* srcDisp, cv::_OutputArray* dstDisp, int ndisp, cv::cuda::Stream* stream);

#endif