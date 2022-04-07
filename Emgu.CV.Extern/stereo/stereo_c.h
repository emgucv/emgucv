//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_STEREO_C_H
#define EMGU_STEREO_C_H

#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_STEREO
#include "opencv2/stereo.hpp"
#else
static inline CV_NORETURN void throw_no_stereo() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without stereo support"); }
/*
namespace cv {
	namespace stereo {
		class QuasiDenseStereo {};
		struct PropagationParameters {};
	}
}
*/
#endif

//StereoSGBM
CVAPI(cv::StereoSGBM*) cveStereoSGBMCreate(
	int minDisparity, int numDisparities, int blockSize,
	int P1, int P2, int disp12MaxDiff,
	int preFilterCap, int uniquenessRatio,
	int speckleWindowSize, int speckleRange,
	int mode, cv::StereoMatcher** stereoMatcher, cv::Ptr<cv::StereoSGBM>** sharedPtr);
CVAPI(void) cveStereoSGBMRelease(cv::Ptr<cv::StereoSGBM>** sharedPtr);

//StereoBM
CVAPI(cv::StereoMatcher*) cveStereoBMCreate(int mode, int numberOfDisparities, cv::Ptr<cv::StereoMatcher>** sharedPtr);

//StereoMatcher
CVAPI(void) cveStereoMatcherCompute(cv::StereoMatcher* disparitySolver, cv::_InputArray* left, cv::_InputArray* right, cv::_OutputArray* disparity);
CVAPI(void) cveStereoMatcherRelease(cv::Ptr<cv::StereoMatcher>** sharedPtr);

CVAPI(bool) cveStereoRectifyUncalibrated(cv::_InputArray* points1, cv::_InputArray* points2, cv::_InputArray* f, CvSize* imgSize, cv::_OutputArray* h1, cv::_OutputArray* h2, double threshold);

CVAPI(void) cveStereoRectify(
	cv::_InputArray* cameraMatrix1, cv::_InputArray* distCoeffs1,
	cv::_InputArray* cameraMatrix2, cv::_InputArray* distCoeffs2,
	CvSize* imageSize, cv::_InputArray* r, cv::_InputArray* t,
	cv::_OutputArray* r1, cv::_OutputArray* r2,
	cv::_OutputArray* p1, cv::_OutputArray* p2,
	cv::_OutputArray* q, int flags,
	double alpha, CvSize* newImageSize,
	CvRect* validPixROI1, CvRect* validPixROI2);

#endif