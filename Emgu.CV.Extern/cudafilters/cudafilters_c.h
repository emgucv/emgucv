//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDAFILTERS_C_H
#define EMGU_CUDAFILTERS_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_CUDAFILTERS

#include "opencv2/cudafilters.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "emgu_c.h"

#else
static inline CV_NORETURN void throw_no_cudafilters() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without CUDA Filters support"); }

namespace cv
{
	namespace cuda
	{
		class Filter
		{
		};
	}
}

#endif

CVAPI(cv::cuda::Filter*) cudaCreateSobelFilter(int srcType, int dstType,  int dx, int dy, int ksize, double scale, int rowBorderType, int columnBorderType, cv::Ptr<cv::cuda::Filter>** sharedPtr);

CVAPI(cv::cuda::Filter*) cudaCreateGaussianFilter(int srcType, int dstType, CvSize* ksize, double sigma1, double sigma2, int rowBorderType, int columnBorderType, cv::Ptr<cv::cuda::Filter>** sharedPtr);

CVAPI(cv::cuda::Filter*) cudaCreateLaplacianFilter(int srcType, int dstType, int ksize, double scale, int borderMode, CvScalar* borderValue, cv::Ptr<cv::cuda::Filter>** sharedPtr);

CVAPI(cv::cuda::Filter*) cudaCreateLinearFilter(int srcType, int dstType, cv::_InputArray* kernel, CvPoint* anchor, int borderMode, CvScalar* borderValue, cv::Ptr<cv::cuda::Filter>** sharedPtr);

CVAPI(cv::cuda::Filter*) cudaCreateBoxFilter(int srcType, int dstType, CvSize* ksize, CvPoint* anchor, int borderMode, CvScalar* borderValue, cv::Ptr<cv::cuda::Filter>** sharedPtr);

CVAPI(cv::cuda::Filter*) cudaCreateBoxMaxFilter( int srcType, CvSize* ksize, CvPoint* anchor, int borderMode, CvScalar* borderValue, cv::Ptr<cv::cuda::Filter>** sharedPtr);

CVAPI(cv::cuda::Filter*) cudaCreateBoxMinFilter( int srcType, CvSize* ksize, CvPoint* anchor, int borderMode, CvScalar* borderValue, cv::Ptr<cv::cuda::Filter>** sharedPtr);

CVAPI(cv::cuda::Filter*) cudaCreateMorphologyFilter( int op, int srcType, cv::_InputArray* kernel, CvPoint* anchor, int iterations, cv::Ptr<cv::cuda::Filter>** sharedPtr);

CVAPI(cv::cuda::Filter*) cudaCreateSeparableLinearFilter(
	int srcType, int dstType, cv::_InputArray* rowKernel, cv::_InputArray* columnKernel,
	CvPoint* anchor, int rowBorderMode, int columnBorderMode, cv::Ptr<cv::cuda::Filter>** sharedPtr);

CVAPI(cv::cuda::Filter*) cudaCreateDerivFilter(int srcType, int dstType, int dx, int dy,
	int ksize, bool normalize, double scale,
	int rowBorderMode, int columnBorderMode, cv::Ptr<cv::cuda::Filter>** sharedPtr);

CVAPI(cv::cuda::Filter*) cudaCreateScharrFilter(
	int srcType, int dstType, int dx, int dy,
	double scale, int rowBorderMode, int columnBorderMode, cv::Ptr<cv::cuda::Filter>** sharedPtr);

CVAPI(cv::cuda::Filter*) cudaCreateRowSumFilter(int srcType, int dstType, int ksize, int anchor, int borderMode, CvScalar* borderVal, cv::Ptr<cv::cuda::Filter>** sharedPtr);

CVAPI(cv::cuda::Filter*) cudaCreateColumnSumFilter(int srcType, int dstType, int ksize, int anchor, int borderMode, CvScalar* borderVal, cv::Ptr<cv::cuda::Filter>** sharedPtr);

CVAPI(cv::cuda::Filter*) cudaCreateMedianFilter(int srcType, int windowSize, int partition, cv::Ptr<cv::cuda::Filter>** sharedPtr);


//----------------------------------------------------------------------------
//
//  CudaFilter
//
//----------------------------------------------------------------------------
CVAPI(void) cudaFilterApply(cv::cuda::Filter* filter, cv::_InputArray* image, cv::_OutputArray* dst, cv::cuda::Stream* stream);
CVAPI(void) cudaFilterRelease(cv::Ptr<cv::cuda::Filter>** filter);

#endif