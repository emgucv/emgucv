//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------


#include "videostab_c.h"

CaptureFrameSource* VideostabCaptureFrameSourceCreate(cv::VideoCapture* capture, cv::videostab::IFrameSource** frameSource)
{
   CaptureFrameSource* stabilizer = new CaptureFrameSource(capture);
   *frameSource = dynamic_cast<cv::videostab::IFrameSource*>(stabilizer);
   return stabilizer;
}
void VideostabCaptureFrameSourceRelease(CaptureFrameSource** captureFrameSource)
{
   delete *captureFrameSource;
   *captureFrameSource = 0;
}

bool VideostabFrameSourceGetNextFrame(cv::videostab::IFrameSource* frameSource, cv::Mat* nextFrame)
{
   cv::Mat mat = frameSource->nextFrame();
   if (mat.empty())
      return false;

   cv::swap(mat, *nextFrame);
   return true;
}


void StabilizerBaseSetMotionEstimator(cv::videostab::StabilizerBase* stabalizer, cv::videostab::ImageMotionEstimatorBase* motionEstimator)
{
   cv::Ptr<cv::videostab::ImageMotionEstimatorBase> ptr(motionEstimator);
   ptr.addref(); // add reference such that it won't release the motion estimator
   stabalizer->setMotionEstimator(motionEstimator);
}

template<class cvstabilizer> cvstabilizer* StabilizerCreate(cv::videostab::IFrameSource* baseFrameSource, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource)
{
   cvstabilizer* stabilizer = new cvstabilizer();
   cv::Ptr<cv::videostab::IFrameSource> ptr(baseFrameSource);
   ptr.addref(); // add reference such that it won't release the CaptureFrameSource
   stabilizer->setFrameSource(ptr);
   *stabilizerBase = dynamic_cast<cv::videostab::StabilizerBase*>(stabilizer);
   *frameSource = dynamic_cast<cv::videostab::IFrameSource*>(stabilizer);
   return stabilizer;
}

cv::videostab::OnePassStabilizer* OnePassStabilizerCreate(cv::videostab::IFrameSource* baseFrameSource, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource)
{
   return StabilizerCreate<cv::videostab::OnePassStabilizer>(baseFrameSource, stabilizerBase, frameSource);
   /*
   cv::videostab::OnePassStabilizer* stabilizer = new cv::videostab::OnePassStabilizer();
   cv::Ptr<cv::videostab::IFrameSource> ptr(capture);
   ptr.addref(); // add reference such that it won't release the CaptureFrameSource
   stabilizer->setFrameSource(ptr);
   *stabilizerBase = static_cast<cv::videostab::StabilizerBase*>(stabilizer);
   *frameSource = static_cast<cv::videostab::IFrameSource*>(stabilizer);
   return stabilizer;*/
}

void OnePassStabilizerSetMotionFilter(cv::videostab::OnePassStabilizer* stabilizer, cv::videostab::MotionFilterBase* motionFilter)
{
   cv::Ptr<cv::videostab::MotionFilterBase> ptr(motionFilter);
   ptr.addref(); // add reference such that it won't release the motion filter
   stabilizer->setMotionFilter(ptr);
}

void OnePassStabilizerRelease(cv::videostab::OnePassStabilizer** stabilizer)
{
   delete *stabilizer;
   *stabilizer = 0;
}

cv::videostab::TwoPassStabilizer* TwoPassStabilizerCreate(cv::videostab::IFrameSource* baseFrameSource, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource)
{
   return StabilizerCreate<cv::videostab::TwoPassStabilizer>(baseFrameSource, stabilizerBase, frameSource);
   /*
   cv::videostab::TwoPassStabilizer* stabilizer = new cv::videostab::TwoPassStabilizer();
   cv::Ptr<cv::videostab::IFrameSource> ptr(capture);
   ptr.addref(); // add reference such that it won't release the CaptureFrameSource
   stabilizer->setFrameSource(ptr);
   *stabilizerBase = static_cast<cv::videostab::StabilizerBase*>(stabilizer);
   *frameSource = static_cast<cv::videostab::IFrameSource*>(stabilizer);
   return stabilizer;*/
}
void TwoPassStabilizerRelease(cv::videostab::TwoPassStabilizer** stabilizer)
{
   delete *stabilizer;
   *stabilizer = 0;
}

cv::videostab::GaussianMotionFilter* GaussianMotionFilterCreate(int radius, float stdev)
{
   return new cv::videostab::GaussianMotionFilter(radius, stdev);
}
void GaussianMotionFilterRelease(cv::videostab::GaussianMotionFilter** filter)
{
   delete *filter;
   *filter = 0;
}
