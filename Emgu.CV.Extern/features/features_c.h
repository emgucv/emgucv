//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_FEATURES_C_H
#define EMGU_FEATURES_C_H

#include "cvapi_compat.h"
#include "opencv2/imgproc/imgproc.hpp"

#ifdef HAVE_OPENCV_FEATURES
#include "opencv2/features/features.hpp"

#include "flann_c.h"
#ifndef HAVE_OPENCV_FLANN
namespace cv
{
	class FlannBasedMatcher {};
}
#endif

#ifndef HAVE_OPENCV_DNN
namespace cv
{
	class DISK {};
}
#endif

#else
static inline CV_NORETURN void throw_no_features() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without features support. To use this module, please switch to the full Emgu CV runtime."); }

namespace cv {
	class ORB {};
	class Feature2D {};
	class BRISK {};
	class FastFeatureDetector {};
	class GFTTDetector {};
	class MSER {};
	class SimpleBlobDetector
	{
	public:
		class Params {};
	};
	class DescriptorMatcher {};
	class BFMatcher {};
	class FlannBasedMatcher {};
	class BOWKMeansTrainer {};
	class BOWImgDescriptorExtractor {};
	class KAZE {};
	class AKAZE {};
	class AgastFeatureDetector {};
	class SIFT {};
	class ALIKED {};
	class DISK {};
	class LightGlueMatcher {};
	class AffineFeature {};
	class ANNIndex {};
		
	namespace flann
	{
		class IndexParams {};
		class SearchParams {};
	}
}
#endif

#include "vectors_c.h"

//ORB
CVAPI(cv::ORB*) cveOrbCreate(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTA_K, int scoreType, int patchSize, int fastThreshold, cv::Feature2D** feature2D, cv::Ptr<cv::ORB>** sharedPtr);
CVAPI(void) cveOrbRelease(cv::Ptr<cv::ORB>** sharedPtr);



//FAST algorithm
CVAPI(cv::FastFeatureDetector*) cveFASTFeatureDetectorCreate(int threshold, bool nonmax_supression, int type, cv::Feature2D** feature2D, cv::Ptr<cv::FastFeatureDetector>** sharedPtr);
CVAPI(void) cveFASTFeatureDetectorRelease(cv::Ptr<cv::FastFeatureDetector>** sharedPtr);

//GFTT
CVAPI(cv::GFTTDetector*) cveGFTTDetectorCreate(int maxCorners, double qualityLevel, double minDistance, int blockSize, bool useHarrisDetector, double k, cv::Feature2D** feature2D, cv::Ptr<cv::GFTTDetector>** sharedPtr);
CVAPI(void) cveGFTTDetectorRelease(cv::Ptr<cv::GFTTDetector>** sharedPtr);

// MSER detector
CVAPI(cv::MSER*) cveMserCreate(
	int delta,
	int minArea,
	int maxArea,
	double maxVariation,
	double minDiversity,
	int maxEvolution,
	double areaThreshold,
	double minMargin,
	int edgeBlurSize,
	cv::Feature2D** feature2D,
	cv::Ptr<cv::MSER>** sharedPtr);
CVAPI(void) cveMserDetectRegions(
	cv::MSER* mserPtr,
	cv::_InputArray* image,
	std::vector< std::vector<cv::Point> >* msers,
	std::vector< cv::Rect >* bboxes);
CVAPI(void) cveMserRelease(cv::Ptr<cv::MSER>** sharedPtr);


// SimpleBlobDetector
CVAPI(cv::SimpleBlobDetector*) cveSimpleBlobDetectorCreate(cv::Feature2D** feature2DPtr, cv::Ptr<cv::SimpleBlobDetector>** sharedPtr);
CVAPI(cv::SimpleBlobDetector*) cveSimpleBlobDetectorCreateWithParams(cv::Feature2D** feature2DPtr, cv::SimpleBlobDetector::Params* params, cv::Ptr<cv::SimpleBlobDetector>** sharedPtr);
CVAPI(void) cveSimpleBlobDetectorRelease(cv::Ptr<cv::SimpleBlobDetector>** sharedPtr);
CVAPI(cv::SimpleBlobDetector::Params*) cveSimpleBlobDetectorParamsCreate();
CVAPI(void) cveSimpleBlobDetectorParamsRelease(cv::SimpleBlobDetector::Params** params);
CVAPI(const std::vector< std::vector< cv::Point > >*) cveSimpleBlobDetectorGetBlobContours(cv::SimpleBlobDetector* detector);

// Draw keypoints.
CVAPI(void) cveDrawKeypoints(
	cv::_InputArray* image,
	const std::vector<cv::KeyPoint>* keypoints,
	cv::_InputOutputArray* outImage,
	const cv::Scalar* color,
	int flags);

// Draws matches of keypoints from two images on output image.
CVAPI(void) cveDrawMatchedFeatures1(
	cv::_InputArray* img1,
	const std::vector<cv::KeyPoint>* keypoints1,
	cv::_InputArray* img2,
	const std::vector<cv::KeyPoint>* keypoints2,
	std::vector< cv::DMatch >* matches,
	cv::_InputOutputArray* outImg,
	const cv::Scalar* matchColor,
	const cv::Scalar* singlePointColor,
	std::vector< unsigned char >* matchesMask,
	int flags);

