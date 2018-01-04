//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDALEGACY_C_H
#define EMGU_CUDLEGACY_C_H

//#include "opencv2/cuda.hpp"
#include "opencv2/cudalegacy.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "emgu_c.h"

//----------------------------------------------------------------------------
//
//  Cuda GMG
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::BackgroundSubtractorGMG*) cudaBackgroundSubtractorGMGCreate(int initializationFrames, double decisionThreshold);
CVAPI(void) cudaBackgroundSubtractorGMGApply(cv::cuda::BackgroundSubtractorGMG* gmg, cv::_InputArray* frame, cv::_OutputArray* fgMask, double learningRate, cv::cuda::Stream* stream);
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
CVAPI(void) cudaBackgroundSubtractorFGDApply(cv::cuda::BackgroundSubtractorFGD* fgd, cv::_InputArray* frame, cv::_OutputArray* fgMask, double learningRate);
CVAPI(void) cudaBackgroundSubtractorFGDRelease(cv::cuda::BackgroundSubtractorFGD** fgd);
#endif