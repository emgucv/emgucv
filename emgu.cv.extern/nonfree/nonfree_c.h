//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_NONFREE_C_H
#define EMGU_NONFREE_C_H

#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/features2d/features2d.hpp"
#include "opencv2/calib3d/calib3d.hpp"
#include "opencv2/contrib/contrib.hpp"
#include "opencv2/legacy/legacy.hpp"
#include "opencv2/legacy/compat.hpp"
#include "opencv2/nonfree/nonfree.hpp"
#include "opencv2/nonfree/features2d.hpp"
//#include "vectors_c.h"

//SIFTDetector
CVAPI(cv::SIFT*) CvSIFTDetectorCreate(
   int nFeatures, int nOctaveLayers, 
   double contrastThreshold, double edgeThreshold, 
   double sigma, cv::FeatureDetector** featureDetector, cv::DescriptorExtractor** descriptorExtractor);
CVAPI(void) CvSIFTDetectorRelease(cv::SIFT** detector);

//SURFDetector
CVAPI(cv::SURF*) CvSURFDetectorCreate(CvSURFParams* detector, cv::FeatureDetector** featureDetector, cv::DescriptorExtractor** descriptorExtractor);
CVAPI(void) CvSURFDetectorRelease(cv::SURF** detector);

/*
//----------------------------------------------------------------------------
//
//  GpuSURFDetector
//
//----------------------------------------------------------------------------

CVAPI(cv::gpu::SURF_GPU*) gpuSURFDetectorCreate(double _hessianThreshold, int _nOctaves, int _nOctaveLayers, bool _extended, float _keypointsRatio, bool _upright);

CVAPI(void) gpuSURFDetectorRelease(cv::gpu::SURF_GPU** detector);

CVAPI(void) gpuSURFDetectorDetectKeyPoints(cv::gpu::SURF_GPU* detector, const cv::gpu::GpuMat* img, const cv::gpu::GpuMat* mask, cv::gpu::GpuMat* keypoints);

CVAPI(void) gpuSURFDownloadKeypoints(cv::gpu::SURF_GPU* detector, const cv::gpu::GpuMat* keypointsGPU, std::vector<cv::KeyPoint>* keypoints);

CVAPI(void) gpuSURFUploadKeypoints(cv::gpu::SURF_GPU* detector, const std::vector<cv::KeyPoint>* keypoints, cv::gpu::GpuMat* keypointsGPU);

CVAPI(void) gpuSURFDetectorCompute(
   cv::gpu::SURF_GPU* detector, 
   const cv::gpu::GpuMat* img, 
   const cv::gpu::GpuMat* mask, 
   cv::gpu::GpuMat* keypoints, 
   cv::gpu::GpuMat* descriptors, 
   bool useProvidedKeypoints);

CVAPI(int) gpuSURFDetectorGetDescriptorSize(cv::gpu::SURF_GPU* detector);

//----------------------------------------------------------------------------
//
//  VIBE GPU
//
//----------------------------------------------------------------------------
CVAPI(cv::gpu::VIBE_GPU*) gpuVibeCreate(unsigned long rngSeed, cv::gpu::GpuMat* firstFrame, cv::gpu::Stream* stream);
CVAPI(void) gpuVibeCompute(cv::gpu::VIBE_GPU* vibe, cv::gpu::GpuMat* frame, cv::gpu::GpuMat* fgMask, cv::gpu::Stream* stream);
CVAPI(void) gpuVibeRelease(cv::gpu::VIBE_GPU** vibe);
*/

#endif