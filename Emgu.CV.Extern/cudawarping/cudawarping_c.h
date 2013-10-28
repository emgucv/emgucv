//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDAWARPING_C_H
#define EMGU_CUDAWARPING_C_H

#include "opencv2/cuda.hpp"
#include "opencv2/cudaarithm.hpp"
#include "opencv2/cudawarping.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "emgu_c.h"


CVAPI(void) cudaPyrDown(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaPyrUp(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaWarpAffine( const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, const CvArr* M, int flags, int borderMode, CvScalar borderValue, cv::cuda::Stream* stream);

CVAPI(void) cudaWarpPerspective( const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst,  const CvArr* M, int flags, int borderMode, CvScalar borderValue, cv::cuda::Stream* stream);

CVAPI(void) cudaRemap(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, const cv::cuda::GpuMat* xmap, const cv::cuda::GpuMat* ymap, int interpolation, int borderMode, CvScalar borderValue, cv::cuda::Stream* stream);

CVAPI(void) cudaResize(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int interpolation, cv::cuda::Stream* stream);

CVAPI(void) cudaRotate(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, double angle, double xShift, double yShift, int interpolation, cv::cuda::Stream* s);

#endif