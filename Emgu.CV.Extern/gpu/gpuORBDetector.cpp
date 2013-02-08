//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gpu_c.h"

cv::gpu::ORB_GPU* gpuORBDetectorCreate(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTA_K, int scoreType, int patchSize)
{
   return new cv::gpu::ORB_GPU(numberOfFeatures, scaleFactor, nLevels, edgeThreshold, firstLevel, WTA_K, scoreType, patchSize);
}

void gpuORBDetectorRelease(cv::gpu::ORB_GPU** detector)
{
   delete *detector;
   *detector = 0;
}

void gpuORBDetectorDetectKeyPoints(cv::gpu::ORB_GPU* detector, const cv::gpu::GpuMat* img, const cv::gpu::GpuMat* mask, cv::gpu::GpuMat* keypoints)
{
   (*detector)(*img, mask ? *mask : cv::gpu::GpuMat() , *keypoints);
}

void gpuORBDownloadKeypoints(cv::gpu::ORB_GPU* detector, cv::gpu::GpuMat* keypointsGPU, std::vector<cv::KeyPoint>* keypoints)
{   
   detector->downloadKeyPoints(*keypointsGPU, *keypoints);
}

void gpuORBDetectorCompute(
   cv::gpu::ORB_GPU* detector, 
   const cv::gpu::GpuMat* img, 
   const cv::gpu::GpuMat* mask, 
   cv::gpu::GpuMat* keypoints, 
   cv::gpu::GpuMat* descriptors)
{
     (*detector)(
      *img, 
      mask? *mask : cv::gpu::GpuMat(), 
      *keypoints,
      *descriptors);
}

int gpuORBDetectorGetDescriptorSize(cv::gpu::ORB_GPU* detector)
{
   return detector->descriptorSize();
}