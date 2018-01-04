//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_OPTIM_C_H
#define EMGU_OPTIM_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/core/optim.hpp"

CVAPI(int) cveSolveLP(const cv::Mat* Func, const cv::Mat* Constr, cv::Mat* z);

#endif