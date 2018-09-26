//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudafeatures2d_c.h"

cv::cuda::DescriptorMatcher* cveCudaDescriptorMatcherCreateBFMatcher(int distType, cv::Algorithm** algorithm, cv::Ptr<cv::cuda::DescriptorMatcher>** sharedPtr)
{
	cv::Ptr<cv::cuda::DescriptorMatcher> ptr = cv::cuda::DescriptorMatcher::createBFMatcher(distType);
	*sharedPtr = new cv::Ptr<cv::cuda::DescriptorMatcher>(ptr);
	cv::cuda::DescriptorMatcher* matcher = ptr.get();
	*algorithm = static_cast<cv::Algorithm*>(matcher);
	return matcher;
}

void cveCudaDescriptorMatcherRelease(cv::Ptr<cv::cuda::DescriptorMatcher>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
}

void cveCudaDescriptorMatcherAdd(cv::cuda::DescriptorMatcher* matcher, const std::vector<cv::cuda::GpuMat>* trainDescs)
{
	matcher->add(*trainDescs);
}

bool cveCudaDescriptorMatcherIsMaskSupported(cv::cuda::DescriptorMatcher* matcher)
{
	return matcher->isMaskSupported();
}
void cveCudaDescriptorMatcherClear(cv::cuda::DescriptorMatcher* matcher)
{
	return matcher->clear();
}
bool cveCudaDescriptorMatcherEmpty(cv::cuda::DescriptorMatcher* matcher)
{
	return matcher->empty();
}
void cveCudaDescriptorMatcherTrain(cv::cuda::DescriptorMatcher* matcher)
{
	return matcher->train();
}
void cveCudaDescriptorMatcherMatch1(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_InputArray* trainDescriptors,
	std::vector< cv::DMatch >* matches,
	cv::_InputArray* mask)
{
	matcher->match(
		*queryDescriptors,
		*trainDescriptors,
		*matches,
		mask ? *mask : (cv::InputArray) cv::noArray());
}
void cveCudaDescriptorMatcherMatch2(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	std::vector< cv::DMatch >* matches,
	std::vector< cv::cuda::GpuMat >* masks)
{
	matcher->match(
		*queryDescriptors,
		*matches,
		masks ? *masks : std::vector< cv::cuda::GpuMat >());
}

void cveCudaDescriptorMatcherMatchAsync1(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_InputArray* trainDescriptors,
	cv::_OutputArray* matches,
	cv::_InputArray* mask,
	cv::cuda::Stream* stream)
{
	matcher->matchAsync(
		*queryDescriptors,
		*trainDescriptors,
		*matches,
		mask ? *mask : (cv::InputArray) cv::noArray(),
		stream ? *stream : cv::cuda::Stream::Null());
}

void cveCudaDescriptorMatcherMatchAsync2(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_OutputArray* matches,
	std::vector< cv::cuda::GpuMat >* masks,
	cv::cuda::Stream* stream)
{
	matcher->matchAsync(
		*queryDescriptors,
		*matches,
		masks ? *masks : std::vector< cv::cuda::GpuMat >(),
		stream ? *stream : cv::cuda::Stream::Null()
	);
}
void cveCudaDescriptorMatcherMatchConvert(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* gpuMatches,
	std::vector< cv::DMatch >* matches)
{
	matcher->matchConvert(*gpuMatches, *matches);
}

void cveCudaDescriptorMatcherKnnMatch1(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescs,
	cv::_InputArray* trainDescs,
	std::vector< std::vector< cv::DMatch > >* matches,
	int k,
	cv::_InputArray* masks,
	bool compactResult)
{
	matcher->knnMatch(*queryDescs, *trainDescs, *matches, k, masks ? *masks : (cv::_InputArray) cv::noArray(), compactResult);
}

void cveCudaDescriptorMatcherKnnMatch2(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	std::vector< std::vector< cv::DMatch > >* matches,
	int k,
	std::vector< cv::cuda::GpuMat >* masks,
	bool compactResult)
{
	matcher->knnMatch(
		*queryDescriptors,
		*matches,
		k,
		masks ? *masks : std::vector< cv::cuda::GpuMat >(),
		compactResult);
}

void cveCudaDescriptorMatcherKnnMatchAsync1(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_InputArray* trainDescriptors,
	cv::_OutputArray* matches,
	int k,
	cv::_InputArray* mask,
	cv::cuda::Stream* stream)
{
	matcher->knnMatchAsync(
		*queryDescriptors,
		*trainDescriptors,
		*matches,
		k,
		mask ? *mask : (cv::InputArray) cv::noArray(),
		stream ? *stream : cv::cuda::Stream::Null());
}

void cveCudaDescriptorMatcherKnnMatchAsync2(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_OutputArray* matches,
	int k,
	std::vector< cv::cuda::GpuMat >* masks,
	cv::cuda::Stream* stream)
{
	matcher->knnMatchAsync(
		*queryDescriptors,
		*matches,
		k,
		masks ? *masks : std::vector< cv::cuda::GpuMat >(),
		stream ? *stream : cv::cuda::Stream::Null()
	);
}

