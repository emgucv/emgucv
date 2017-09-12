//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_XFEATURES2D_C_H
#define EMGU_XFEATURES2D_C_H

#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/xfeatures2d.hpp"
#include "vectors_c.h"

//StarDetector
CVAPI(cv::xfeatures2d::StarDetector*) cveStarDetectorCreate(int maxSize, int responseThreshold, int lineThresholdProjected, int lineThresholdBinarized, int suppressNonmaxSize, cv::Feature2D** feature2D);
CVAPI(void) cveStarDetectorRelease(cv::xfeatures2d::StarDetector** detector);

/*
//GridAdaptedFeatureDetector
CVAPI(cv::xfeatures2d ::GridAdaptedFeatureDetector*) GridAdaptedFeatureDetectorCreate(   
   cv::FeatureDetector* detector,
   int maxTotalKeypoints,
   int gridRows, int gridCols);

CVAPI(void) GridAdaptedFeatureDetectorRelease(cv::GridAdaptedFeatureDetector** detector);
*/
//Freak
CVAPI(cv::xfeatures2d::FREAK*) cveFreakCreate(bool orientationNormalized, bool scaleNormalized, float patternScale, int nOctaves, cv::Feature2D** descriptorExtractor);
CVAPI(void) cveFreakRelease(cv::xfeatures2d::FREAK** detector);

//Brief descriptor extractor
CVAPI(cv::xfeatures2d::BriefDescriptorExtractor*) cveBriefDescriptorExtractorCreate(int descriptorSize, cv::Feature2D** feature2D);
CVAPI(void) cveBriefDescriptorExtractorRelease(cv::xfeatures2d::BriefDescriptorExtractor** extractor);

/*
//DenseFeatureDetector
CVAPI(cv::xfeatures2d::DenseFeatureDetector*) CvDenseFeatureDetectorCreate( float initFeatureScale, int featureScaleLevels, float featureScaleMul, int initXyStep, int initImgBound, bool varyXyStepWithScale, bool varyImgBoundWithScale);
CVAPI(void) CvDenseFeatureDetectorRelease(cv::xfeatures2d::DenseFeatureDetector** detector);
*/

//LUCID
CVAPI(cv::xfeatures2d::LUCID*) cveLUCIDCreate(int lucidKernel, int blurKernel, cv::Feature2D** feature2D);
CVAPI(void) cveLUCIDRelease(cv::xfeatures2d::LUCID** lucid);


//LATCH
CVAPI(cv::xfeatures2d::LATCH*) cveLATCHCreate(int bytes, bool rotationInvariance, int halfSsdSize, cv::Feature2D** extractor);
CVAPI(void) cveLATCHRelease(cv::xfeatures2d::LATCH** lucid);

//DAISY
CVAPI(cv::xfeatures2d::DAISY*) cveDAISYCreate(float radius, int qRadius, int qTheta,
   int qHist, int norm, cv::_InputArray* H,
   bool interpolation, bool useOrientation, cv::Feature2D** extractor);
CVAPI(void) cveDAISYRelease(cv::xfeatures2d::DAISY** daisy);

//BoostDesc
CVAPI(cv::xfeatures2d::BoostDesc*) cveBoostDescCreate(int desc,	bool useScaleOrientation, float scalefactor, cv::Feature2D** feature2D);
CVAPI(void) cveBoostDescRelease(cv::xfeatures2d::BoostDesc** extractor);

//MSD
CVAPI(cv::xfeatures2d::MSDDetector*) cveMSDDetectorCreate(int m_patch_radius, int m_search_area_radius,
	int m_nms_radius, int m_nms_scale_radius, float m_th_saliency, int m_kNN,
	float m_scale_factor, int m_n_scales, bool m_compute_orientation, cv::Feature2D** feature2D);
CVAPI(void) cveMSDDetectorRelease(cv::xfeatures2d::MSDDetector** detector);

//VGG
CVAPI(cv::xfeatures2d::VGG*) cveVGGCreate(
	int desc, float isigma, bool imgNormalize, bool useScaleOrientation,
	float scaleFactor, bool dscNormalize, cv::Feature2D** feature2D);
CVAPI(void) cveVGGRelease(cv::xfeatures2d::VGG** extractor);

CVAPI(cv::xfeatures2d::PCTSignatures*) cvePCTSignaturesCreate(int initSampleCount,	int initSeedCount,	int pointDistribution);
CVAPI(cv::xfeatures2d::PCTSignatures*) cvePCTSignaturesCreate2(std::vector<cv::Point2f>* initSamplingPoints, int initSeedCount);
CVAPI(cv::xfeatures2d::PCTSignatures*) cvePCTSignaturesCreate3(std::vector<cv::Point2f>* initSamplingPoints, std::vector<int>* initClusterSeedIndexes);
CVAPI(void) cvePCTSignaturesRelease(cv::xfeatures2d::PCTSignatures** pct);
CVAPI(void) cvePCTSignaturesComputeSignature(cv::xfeatures2d::PCTSignatures* pct, cv::_InputArray* image,cv::_OutputArray* signature);
CVAPI(void) cvePCTSignaturesDrawSignature(cv::_InputArray* source, cv::_InputArray* signature, cv::_OutputArray* result, float radiusToShorterSideRatio, int borderThickness);

CVAPI(cv::xfeatures2d::PCTSignaturesSQFD*) cvePCTSignaturesSQFDCreate(
	int distanceFunction,
	int similarityFunction,
	float similarityParameter);
CVAPI(float) cvePCTSignaturesSQFDComputeQuadraticFormDistance(
	cv::xfeatures2d::PCTSignaturesSQFD* sqfd,
	cv::_InputArray* signature0,
	cv::_InputArray* signature1);
CVAPI(void) cvePCTSignaturesSQFDComputeQuadraticFormDistances(
	cv::xfeatures2d::PCTSignaturesSQFD* sqfd,
	cv::Mat* sourceSignature,
	std::vector<cv::Mat>* imageSignatures,
	std::vector<float>* distances);
CVAPI(void) cvePCTSignaturesSQFDRelease(cv::xfeatures2d::PCTSignaturesSQFD** sqfd);

#endif