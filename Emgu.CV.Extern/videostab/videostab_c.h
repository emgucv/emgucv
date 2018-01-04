//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_VIDEOSTAB_C_H
#define EMGU_VIDEOSTAB_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/highgui/highgui_c.h"
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/videostab/stabilizer.hpp"

class CaptureFrameSource : public cv::videostab::IFrameSource
{
public:
   CaptureFrameSource(cv::VideoCapture* capture)
      : _capture(capture)
   {};

   virtual void reset() 
   { 
      _capture->set(CV_CAP_PROP_POS_FRAMES, 0);  
   };

   virtual cv::Mat nextFrame()
   {
      cv::Mat m;
      _capture->read(m);
      return m;
   }
protected:
   cv::VideoCapture* _capture;
};


CVAPI(CaptureFrameSource*) VideostabCaptureFrameSourceCreate(cv::VideoCapture* capture, cv::videostab::IFrameSource** frameSource);
CVAPI(void) VideostabCaptureFrameSourceRelease(CaptureFrameSource** captureFrameSource);
CVAPI(bool) VideostabFrameSourceGetNextFrame(cv::videostab::IFrameSource* frameSource, cv::Mat* nextFrame);


CVAPI(void) StabilizerBaseSetMotionEstimator(cv::videostab::StabilizerBase* stabalizer, cv::videostab::ImageMotionEstimatorBase* motionEstimator);

CVAPI(cv::videostab::OnePassStabilizer*) OnePassStabilizerCreate(cv::videostab::IFrameSource* baseFrameSource, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource);
CVAPI(void) OnePassStabilizerSetMotionFilter(cv::videostab::OnePassStabilizer* stabilizer, cv::videostab::MotionFilterBase* motionFilter);
CVAPI(void) OnePassStabilizerRelease(cv::videostab::OnePassStabilizer** stabilizer);

CVAPI(cv::videostab::TwoPassStabilizer*) TwoPassStabilizerCreate(cv::videostab::IFrameSource* baseFrameSource, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource);
CVAPI(void) TwoPassStabilizerRelease(cv::videostab::TwoPassStabilizer** stabilizer);

CVAPI(cv::videostab::GaussianMotionFilter*) GaussianMotionFilterCreate(int radius, float stdev);
CVAPI(void) GaussianMotionFilterRelease(cv::videostab::GaussianMotionFilter** filter);

#endif
