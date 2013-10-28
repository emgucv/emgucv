//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
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

void cudaBruteForceMatcherKnnMatchSingle(
                                  cv::cuda::BFMatcher_CUDA* matcher,
                                  const cv::cuda::GpuMat* queryDescs, const cv::cuda::GpuMat* trainDescs,
                                  cv::cuda::GpuMat* trainIdx, cv::cuda::GpuMat* distance, 
                                  int k, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream)
{
   cv::cuda::GpuMat emptyMat;
   mask = mask ? mask : &emptyMat;

   if (k == 2)
   {  //special case for k == 2;
      cv::cuda::GpuMat idxMat = trainIdx->reshape(2, 1);
      cv::cuda::GpuMat distMat = distance->reshape(2, 1);
      matcher->knnMatchSingle(*queryDescs, *trainDescs, 
         idxMat, distMat, 
         emptyMat, k, *mask,
         stream ? *stream : cv::cuda::Stream::Null());
      CV_Assert(idxMat.channels() == 2);
      CV_Assert(distMat.channels() == 2);
      CV_Assert(idxMat.data == trainIdx->data);
      CV_Assert(distMat.data == distance->data);
   } else
      matcher->knnMatchSingle(*queryDescs, *trainDescs, *trainIdx, *distance, emptyMat, k, *mask, stream ? *stream : cv::cuda::Stream::Null());
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
