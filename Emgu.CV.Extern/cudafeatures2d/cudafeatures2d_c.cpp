//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudafeatures2d_c.h"

cv::cuda::DescriptorMatcher* cveCudaDescriptorMatcherCreateBFMatcher(int distType, cv::Algorithm** algorithm, cv::Ptr<cv::cuda::DescriptorMatcher>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	cv::Ptr<cv::cuda::DescriptorMatcher> ptr = cv::cuda::DescriptorMatcher::createBFMatcher(distType);
	*sharedPtr = new cv::Ptr<cv::cuda::DescriptorMatcher>(ptr);
	cv::cuda::DescriptorMatcher* matcher = ptr.get();
	*algorithm = dynamic_cast<cv::Algorithm*>(matcher);
	return matcher;
#else
	throw_no_cudafeature2d();
#endif
}

void cveCudaDescriptorMatcherRelease(cv::Ptr<cv::cuda::DescriptorMatcher>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_cudafeature2d();
#endif
}

void cveCudaDescriptorMatcherAdd(cv::cuda::DescriptorMatcher* matcher, const std::vector<cv::cuda::GpuMat>* trainDescs)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	matcher->add(*trainDescs);
#else
	throw_no_cudafeature2d();
#endif
}

bool cveCudaDescriptorMatcherIsMaskSupported(cv::cuda::DescriptorMatcher* matcher)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	return matcher->isMaskSupported();
#else
	throw_no_cudafeature2d();
#endif
}
void cveCudaDescriptorMatcherClear(cv::cuda::DescriptorMatcher* matcher)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	return matcher->clear();
#else
	throw_no_cudafeature2d();
#endif
}
bool cveCudaDescriptorMatcherEmpty(cv::cuda::DescriptorMatcher* matcher)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	return matcher->empty();
#else
	throw_no_cudafeature2d();
#endif
}
void cveCudaDescriptorMatcherTrain(cv::cuda::DescriptorMatcher* matcher)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	return matcher->train();
#else
	throw_no_cudafeature2d();
#endif
}
void cveCudaDescriptorMatcherMatch1(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_InputArray* trainDescriptors,
	std::vector< cv::DMatch >* matches,
	cv::_InputArray* mask)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	matcher->match(
		*queryDescriptors,
		*trainDescriptors,
		*matches,
		mask ? *mask : (cv::InputArray) cv::noArray());
#else
	throw_no_cudafeature2d();
#endif
}
void cveCudaDescriptorMatcherMatch2(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	std::vector< cv::DMatch >* matches,
	std::vector< cv::cuda::GpuMat >* masks)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	matcher->match(
		*queryDescriptors,
		*matches,
		masks ? *masks : std::vector< cv::cuda::GpuMat >());
#else
	throw_no_cudafeature2d();
#endif
}

void cveCudaDescriptorMatcherMatchAsync1(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_InputArray* trainDescriptors,
	cv::_OutputArray* matches,
	cv::_InputArray* mask,
	cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	matcher->matchAsync(
		*queryDescriptors,
		*trainDescriptors,
		*matches,
		mask ? *mask : (cv::InputArray) cv::noArray(),
		stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudafeature2d();
#endif
}

void cveCudaDescriptorMatcherMatchAsync2(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_OutputArray* matches,
	std::vector< cv::cuda::GpuMat >* masks,
	cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	matcher->matchAsync(
		*queryDescriptors,
		*matches,
		masks ? *masks : std::vector< cv::cuda::GpuMat >(),
		stream ? *stream : cv::cuda::Stream::Null()
	);
#else
	throw_no_cudafeature2d();
#endif
}
void cveCudaDescriptorMatcherMatchConvert(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* gpuMatches,
	std::vector< cv::DMatch >* matches)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	matcher->matchConvert(*gpuMatches, *matches);
#else
	throw_no_cudafeature2d();
#endif
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
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	matcher->knnMatch(*queryDescs, *trainDescs, *matches, k, masks ? *masks : (cv::_InputArray) cv::noArray(), compactResult);
#else
	throw_no_cudafeature2d();
#endif
}

void cveCudaDescriptorMatcherKnnMatch2(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	std::vector< std::vector< cv::DMatch > >* matches,
	int k,
	std::vector< cv::cuda::GpuMat >* masks,
	bool compactResult)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	matcher->knnMatch(
		*queryDescriptors,
		*matches,
		k,
		masks ? *masks : std::vector< cv::cuda::GpuMat >(),
		compactResult);
#else
	throw_no_cudafeature2d();
#endif
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
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	matcher->knnMatchAsync(
		*queryDescriptors,
		*trainDescriptors,
		*matches,
		k,
		mask ? *mask : (cv::InputArray) cv::noArray(),
		stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudafeature2d();
#endif
}

void cveCudaDescriptorMatcherKnnMatchAsync2(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_OutputArray* matches,
	int k,
	std::vector< cv::cuda::GpuMat >* masks,
	cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	matcher->knnMatchAsync(
		*queryDescriptors,
		*matches,
		k,
		masks ? *masks : std::vector< cv::cuda::GpuMat >(),
		stream ? *stream : cv::cuda::Stream::Null()
	);
#else
	throw_no_cudafeature2d();
#endif
}

