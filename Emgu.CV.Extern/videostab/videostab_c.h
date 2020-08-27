//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_VIDEOSTAB_C_H
#define EMGU_VIDEOSTAB_C_H


#include "opencv2/opencv_modules.hpp"

#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_VIDEOSTAB
#include "opencv2/videostab/stabilizer.hpp"
#else
static inline CV_NORETURN void throw_no_videostab() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without Videoio support"); }
namespace cv {
	namespace videostab {
		class IFrameSource {};
	}
}
#endif

#ifdef HAVE_OPENCV_VIDEOIO
#include "opencv2/videoio.hpp"

class CaptureFrameSource : public cv::videostab::IFrameSource
{
public:
    CaptureFrameSource(cv::VideoCapture* capture)
        : _capture(capture)
    {
#ifndef HAVE_OPENCV_HIGHGUI
		throw_no_highgui();
#endif
    };

    virtual void reset()
    {
#ifdef HAVE_OPENCV_HIGHGUI
        _capture->set(cv::VideoCaptureProperties::CAP_PROP_POS_FRAMES, 0);
#else
		throw_no_highgui();
#endif    	
    };

    virtual cv::Mat nextFrame()
    {
#ifdef HAVE_OPENCV_HIGHGUI
        cv::Mat m;
        _capture->read(m);
        return m;
#else
		throw_no_highgui();
#endif
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


#ifdef HAVE_OPENCV_VIDEOSTAB
#else
namespace cv {
	namespace videostab {
		class StabilizerBase {};
		class ImageMotionEstimatorBase {};
		class OnePassStabilizer {};
		class TwoPassStabilizer {};
		class MotionFilterBase {};
		class GaussianMotionFilter {};
	}
}

#endif

CVAPI(CaptureFrameSource*) cveVideostabCaptureFrameSourceCreate(cv::VideoCapture* capture, cv::videostab::IFrameSource** frameSource);
CVAPI(void) cveVideostabCaptureFrameSourceRelease(CaptureFrameSource** captureFrameSource);
CVAPI(bool) cveVideostabFrameSourceGetNextFrame(cv::videostab::IFrameSource* frameSource, cv::Mat* nextFrame);


CVAPI(void) cveStabilizerBaseSetMotionEstimator(cv::videostab::StabilizerBase* stabalizer, cv::videostab::ImageMotionEstimatorBase* motionEstimator);

CVAPI(cv::videostab::OnePassStabilizer*) cveOnePassStabilizerCreate(cv::videostab::IFrameSource* baseFrameSource, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource);
CVAPI(void) cveOnePassStabilizerSetMotionFilter(cv::videostab::OnePassStabilizer* stabilizer, cv::videostab::MotionFilterBase* motionFilter);
CVAPI(void) cveOnePassStabilizerRelease(cv::videostab::OnePassStabilizer** stabilizer);

CVAPI(cv::videostab::TwoPassStabilizer*) cveTwoPassStabilizerCreate(cv::videostab::IFrameSource* baseFrameSource, cv::videostab::StabilizerBase** stabilizerBase, cv::videostab::IFrameSource** frameSource);
CVAPI(void) cveTwoPassStabilizerRelease(cv::videostab::TwoPassStabilizer** stabilizer);

CVAPI(cv::videostab::GaussianMotionFilter*) cveGaussianMotionFilterCreate(int radius, float stdev);
CVAPI(void) cveGaussianMotionFilterRelease(cv::videostab::GaussianMotionFilter** filter);

#endif
