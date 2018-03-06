//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudafeatures2d_c.h"

cv::cuda::DescriptorMatcher* cveCudaDescriptorMatcherCreateBFMatcher(int distType, cv::Algorithm** algorithm)
{
   cv::Ptr<cv::cuda::DescriptorMatcher> ptr = cv::cuda::DescriptorMatcher::createBFMatcher(distType);
   ptr.addref();
   cv::cuda::DescriptorMatcher* matcher = ptr.get();
   *algorithm = static_cast<cv::Algorithm*>(matcher);
   return matcher;
}

void cveCudaDescriptorMatcherRelease(cv::cuda::DescriptorMatcher** matcher)
{
   delete *matcher;
   *matcher = 0;
}

void cveCudaDescriptorMatcherAdd(cv::cuda::DescriptorMatcher* matcher, const std::vector<cv::cuda::GpuMat>* trainDescs)
{
   matcher->add(*trainDescs);
}

void cveCudaDescriptorMatcherKnnMatch(
   cv::cuda::DescriptorMatcher* matcher,
   cv::_InputArray* queryDescs, cv::_InputArray* trainDescs,
   std::vector< std::vector< cv::DMatch > >* matches,
   int k, cv::_InputArray* masks, bool compactResult)
{
   matcher->knnMatch(*queryDescs, *trainDescs, *matches, k, masks ? *masks : (cv::_InputArray) cv::noArray(), compactResult);
}

//----------------------------------------------------------------------------
//
//  Feature2dAsync
//
//----------------------------------------------------------------------------
void cveCudaFeature2dAsyncDetectAsync(
   cv::cuda::Feature2DAsync* feature2d,
   cv::_InputArray* image,
   cv::_OutputArray* keypoints,
   cv::_InputArray* mask,
   cv::cuda::Stream* stream)
{
  feature2d->detectAsync(*image, *keypoints, mask ? *mask : (cv::InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}

void cveCudaFeature2dAsyncComputeAsync(
   cv::cuda::Feature2DAsync* feature2d,
   cv::_InputArray* image,
   cv::_OutputArray* keypoints,
   cv::_OutputArray* descriptors,
   cv::cuda::Stream* stream)
{
   feature2d->computeAsync(*image, *keypoints, *descriptors, stream ? *stream : cv::cuda::Stream::Null());
}

void cveCudaFeature2dAsyncDetectAndComputeAsync(
   cv::cuda::Feature2DAsync* feature2d,
   cv::_InputArray* image,
   cv::_InputArray* mask,
   cv::_OutputArray* keypoints,
   cv::_OutputArray* descriptors,
   bool useProvidedKeypoints,
   cv::cuda::Stream* stream)
{
  feature2d->detectAndComputeAsync(*image, mask ? *mask : (cv::InputArray) cv::noArray(), *keypoints, *descriptors, useProvidedKeypoints, stream ? *stream : cv::cuda::Stream::Null());
}

void cveCudaFeature2dAsyncConvert(
   cv::cuda::Feature2DAsync* feature2d,
   cv::_InputArray* gpu_keypoints,
   std::vector<cv::KeyPoint>* keypoints)
{
   feature2d->convert(*gpu_keypoints, *keypoints);
}

//----------------------------------------------------------------------------
//
//  CudaFastFeatureDetector
//
//----------------------------------------------------------------------------

cv::cuda::FastFeatureDetector* cveCudaFastFeatureDetectorCreate(int threshold, bool nonmaxSupression, int type, int maxPoints, cv::Feature2D** feature2D, cv::cuda::Feature2DAsync** feature2dAsync)
{
   cv::Ptr<cv::cuda::FastFeatureDetector> ptr = cv::cuda::FastFeatureDetector::create(threshold, nonmaxSupression, type, maxPoints);
   ptr.addref();
   cv::cuda::FastFeatureDetector* detector = ptr.get();
   *feature2D = static_cast<cv::Feature2D*>(detector);
   *feature2dAsync = static_cast<cv::cuda::Feature2DAsync*>(detector);
   return detector;
}

void cveCudaFastFeatureDetectorRelease(cv::cuda::FastFeatureDetector** detector)
{
   delete *detector;
   *detector = 0;
}

//----------------------------------------------------------------------------
//
//  CudaORB
//
//----------------------------------------------------------------------------
cv::cuda::ORB* cveCudaORBCreate(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTA_K, int scoreType, int patchSize, int fastThreshold, bool blurForDescriptor, cv::Feature2D** feature2D, cv::cuda::Feature2DAsync** feature2dAsync)
{
   cv::Ptr<cv::cuda::ORB> ptr = cv::cuda::ORB::create(numberOfFeatures, scaleFactor, nLevels, edgeThreshold, firstLevel, WTA_K, scoreType, patchSize, fastThreshold, blurForDescriptor);
   ptr.addref();
   cv::cuda::ORB* detector = ptr.get();
   *feature2D = static_cast<cv::Feature2D*>(detector);
   *feature2dAsync = static_cast<cv::cuda::Feature2DAsync*>(detector);
   return detector;
}

void cveCudaORBRelease(cv::cuda::ORB** detector)
{
   delete *detector;
   *detector = 0;
}
