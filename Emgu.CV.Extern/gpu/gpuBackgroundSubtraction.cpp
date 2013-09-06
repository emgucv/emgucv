//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gpu_c.h"

//----------------------------------------------------------------------------
//
//  MOG2 GPU
//
//----------------------------------------------------------------------------
cv::gpu::BackgroundSubtractorMOG2* gpuMog2Create(int history, double varThreshold, bool detectShadows)
{
   cv::Ptr<cv::gpu::BackgroundSubtractorMOG2> ptr = cv::gpu::createBackgroundSubtractorMOG2(history, varThreshold, detectShadows);
   ptr.addref();
   return ptr.obj;
}

void gpuMog2Compute(cv::gpu::BackgroundSubtractorMOG2* mog, cv::gpu::GpuMat* frame, float learningRate, cv::gpu::GpuMat* fgMask, cv::gpu::Stream* stream)
{
   mog->apply(*frame, *fgMask, learningRate, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMog2Release(cv::gpu::BackgroundSubtractorMOG2** mog)
{
   delete (*mog);
   *mog = 0;
}