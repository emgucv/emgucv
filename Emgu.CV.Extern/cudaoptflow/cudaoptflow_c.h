//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDAOPTFLOW_C_H
#define EMGU_CUDAOPTFLOW_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_CUDAOPTFLOW
#include "opencv2/cudaoptflow.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "emgu_c.h"
#else
static inline CV_NORETURN void throw_no_cudaoptflow() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without CUDA Optflow support"); }

namespace cv
{
	namespace cuda
	{
		class DenseOpticalFlow
		{
		};

		class SparseOpticalFlow
		{
		};

		class BroxOpticalFlow
		{
		};

		class DensePyrLKOpticalFlow
		{
		};

		class SparsePyrLKOpticalFlow
		{
		};

		class FarnebackOpticalFlow
		{
		};

		class OpticalFlowDual_TVL1
		{
		};
	}
}

#endif

//----------------------------------------------------------------------------
//
//  DenseOpticalFlow 
//
//----------------------------------------------------------------------------

CVAPI(void) cudaDenseOpticalFlowCalc(
	cv::cuda::DenseOpticalFlow* opticalFlow,
	cv::_InputArray* I0,
	cv::_InputArray* I1,
	cv::_InputOutputArray* flow,
	cv::cuda::Stream* stream);

//----------------------------------------------------------------------------
//
//  SparseOpticalFlow 
//
//----------------------------------------------------------------------------

CVAPI(void) cudaSparseOpticalFlowCalc(
	cv::cuda::SparseOpticalFlow* opticalFlow,
	cv::_InputArray* prevImg, cv::_InputArray* nextImg,
	cv::_InputArray* prevPts, cv::_InputOutputArray* nextPts,
	cv::_OutputArray* status,
	cv::_OutputArray* err,
	cv::cuda::Stream* stream);

//----------------------------------------------------------------------------
//
//  CudaBroxOpticalFlow 
//
//----------------------------------------------------------------------------

CVAPI(cv::cuda::BroxOpticalFlow*) cudaBroxOpticalFlowCreate(
	double alpha, double gamma, double scaleFactor,
	int innerIterations, int outerIterations, int solverIterations,
	cv::cuda::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm,
	cv::Ptr<cv::cuda::BroxOpticalFlow>** sharedPtr);

CVAPI(void) cudaBroxOpticalFlowRelease(cv::Ptr<cv::cuda::BroxOpticalFlow>** flow);

//----------------------------------------------------------------------------
//
//  CudaDensePyrLKOpticalFlow
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::DensePyrLKOpticalFlow *) cudaDensePyrLKOpticalFlowCreate(
	CvSize* winSize, 
	int maxLevel, 
	int iters, 
	bool useInitialFlow, 
	cv::cuda::DenseOpticalFlow** denseFlow, 
	cv::Algorithm** algorithm,
	cv::Ptr<cv::cuda::DensePyrLKOpticalFlow>** sharedPtr);
CVAPI(void) cudaDensePyrLKOpticalFlowRelease(cv::Ptr<cv::cuda::DensePyrLKOpticalFlow>** flow);

//----------------------------------------------------------------------------
//
//  CudaSparsePyrLKOpticalFlow
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::SparsePyrLKOpticalFlow *) cudaSparsePyrLKOpticalFlowCreate(
	CvSize* winSize, 
	int maxLevel, 
	int iters, 
	bool useInitialFlow, 
	cv::cuda::SparseOpticalFlow** sparseFlow, 
	cv::Algorithm** algorithm,
	cv::Ptr<cv::cuda::SparsePyrLKOpticalFlow>** sharedPtr);
CVAPI(void) cudaSparsePyrLKOpticalFlowRelease(cv::Ptr<cv::cuda::SparsePyrLKOpticalFlow>** flow);

//----------------------------------------------------------------------------
//
//  CudaFarnebackOpticalFlow
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::FarnebackOpticalFlow*) cudaFarnebackOpticalFlowCreate(
	int numLevels,
	double pyrScale,
	bool fastPyramids,
	int winSize,
	int numIters,
	int polyN,
	double polySigma,
	int flags,
	cv::cuda::DenseOpticalFlow** denseFlow,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::cuda::FarnebackOpticalFlow>** sharedPtr);

CVAPI(void) cudaFarnebackOpticalFlowRelease(cv::Ptr<cv::cuda::FarnebackOpticalFlow>** flow);

//----------------------------------------------------------------------------
//
//  CudaOpticalFlowDualTvl1
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::OpticalFlowDual_TVL1*) cudaOpticalFlowDualTvl1Create(
	double tau, double lambda, double theta, int nscales, int warps,
	double epsilon, int iterations, double scaleStep, double gamma, bool useInitialFlow,
	cv::cuda::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm,
	cv::Ptr<cv::cuda::OpticalFlowDual_TVL1>** sharedPtr);

CVAPI(void) cudaOpticalFlowDualTvl1Release(cv::Ptr<cv::cuda::OpticalFlowDual_TVL1>** flow);

/*
//----------------------------------------------------------------------------
//
//  Utilities
//
//----------------------------------------------------------------------------
CVAPI(void) cudaCreateOpticalFlowNeedleMap(const cv::cuda::GpuMat* u, const cv::cuda::GpuMat* v, cv::cuda::GpuMat* vertex, cv::cuda::GpuMat* colors);
*/
#endif