//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_FUZZY_C_H
#define EMGU_FUZZY_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/fuzzy.hpp"

CVAPI(void) cveFtCreateKernel(cv::_InputArray* A, cv::_InputArray* B, cv::_OutputArray* kernel, int chn);
#endif