void cveCudaDescriptorMatcherKnnMatchConvert(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* gpuMatches,
	std::vector< std::vector< cv::DMatch > >* matches,
	bool compactResult)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	matcher->knnMatchConvert(*gpuMatches, *matches, compactResult);
#else
	throw_no_cudafeature2d();
#endif
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
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	matcher->radiusMatch(
		*queryDescriptors,
		*trainDescriptors,
		*matches,
		maxDistance,
		mask ? *mask : (cv::InputArray) cv::noArray(),
		compactResult);
#else
	throw_no_cudafeature2d();
#endif
}

void cveCudaDescriptorMatcherRadiusMatch2(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	std::vector< std::vector< cv::DMatch > >* matches,
	float maxDistance,
	std::vector< cv::cuda::GpuMat >* masks,
	bool compactResult)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	matcher->radiusMatch(
		*queryDescriptors,
		*matches,
		maxDistance,
		masks ? *masks : std::vector< cv::cuda::GpuMat >(),
		compactResult);
#else
	throw_no_cudafeature2d();
#endif
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
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	matcher->radiusMatchAsync(
		*queryDescriptors,
		*trainDescriptors,
		*matches,
		maxDistance,
		mask ? *mask : (cv::InputArray) cv::noArray(),
		stream ? *stream : cv::cuda::Stream::Null()
	);
#else
	throw_no_cudafeature2d();
#endif
}

void cveCudaDescriptorMatcherRadiusMatchAsync2(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_OutputArray* matches,
	float maxDistance,
	std::vector< cv::cuda::GpuMat >* masks,
	cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	matcher->radiusMatchAsync(
		*queryDescriptors,
		*matches,
		maxDistance,
		masks ? *masks : std::vector< cv::cuda::GpuMat >(),
		stream ? *stream : cv::cuda::Stream::Null()
	);
#else
	throw_no_cudafeature2d();
#endif
}

void cveCudaDescriptorMatcherRadiusMatchConvert(
	cv::cuda::DescriptorMatcher* matcher,
	cv::_InputArray* gpu_matches,
	std::vector< std::vector< cv::DMatch > >* matches,
	bool compactResult)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	matcher->radiusMatchConvert(
		*gpu_matches,
		*matches,
		compactResult);
#else
	throw_no_cudafeature2d();
#endif
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
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	feature2d->detectAsync(*image, *keypoints, mask ? *mask : (cv::InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudafeature2d();
#endif
}

void cveCudaFeature2dAsyncComputeAsync(
	cv::cuda::Feature2DAsync* feature2d,
	cv::_InputArray* image,
	cv::_OutputArray* keypoints,
	cv::_OutputArray* descriptors,
	cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	feature2d->computeAsync(*image, *keypoints, *descriptors, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudafeature2d();
#endif
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
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	feature2d->detectAndComputeAsync(*image, mask ? *mask : (cv::InputArray) cv::noArray(), *keypoints, *descriptors, useProvidedKeypoints, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudafeature2d();
#endif
}

void cveCudaFeature2dAsyncConvert(
	cv::cuda::Feature2DAsync* feature2d,
	cv::_InputArray* gpu_keypoints,
	std::vector<cv::KeyPoint>* keypoints)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	feature2d->convert(*gpu_keypoints, *keypoints);
#else
	throw_no_cudafeature2d();
#endif
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
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	cv::Ptr<cv::cuda::FastFeatureDetector> ptr = cv::cuda::FastFeatureDetector::create(threshold, nonmaxSupression, type, maxPoints);
	*sharedPtr = new cv::Ptr<cv::cuda::FastFeatureDetector>(ptr);
	cv::cuda::FastFeatureDetector* detector = ptr.get();
	*feature2D = dynamic_cast<cv::Feature2D*>(detector);
	*feature2dAsync = dynamic_cast<cv::cuda::Feature2DAsync*>(detector);
	return detector;
#else
	throw_no_cudafeature2d();
#endif
}

void cveCudaFastFeatureDetectorRelease(cv::Ptr<cv::cuda::FastFeatureDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_cudafeature2d();
#endif
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
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	cv::Ptr<cv::cuda::ORB> ptr = cv::cuda::ORB::create(numberOfFeatures, scaleFactor, nLevels, edgeThreshold, firstLevel, WTA_K, scoreType, patchSize, fastThreshold, blurForDescriptor);
	*sharedPtr = new cv::Ptr<cv::cuda::ORB>(ptr);
	cv::cuda::ORB* detector = ptr.get();
	*feature2D = dynamic_cast<cv::Feature2D*>(detector);
	*feature2dAsync = dynamic_cast<cv::cuda::Feature2DAsync*>(detector);
	return detector;
#else
	throw_no_cudafeature2d();
#endif
}

void cveCudaORBRelease(cv::Ptr<cv::cuda::ORB>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_cudafeature2d();
#endif
}
