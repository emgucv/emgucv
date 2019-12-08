//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_VIDEOSTAB_C_H
#define EMGU_VIDEOSTAB_C_H


#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_VIDEOSTAB

#include "opencv2/highgui/highgui_c.h"
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/videostab/stabilizer.hpp"

#else
static inline CV_NORETURN void throw_no_videostab() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without Videoio support"); }

#endif

#ifdef HAVE_OPENCV_VIDEOIO
class CaptureFrameSource : public cv::videostab::IFrameSource
{
public:
    CaptureFrameSource(cv::VideoCapture* capture)
        : _capture(capture)
    {};

    virtual void reset()
    {
        _capture->set(cv::VideoCaptureProperties::CAP_PROP_POS_FRAMES, 0);
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

#else
static inline CV_NORETURN void throw_no_videoio() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without Videoio support"); }

namespace cv
{
	class VideoCapture
	{
		
	};
}

class CaptureFrameSource : public cv::videostab::IFrameSource
{
public:
	CaptureFrameSource(void* capture)
	{
		throw_no_videoio();
	};

	virtual void reset()
	{
		throw_no_videoio();
	};

	virtual cv::Mat nextFrame()
	{
		throw_no_videoio();
	}
protected:
};

#endif



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
