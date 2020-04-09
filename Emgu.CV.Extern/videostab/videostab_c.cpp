//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------


#include "videostab_c.h"

CaptureFrameSource* cveVideostabCaptureFrameSourceCreate(cv::VideoCapture* capture, cv::videostab::IFrameSource** frameSource)
{
   CaptureFrameSource* stabilizer = new CaptureFrameSource(capture);
   *frameSource = dynamic_cast<cv::videostab::IFrameSource*>(stabilizer);
   return stabilizer;
}
void cveVideostabCaptureFrameSourceRelease(CaptureFrameSource** captureFrameSource)
{
   delete *captureFrameSource;
   *captureFrameSource = 0;
}

bool cveVideostabFrameSourceGetNextFrame(cv::videostab::IFrameSource* frameSource, cv::Mat* nextFrame)
{
   cv::Mat mat = frameSource->nextFrame();
   if (mat.empty())
      return false;

   cv::swap(mat, *nextFrame);
   return true;
}


void cveStabilizerBaseSetMotionEstimator(cv::videostab::StabilizerBase* stabalizer, cv::videostab::ImageMotionEstimatorBase* motionEstimator)
{
   cv::Ptr<cv::videostab::ImageMotionEstimatorBase> ptr(motionEstimator, [] (cv::videostab::ImageMotionEstimatorBase*){});
   stabalizer->setMotionEstimator(ptr);
}

template<class cvstabilizer> cvstabilizer* StabilizerCreate(cv::videostab::IFrameSource* baseFrameSource, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource)
{
   cvstabilizer* stabilizer = new cvstabilizer();
   cv::Ptr<cv::videostab::IFrameSource> ptr(baseFrameSource, [](cv::videostab::IFrameSource*){});
   stabilizer->setFrameSource(ptr);
   *stabilizerBase = dynamic_cast<cv::videostab::StabilizerBase*>(stabilizer);
   *frameSource = dynamic_cast<cv::videostab::IFrameSource*>(stabilizer);
   return stabilizer;
}

cv::videostab::OnePassStabilizer* cveOnePassStabilizerCreate(cv::videostab::IFrameSource* baseFrameSource, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource)
{
   return StabilizerCreate<cv::videostab::OnePassStabilizer>(baseFrameSource, stabilizerBase, frameSource);
}

void cveOnePassStabilizerSetMotionFilter(cv::videostab::OnePassStabilizer* stabilizer, cv::videostab::MotionFilterBase* motionFilter)
{
   cv::Ptr<cv::videostab::MotionFilterBase> ptr(motionFilter, [] (cv::videostab::MotionFilterBase*){});
   stabilizer->setMotionFilter(ptr);
}

void cveOnePassStabilizerRelease(cv::videostab::OnePassStabilizer** stabilizer)
{
   delete *stabilizer;
   *stabilizer = 0;
}

cv::videostab::TwoPassStabilizer* cveTwoPassStabilizerCreate(cv::videostab::IFrameSource* baseFrameSource, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource)
{
   return StabilizerCreate<cv::videostab::TwoPassStabilizer>(baseFrameSource, stabilizerBase, frameSource);
}
void cveTwoPassStabilizerRelease(cv::videostab::TwoPassStabilizer** stabilizer)
{
   delete *stabilizer;
   *stabilizer = 0;
}

cv::videostab::GaussianMotionFilter* cveGaussianMotionFilterCreate(int radius, float stdev)
{
   return new cv::videostab::GaussianMotionFilter(radius, stdev);
}
void cveGaussianMotionFilterRelease(cv::videostab::GaussianMotionFilter** filter)
{
   delete *filter;
   *filter = 0;
}
