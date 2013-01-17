//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gpu_c.h"

//----------------------------------------------------------------------------
//
//  VIBE GPU
//
//----------------------------------------------------------------------------
cv::gpu::VIBE_GPU* gpuVibeCreate(unsigned long rngSeed, cv::gpu::GpuMat* firstFrame, cv::gpu::Stream* stream)
{
   cv::gpu::VIBE_GPU* vibe = new cv::gpu::VIBE_GPU(rngSeed);
   vibe->initialize(*firstFrame, stream ? *stream : cv::gpu::Stream::Null());
   return vibe;
}
void gpuVibeCompute(cv::gpu::VIBE_GPU* vibe, cv::gpu::GpuMat* frame, cv::gpu::GpuMat* fgMask, cv::gpu::Stream* stream)
{
   (*vibe)(*frame, *fgMask, stream ? *stream : cv::gpu::Stream::Null());
}
void gpuVibeRelease(cv::gpu::VIBE_GPU** vibe)
{
   (*vibe)->release();
   delete *vibe;
   *vibe = 0;
}

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