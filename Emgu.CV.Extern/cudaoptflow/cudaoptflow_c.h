//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDAOPTFLOW_C_H
#define EMGU_CUDAOPTFLOW_C_H

//#include "opencv2/cuda.hpp"
#include "opencv2/cudaoptflow.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "emgu_c.h"

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

CVAPI(cv::cuda::BroxOpticalFlow*) cudaBroxOpticalFlowCreate(double alpha, double gamma, double scaleFactor, int innerIterations, int outerIterations, int solverIterations, cv::cuda::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm);

CVAPI(void) cudaBroxOpticalFlowRelease(cv::cuda::BroxOpticalFlow** flow);

//----------------------------------------------------------------------------
//
//  CudaDensePyrLKOpticalFlow
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::DensePyrLKOpticalFlow *) cudaDensePyrLKOpticalFlowCreate(CvSize* winSize, int maxLevel, int iters, bool useInitialFlow, cv::cuda::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm);
CVAPI(void) cudaDensePyrLKOpticalFlowRelease(cv::cuda::DensePyrLKOpticalFlow** flow);

//----------------------------------------------------------------------------
//
//  CudaSparsePyrLKOpticalFlow
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::SparsePyrLKOpticalFlow *) cudaSparsePyrLKOpticalFlowCreate(CvSize* winSize, int maxLevel, int iters, bool useInitialFlow, cv::cuda::SparseOpticalFlow** sparseFlow, cv::Algorithm** algorithm);
CVAPI(void) cudaSparsePyrLKOpticalFlowRelease(cv::cuda::SparsePyrLKOpticalFlow** flow);

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
   cv::Algorithm** algorithm);

CVAPI(void) cudaFarnebackOpticalFlowRelease(cv::cuda::FarnebackOpticalFlow** flow);

//----------------------------------------------------------------------------
//
//  CudaOpticalFlowDualTvl1
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::OpticalFlowDual_TVL1*) cudaOpticalFlowDualTvl1Create(
   double tau, double lambda, double theta, int nscales, int warps,
   double epsilon, int iterations, double scaleStep, double gamma, bool useInitialFlow,
   cv::cuda::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm);

CVAPI(void) cudaOpticalFlowDualTvl1Release(cv::cuda::OpticalFlowDual_TVL1** flow);

/*
//----------------------------------------------------------------------------
//
//  Utilities
//
//----------------------------------------------------------------------------
CVAPI(void) cudaCreateOpticalFlowNeedleMap(const cv::cuda::GpuMat* u, const cv::cuda::GpuMat* v, cv::cuda::GpuMat* vertex, cv::cuda::GpuMat* colors);
*/
#endif