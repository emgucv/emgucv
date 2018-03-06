//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_NONFREE_GPU_C_H
#define EMGU_NONFREE_GPU_C_H

#include "opencv2/imgproc/imgproc.hpp"
//#include "opencv2/xfeatures2d.hpp"
#include "opencv2/core/core_c.h"
//#include "opencv2/nonfree/nonfree.hpp"
#include "opencv2/xfeatures2d/cuda.hpp"
//#include "opencv2/nonfree/cuda.hpp"


//----------------------------------------------------------------------------
//
//  GpuSURFDetector
//
//----------------------------------------------------------------------------

CVAPI(cv::cuda::SURF_CUDA*) cudaSURFDetectorCreate(double _hessianThreshold, int _nOctaves, int _nOctaveLayers, bool _extended, float _keypointsRatio, bool _upright);

CVAPI(void) cudaSURFDetectorRelease(cv::cuda::SURF_CUDA** detector);

CVAPI(void) cudaSURFDetectorDetectKeyPoints(cv::cuda::SURF_CUDA* detector, const cv::cuda::GpuMat* img, const cv::cuda::GpuMat* mask, cv::cuda::GpuMat* keypoints);

CVAPI(void) cudaSURFDownloadKeypoints(cv::cuda::SURF_CUDA* detector, const cv::cuda::GpuMat* keypointsGPU, std::vector<cv::KeyPoint>* keypoints);

CVAPI(void) cudaSURFUploadKeypoints(cv::cuda::SURF_CUDA* detector, const std::vector<cv::KeyPoint>* keypoints, cv::cuda::GpuMat* keypointsGPU);

CVAPI(void) cudaSURFDetectorCompute(
   cv::cuda::SURF_CUDA* detector, 
   const cv::cuda::GpuMat* img, 
   const cv::cuda::GpuMat* mask, 
   cv::cuda::GpuMat* keypoints, 
   cv::cuda::GpuMat* descriptors, 
   bool useProvidedKeypoints);

CVAPI(int) cudaSURFDetectorGetDescriptorSize(cv::cuda::SURF_CUDA* detector);


#endif