//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudafeatures2d_c.h"


cv::cuda::BFMatcher_CUDA* cudaBruteForceMatcherCreate(int distType) 
{
   return new cv::cuda::BFMatcher_CUDA(distType);
}

void cudaBruteForceMatcherRelease(cv::cuda::BFMatcher_CUDA** matcher) 
{
   delete *matcher;
   *matcher = 0;
}

void cudaBruteForceMatcherAdd(cv::cuda::BFMatcher_CUDA* matcher, const cv::cuda::GpuMat* trainDescs)
{
   std::vector< cv::cuda::GpuMat > mats;
   mats.push_back( *trainDescs );
   matcher->add(mats);
}

void cudaBruteForceMatcherKnnMatch(
                                  cv::cuda::BFMatcher_CUDA* matcher,
                                  const cv::cuda::GpuMat* queryDescs, const cv::cuda::GpuMat* trainDescs,
                                  std::vector< std::vector< cv::DMatch > >* matches, 
                                  int k, const cv::cuda::GpuMat* mask, bool compactResult)
{
   matcher->knnMatch(*queryDescs, *trainDescs, *matches, k, mask ? * mask : cv::cuda::GpuMat(), compactResult);
}

//----------------------------------------------------------------------------
//
//  GpuFASTDetector
//
//----------------------------------------------------------------------------

cv::cuda::FAST_CUDA* cudaFASTDetectorCreate(int threshold, bool nonmaxSupression, double keypointsRatio)
{
   return new cv::cuda::FAST_CUDA(threshold, nonmaxSupression, keypointsRatio);
}

void cudaFASTDetectorRelease(cv::cuda::FAST_CUDA** detector)
{
   delete *detector;
   *detector = 0;
}

void cudaFASTDetectorDetectKeyPoints(cv::cuda::FAST_CUDA* detector, const cv::cuda::GpuMat* img, const cv::cuda::GpuMat* mask, cv::cuda::GpuMat* keypoints)
{
   (*detector)(*img, mask ? *mask : cv::cuda::GpuMat(), *keypoints);
}

void cudaFASTDownloadKeypoints(cv::cuda::FAST_CUDA* detector, cv::cuda::GpuMat* keypointsGPU, std::vector<cv::KeyPoint>* keypoints)
{
   detector->downloadKeypoints(*keypointsGPU, *keypoints);
}


//----------------------------------------------------------------------------
//
//  GpuORBDetector
//
//----------------------------------------------------------------------------
cv::cuda::ORB_CUDA* cudaORBDetectorCreate(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTA_K, int scoreType, int patchSize)
{
   return new cv::cuda::ORB_CUDA(numberOfFeatures, scaleFactor, nLevels, edgeThreshold, firstLevel, WTA_K, scoreType, patchSize);
}

void cudaORBDetectorRelease(cv::cuda::ORB_CUDA** detector)
{
   delete *detector;
   *detector = 0;
}

void cudaORBDetectorDetectKeyPoints(cv::cuda::ORB_CUDA* detector, const cv::cuda::GpuMat* img, const cv::cuda::GpuMat* mask, cv::cuda::GpuMat* keypoints)
{
   (*detector)(*img, mask ? *mask : cv::cuda::GpuMat() , *keypoints);
}

void cudaORBDownloadKeypoints(cv::cuda::ORB_CUDA* detector, cv::cuda::GpuMat* keypointsGPU, std::vector<cv::KeyPoint>* keypoints)
{   
   detector->downloadKeyPoints(*keypointsGPU, *keypoints);
}

void cudaORBDetectorCompute(
   cv::cuda::ORB_CUDA* detector, 
   const cv::cuda::GpuMat* img, 
   const cv::cuda::GpuMat* mask, 
   cv::cuda::GpuMat* keypoints, 
   cv::cuda::GpuMat* descriptors)
{
     (*detector)(
      *img, 
      mask? *mask : cv::cuda::GpuMat(), 
      *keypoints,
      *descriptors);
}

int cudaORBDetectorGetDescriptorSize(cv::cuda::ORB_CUDA* detector)
{
   return detector->descriptorSize();
}
