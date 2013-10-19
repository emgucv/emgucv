//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_OPTIM_C_H
#define EMGU_OPTIM_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/optim/optim.hpp"

CVAPI(int) cvSolveLP(const CvMat* Func, const CvMat* Constr, CvMat* z);

#endif