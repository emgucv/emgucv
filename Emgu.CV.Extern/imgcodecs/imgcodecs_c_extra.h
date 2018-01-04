//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_IMGCODECS_C_H
#define EMGU_IMGCODECS_C_H

#include "opencv2/imgcodecs/imgcodecs_c.h"
#include "opencv2/imgcodecs/imgcodecs.hpp"

CVAPI(bool) cveImwrite(cv::String* filename, cv::_InputArray* img, const std::vector<int>* params);

CVAPI(void) cveImread(cv::String* fileName, int flags, cv::Mat* result);
CVAPI(bool) cveImreadmulti(const cv::String* filename, std::vector<cv::Mat>* mats, int flags);

CVAPI(void) cveImdecode(cv::_InputArray* buf, int flags, cv::Mat* dst);
CVAPI(void) cveImencode(cv::String* ext, cv::_InputArray* img, std::vector< unsigned char >* buf, std::vector< int >* params);

#endif