//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------


#include "videostab_c.h"

CaptureFrameSource* cveVideostabCaptureFrameSourceCreate(cv::VideoCapture* capture, cv::videostab::IFrameSource** frameSource)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	CaptureFrameSource* stabilizer = new CaptureFrameSource(capture);
	*frameSource = dynamic_cast<cv::videostab::IFrameSource*>(stabilizer);
	return stabilizer;
#else
	throw_no_videostab();
#endif
}
void cveVideostabCaptureFrameSourceRelease(CaptureFrameSource** captureFrameSource)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	delete* captureFrameSource;
	*captureFrameSource = 0;
#else
	throw_no_videostab();
#endif
}

bool cveVideostabFrameSourceGetNextFrame(cv::videostab::IFrameSource* frameSource, cv::Mat* nextFrame)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	cv::Mat mat = frameSource->nextFrame();
	if (mat.empty())
		return false;

	cv::swap(mat, *nextFrame);
	return true;
#else
	throw_no_videostab();
#endif
}


void cveStabilizerBaseSetMotionEstimator(cv::videostab::StabilizerBase* stabilizer, cv::videostab::ImageMotionEstimatorBase* motionEstimator)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	cv::Ptr<cv::videostab::ImageMotionEstimatorBase> ptr(motionEstimator, [](cv::videostab::ImageMotionEstimatorBase*) {});
	stabilizer->setMotionEstimator(ptr);
#else
	throw_no_videostab();
#endif
}

template<class cvstabilizer> cvstabilizer* StabilizerCreate(cv::videostab::IFrameSource* baseFrameSource, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	cvstabilizer* stabilizer = new cvstabilizer();
	cv::Ptr<cv::videostab::IFrameSource> ptr(baseFrameSource, [](cv::videostab::IFrameSource*) {});
	stabilizer->setFrameSource(ptr);
	*stabilizerBase = dynamic_cast<cv::videostab::StabilizerBase*>(stabilizer);
	*frameSource = dynamic_cast<cv::videostab::IFrameSource*>(stabilizer);
	return stabilizer;
#else
	throw_no_videostab();
#endif
}

cv::videostab::OnePassStabilizer* cveOnePassStabilizerCreate(cv::videostab::IFrameSource* baseFrameSource, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	return StabilizerCreate<cv::videostab::OnePassStabilizer>(baseFrameSource, stabilizerBase, frameSource);
#else
	throw_no_videostab();
#endif
}

void cveOnePassStabilizerSetMotionFilter(cv::videostab::OnePassStabilizer* stabilizer, cv::videostab::MotionFilterBase* motionFilter)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	cv::Ptr<cv::videostab::MotionFilterBase> ptr(motionFilter, [](cv::videostab::MotionFilterBase*) {});
	stabilizer->setMotionFilter(ptr);
#else
	throw_no_videostab();
#endif
}

void cveOnePassStabilizerRelease(cv::videostab::OnePassStabilizer** stabilizer)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	delete* stabilizer;
	*stabilizer = 0;
#else
	throw_no_videostab();
#endif
}

cv::videostab::TwoPassStabilizer* cveTwoPassStabilizerCreate(cv::videostab::IFrameSource* baseFrameSource, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	return StabilizerCreate<cv::videostab::TwoPassStabilizer>(baseFrameSource, stabilizerBase, frameSource);
#else
	throw_no_videostab();
#endif
}

void cveTwoPassStabilizerRelease(cv::videostab::TwoPassStabilizer** stabilizer)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	delete* stabilizer;
	*stabilizer = 0;
#else
	throw_no_videostab();
#endif
}

cv::videostab::GaussianMotionFilter* cveGaussianMotionFilterCreate(int radius, float stdev)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	return new cv::videostab::GaussianMotionFilter(radius, stdev);
#else
	throw_no_videostab();
#endif
}

void cveGaussianMotionFilterRelease(cv::videostab::GaussianMotionFilter** filter)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	delete* filter;
	*filter = 0;
#else
	throw_no_videostab();
#endif
}

cv::videostab::MotionEstimatorRansacL2* cveMotionEstimatorRansacL2Create(int motionModel, cv::videostab::MotionEstimatorBase** motionEstimatorBase)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	cv::videostab::MotionEstimatorRansacL2* estimator = new cv::videostab::MotionEstimatorRansacL2(static_cast<cv::videostab::MotionModel>(motionModel));
	*motionEstimatorBase = dynamic_cast<cv::videostab::MotionEstimatorBase*>(estimator);
	return estimator;
#else
	throw_no_videostab();
#endif
}

void cveMotionEstimatorRansacL2Release(cv::videostab::MotionEstimatorRansacL2** estimator)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	delete* estimator;
	*estimator = 0;
#else
	throw_no_videostab();
#endif
}

cv::videostab::KeypointBasedMotionEstimator* cveKeypointBasedMotionEstimatorCreate(cv::videostab::MotionEstimatorBase* estimator, cv::videostab::ImageMotionEstimatorBase** imageMotionEstimatorBase)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	cv::Ptr<cv::videostab::MotionEstimatorBase> ptr(estimator, [](cv::videostab::MotionEstimatorBase*) {});
	cv::videostab::KeypointBasedMotionEstimator* kbme = new cv::videostab::KeypointBasedMotionEstimator(ptr);
	*imageMotionEstimatorBase = dynamic_cast<cv::videostab::ImageMotionEstimatorBase*>(kbme);
	return kbme;
#else
	throw_no_videostab();
#endif
}

void cveKeypointBasedMotionEstimatorRelease(cv::videostab::KeypointBasedMotionEstimator** estimator)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	delete* estimator;
	*estimator = 0;
#else
	throw_no_videostab();
#endif
}

float cveCalcBlurriness(cv::Mat* frame)
{
#ifdef HAVE_OPENCV_VIDEOSTAB
	return cv::videostab::calcBlurriness(*frame);
#else
	throw_no_videostab();
#endif	
}