void cveCudaDescriptorMatcherKnnMatchConvert(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* gpuMatches,
	std::vector< std::vector< cv::DMatch > >* matches,
	bool compactResult)
{
	matcher->knnMatchConvert(*gpuMatches, *matches, compactResult);
}

void cveCudaDescriptorMatcherRadiusMatch1(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_InputArray* trainDescriptors,
	std::vector< std::vector< cv::DMatch > >* matches,
	float maxDistance,
	cv::_InputArray* mask,
	bool compactResult)
{
	matcher->radiusMatch(
		*queryDescriptors,
		*trainDescriptors,
		*matches,
		maxDistance,
		mask ? *mask : (cv::InputArray) cv::noArray(),
		compactResult);
}

void cveCudaDescriptorMatcherRadiusMatch2(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	std::vector< std::vector< cv::DMatch > >* matches,
	float maxDistance,
	std::vector< cv::cuda::GpuMat >* masks,
	bool compactResult)
{
	matcher->radiusMatch(
		*queryDescriptors,
		*matches,
		maxDistance,
		masks ? *masks : std::vector< cv::cuda::GpuMat >(),
		compactResult);
}

void cveCudaDescriptorMatcherRadiusMatchAsync1(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_InputArray* trainDescriptors,
	cv::_OutputArray* matches,
	float maxDistance,
	cv::_InputArray* mask,
	cv::cuda::Stream* stream)
{
	matcher->radiusMatchAsync(
		*queryDescriptors,
		*trainDescriptors,
		*matches,
		maxDistance,
		mask ? *mask : (cv::InputArray) cv::noArray(),
		stream ? *stream : cv::cuda::Stream::Null()
	);
}

void cveCudaDescriptorMatcherRadiusMatchAsync2(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_OutputArray* matches,
	float maxDistance,
	std::vector< cv::cuda::GpuMat >* masks,
	cv::cuda::Stream* stream)
{
	matcher->radiusMatchAsync(
		*queryDescriptors,
		*matches,
		maxDistance,
		masks ? *masks : std::vector< cv::cuda::GpuMat >(),
		stream ? *stream : cv::cuda::Stream::Null()
	);
}

void cveCudaDescriptorMatcherRadiusMatchConvert(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* gpu_matches,
	std::vector< std::vector< cv::DMatch > >* matches,
	bool compactResult)
{
	matcher->radiusMatchConvert(
		*gpu_matches,
		*matches,
		compactResult);
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

cv::cuda::FastFeatureDetector* cveCudaFastFeatureDetectorCreate(
	int threshold, bool nonmaxSupression, int type, int maxPoints, 
	cv::Feature2D** feature2D, cv::cuda::Feature2DAsync** feature2dAsync,
	cv::Ptr<cv::cuda::FastFeatureDetector>** sharedPtr)
{
	cv::Ptr<cv::cuda::FastFeatureDetector> ptr = cv::cuda::FastFeatureDetector::create(threshold, nonmaxSupression, type, maxPoints);
	*sharedPtr = new cv::Ptr<cv::cuda::FastFeatureDetector>(ptr);
	cv::cuda::FastFeatureDetector* detector = ptr.get();
	*feature2D = static_cast<cv::Feature2D*>(detector);
	*feature2dAsync = static_cast<cv::cuda::Feature2DAsync*>(detector);
	return detector;
}

void cveCudaFastFeatureDetectorRelease(cv::cuda::FastFeatureDetector** detector, cv::Ptr<cv::cuda::FastFeatureDetector>** sharedPtr)
{
	delete *sharedPtr;
	*detector = 0;
	*sharedPtr = 0;
}

//----------------------------------------------------------------------------
//
//  CudaORB
//
//----------------------------------------------------------------------------
cv::cuda::ORB* cveCudaORBCreate(
	int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTA_K, int scoreType, int patchSize, int fastThreshold, bool blurForDescriptor, 
	cv::Feature2D** feature2D, cv::cuda::Feature2DAsync** feature2dAsync,
	cv::Ptr<cv::cuda::ORB>** sharedPtr)
{
	cv::Ptr<cv::cuda::ORB> ptr = cv::cuda::ORB::create(numberOfFeatures, scaleFactor, nLevels, edgeThreshold, firstLevel, WTA_K, scoreType, patchSize, fastThreshold, blurForDescriptor);
	*sharedPtr = new cv::Ptr<cv::cuda::ORB>(ptr);
	cv::cuda::ORB* detector = ptr.get();
	*feature2D = static_cast<cv::Feature2D*>(detector);
	*feature2dAsync = static_cast<cv::cuda::Feature2DAsync*>(detector);
	return detector;
}

void cveCudaORBRelease(cv::cuda::ORB** detector, cv::Ptr<cv::cuda::ORB>** sharedPtr)
{
	delete *sharedPtr;
	*detector = 0;
	*sharedPtr = 0;
}
