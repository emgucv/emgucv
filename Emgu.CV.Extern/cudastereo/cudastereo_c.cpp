//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudastereo_c.h"

cv::cuda::StereoBM* cudaStereoBMCreate(int numDisparities, int blockSize, cv::Ptr<cv::cuda::StereoBM>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDASTEREO
	cv::Ptr<cv::cuda::StereoBM> ptr = cv::cuda::createStereoBM(numDisparities, blockSize);
	*sharedPtr = new cv::Ptr<cv::cuda::StereoBM>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudastereo();
#endif
}

void cudaStereoBMFindStereoCorrespondence(cv::cuda::StereoBM* stereo, cv::_InputArray* left, cv::_InputArray* right, cv::_OutputArray* disparity, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDASTEREO
	stereo->compute(*left, *right, *disparity, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudastereo();
#endif
}

void cudaStereoBMRelease(cv::Ptr<cv::cuda::StereoBM>** stereoBM)
{
#ifdef HAVE_OPENCV_CUDASTEREO
	delete *stereoBM;
	*stereoBM = 0;
#else
	throw_no_cudastereo();
#endif
}

cv::cuda::StereoConstantSpaceBP* cudaStereoConstantSpaceBPCreate(int ndisp, int iters, int levels, int nr_plane, cv::Ptr<cv::cuda::StereoConstantSpaceBP>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDASTEREO
	cv::Ptr<cv::cuda::StereoConstantSpaceBP> ptr = cv::cuda::createStereoConstantSpaceBP(ndisp, iters, levels, nr_plane, CV_32F);
	*sharedPtr = new cv::Ptr<cv::cuda::StereoConstantSpaceBP>(ptr);
	return ptr.get();
#else
	throw_no_cudastereo();
#endif
}

void cudaStereoConstantSpaceBPFindStereoCorrespondence(cv::cuda::StereoConstantSpaceBP* stereo, cv::_InputArray* left, cv::_InputArray* right, cv::_OutputArray* disparity, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDASTEREO
	stereo->compute(*left, *right, *disparity, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudastereo();
#endif
}

void cudaStereoConstantSpaceBPRelease(cv::Ptr<cv::cuda::StereoConstantSpaceBP>** stereo)
{
#ifdef HAVE_OPENCV_CUDASTEREO
	delete *stereo;
	*stereo = 0;
#else
	throw_no_cudastereo();
#endif
}

cv::cuda::DisparityBilateralFilter* cudaDisparityBilateralFilterCreate(int ndisp, int radius, int iters, cv::Ptr<cv::cuda::DisparityBilateralFilter>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDASTEREO
	cv::Ptr<cv::cuda::DisparityBilateralFilter> ptr = cv::cuda::createDisparityBilateralFilter(ndisp, radius, iters);
	*sharedPtr = new cv::Ptr<cv::cuda::DisparityBilateralFilter>(ptr);
	return ptr.get();
#else
	throw_no_cudastereo();
#endif
}

void cudaDisparityBilateralFilterApply(cv::cuda::DisparityBilateralFilter* filter, cv::_InputArray* disparity, cv::_InputArray* image, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDASTEREO
	filter->apply(*disparity, *image, *dst, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudastereo();
#endif
}

void cudaDisparityBilateralFilterRelease(cv::Ptr<cv::cuda::DisparityBilateralFilter>** filter)
{
#ifdef HAVE_OPENCV_CUDASTEREO
	delete *filter;
	*filter = 0;
#else
	throw_no_cudastereo();
#endif
}

void cudaDrawColorDisp(cv::_InputArray* srcDisp, cv::_OutputArray* dstDisp, int ndisp, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDASTEREO
	cv::cuda::drawColorDisp(*srcDisp, *dstDisp, ndisp, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudastereo();
#endif
}

