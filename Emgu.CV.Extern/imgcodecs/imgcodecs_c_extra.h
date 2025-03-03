//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_IMGCODECS_C_H
#define EMGU_IMGCODECS_C_H

#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_IMGCODECS
#include "opencv2/imgcodecs/imgcodecs.hpp"
#else
static inline CV_NORETURN void throw_no_imgcodecs() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without imgcodecs support. To use this module, please switch to the full Emgu CV runtime."); }
#endif


CVAPI(bool) cveHaveImageReader(cv::String* filename);
CVAPI(bool) cveHaveImageWriter(cv::String* filename);

CVAPI(bool) cveImwrite(cv::String* filename, cv::_InputArray* img, std::vector<int>* params);
CVAPI(bool) cveImwritemulti(cv::String* filename, cv::_InputArray* img, std::vector<int>* params);

CVAPI(void) cveImread(cv::String* fileName, int flags, cv::Mat* result);
CVAPI(bool) cveImreadmulti(const cv::String* filename, std::vector<cv::Mat>* mats, int flags);

CVAPI(void) cveImdecode(cv::_InputArray* buf, int flags, cv::Mat* dst);
CVAPI(bool) cveImencode(cv::String* ext, cv::_InputArray* img, std::vector< unsigned char >* buf, std::vector< int >* params);


CVAPI(cv::Animation*) cveAnimationCreate(int loopCount, CvScalar* bgColor);
CVAPI(void) cveAnimationRelease(cv::Animation** animation);
CVAPI(std::vector<int>*) cveAnimationGetDurations(cv::Animation* animation);
CVAPI(std::vector<cv::Mat>*) cveAnimationGetFrames(cv::Animation* animation);


CVAPI(bool) cveImreadAnimation(cv::String* filename, cv::Animation* animation, int start, int count);
CVAPI(bool) cveImwriteAnimation(cv::String* filename, cv::Animation* animation, std::vector<int>* params);


#endif