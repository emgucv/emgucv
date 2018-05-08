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

CVAPI(bool) cveCudaDescriptorMatcherIsMaskSupported(cv::cuda::DescriptorMatcher* matcher);
CVAPI(void) cveCudaDescriptorMatcherClear(cv::cuda::DescriptorMatcher* matcher);
CVAPI(bool) cveCudaDescriptorMatcherEmpty(cv::cuda::DescriptorMatcher* matcher);
CVAPI(void) cveCudaDescriptorMatcherTrain(cv::cuda::DescriptorMatcher* matcher);
CVAPI(void) cveCudaDescriptorMatcherMatch1(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_InputArray* trainDescriptors,
	std::vector< cv::DMatch >* matches,
	cv::_InputArray* mask);
CVAPI(void) cveCudaDescriptorMatcherMatch2(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	std::vector< cv::DMatch >* matches,
	std::vector< cv::cuda::GpuMat >* masks);
CVAPI(void) cveCudaDescriptorMatcherMatchAsync(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_OutputArray* matches,
	std::vector< cv::cuda::GpuMat >* masks,
	cv::cuda::Stream* stream);
CVAPI(void) cveCudaDescriptorMatcherMatchConvert(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* gpuMatches,
	std::vector< cv::DMatch >* matches);
CVAPI(void) cveCudaDescriptorMatcherKnnMatch1(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescs, 
	cv::_InputArray* trainDescs,
	std::vector< std::vector< cv::DMatch > >* matches,
	int k, 
	cv::_InputArray* masks, 
	bool compactResult);
CVAPI(void) cveCudaDescriptorMatcherKnnMatch2(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	std::vector< std::vector< cv::DMatch > >* matches,
	int k,
	std::vector< cv::cuda::GpuMat >* masks,
	bool compactResult);

CVAPI(void) cveCudaDescriptorMatcherKnnMatchAsync1(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors, 
	cv::_InputArray* trainDescriptors,
	cv::_OutputArray* matches,
	int k,
	cv::_InputArray* mask,
	cv::cuda::Stream* stream);

CVAPI(void) cveCudaDescriptorMatcherKnnMatchAsync2(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_OutputArray* matches,
	int k,
	std::vector< cv::cuda::GpuMat >* masks,
	cv::cuda::Stream* stream);

CVAPI(void) cveCudaDescriptorMatcherKnnMatchConvert(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* gpuMatches,
	std::vector< std::vector< cv::DMatch > >* matches,
	bool compactResult);

CVAPI(void) cveCudaDescriptorMatcherRadiusMatch1(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors, 
	cv::_InputArray* trainDescriptors,
	std::vector< std::vector< cv::DMatch > >* matches,
	float maxDistance,
	cv::_InputArray* mask,
	bool compactResult);

CVAPI(void) cveCudaDescriptorMatcherRadiusMatch2(
	cv::cuda::DescriptorMatcher* matcher, 
	cv::_InputArray* queryDescriptors,
	std::vector< std::vector< cv::DMatch > >* matches,
	float maxDistance,
	std::vector< cv::cuda::GpuMat >* masks,
	bool compactResult);

CVAPI(void) cveCudaDescriptorMatcherRadiusMatchAsync1(
	cv::cuda::DescriptorMatcher* matcher, 
	cv::_InputArray* queryDescriptors, 
	cv::_InputArray* trainDescriptors,
	cv::_OutputArray* matches,
	float maxDistance,
	cv::_InputArray* mask,
	cv::cuda::Stream* stream);

CVAPI(void) cveCudaDescriptorMatcherRadiusMatchAsync2(
	cv::cuda::DescriptorMatcher* matcher, 
	cv::_InputArray* queryDescriptors,
	cv::_OutputArray* matches,
	float maxDistance,
	std::vector< cv::cuda::GpuMat >* masks,
	cv::cuda::Stream* stream);

CVAPI(void) cveCudaDescriptorMatcherRadiusMatchConvert(
	cv::cuda::DescriptorMatcher* matcher, 
	cv::_InputArray* gpu_matches,
	std::vector< std::vector< cv::DMatch > >* matches,
	bool compactResult);

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