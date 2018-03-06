//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_SOFTCASCADE_C_H
#define EMGU_SOFTCASCADE_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/softcascade.hpp"

CVAPI(cv::softcascade::Detector*) cveSoftCascadeDetectorCreate(cv::String* fileName, double minScale, double maxScale, int scales, int rejCriteria);

CVAPI(void) cveSoftCascadeDetectorDetect(cv::softcascade::Detector* detector, cv::_InputArray* image, std::vector<cv::Rect>* rois, std::vector<cv::Rect>* rects, std::vector<float>* confidents);

CVAPI(void) cveSoftCascadeDetectorRelease(cv::softcascade::Detector** detector);

CVAPI(cv::softcascade::SCascade*) cudaSoftCascadeDetectorCreate(cv::String* fileName, const double minScale, const double maxScale, const int scales, const int flags);

CVAPI(void) cudaSoftCascadeDetectorDetect(cv::softcascade::SCascade* detector, cv::cuda::GpuMat* image, cv::cuda::GpuMat* rois, cv::cuda::GpuMat* detections, cv::cuda::Stream* stream);

CVAPI(void) cudaSoftCascadeDetectorRelease(cv::softcascade::SCascade** detector);
#endif