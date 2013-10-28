//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDAFEATURES2D_C_H
#define EMGU_CUDAFEATURES2D_C_H

#include "opencv2/cuda.hpp"
#include "opencv2/cudafeatures2d.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "emgu_c.h"

//----------------------------------------------------------------------------
//
//  CudaBruteForceMatcher
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::BFMatcher_CUDA*) cudaBruteForceMatcherCreate(int distType);

CVAPI(void) cudaBruteForceMatcherRelease(cv::cuda::BFMatcher_CUDA** matcher);

CVAPI(void) cudaBruteForceMatcherAdd(cv::cuda::BFMatcher_CUDA* matcher, const cv::cuda::GpuMat* trainDescs);

CVAPI(void) cudaBruteForceMatcherKnnMatchSingle(
   cv::cuda::BFMatcher_CUDA* matcher,
   const cv::cuda::GpuMat* queryDescs, const cv::cuda::GpuMat* trainDescs,
   cv::cuda::GpuMat* trainIdx, cv::cuda::GpuMat* distance, 
   int k, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream);
   
//----------------------------------------------------------------------------
//
//  CudaFASTDetector
//
//----------------------------------------------------------------------------

CVAPI(cv::cuda::FAST_CUDA*) cudaFASTDetectorCreate(int threshold, bool nonmaxSupression, double keypointsRatio);

CVAPI(void) cudaFASTDetectorRelease(cv::cuda::FAST_CUDA** detector);

CVAPI(void) cudaFASTDetectorDetectKeyPoints(cv::cuda::FAST_CUDA* detector, const cv::cuda::GpuMat* img, const cv::cuda::GpuMat* mask, cv::cuda::GpuMat* keypoints);

CVAPI(void) cudaFASTDownloadKeypoints(cv::cuda::FAST_CUDA* detector, cv::cuda::GpuMat* keypointsGPU, std::vector<cv::KeyPoint>* keypoints);

//----------------------------------------------------------------------------
//
//  CudaORBDetector
//
//----------------------------------------------------------------------------

CVAPI(cv::cuda::ORB_CUDA*) cudaORBDetectorCreate(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTA_K, int scoreType, int patchSize);

CVAPI(void) cudaORBDetectorRelease(cv::cuda::ORB_CUDA** detector);

CVAPI(void) cudaORBDetectorDetectKeyPoints(cv::cuda::ORB_CUDA* detector, const cv::cuda::GpuMat* img, const cv::cuda::GpuMat* mask, cv::cuda::GpuMat* keypoints);

CVAPI(void) cudaORBDownloadKeypoints(cv::cuda::ORB_CUDA* detector, cv::cuda::GpuMat* keypointsGPU, std::vector<cv::KeyPoint>* keypoints);

CVAPI(void) cudaORBDetectorCompute(
   cv::cuda::ORB_CUDA* detector, 
   const cv::cuda::GpuMat* img, 
   const cv::cuda::GpuMat* mask, 
   cv::cuda::GpuMat* keypoints, 
   cv::cuda::GpuMat* descriptors);

CVAPI(int) cudaORBDetectorGetDescriptorSize(cv::cuda::ORB_CUDA* detector);
#endif