//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_XFEATURES2D_C_H
#define EMGU_XFEATURES2D_C_H

#include "opencv2/imgproc/imgproc.hpp"
#include "vectors_c.h"

#ifdef HAVE_OPENCV_XFEATURES2D
#include "opencv2/xfeatures2d.hpp"
#else
static inline CV_NORETURN void throw_no_xfeatures2d() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without xfeatures2d support"); }
namespace cv {

	namespace xfeatures2d {
		class StarDetector {};
		class FREAK {};
		class BriefDescriptorExtractor {};
		class LUCID {};
		class LATCH {};
		class DAISY {};
		class BoostDesc {};
		class MSDDetector {};
		class VGG {};
		class PCTSignatures {};
		class PCTSignaturesSQFD {};
		class HarrisLaplaceFeatureDetector {};

	}
}
#endif

//StarDetector
CVAPI(cv::xfeatures2d::StarDetector*) cveStarDetectorCreate(int maxSize, int responseThreshold, int lineThresholdProjected, int lineThresholdBinarized, int suppressNonmaxSize, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::StarDetector>** sharedPtr);
CVAPI(void) cveStarDetectorRelease(cv::Ptr<cv::xfeatures2d::StarDetector>** sharedPtr);

/*
//GridAdaptedFeatureDetector
CVAPI(cv::xfeatures2d ::GridAdaptedFeatureDetector*) GridAdaptedFeatureDetectorCreate(
   cv::FeatureDetector* detector,
   int maxTotalKeypoints,
   int gridRows, int gridCols);

CVAPI(void) GridAdaptedFeatureDetectorRelease(cv::GridAdaptedFeatureDetector** detector);
*/
//Freak
CVAPI(cv::xfeatures2d::FREAK*) cveFreakCreate(bool orientationNormalized, bool scaleNormalized, float patternScale, int nOctaves, cv::Feature2D** descriptorExtractor, cv::Ptr<cv::xfeatures2d::FREAK>** sharedPtr);
CVAPI(void) cveFreakRelease(cv::Ptr<cv::xfeatures2d::FREAK>** sharedPtr);

//Brief descriptor extractor
CVAPI(cv::xfeatures2d::BriefDescriptorExtractor*) cveBriefDescriptorExtractorCreate(int descriptorSize, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::BriefDescriptorExtractor>** sharedPtr);
CVAPI(void) cveBriefDescriptorExtractorRelease(cv::Ptr<cv::xfeatures2d::BriefDescriptorExtractor>** sharedPtr);

/*
//DenseFeatureDetector
CVAPI(cv::xfeatures2d::DenseFeatureDetector*) CvDenseFeatureDetectorCreate( float initFeatureScale, int featureScaleLevels, float featureScaleMul, int initXyStep, int initImgBound, bool varyXyStepWithScale, bool varyImgBoundWithScale);
CVAPI(void) CvDenseFeatureDetectorRelease(cv::xfeatures2d::DenseFeatureDetector** detector);
*/

//LUCID
CVAPI(cv::xfeatures2d::LUCID*) cveLUCIDCreate(int lucidKernel, int blurKernel, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::LUCID>** sharedPtr);
CVAPI(void) cveLUCIDRelease(cv::Ptr<cv::xfeatures2d::LUCID>** sharedPtr);


//LATCH
CVAPI(cv::xfeatures2d::LATCH*) cveLATCHCreate(int bytes, bool rotationInvariance, int halfSsdSize, cv::Feature2D** extractor, cv::Ptr<cv::xfeatures2d::LATCH>** sharedPtr);
CVAPI(void) cveLATCHRelease(cv::Ptr<cv::xfeatures2d::LATCH>** sharedPtr);

//DAISY
CVAPI(cv::xfeatures2d::DAISY*) cveDAISYCreate(
	float radius, int qRadius, int qTheta,
	int qHist, int norm, cv::_InputArray* H,
	bool interpolation, bool useOrientation,
	cv::Feature2D** extractor, cv::Ptr<cv::xfeatures2d::DAISY>** sharedPtr);
CVAPI(void) cveDAISYRelease(cv::Ptr<cv::xfeatures2d::DAISY>** sharedPtr);

//BoostDesc
CVAPI(cv::xfeatures2d::BoostDesc*) cveBoostDescCreate(int desc, bool useScaleOrientation, float scalefactor, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::BoostDesc>** sharedPtr);
CVAPI(void) cveBoostDescRelease(cv::Ptr<cv::xfeatures2d::BoostDesc>** sharedPtr);

