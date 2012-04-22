//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "videostab_c.h"

CaptureFrameSource* CaptureFrameSourceCreate(CvCapture* capture)
{
   return new CaptureFrameSource(capture);
}
void CaptureFrameSourceRelease(CaptureFrameSource** captureFrameSource)
{
   delete *captureFrameSource;
   *captureFrameSource = 0;
}

bool FrameSourceGetNextFrame(cv::videostab::IFrameSource* frameSource, IplImage** nextFrame)
{
   cv::Mat mat = frameSource->nextFrame();
   if (mat.empty())
      return false;

   IplImage tmp = (IplImage) mat;
   if (!(*nextFrame))
   {
      *nextFrame = cvCreateImage(cvSize(tmp.width, tmp.height), tmp.depth, tmp.nChannels);
   }
   CV_Assert(*nextFrame && mat.rows == (*nextFrame)->height && mat.cols == (*nextFrame)->width && mat.channels() == (*nextFrame)->nChannels);
   cv::Mat tmpMat = cv::cvarrToMat(*nextFrame);
   mat.copyTo(tmpMat);
   return true;
}

void StabilizerBaseSetMotionEstimator(cv::videostab::StabilizerBase* stabalizer, cv::videostab::GlobalMotionEstimatorBase* motionEstimator)
{
   cv::Ptr<cv::videostab::GlobalMotionEstimatorBase> ptr(motionEstimator);
   ptr.addref(); // add reference such that it won't release the motion estimator
   stabalizer->setMotionEstimator(motionEstimator);
}

cv::videostab::OnePassStabilizer* OnePassStabilizerCreate(CaptureFrameSource* capture, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource)
{
   cv::videostab::OnePassStabilizer* stabilizer = new cv::videostab::OnePassStabilizer();
   cv::Ptr<cv::videostab::IFrameSource> ptr(capture);
   ptr.addref(); // add reference such that it won't release the CaptureFrameSource
   stabilizer->setFrameSource(ptr);
   *stabilizerBase = static_cast<cv::videostab::StabilizerBase*>(stabilizer);
   *frameSource = static_cast<cv::videostab::IFrameSource*>(stabilizer);
   return stabilizer;
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

cv::videostab::TwoPassStabilizer* TwoPassStabilizerCreate(CaptureFrameSource* capture, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource)
{
   cv::videostab::TwoPassStabilizer* stabilizer = new cv::videostab::TwoPassStabilizer();
   cv::Ptr<cv::videostab::IFrameSource> ptr(capture);
   ptr.addref(); // add reference such that it won't release the CaptureFrameSource
   stabilizer->setFrameSource(ptr);
   *stabilizerBase = static_cast<cv::videostab::StabilizerBase*>(stabilizer);
   *frameSource = static_cast<cv::videostab::IFrameSource*>(stabilizer);
   return stabilizer;
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

cv::videostab::PyrLkRobustMotionEstimator* PyrLkRobustMotionEstimatorCreate(int motionModel)
{
   cv::videostab::PyrLkRobustMotionEstimator* result = new cv::videostab::PyrLkRobustMotionEstimator((cv::videostab::MotionModel) motionModel);
   cv::videostab::GlobalMotionEstimatorBase* base = static_cast<cv::videostab::GlobalMotionEstimatorBase*>(result);
   CV_Assert((void*)result == (void*)base);
   return result;
}
void PyrLkRobustMotionEstimatorSetDetector(cv::videostab::PyrLkRobustMotionEstimator* motionEstimator, cv::Feature2D::FeatureDetector* featureDetector)
{
   cv::Ptr<cv::Feature2D::FeatureDetector> ptr(featureDetector);
   ptr.addref(); // add reference such that it won't release the deature detector;
   motionEstimator->setDetector(ptr);
}
void PyrLkRobustMotionEstimatorRelease(cv::videostab::PyrLkRobustMotionEstimator** estimator)
{
   delete *estimator;
   *estimator = 0;
}