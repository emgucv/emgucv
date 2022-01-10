//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "nonfree_gpu_c.h"
#include "nonfree_c.h"

cv::cuda::SURF_CUDA* cudaSURFDetectorCreate(double _hessianThreshold, int _nOctaves, int _nOctaveLayers, bool _extended, float _keypointsRatio, bool _upright)
{
#ifdef HAVE_OPENCV_XFEATURES2D
   return new cv::cuda::SURF_CUDA(_hessianThreshold, _nOctaves, _nOctaveLayers, _extended, _keypointsRatio, _upright);
#else
    throw_no_xfeatures2d();
#endif
}

void cudaSURFDetectorRelease(cv::cuda::SURF_CUDA** detector)
{
#ifdef HAVE_OPENCV_XFEATURES2D
   delete *detector;
   *detector = 0;
#else
    throw_no_xfeatures2d();
#endif
}

void cudaSURFDetectorDetectKeyPoints(cv::cuda::SURF_CUDA* detector, const cv::cuda::GpuMat* img, const cv::cuda::GpuMat* mask, cv::cuda::GpuMat* keypoints)
{
#ifdef HAVE_OPENCV_XFEATURES2D
   (*detector)(*img, mask ? *mask : cv::cuda::GpuMat() , *keypoints);
#else
    throw_no_xfeatures2d();
#endif
}

void cudaSURFDownloadKeypoints(cv::cuda::SURF_CUDA* detector, const cv::cuda::GpuMat* keypointsGPU, std::vector<cv::KeyPoint>* keypoints)
{
#ifdef HAVE_OPENCV_XFEATURES2D
   detector->downloadKeypoints(*keypointsGPU, *keypoints);
#else
    throw_no_xfeatures2d();
#endif
}

void cudaSURFUploadKeypoints(cv::cuda::SURF_CUDA* detector, const std::vector<cv::KeyPoint>* keypoints, cv::cuda::GpuMat* keypointsGPU)
{
#ifdef HAVE_OPENCV_XFEATURES2D
   detector->uploadKeypoints(*keypoints, *keypointsGPU);
#else
    throw_no_xfeatures2d();
#endif
}

void cudaSURFDetectorCompute(
   cv::cuda::SURF_CUDA* detector, 
   const cv::cuda::GpuMat* img, 
   const cv::cuda::GpuMat* mask, 
   cv::cuda::GpuMat* keypoints, 
   cv::cuda::GpuMat* descriptors, 
   bool useProvidedKeypoints)
{
#ifdef HAVE_OPENCV_XFEATURES2D
   (*detector)(
      *img, 
      mask? *mask : cv::cuda::GpuMat(), 
      *keypoints,
      *descriptors,
      useProvidedKeypoints);
#else
    throw_no_xfeatures2d();
#endif
}

int cudaSURFDetectorGetDescriptorSize(cv::cuda::SURF_CUDA* detector)
{
#ifdef HAVE_OPENCV_XFEATURES2D
   return detector->descriptorSize();
#else
    throw_no_xfeatures2d();
#endif
}