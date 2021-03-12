//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_GAPI_C_H
#define EMGU_GAPI_C_H

#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_GAPI
#include "opencv2/gapi.hpp"
#include "opencv2/gapi/core.hpp"
#include "opencv2/gapi/imgproc.hpp"
#else
static inline CV_NORETURN void throw_no_gapi() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without gapi support"); }
namespace cv {
	namespace gapi {
		
	}
}
#endif

CVAPI(cv::GMat*) cveGMatCreate();
CVAPI(void) cveGMatRelease(cv::GMat** gmat);

CVAPI(cv::GMat*) cveGapiResize(cv::GMat* src, cv::Size* dsize, double fx, double fy, int interpolation);
CVAPI(cv::GMat*) cveGapiBitwiseNot(cv::GMat* src);


CVAPI(cv::GComputation*) cveGComputationCreate(cv::GMat* input, cv::GMat* output);
CVAPI(void) cveGComputationRelease(cv::GComputation** computation);
CVAPI(void) cveGComputationApply(cv::GComputation* computation, cv::Mat* in, cv::Mat* out);

#endif