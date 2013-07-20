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
#include "opencv2/nonfree/ocl.hpp"
//#include "vectors_c.h"

//----------------------------------------------------------------------------
//
//  OclSURFDetector
//
//----------------------------------------------------------------------------

CVAPI(cv::ocl::SURF_OCL*) oclSURFDetectorCreate(double _hessianThreshold, int _nOctaves, int _nOctaveLayers, bool _extended, float _keypointsRatio, bool _upright);

CVAPI(void) oclSURFDetectorRelease(cv::ocl::SURF_OCL** detector);

CVAPI(void) oclSURFDetectorDetectKeyPoints(cv::ocl::SURF_OCL* detector, const cv::ocl::oclMat* img, const cv::ocl::oclMat* mask, cv::ocl::oclMat* keypoints);

CVAPI(void) oclSURFDownloadKeypoints(cv::ocl::SURF_OCL* detector, const cv::ocl::oclMat* keypointsGPU, std::vector<cv::KeyPoint>* keypoints);

CVAPI(void) oclSURFUploadKeypoints(cv::ocl::SURF_OCL* detector, const std::vector<cv::KeyPoint>* keypoints, cv::ocl::oclMat* keypointsGPU);

CVAPI(void) oclSURFDetectorCompute(
   cv::ocl::SURF_OCL* detector, 
   const cv::ocl::oclMat* img, 
   const cv::ocl::oclMat* mask, 
   cv::ocl::oclMat* keypoints, 
   cv::ocl::oclMat* descriptors, 
   bool useProvidedKeypoints);

CVAPI(int) oclSURFDetectorGetDescriptorSize(cv::ocl::SURF_OCL* detector);
#endif