//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDAFILTERS_C_H
#define EMGU_CUDAFILTERS_C_H

#include "opencv2/cuda.hpp"
#include "opencv2/cudafilters.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "emgu_c.h"

CVAPI(cv::cuda::Filter*) cudaCreateSobelFilter(int srcType, int dstType,  int dx, int dy, int ksize, double scale, int rowBorderType, int columnBorderType);

CVAPI(cv::cuda::Filter*) cudaCreateGaussianFilter(int srcType, int dstType, emgu::size* ksize, double sigma1, double sigma2, int rowBorderType, int columnBorderType);

CVAPI(cv::cuda::Filter*) cudaCreateLaplacianFilter(int srcType, int dstType, int ksize, double scale, int borderMode, CvScalar* borderValue);

CVAPI(cv::cuda::Filter*) cudaCreateLinearFilter(int srcType, int dstType, const CvArr* kernel, CvPoint* anchor, int borderMode, CvScalar* borderValue);

CVAPI(cv::cuda::Filter*) cudaCreateBoxMaxFilter( int srcType, emgu::size* ksize, CvPoint* anchor, int borderMode, CvScalar* borderValue);

CVAPI(cv::cuda::Filter*) cudaCreateBoxMinFilter( int srcType, emgu::size* ksize, CvPoint* anchor, int borderMode, CvScalar* borderValue);

CVAPI(cv::cuda::Filter*) cudaCreateMorphologyFilter( int op, int srcType, const CvArr* kernel, CvPoint* anchor, int iterations);

//----------------------------------------------------------------------------
//
//  CudaFilter
//
//----------------------------------------------------------------------------
CVAPI(void) cudaFilterApply(cv::cuda::Filter* filter, cv::_InputArray* image, cv::_OutputArray* dst, cv::cuda::Stream* stream);
CVAPI(void) cudaFilterRelease(cv::cuda::Filter** filter);

#endif