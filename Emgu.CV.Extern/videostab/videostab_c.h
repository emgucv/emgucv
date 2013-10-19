//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_VIDEOSTAB_C_H
#define EMGU_VIDEOSTAB_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/highgui/highgui_c.h"
#include "opencv2/videostab/stabilizer.hpp"
#include "captureFrameSource.h"

CVAPI(CaptureFrameSource<cv::videostab::IFrameSource>*) VideostabCaptureFrameSourceCreate(CvCapture* capture, cv::videostab::IFrameSource** frameSource);
CVAPI(void) VideostabCaptureFrameSourceRelease(CaptureFrameSource<cv::videostab::IFrameSource>** captureFrameSource);
CVAPI(bool) VideostabFrameSourceGetNextFrame(cv::videostab::IFrameSource* frameSource, IplImage** nextFrame);

/*
CVAPI(void) StabilizerBaseSetMotionEstimator(cv::videostab::StabilizerBase* stabalizer, cv::videostab::IGlobalMotionEstimator* motionEstimator);
*/
CVAPI(cv::videostab::OnePassStabilizer*) OnePassStabilizerCreate(cv::videostab::IFrameSource* baseFrameSource, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource);
CVAPI(void) OnePassStabilizerSetMotionFilter(cv::videostab::OnePassStabilizer* stabilizer, cv::videostab::MotionFilterBase* motionFilter);
CVAPI(void) OnePassStabilizerRelease(cv::videostab::OnePassStabilizer** stabilizer);

CVAPI(cv::videostab::TwoPassStabilizer*) TwoPassStabilizerCreate(cv::videostab::IFrameSource* baseFrameSource, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource);
CVAPI(void) TwoPassStabilizerRelease(cv::videostab::TwoPassStabilizer** stabilizer);

CVAPI(cv::videostab::GaussianMotionFilter*) GaussianMotionFilterCreate(int radius, float stdev);
CVAPI(void) GaussianMotionFilterRelease(cv::videostab::GaussianMotionFilter** filter);

#endif
