//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_NONFREE_C_H
#define EMGU_NONFREE_C_H

#include "opencv2/core/core_c.h"
//#include "opencv2/nonfree/nonfree.hpp"
#include "opencv2/xfeatures2d.hpp"

//SIFTDetector
CVAPI(cv::xfeatures2d::SIFT*) CvSIFTDetectorCreate(
   int nFeatures, int nOctaveLayers, 
   double contrastThreshold, double edgeThreshold, 
   double sigma, cv::Feature2D** feature2D);
CVAPI(void) CvSIFTDetectorRelease(cv::xfeatures2d::SIFT** detector);

//SURFDetector
CVAPI(cv::xfeatures2d::SURF*) CvSURFDetectorCreate(double hessianThresh, int nOctaves, int nOctaveLayers, bool extended, bool upright, cv::Feature2D** feature2D);
CVAPI(void) CvSURFDetectorRelease(cv::xfeatures2d::SURF** detector);

/*
//----------------------------------------------------------------------------
//
//  VIBE GPU
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::VIBE_GPU*) gpuVibeCreate(unsigned long rngSeed, cv::cuda::GpuMat* firstFrame, cv::cuda::Stream* stream);
CVAPI(void) gpuVibeCompute(cv::cuda::VIBE_GPU* vibe, cv::cuda::GpuMat* frame, cv::cuda::GpuMat* fgMask, cv::cuda::Stream* stream);
CVAPI(void) gpuVibeRelease(cv::cuda::VIBE_GPU** vibe);
*/

#endif