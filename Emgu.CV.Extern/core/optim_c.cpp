//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "optim_c.h"

int cveSolveLP(const cv::Mat* Func, const cv::Mat* Constr, cv::Mat* z)
{
   return cv::solveLP(*Func, *Constr, *z);
}
