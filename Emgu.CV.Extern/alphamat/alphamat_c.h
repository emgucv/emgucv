//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_ALPHAMAT_C_H
#define EMGU_ALPHAMAT_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_ALPHAMAT

#include "opencv2/alphamat.hpp"

#else

static inline CV_NORETURN void throw_no_alphamat() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without Alphamat support"); }
#endif

CVAPI(void) cveAlphamatInfoFlow(cv::_InputArray* image, cv::_InputArray* tmap, cv::_OutputArray* result);

#endif