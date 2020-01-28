//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "fuzzy_c.h"


void cveFtCreateKernel(cv::_InputArray* A, cv::_InputArray* B, cv::_OutputArray* kernel, int chn)
{
#ifdef HAVE_OPENCV_FUZZY
	cv::ft::createKernel(*A, *B, *kernel, chn);
#else
	throw_no_fuzzy();
#endif
}
void cveFtcreateKernelFromFunction(int function, int radius, cv::_OutputArray* kernel, int chn)
{
#ifdef HAVE_OPENCV_FUZZY
	cv::ft::createKernel(function, radius, *kernel, chn);
#else
	throw_no_fuzzy();
#endif
}
void cveFtInpaint(cv::Mat* image, cv::Mat* mask, cv::Mat* output, int radius, int function, int algorithm)
{
#ifdef HAVE_OPENCV_FUZZY
	cv::ft::inpaint(*image, *mask, *output, radius, function, algorithm);
#else
	throw_no_fuzzy();
#endif
}
void cveFtFilter(cv::Mat* image, cv::Mat* kernel, cv::Mat* output)
{
#ifdef HAVE_OPENCV_FUZZY
	cv::ft::filter(*image, *kernel, *output);
#else
	throw_no_fuzzy();
#endif
}