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

class CaptureFrameSource : public cv::videostab::IFrameSource
{
public:
   CaptureFrameSource(CvCapture* capture)
      : _capture(capture)
   {};

   virtual void reset() 
   { 
      cvSetCaptureProperty(_capture, CV_CAP_PROP_POS_FRAMES, 0);  
   };

   virtual cv::Mat nextFrame()
   {
      IplImage* tmp = cvQueryFrame(_capture);
      return cv::cvarrToMat(tmp);
   }
protected:
   CvCapture* _capture;
};


CVAPI(CaptureFrameSource*) VideostabCaptureFrameSourceCreate(CvCapture* capture, cv::videostab::IFrameSource** frameSource);
CVAPI(void) VideostabCaptureFrameSourceRelease(CaptureFrameSource** captureFrameSource);
CVAPI(bool) VideostabFrameSourceGetNextFrame(cv::videostab::IFrameSource* frameSource, cv::Mat* nextFrame);

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
