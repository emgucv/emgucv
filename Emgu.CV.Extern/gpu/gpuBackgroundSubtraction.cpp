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
cv::gpu::MOG2_GPU* gpuMog2Create(int nMixtures)
{
   return new cv::gpu::MOG2_GPU(nMixtures);
}

void gpuMog2Compute(cv::gpu::MOG2_GPU* mog, cv::gpu::GpuMat* frame, float learningRate, cv::gpu::GpuMat* fgMask, cv::gpu::Stream* stream)
{
   (*mog)(*frame, *fgMask, learningRate, stream ? *stream : cv::gpu::Stream::Null());
}

void gpuMog2Release(cv::gpu::MOG2_GPU** mog)
{
   (*mog)->release();
   delete (*mog);
   *mog = 0;
}