//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDAFEATURES2D_C_H
#define EMGU_CUDAFEATURES2D_C_H

//#include "opencv2/cuda.hpp"
#include "opencv2/cudafeatures2d.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "emgu_c.h"

//----------------------------------------------------------------------------
//
//  CudaDescriptorMatcher
//
//----------------------------------------------------------------------------

CVAPI(cv::cuda::DescriptorMatcher*) cveCudaDescriptorMatcherCreateBFMatcher(int distType, cv::Algorithm** algorithm);

CVAPI(void) cveCudaDescriptorMatcherRelease(cv::cuda::DescriptorMatcher** matcher);

CVAPI(void) cveCudaDescriptorMatcherAdd(cv::cuda::DescriptorMatcher* matcher, const std::vector<cv::cuda::GpuMat>* trainDescs);

CVAPI(void) cveCudaDescriptorMatcherKnnMatch(
                                  cv::cuda::DescriptorMatcher* matcher,
                                  cv::_InputArray* queryDescs, cv::_InputArray* trainDescs,
                                  std::vector< std::vector< cv::DMatch > >* matches, 
                                  int k, cv::_InputArray* masks, bool compactResult);
   
//----------------------------------------------------------------------------
//
//  Feature2dAsync
//
//----------------------------------------------------------------------------
CVAPI(void) cveCudaFeature2dAsyncDetectAsync(
   cv::cuda::Feature2DAsync* feature2d,
   cv::_InputArray* image,
   cv::_OutputArray* keypoints,
   cv::_InputArray* mask,
   cv::cuda::Stream* stream);

CVAPI(void) cveCudaFeature2dAsyncComputeAsync(
   cv::cuda::Feature2DAsync* feature2d,
   cv::_InputArray* image,
   cv::_OutputArray* keypoints,
   cv::_OutputArray* descriptors,
   cv::cuda::Stream* stream);

CVAPI(void) cveCudaFeature2dAsyncDetectAndComputeAsync(
   cv::cuda::Feature2DAsync* feature2d,
   cv::_InputArray* image,
   cv::_InputArray* mask,
   cv::_OutputArray* keypoints,
   cv::_OutputArray* descriptors,
   bool useProvidedKeypoints,
   cv::cuda::Stream* stream);

CVAPI(void) cveCudaFeature2dAsyncConvert(
   cv::cuda::Feature2DAsync* feature2d,
   cv::_InputArray* gpu_keypoints,
   std::vector<cv::KeyPoint>* keypoints);

//----------------------------------------------------------------------------
//
//  CudaFastFeatureDetector
//
//----------------------------------------------------------------------------

CVAPI(cv::cuda::FastFeatureDetector*) cveCudaFastFeatureDetectorCreate(int threshold, bool nonmaxSupression, int type, int maxPoints, cv::Feature2D** feature2D, cv::cuda::Feature2DAsync** feature2dAsync);

CVAPI(void) cveCudaFastFeatureDetectorRelease(cv::cuda::FastFeatureDetector** detector);


//----------------------------------------------------------------------------
//
//  CudaORB
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::ORB*) cveCudaORBCreate(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTA_K, int scoreType, int patchSize, int fastThreshold, bool blurForDescriptor, cv::Feature2D** feature2D, cv::cuda::Feature2DAsync** feature2dAsync);

CVAPI(void) cveCudaORBRelease(cv::cuda::ORB** detector);

#endif