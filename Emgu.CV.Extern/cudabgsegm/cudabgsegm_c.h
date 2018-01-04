//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDABGSEGM_C_H
#define EMGU_CUDABGSEGM_C_H

//#include "opencv2/cuda.hpp"
#include "opencv2/cudabgsegm.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "emgu_c.h"

//----------------------------------------------------------------------------
//
//  Cuda MOG
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::BackgroundSubtractorMOG*) cudaBackgroundSubtractorMOGCreate(int history, int nmixtures, double backgroundRatio, double noiseSigma, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm);
CVAPI(void) cudaBackgroundSubtractorMOGApply(cv::cuda::BackgroundSubtractorMOG* mog, cv::_InputArray* frame, cv::_OutputArray* fgMask, double learningRate, cv::cuda::Stream* stream);
CVAPI(void) cudaBackgroundSubtractorMOGRelease(cv::cuda::BackgroundSubtractorMOG** mog);

//----------------------------------------------------------------------------
//
//  Cuda MOG2
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::BackgroundSubtractorMOG2*) cudaBackgroundSubtractorMOG2Create(int history, double varThreshold, bool detectShadows, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm);
CVAPI(void) cudaBackgroundSubtractorMOG2Apply(cv::cuda::BackgroundSubtractorMOG2* mog, cv::_InputArray* frame, cv::_OutputArray* fgMask, double learningRate, cv::cuda::Stream* stream);
CVAPI(void) cudaBackgroundSubtractorMOG2Release(cv::cuda::BackgroundSubtractorMOG2** mog);

#endif