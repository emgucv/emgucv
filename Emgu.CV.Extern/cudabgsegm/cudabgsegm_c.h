//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDABGSEGM_C_H
#define EMGU_CUDABGSEGM_C_H

#include "opencv2/cuda.hpp"
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
CVAPI(cv::cuda::BackgroundSubtractorMOG*) cudaBackgroundSubtractorMOGCreate(int history, int nmixtures, double backgroundRatio, double noiseSigma);
CVAPI(void) cudaBackgroundSubtractorMOGApply(cv::cuda::BackgroundSubtractorMOG* mog, cv::_InputArray* frame, float learningRate, cv::_OutputArray* fgMask, cv::cuda::Stream* stream);
CVAPI(void) cudaBackgroundSubtractorMOGRelease(cv::cuda::BackgroundSubtractorMOG** mog);

//----------------------------------------------------------------------------
//
//  Cuda MOG2
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::BackgroundSubtractorMOG2*) cudaBackgroundSubtractorMOG2Create(int history, double varThreshold, bool detectShadows);
CVAPI(void) cudaBackgroundSubtractorMOG2Apply(cv::cuda::BackgroundSubtractorMOG2* mog, cv::_InputArray* frame, float learningRate, cv::_OutputArray* fgMask, cv::cuda::Stream* stream);
CVAPI(void) cudaBackgroundSubtractorMOG2Release(cv::cuda::BackgroundSubtractorMOG2** mog);

//----------------------------------------------------------------------------
//
//  Cuda GMG
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::BackgroundSubtractorGMG*) cudaBackgroundSubtractorGMGCreate(int initializationFrames, double decisionThreshold);
CVAPI(void) cudaBackgroundSubtractorGMGApply(cv::cuda::BackgroundSubtractorGMG* gmg, cv::cuda::GpuMat* frame, double learningRate, cv::_OutputArray* fgMask, cv::cuda::Stream* stream);
CVAPI(void) cudaBackgroundSubtractorGMGRelease(cv::cuda::BackgroundSubtractorGMG** gmg);

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
   float minArea);
CVAPI(void) cudaBackgroundSubtractorFGDApply(cv::cuda::BackgroundSubtractorFGD* fgd, cv::cuda::GpuMat* frame, double learningRate, cv::cuda::GpuMat* fgMask);
CVAPI(void) cudaBackgroundSubtractorFGDRelease(cv::cuda::BackgroundSubtractorFGD** fgd);
#endif