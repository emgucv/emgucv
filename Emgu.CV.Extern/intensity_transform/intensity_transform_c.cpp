//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "intensity_transform_c.h"

void cveLogTransform(cv::Mat* input, cv::Mat* output)
{
	cv::intensity_transform::logTransform(*input, *output);
}

void cveGammaCorrection(cv::Mat* input, cv::Mat* output, float gamma)
{
	cv::intensity_transform::gammaCorrection(*input, *output, gamma);
}

void cveAutoscaling(cv::Mat* input, cv::Mat* output)
{
	cv::intensity_transform::autoscaling(*input, *output);
}

void cveContrastStretching(cv::Mat* input, cv::Mat* output, int r1, int s1, int r2, int s2)
{
	cv::intensity_transform::contrastStretching(*input, *output, r1, s1, r2, s2);
}

void cveBIMEF(cv::_InputArray* input, cv::_OutputArray* output, float mu, float a, float b)
{
	cv::intensity_transform::BIMEF(*input, *output, mu, a, b);
}

void cveBIMEF2(cv::_InputArray* input, cv::_OutputArray* output, float k, float mu, float a, float b)
{
	cv::intensity_transform::BIMEF(*input, *output, k, mu, a, b);
}