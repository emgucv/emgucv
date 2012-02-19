//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gpu_c.h"

//----------------------------------------------------------------------------
//
//  GpuBroxOpticalFlow 
//
//----------------------------------------------------------------------------

cv::gpu::BroxOpticalFlow* gpuBroxOpticalFlowCreate(float alpha, float gamma, float scaleFactor, int innerIterations, int outerIterations, int solverIterations)
{
   return new cv::gpu::BroxOpticalFlow(alpha, gamma, scaleFactor, innerIterations, outerIterations, solverIterations);
}

void gpuBroxOpticalFlowCompute(cv::gpu::BroxOpticalFlow* flow, cv::gpu::GpuMat* frame0, const cv::gpu::GpuMat* frame1, cv::gpu::GpuMat* u, cv::gpu::GpuMat* v, cv::gpu::Stream* stream)
{
   (*flow)(*frame0, *frame1, *u, *v, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuBroxOpticalFlowRelease(cv::gpu::BroxOpticalFlow** flow)
{
   delete *flow;
   *flow = 0;
}

//----------------------------------------------------------------------------
//
//  GpuFarnebackOpticalFlow
//
//----------------------------------------------------------------------------
cv::gpu::FarnebackOpticalFlow* gpuFarnebackOpticalFlowCreate(    
   int numLevels,
   double pyrScale,
   bool fastPyramids,
   int winSize,
   int numIters,
   int polyN,
   double polySigma,
   int flags)
{
   cv::gpu::FarnebackOpticalFlow* flow = new cv::gpu::FarnebackOpticalFlow();
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

void gpuFarnebackOpticalFlowCompute(cv::gpu::FarnebackOpticalFlow* flow, cv::gpu::GpuMat* frame0, const cv::gpu::GpuMat* frame1, cv::gpu::GpuMat* u, cv::gpu::GpuMat* v, cv::gpu::Stream* stream)
{
   (*flow)(*frame0, *frame1, *u, *v, stream? *stream : cv::gpu::Stream());
}

void gpuFarnebackOpticalFlowRelease(cv::gpu::FarnebackOpticalFlow** flow)
{
   delete *flow;
   *flow = 0;
}

//----------------------------------------------------------------------------
//
//  GpuPyrLKOpticalFlow
//
//----------------------------------------------------------------------------
cv::gpu::PyrLKOpticalFlow* gpuPryLKOpticalFlowCreate(cv::Size winSize, int maxLevel, int iters, double derivLambda, bool useInitialFlow, float minEigThreshold)
{
   cv::gpu::PyrLKOpticalFlow* flow = new cv::gpu::PyrLKOpticalFlow();
   
   flow->winSize = winSize;
   flow->maxLevel = maxLevel;
   flow->iters = iters;
   flow->derivLambda = derivLambda;
   flow->useInitialFlow = useInitialFlow;
   flow->minEigThreshold = minEigThreshold;
   return flow;
}

void gpuPryLKOpticalFlowSparse(
   cv::gpu::PyrLKOpticalFlow* flow, 
   const cv::gpu::GpuMat* prevImg, 
   const cv::gpu::GpuMat* nextImg, 
   const cv::gpu::GpuMat* prevPts, 
   cv::gpu::GpuMat* nextPts,
   cv::gpu::GpuMat* status, 
   cv::gpu::GpuMat* err)
{
   flow->sparse(*prevImg, *nextImg, *prevPts, *nextPts, *status, err);
}

void gpuPryLKOpticalFlowDense(
   cv::gpu::PyrLKOpticalFlow* flow, 
   const cv::gpu::GpuMat* prevImg, 
   const cv::gpu::GpuMat* nextImg,
   cv::gpu::GpuMat* u, 
   cv::gpu::GpuMat* v, 
   cv::gpu::GpuMat* err)
{
   flow->dense(*prevImg, *nextImg, *u, *v, err);
}

void gpuPryLKOpticalFlowRelease(cv::gpu::PyrLKOpticalFlow** flow)
{
   delete *flow;
   *flow = 0;
}