CVAPI(void) cveDrawMatchedFeatures2(
	cv::_InputArray* img1,
	const std::vector<cv::KeyPoint>* keypoints1,
	cv::_InputArray* img2,
	const std::vector<cv::KeyPoint>* keypoints2,
	std::vector< std::vector< cv::DMatch > >* matches,
	cv::_InputOutputArray* outImg,
	const cv::Scalar* matchColor,
	const cv::Scalar* singlePointColor,
	std::vector< std::vector< unsigned char > >* matchesMask,
	int flags);

CVAPI(void) cveDrawMatchedFeatures3(
	cv::_InputArray* img1, const std::vector<cv::KeyPoint>* keypoints1,
	cv::_InputArray* img2, const std::vector<cv::KeyPoint>* keypoints2,
	std::vector< std::vector< cv::DMatch > >* matches,
	cv::_InputOutputArray* outImg,
	const cv::Scalar* matchColor, 
	const cv::Scalar* singlePointColor,
	cv::_InputArray* matchesMask,
	int flags);

//DescriptorMatcher
CVAPI(void) cveDescriptorMatcherAdd(cv::DescriptorMatcher* matcher, cv::_InputArray* trainDescriptors);

CVAPI(void) cveDescriptorMatcherKnnMatch1(
	cv::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_InputArray* trainDescriptors,
	std::vector< std::vector< cv::DMatch > >* matches,
	int k,
	cv::_InputArray* mask,
	bool compactResult);

CVAPI(void) cveDescriptorMatcherKnnMatch2(
	cv::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	std::vector< std::vector< cv::DMatch > >* matches,
	int k,
	cv::_InputArray* mask,
	bool compactResult);

CVAPI(cv::Algorithm*) cveDescriptorMatcherGetAlgorithm(cv::DescriptorMatcher* matcher);

CVAPI(void) cveDescriptorMatcherClear(cv::DescriptorMatcher* matcher);
CVAPI(bool) cveDescriptorMatcherEmpty(cv::DescriptorMatcher* matcher);
CVAPI(bool) cveDescriptorMatcherIsMaskSupported(cv::DescriptorMatcher* matcher);
CVAPI(void) cveDescriptorMatcherTrain(cv::DescriptorMatcher* matcher);
CVAPI(void) cveDescriptorMatcherMatch1(
	cv::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_InputArray* trainDescriptors,
	std::vector< cv::DMatch >* matches,
	cv::_InputArray* mask);
CVAPI(void) cveDescriptorMatcherMatch2(
	cv::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	std::vector< cv::DMatch >* matches,
	cv::_InputArray* masks);

CVAPI(void) cveDescriptorMatcherRadiusMatch1(
	cv::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_InputArray* trainDescriptors,
	std::vector< std::vector<cv::DMatch> >* matches,
	float maxDistance,
	cv::_InputArray* mask,
	bool compactResult);
CVAPI(void) cveDescriptorMatcherRadiusMatch2(
	cv::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	std::vector< std::vector<cv::DMatch> >* matches,
	float maxDistance,
	cv::_InputArray* masks,
	bool compactResult);


//BruteForceMatcher
CVAPI(cv::BFMatcher*) cveBFMatcherCreate(int distanceType, bool crossCheck, cv::DescriptorMatcher** m);
CVAPI(void) cveBFMatcherRelease(cv::BFMatcher** matcher);

//FlannBasedMatcher
CVAPI(cv::FlannBasedMatcher*) cveFlannBasedMatcherCreate(cv::flann::IndexParams* indexParams, cv::flann::SearchParams* searchParams, cv::DescriptorMatcher** m);
CVAPI(void) cveFlannBasedMatcherRelease(cv::FlannBasedMatcher** matcher);

//2D Tracker
CVAPI(int) cveVoteForSizeAndOrientation(std::vector<cv::KeyPoint>* modelKeyPoints, std::vector<cv::KeyPoint>* observedKeyPoints, std::vector< std::vector< cv::DMatch > >* matches, cv::Mat* mask, double scaleIncrement, int rotationBins);

//Feature2D
CVAPI(void) cveFeature2DDetectAndCompute(cv::Feature2D* feature2D, cv::_InputArray* image, cv::_InputArray* mask, std::vector<cv::KeyPoint>* keypoints, cv::_OutputArray* descriptors, bool useProvidedKeyPoints);
CVAPI(void) cveFeature2DDetect(cv::Feature2D* feature2D, cv::_InputArray* image, std::vector<cv::KeyPoint>* keypoints, cv::_InputArray* mask);
CVAPI(void) cveFeature2DCompute(cv::Feature2D* feature2D, cv::_InputArray* image, std::vector<cv::KeyPoint>* keypoints, cv::_OutputArray* descriptors);
CVAPI(int) cveFeature2DGetDescriptorSize(cv::Feature2D* feature2D);
CVAPI(cv::Algorithm*) cveFeature2DGetAlgorithm(cv::Feature2D* feature2D);


