//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_SIGNAL_C_H
#define EMGU_SIGNAL_C_H

#include "opencv2/core.hpp"
#include "cvapi_compat.h"

#ifdef HAVE_OPENCV_SIGNAL
#include "opencv2/signal.hpp"
#else
static inline CV_NORETURN void throw_no_signal() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without signal module support. To use this module, please switch to the full Emgu CV runtime."); }
#endif

CVAPI(void) cveResampleSignal(cv::_InputArray* inputSignal, cv::_OutputArray* outSignal, int inFreq, int outFreq);

#endif
