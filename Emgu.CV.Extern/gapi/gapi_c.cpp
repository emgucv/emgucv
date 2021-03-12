//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gapi_c.h"

cv::GMat* cveGMatCreate()
{
	return new cv::GMat();
}
void cveGMatRelease(cv::GMat** gmat)
{
	delete* gmat;
	*gmat = 0;
}

cv::GMat* cveGapiResize(cv::GMat* src, cv::Size* dsize, double fx, double fy, int interpolation)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::resize(*src, *dsize, fx, fy, interpolation);
	return result;
}

cv::GMat* cveGapiBitwiseNot(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::bitwise_not(*src);
	return result;
}

cv::GComputation* cveGComputationCreate(cv::GMat* in, cv::GMat* out)
{
	return new cv::GComputation(*in, *out);
}

void cveGComputationApply(cv::GComputation* computation, cv::Mat* in, cv::Mat* out)
{
	computation->apply(*in, *out);
}