//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gpu_c.h"

cv::gpu::SURF_GPU* gpuSURFDetectorCreate(double _hessianThreshold, int _nOctaves, int _nOctaveLayers, bool _extended, float _keypointsRatio, bool _upright)
{
   return new cv::gpu::SURF_GPU(_hessianThreshold, _nOctaves, _nOctaveLayers, _extended, _keypointsRatio, _upright);
}

void gpuSURFDetectorRelease(cv::gpu::SURF_GPU** detector)
{
   delete *detector;
}

void gpuSURFDetectorDetectKeyPoints(cv::gpu::SURF_GPU* detector, const cv::gpu::GpuMat* img, const cv::gpu::GpuMat* mask, cv::gpu::GpuMat* keypoints)
{
   (*detector)(*img, mask ? *mask : cv::gpu::GpuMat() , *keypoints);
}

void gpuDownloadKeypoints(cv::gpu::SURF_GPU* detector, const cv::gpu::GpuMat* keypointsGPU, std::vector<cv::KeyPoint>* keypoints)
{
   detector->downloadKeypoints(*keypointsGPU, *keypoints);
}

void gpuUploadKeypoints(cv::gpu::SURF_GPU* detector, const std::vector<cv::KeyPoint>* keypoints, cv::gpu::GpuMat* keypointsGPU)
{
   detector->uploadKeypoints(*keypoints, *keypointsGPU);
}

void gpuSURFDetectorCompute(
   cv::gpu::SURF_GPU* detector, 
   const cv::gpu::GpuMat* img, 
   const cv::gpu::GpuMat* mask, 
   cv::gpu::GpuMat* keypoints, 
   cv::gpu::GpuMat* descriptors, 
   bool useProvidedKeypoints)
{
   (*detector)(
      *img, 
      mask? *mask : cv::gpu::GpuMat(), 
      *keypoints,
      *descriptors,
      useProvidedKeypoints);
}

int gpuSURFDetectorGetDescriptorSize(cv::gpu::SURF_GPU* detector)
{
   return detector->descriptorSize();
}