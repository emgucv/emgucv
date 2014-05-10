//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "optim_c.h"

int cveSolveLP(const cv::Mat* Func, const cv::Mat* Constr, cv::Mat* z)
{
   return cv::optim::solveLP(*Func, *Constr, *z);
}

void cveDenoiseTVL1(const std::vector< cv::Mat >* observations, cv::Mat* result, double lambda, int niters)
{
   cv::optim::denoise_TVL1(*observations, *result, lambda, niters);
}