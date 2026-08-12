//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "fuzzy_c.h"


void cveFtCreateKernel(cv::_InputArray* A, cv::_InputArray* B, cv::_OutputArray* kernel, int chn)
{
	try
	{
	#ifdef HAVE_OPENCV_FUZZY
		cv::ft::createKernel(*A, *B, *kernel, chn);
	#else
		throw_no_fuzzy();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFtcreateKernelFromFunction(int function, int radius, cv::_OutputArray* kernel, int chn)
{
	try
	{
	#ifdef HAVE_OPENCV_FUZZY
		cv::ft::createKernel(function, radius, *kernel, chn);
	#else
		throw_no_fuzzy();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFtInpaint(cv::Mat* image, cv::Mat* mask, cv::Mat* output, int radius, int function, int algorithm)
{
	try
	{
	#ifdef HAVE_OPENCV_FUZZY
		cv::ft::inpaint(*image, *mask, *output, radius, function, algorithm);
	#else
		throw_no_fuzzy();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveFtFilter(cv::Mat* image, cv::Mat* kernel, cv::Mat* output)
{
	try
	{
	#ifdef HAVE_OPENCV_FUZZY
		cv::ft::filter(*image, *kernel, *output);
	#else
		throw_no_fuzzy();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}