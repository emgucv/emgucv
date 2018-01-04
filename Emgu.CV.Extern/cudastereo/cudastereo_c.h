//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDASTEREO_C_H
#define EMGU_CUDASTEREO_C_H

//#include "opencv2/cuda.hpp"
#include "opencv2/cudastereo.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "emgu_c.h"

//----------------------------------------------------------------------------
//
//  Cuda Stereo
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::StereoBM*) cudaStereoBMCreate(int numDisparities, int blockSize);

CVAPI(void) cudaStereoBMFindStereoCorrespondence(cv::cuda::StereoBM* stereo, cv::_InputArray* left, cv::_InputArray* right, cv::_OutputArray* disparity, cv::cuda::Stream* stream);

CVAPI(void) cudaStereoBMRelease(cv::cuda::StereoBM** stereoBM);

CVAPI(cv::cuda::StereoConstantSpaceBP*) cudaStereoConstantSpaceBPCreate(int ndisp, int iters, int levels, int nr_plane);

CVAPI(void) cudaStereoConstantSpaceBPFindStereoCorrespondence(cv::cuda::StereoConstantSpaceBP* stereo, cv::_InputArray* left, cv::_InputArray* right, cv::_OutputArray* disparity, cv::cuda::Stream* stream);

CVAPI(void) cudaStereoConstantSpaceBPRelease(cv::cuda::StereoConstantSpaceBP** stereoBM);

CVAPI(cv::cuda::DisparityBilateralFilter*) cudaDisparityBilateralFilterCreate(int ndisp, int radius, int iters);

CVAPI(void) cudaDisparityBilateralFilterApply(cv::cuda::DisparityBilateralFilter* filter, cv::_InputArray* disparity, cv::_InputArray* image, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaDisparityBilateralFilterRelease(cv::cuda::DisparityBilateralFilter** filter);
#endif