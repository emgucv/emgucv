//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudaoptflow_c.h"

//----------------------------------------------------------------------------
//
//  CudaBroxOpticalFlow 
//
//----------------------------------------------------------------------------

cv::cuda::BroxOpticalFlow* cudaBroxOpticalFlowCreate(float alpha, float gamma, float scaleFactor, int innerIterations, int outerIterations, int solverIterations)
{
   return new cv::cuda::BroxOpticalFlow(alpha, gamma, scaleFactor, innerIterations, outerIterations, solverIterations);
}

void cudaBroxOpticalFlowCompute(cv::cuda::BroxOpticalFlow* flow, cv::cuda::GpuMat* frame0, const cv::cuda::GpuMat* frame1, cv::cuda::GpuMat* u, cv::cuda::GpuMat* v, cv::cuda::Stream* stream)
{
   (*flow)(*frame0, *frame1, *u, *v, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBroxOpticalFlowRelease(cv::cuda::BroxOpticalFlow** flow)
{
   delete *flow;
   *flow = 0;
}

//----------------------------------------------------------------------------
//
//  CudaFarnebackOpticalFlow
//
//----------------------------------------------------------------------------
cv::cuda::FarnebackOpticalFlow* cudaFarnebackOpticalFlowCreate(    
   int numLevels,
   double pyrScale,
   bool fastPyramids,
   int winSize,
   int numIters,
   int polyN,
   double polySigma,
   int flags)
{
   cv::cuda::FarnebackOpticalFlow* flow = new cv::cuda::FarnebackOpticalFlow();
   flow->numIters = numLevels;
   flow->pyrScale = pyrScale;
   flow->fastPyramids = fastPyramids;
   flow->winSize = winSize;
   flow->numIters = numIters;
   flow->polyN = polyN;
   flow->polySigma = polySigma;
   flow->flags = flags;
   return flow;
}

void cudaFarnebackOpticalFlowCompute(cv::cuda::FarnebackOpticalFlow* flow, const cv::cuda::GpuMat* frame0, const cv::cuda::GpuMat* frame1, cv::cuda::GpuMat* u, cv::cuda::GpuMat* v, cv::cuda::Stream* stream)
{
   (*flow)(*frame0, *frame1, *u, *v, stream? *stream : cv::cuda::Stream::Null());
}

void cudaFarnebackOpticalFlowRelease(cv::cuda::FarnebackOpticalFlow** flow)
{
   delete *flow;
   *flow = 0;
}

//----------------------------------------------------------------------------
//
//  CudaOpticalFlowDualTvl1
//
//----------------------------------------------------------------------------
cv::cuda::OpticalFlowDual_TVL1_CUDA* cudaOpticalFlowDualTvl1Create()
{
   return new cv::cuda::OpticalFlowDual_TVL1_CUDA();
}

void cudaOpticalFlowDualTvl1Compute(cv::cuda::OpticalFlowDual_TVL1_CUDA* flow, const cv::cuda::GpuMat* frame0, const cv::cuda::GpuMat* frame1, cv::cuda::GpuMat* u, cv::cuda::GpuMat* v)
{
   (*flow)(*frame0, *frame1, *u, *v);
}

void cudaOpticalFlowDualTvl1Release(cv::cuda::OpticalFlowDual_TVL1_CUDA** flow)
{
   delete *flow;
   *flow = 0;
}

//----------------------------------------------------------------------------
//
//  CudaPyrLKOpticalFlow
//
//----------------------------------------------------------------------------
cv::cuda::PyrLKOpticalFlow* cudaPyrLKOpticalFlowCreate(emgu::size winSize, int maxLevel, int iters, bool useInitialFlow)
{
   cv::cuda::PyrLKOpticalFlow* flow = new cv::cuda::PyrLKOpticalFlow();
   cv::Size s(winSize.width, winSize.height);
   flow->winSize = s;
   flow->maxLevel = maxLevel;
   flow->iters = iters;
   flow->useInitialFlow = useInitialFlow;
   return flow;
}

void cudaPyrLKOpticalFlowSparse(
   cv::cuda::PyrLKOpticalFlow* flow, 
   const cv::cuda::GpuMat* prevImg, 
   const cv::cuda::GpuMat* nextImg, 
   const cv::cuda::GpuMat* prevPts, 
   cv::cuda::GpuMat* nextPts,
   cv::cuda::GpuMat* status, 
   cv::cuda::GpuMat* err)
{
   flow->sparse(*prevImg, *nextImg, *prevPts, *nextPts, *status, err);
}

void cudaPyrLKOpticalFlowDense(
   cv::cuda::PyrLKOpticalFlow* flow, 
   const cv::cuda::GpuMat* prevImg, 
   const cv::cuda::GpuMat* nextImg,
   cv::cuda::GpuMat* u, 
   cv::cuda::GpuMat* v, 
   cv::cuda::GpuMat* err)
{
   flow->dense(*prevImg, *nextImg, *u, *v, err);
}

void cudaPyrLKOpticalFlowRelease(cv::cuda::PyrLKOpticalFlow** flow)
{
   delete *flow;
   *flow = 0;
}

//----------------------------------------------------------------------------
//
//  Utilities
//
//----------------------------------------------------------------------------
void cudaCreateOpticalFlowNeedleMap(const cv::cuda::GpuMat* u, const cv::cuda::GpuMat* v, cv::cuda::GpuMat* vertex, cv::cuda::GpuMat* colors)
{
   cv::cuda::createOpticalFlowNeedleMap(*u, *v, *vertex, *colors);
}