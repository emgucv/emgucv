//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDAOPTFLOW_C_H
#define EMGU_CUDAOPTFLOW_C_H

#include "opencv2/cuda.hpp"
#include "opencv2/cudaoptflow.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "emgu_c.h"

//----------------------------------------------------------------------------
//
//  CudaBroxOpticalFlow 
//
//----------------------------------------------------------------------------

CVAPI(cv::cuda::BroxOpticalFlow*) cudaBroxOpticalFlowCreate(float alpha, float gamma, float scaleFactor, int innerIterations, int outerIterations, int solverIterations);

CVAPI(void) cudaBroxOpticalFlowCompute(cv::cuda::BroxOpticalFlow* flow, cv::cuda::GpuMat* frame0, const cv::cuda::GpuMat* frame1, cv::cuda::GpuMat* u, cv::cuda::GpuMat* v, cv::cuda::Stream* stream);

CVAPI(void) cudaBroxOpticalFlowRelease(cv::cuda::BroxOpticalFlow** flow);

//----------------------------------------------------------------------------
//
//  GpuPyrLKOpticalFlow
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::PyrLKOpticalFlow*) cudaPyrLKOpticalFlowCreate(emgu::size winSize, int maxLevel, int iters, bool useInitialFlow);
CVAPI(void) cudaPyrLKOpticalFlowSparse(
   cv::cuda::PyrLKOpticalFlow* flow, 
   const cv::cuda::GpuMat* prevImg, 
   const cv::cuda::GpuMat* nextImg, 
   const cv::cuda::GpuMat* prevPts, 
   cv::cuda::GpuMat* nextPts,
   cv::cuda::GpuMat* status, 
   cv::cuda::GpuMat* err);
CVAPI(void) cudaPyrLKOpticalFlowDense(
   cv::cuda::PyrLKOpticalFlow* flow, 
   const cv::cuda::GpuMat* prevImg, 
   const cv::cuda::GpuMat* nextImg,
   cv::cuda::GpuMat* u, 
   cv::cuda::GpuMat* v, 
   cv::cuda::GpuMat* err);
CVAPI(void) cudaPyrLKOpticalFlowRelease(cv::cuda::PyrLKOpticalFlow** flow);

//----------------------------------------------------------------------------
//
//  GpuFarnebackOpticalFlow
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
   int flags);

CVAPI(void) cudaFarnebackOpticalFlowCompute(cv::cuda::FarnebackOpticalFlow* flow, const cv::cuda::GpuMat* frame0, const cv::cuda::GpuMat* frame1, cv::cuda::GpuMat* u, cv::cuda::GpuMat* v, cv::cuda::Stream* stream);

CVAPI(void) cudaFarnebackOpticalFlowRelease(cv::cuda::FarnebackOpticalFlow** flow);


//----------------------------------------------------------------------------
//
//  Utilities
//
//----------------------------------------------------------------------------
CVAPI(void) cudaCreateOpticalFlowNeedleMap(const cv::cuda::GpuMat* u, const cv::cuda::GpuMat* v, cv::cuda::GpuMat* vertex, cv::cuda::GpuMat* colors);

#endif