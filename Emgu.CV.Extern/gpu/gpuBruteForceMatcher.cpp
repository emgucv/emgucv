//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
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
   *matcher = 0;
}

void gpuBruteForceMatcherAdd(cv::gpu::BruteForceMatcher_GPU_base* matcher, const cv::gpu::GpuMat* trainDescs)
{
   std::vector< cv::gpu::GpuMat > mats;
   mats.push_back( *trainDescs );
   matcher->add(mats);
}

void gpuBruteForceMatcherKnnMatchSingle(
                                  cv::gpu::BruteForceMatcher_GPU_base* matcher,
                                  const cv::gpu::GpuMat* queryDescs, const cv::gpu::GpuMat* trainDescs,
                                  cv::gpu::GpuMat* trainIdx, cv::gpu::GpuMat* distance, 
                                  int k, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream)
{
   cv::gpu::GpuMat emptyMat;
   mask = mask ? mask : &emptyMat;

   if (k == 2)
   {  //special case for k == 2;
      cv::gpu::GpuMat idxMat = trainIdx->reshape(2, 1);
      cv::gpu::GpuMat distMat = distance->reshape(2, 1);
      matcher->knnMatchSingle(*queryDescs, *trainDescs, 
         idxMat, distMat, 
         emptyMat, k, *mask,
         stream ? *stream : cv::gpu::Stream::Null());
      CV_Assert(idxMat.channels() == 2);
      CV_Assert(distMat.channels() == 2);
      CV_Assert(idxMat.data == trainIdx->data);
      CV_Assert(distMat.data == distance->data);
   } else
      matcher->knnMatchSingle(*queryDescs, *trainDescs, *trainIdx, *distance, emptyMat, k, *mask, stream ? *stream : cv::gpu::Stream::Null());
}
