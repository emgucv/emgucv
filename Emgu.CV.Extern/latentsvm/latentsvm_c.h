//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_LATENTSVM_C_H
#define EMGU_LATENTSVM_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/latentsvm.hpp"

//Latent svm
CVAPI(cv::lsvm::LSVMDetector*) cveLSVMDetectorCreate(std::vector<cv::String>* filenames, std::vector<cv::String>* classNames);

CVAPI(int) cveLSVMGetClassCount(cv::lsvm::LSVMDetector* detector);
CVAPI(void) cveLSVMGetClassNames(cv::lsvm::LSVMDetector* detector, std::vector<cv::String>* classNames);

CVAPI(void) cveLSVMDetectorDetect(cv::lsvm::LSVMDetector* detector, cv::Mat* image, std::vector<cv::lsvm::LSVMDetector::ObjectDetection>* objects, float overlapThreshold);

CVAPI(void) cveLSVMDetectorRelease(cv::lsvm::LSVMDetector** detector);

#endif