//MSD
CVAPI(cv::xfeatures2d::MSDDetector*) cveMSDDetectorCreate(int m_patch_radius, int m_search_area_radius,
	int m_nms_radius, int m_nms_scale_radius, float m_th_saliency, int m_kNN,
	float m_scale_factor, int m_n_scales, bool m_compute_orientation, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::MSDDetector>** sharedPtr);
CVAPI(void) cveMSDDetectorRelease(cv::Ptr<cv::xfeatures2d::MSDDetector>** sharedPtr);

//VGG
CVAPI(cv::xfeatures2d::VGG*) cveVGGCreate(
	int desc, float isigma, bool imgNormalize, bool useScaleOrientation,
	float scaleFactor, bool dscNormalize, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::VGG>** sharedPtr);
CVAPI(void) cveVGGRelease(cv::Ptr<cv::xfeatures2d::VGG>** sharedPtr);

CVAPI(cv::xfeatures2d::PCTSignatures*) cvePCTSignaturesCreate(int initSampleCount, int initSeedCount, int pointDistribution, cv::Ptr<cv::xfeatures2d::PCTSignatures>** sharedPtr);
CVAPI(cv::xfeatures2d::PCTSignatures*) cvePCTSignaturesCreate2(std::vector<cv::Point2f>* initSamplingPoints, int initSeedCount, cv::Ptr<cv::xfeatures2d::PCTSignatures>** sharedPtr);
CVAPI(cv::xfeatures2d::PCTSignatures*) cvePCTSignaturesCreate3(std::vector<cv::Point2f>* initSamplingPoints, std::vector<int>* initClusterSeedIndexes, cv::Ptr<cv::xfeatures2d::PCTSignatures>** sharedPtr);
CVAPI(void) cvePCTSignaturesRelease(cv::Ptr<cv::xfeatures2d::PCTSignatures>** sharedPtr);
CVAPI(void) cvePCTSignaturesComputeSignature(cv::xfeatures2d::PCTSignatures* pct, cv::_InputArray* image, cv::_OutputArray* signature);
CVAPI(void) cvePCTSignaturesDrawSignature(cv::_InputArray* source, cv::_InputArray* signature, cv::_OutputArray* result, float radiusToShorterSideRatio, int borderThickness);

CVAPI(cv::xfeatures2d::PCTSignaturesSQFD*) cvePCTSignaturesSQFDCreate(
	int distanceFunction,
	int similarityFunction,
	float similarityParameter,
	cv::Ptr<cv::xfeatures2d::PCTSignaturesSQFD>** sharedPtr);
CVAPI(float) cvePCTSignaturesSQFDComputeQuadraticFormDistance(
	cv::xfeatures2d::PCTSignaturesSQFD* sqfd,
	cv::_InputArray* signature0,
	cv::_InputArray* signature1);
CVAPI(void) cvePCTSignaturesSQFDComputeQuadraticFormDistances(
	cv::xfeatures2d::PCTSignaturesSQFD* sqfd,
	cv::Mat* sourceSignature,
	std::vector<cv::Mat>* imageSignatures,
	std::vector<float>* distances);
CVAPI(void) cvePCTSignaturesSQFDRelease(cv::Ptr<cv::xfeatures2d::PCTSignaturesSQFD>** sharedPtr);

//HarrisLaplaceFeatureDetector
CVAPI(cv::xfeatures2d::HarrisLaplaceFeatureDetector*) cveHarrisLaplaceFeatureDetectorCreate(
	int numOctaves,
	float corn_thresh,
	float DOG_thresh,
	int maxCorners,
	int num_layers,
	cv::Ptr<cv::xfeatures2d::HarrisLaplaceFeatureDetector>** sharedPtr);
CVAPI(void) cveHarrisLaplaceFeatureDetectorRelease(cv::Ptr<cv::xfeatures2d::HarrisLaplaceFeatureDetector>** sharedPtr);

CVAPI(void) cveMatchGMS(
	CvSize* size1, CvSize* size2,
	std::vector< cv::KeyPoint >* keypoints1, std::vector< cv::KeyPoint >* keypoints2,
	std::vector< cv::DMatch >* matches1to2, std::vector< cv::DMatch >* matchesGMS,
	bool withRotation, bool withScale, double thresholdFactor);
#endif