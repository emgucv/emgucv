//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "fuzzy_c.h"


void cveFtCreateKernel(cv::_InputArray* A, cv::_InputArray* B, cv::_OutputArray* kernel, int chn)
{
   cv::ft::createKernel(*A, *B, *kernel, chn);
}
void cveFtcreateKernelFromFunction(int function, int radius, cv::_OutputArray* kernel, int chn)
{
   cv::ft::createKernel(function, radius, *kernel, chn);
}
void cveFtInpaint(cv::Mat* image, cv::Mat* mask, cv::Mat* output, int radius, int function, int algorithm)
{
   cv::ft::inpaint(*image, *mask, *output, radius, function, algorithm);
}
void cveFtFilter(cv::Mat* image, cv::Mat* kernel, cv::Mat* output)
{
   cv::ft::filter(*image, *kernel, *output);
}