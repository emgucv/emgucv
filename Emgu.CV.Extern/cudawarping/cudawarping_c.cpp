//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudawarping_c.h"


void cudaPyrDown(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAWARPING
	cv::cuda::pyrDown(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudawarping();
#endif
}

void cudaPyrUp(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAWARPING
	cv::cuda::pyrUp(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudawarping();
#endif
}

void cudaWarpAffine(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* M, CvSize* dSize, int flags, int borderMode, CvScalar* borderValue, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAWARPING
	cv::cuda::warpAffine(*src, *dst, *M, *dSize, flags, borderMode, *borderValue, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudawarping();
#endif
}

void cudaWarpPerspective(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* M, CvSize* size, int flags, int borderMode, CvScalar* borderValue, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAWARPING
	cv::cuda::warpPerspective(*src, *dst, *M, *size, flags, borderMode, *borderValue, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudawarping();
#endif
}

void cudaRemap(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* xmap, cv::_InputArray* ymap, int interpolation, int borderMode, CvScalar* borderValue, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAWARPING
	cv::cuda::remap(*src, *dst, *xmap, *ymap, interpolation, borderMode, *borderValue, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudawarping();
#endif
}

void cudaResize(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* dsize, double fx, double fy, int interpolation, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAWARPING
		cv::cuda::resize(*src, *dst, *dsize, fx, fy, interpolation, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudawarping();
#endif
}

void cudaRotate(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* dSize, double angle, double xShift, double yShift, int interpolation, cv::cuda::Stream* s)
{
#ifdef HAVE_OPENCV_CUDAWARPING
	cv::cuda::rotate(*src, *dst, *dSize, angle, xShift, yShift, interpolation, s ? *s : cv::cuda::Stream::Null());
#else
	throw_no_cudawarping();
#endif
}
