//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_NONFREE_C_H
#define EMGU_NONFREE_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/nonfree/nonfree.hpp"
#include "opencv2/nonfree/features2d.hpp"

//SIFTDetector
CVAPI(cv::SIFT*) CvSIFTDetectorCreate(
   int nFeatures, int nOctaveLayers, 
   double contrastThreshold, double edgeThreshold, 
   double sigma, cv::FeatureDetector** featureDetector, cv::DescriptorExtractor** descriptorExtractor);
CVAPI(void) CvSIFTDetectorRelease(cv::SIFT** detector);

//SURFDetector
CVAPI(cv::SURF*) CvSURFDetectorCreate(double hessianThresh, int nOctaves, int nOctaveLayers, bool extended, bool upright, cv::FeatureDetector** featureDetector, cv::DescriptorExtractor** descriptorExtractor);
CVAPI(void) CvSURFDetectorRelease(cv::SURF** detector);

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