//SIFTDetector
CVAPI(cv::SIFT*) cveSIFTCreate(
	int nFeatures, int nOctaveLayers,
	double contrastThreshold, double edgeThreshold,
	double sigma, cv::Feature2D** feature2D, cv::Ptr<cv::SIFT>** sharedPtr);
CVAPI(void) cveSIFTRelease(cv::Ptr<cv::SIFT>** sharedPtr);

//ALIKED
CVAPI(cv::ALIKED*) cveALIKEDCreate(
	cv::String* modelPath,
	cv::Size* inputSize,
	bool normalizeDescriptors,
	int engine,
	int backend,
	int target,
	cv::Feature2D** feature2D,
	cv::Ptr<cv::ALIKED>** sharedPtr);
CVAPI(void) cveALIKEDRelease(cv::Ptr<cv::ALIKED>** sharedPtr);

//DISK
CVAPI(cv::DISK*) cveDISKCreate(
	cv::String* modelPath,
	int maxKeypoints,
	float scoreThreshold,
	cv::Size* imageSize,
	int backendId,
	int targetId,
	cv::Feature2D** feature2D,
	cv::Ptr<cv::DISK>** sharedPtr);
CVAPI(void) cveDISKRelease(cv::Ptr<cv::DISK>** sharedPtr);

//LightGlueMatcher
CVAPI(cv::LightGlueMatcher*) cveLightGlueMatcherCreate(
	cv::String* modelPath,
	float scoreThreshold,
	int backend,
	int target,
	cv::DescriptorMatcher** matcher,
	cv::Ptr<cv::LightGlueMatcher>** sharedPtr);
CVAPI(void) cveLightGlueMatcherSetPairInfo(
	cv::LightGlueMatcher* matcher,
	cv::_InputArray* queryKpts,
	cv::_InputArray* trainKpts,
	cv::Size* queryImageSize,
	cv::Size* trainImageSize);
CVAPI(void) cveLightGlueMatcherClearPairInfo(cv::LightGlueMatcher* matcher);
CVAPI(void) cveLightGlueMatcherRelease(cv::Ptr<cv::LightGlueMatcher>** sharedPtr);

//In-memory model create overloads
CVAPI(cv::ALIKED*) cveALIKEDCreateFromMemory(
	std::vector<unsigned char>* modelData,
	cv::Size* inputSize,
	bool normalizeDescriptors,
	int engine,
	int backend,
	int target,
	cv::Feature2D** feature2D,
	cv::Ptr<cv::ALIKED>** sharedPtr);
CVAPI(cv::DISK*) cveDISKCreateFromMemory(
	std::vector<unsigned char>* modelData,
	int maxKeypoints,
	float scoreThreshold,
	cv::Size* imageSize,
	int backendId,
	int targetId,
	cv::Feature2D** feature2D,
	cv::Ptr<cv::DISK>** sharedPtr);
CVAPI(cv::LightGlueMatcher*) cveLightGlueMatcherCreateFromMemory(
	std::vector<unsigned char>* modelData,
	float scoreThreshold,
	int backend,
	int target,
	cv::DescriptorMatcher** matcher,
	cv::Ptr<cv::LightGlueMatcher>** sharedPtr);

//AffineFeature
CVAPI(cv::AffineFeature*) cveAffineFeatureCreate(
	cv::Feature2D* backend,
	int maxTilt,
	int minTilt,
	float tiltStep,
	float rotateStepBase,
	cv::Feature2D** feature2D,
	cv::Ptr<cv::AffineFeature>** sharedPtr);
CVAPI(void) cveAffineFeatureRelease(cv::Ptr<cv::AffineFeature>** sharedPtr);

//ANNIndex
CVAPI(cv::ANNIndex*) cveANNIndexCreate(int dim, int distType, cv::Ptr<cv::ANNIndex>** sharedPtr);
CVAPI(void) cveANNIndexRelease(cv::Ptr<cv::ANNIndex>** sharedPtr);
CVAPI(void) cveANNIndexAddItems(cv::ANNIndex* annIndex, cv::_InputArray* features);
CVAPI(void) cveANNIndexBuild(cv::ANNIndex* annIndex, int trees);
CVAPI(void) cveANNIndexKnnSearch(
	cv::ANNIndex* annIndex,
	cv::_InputArray* query,
	cv::_OutputArray* indices,
	cv::_OutputArray* dists,
	int knn,
	int searchK);
CVAPI(void) cveANNIndexSave(cv::ANNIndex* annIndex, cv::String* filename, bool prefault);
CVAPI(void) cveANNIndexLoad(cv::ANNIndex* annIndex, cv::String* filename, bool prefault);
CVAPI(int) cveANNIndexGetTreeNumber(cv::ANNIndex* annIndex);
CVAPI(int) cveANNIndexGetItemNumber(cv::ANNIndex* annIndex);
CVAPI(bool) cveANNIndexSetOnDiskBuild(cv::ANNIndex* annIndex, cv::String* filename);
CVAPI(void) cveANNIndexSetSeed(cv::ANNIndex* annIndex, int seed);
#endif