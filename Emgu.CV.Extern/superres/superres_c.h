//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_SUPERRES_C_H
#define EMGU_SUPERRES_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/superres.hpp"

CVAPI(cv::superres::FrameSource*) cveSuperresCreateFrameSourceVideo(cv::String* fileName, bool useGpu, cv::Ptr<cv::superres::FrameSource>** sharedPtr);
CVAPI(cv::superres::FrameSource*) cveSuperresCreateFrameSourceCamera(int deviceId, cv::Ptr<cv::superres::FrameSource>** sharedPtr);
CVAPI(void) cveSuperresFrameSourceRelease(cv::superres::FrameSource** frameSource, cv::Ptr<cv::superres::FrameSource>** sharedPtr);
CVAPI(void) cveSuperresFrameSourceNextFrame(cv::superres::FrameSource* frameSource, cv::_OutputArray* frame);

CVAPI(cv::superres::SuperResolution*) cveSuperResolutionCreate(int type, cv::superres::FrameSource* frameSource, cv::superres::FrameSource** frameSourceOut);
CVAPI(void) cveSuperResolutionRelease(cv::superres::SuperResolution** superres);
#endif