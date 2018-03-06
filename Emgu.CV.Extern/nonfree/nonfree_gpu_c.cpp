//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "nonfree_gpu_c.h"
#include "nonfree_c.h"

cv::cuda::SURF_CUDA* cudaSURFDetectorCreate(double _hessianThreshold, int _nOctaves, int _nOctaveLayers, bool _extended, float _keypointsRatio, bool _upright)
{
   return new cv::cuda::SURF_CUDA(_hessianThreshold, _nOctaves, _nOctaveLayers, _extended, _keypointsRatio, _upright);
}

void cudaSURFDetectorRelease(cv::cuda::SURF_CUDA** detector)
{
   delete *detector;
   *detector = 0;
}

void cudaSURFDetectorDetectKeyPoints(cv::cuda::SURF_CUDA* detector, const cv::cuda::GpuMat* img, const cv::cuda::GpuMat* mask, cv::cuda::GpuMat* keypoints)
{
   (*detector)(*img, mask ? *mask : cv::cuda::GpuMat() , *keypoints);
}

void cudaSURFDownloadKeypoints(cv::cuda::SURF_CUDA* detector, const cv::cuda::GpuMat* keypointsGPU, std::vector<cv::KeyPoint>* keypoints)
{
   detector->downloadKeypoints(*keypointsGPU, *keypoints);
}

void cudaSURFUploadKeypoints(cv::cuda::SURF_CUDA* detector, const std::vector<cv::KeyPoint>* keypoints, cv::cuda::GpuMat* keypointsGPU)
{
   detector->uploadKeypoints(*keypoints, *keypointsGPU);
}

void cudaSURFDetectorCompute(
   cv::cuda::SURF_CUDA* detector, 
   const cv::cuda::GpuMat* img, 
   const cv::cuda::GpuMat* mask, 
   cv::cuda::GpuMat* keypoints, 
   cv::cuda::GpuMat* descriptors, 
   bool useProvidedKeypoints)
{
   (*detector)(
      *img, 
      mask? *mask : cv::cuda::GpuMat(), 
      *keypoints,
      *descriptors,
      useProvidedKeypoints);
}

int cudaSURFDetectorGetDescriptorSize(cv::cuda::SURF_CUDA* detector)
{
   return detector->descriptorSize();
}