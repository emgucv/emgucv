//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gpu_c.h"

cv::gpu::BruteForceMatcher_GPU_base* gpuBruteForceMatcherCreate(cv::gpu::BruteForceMatcher_GPU_base::DistType distType) 
{
   return new cv::gpu::BruteForceMatcher_GPU_base(distType);
}

void gpuBruteForceMatcherRelease(cv::gpu::BruteForceMatcher_GPU_base** matcher) 
{
   delete *matcher;
}

void gpuBruteForceMatcherKnnMatch(
   cv::gpu::BruteForceMatcher_GPU_base* matcher,
   const cv::gpu::GpuMat* queryDescs, const cv::gpu::GpuMat* trainDescs,
   cv::gpu::GpuMat* trainIdx, cv::gpu::GpuMat* distance, 
   int k, const cv::gpu::GpuMat* mask)
{
   cv::gpu::GpuMat allDist;
   if (mask)
      matcher->knnMatch(*queryDescs, *trainDescs, *trainIdx, *distance, allDist, k, *mask);
   else
      matcher->knnMatch(*queryDescs, *trainDescs, *trainIdx, *distance, allDist, k);
}