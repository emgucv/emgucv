//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

/*
#pragma once
#ifndef EMGU_VIDEOSTAB_C_H
#define EMGU_VIDEOSTAB_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/videostab/stabilizer.hpp"

class CaptureFrameSource : public cv::videostab::IFrameSource
{
public:
   CaptureFrameSource(CvCapture* capture)
      : _capture(capture)
   {};

   virtual void reset() {};

   virtual cv::Mat nextFrame()
   {
      IplImage* tmp = cvQueryFrame(_capture);
      return cv::cvarrToMat(tmp);
   }
protected:
   CvCapture* _capture;
};

CVAPI(CaptureFrameSource*) CaptureFrameSourceCreate(CvCapture* capture);
CVAPI(void) CaptureFrameSourceRelease(CaptureFrameSource** captureFrameSource);

CVAPI(bool) FrameSourceGetNextFrame(cv::videostab::IFrameSource* frameSource, IplImage** nextFrame);

CVAPI(void) StabilizerBaseSetMotionEstimator(cv::videostab::StabilizerBase* stabalizer, cv::videostab::ImageMotionEstimatorBase* motionEstimator);

CVAPI(cv::videostab::OnePassStabilizer*) OnePassStabilizerCreate(CaptureFrameSource* capture, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource);
CVAPI(void) OnePassStabilizerSetMotionFilter(cv::videostab::OnePassStabilizer* stabilizer, cv::videostab::MotionFilterBase* motionFilter);
CVAPI(void) OnePassStabilizerRelease(cv::videostab::OnePassStabilizer** stabilizer);

CVAPI(cv::videostab::TwoPassStabilizer*) TwoPassStabilizerCreate(CaptureFrameSource* capture, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource);
CVAPI(void) TwoPassStabilizerRelease(cv::videostab::TwoPassStabilizer** stabilizer);

CVAPI(cv::videostab::GaussianMotionFilter*) GaussianMotionFilterCreate(int radius, float stdev);
CVAPI(void) GaussianMotionFilterRelease(cv::videostab::GaussianMotionFilter** filter);

#endif
*/