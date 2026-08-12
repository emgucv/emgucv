//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "intensity_transform_c.h"

void cveLogTransform(cv::Mat* input, cv::Mat* output)
{
	try
	{
	#ifdef HAVE_OPENCV_INTENSITY_TRANSFORM
		cv::intensity_transform::logTransform(*input, *output);
	#else
		throw_no_intensity_transform();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveGammaCorrection(cv::Mat* input, cv::Mat* output, float gamma)
{
	try
	{
	#ifdef HAVE_OPENCV_INTENSITY_TRANSFORM
		cv::intensity_transform::gammaCorrection(*input, *output, gamma);
	#else
		throw_no_intensity_transform();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveAutoscaling(cv::Mat* input, cv::Mat* output)
{
	try
	{
	#ifdef HAVE_OPENCV_INTENSITY_TRANSFORM
		cv::intensity_transform::autoscaling(*input, *output);
	#else
		throw_no_intensity_transform();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveContrastStretching(cv::Mat* input, cv::Mat* output, int r1, int s1, int r2, int s2)
{
	try
	{
	#ifdef HAVE_OPENCV_INTENSITY_TRANSFORM
		cv::intensity_transform::contrastStretching(*input, *output, r1, s1, r2, s2);
	#else
		throw_no_intensity_transform();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveBIMEF(cv::_InputArray* input, cv::_OutputArray* output, float mu, float a, float b)
{
	try
	{
	#ifdef HAVE_OPENCV_INTENSITY_TRANSFORM
		cv::intensity_transform::BIMEF(*input, *output, mu, a, b);
	#else
		throw_no_intensity_transform();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveBIMEF2(cv::_InputArray* input, cv::_OutputArray* output, float k, float mu, float a, float b)
{
	try
	{
	#ifdef HAVE_OPENCV_INTENSITY_TRANSFORM
		cv::intensity_transform::BIMEF(*input, *output, k, mu, a, b);
	#else
		throw_no_intensity_transform();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}