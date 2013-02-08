//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gpu_c.h"

cv::gpu::FAST_GPU* gpuFASTDetectorCreate(int threshold, bool nonmaxSupression, double keypointsRatio)
{
   return new cv::gpu::FAST_GPU(threshold, nonmaxSupression, keypointsRatio);
}

void gpuFASTDetectorRelease(cv::gpu::FAST_GPU** detector)
{
   delete *detector;
   *detector = 0;
}

void gpuFASTDetectorDetectKeyPoints(cv::gpu::FAST_GPU* detector, const cv::gpu::GpuMat* img, const cv::gpu::GpuMat* mask, cv::gpu::GpuMat* keypoints)
{
   (*detector)(*img, mask ? *mask : cv::gpu::GpuMat(), *keypoints);
}

void gpuFASTDownloadKeypoints(cv::gpu::FAST_GPU* detector, cv::gpu::GpuMat* keypointsGPU, std::vector<cv::KeyPoint>* keypoints)
{
   detector->downloadKeypoints(*keypointsGPU, *keypoints);
}