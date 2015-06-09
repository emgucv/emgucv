//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_XFEATURES2D_C_H
#define EMGU_XFEATURES2D_C_H

#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/xfeatures2d.hpp"
#include "vectors_c.h"

//StarDetector
CVAPI(cv::xfeatures2d::StarDetector*) CvStarDetectorCreate(int maxSize, int responseThreshold, int lineThresholdProjected, int lineThresholdBinarized, int suppressNonmaxSize, cv::Feature2D** feature2D);
CVAPI(void) CvStarDetectorRelease(cv::xfeatures2d::StarDetector** detector);

/*
//GridAdaptedFeatureDetector
CVAPI(cv::xfeatures2d ::GridAdaptedFeatureDetector*) GridAdaptedFeatureDetectorCreate(   
   cv::FeatureDetector* detector,
   int maxTotalKeypoints,
   int gridRows, int gridCols);

CVAPI(void) GridAdaptedFeatureDetectorRelease(cv::GridAdaptedFeatureDetector** detector);
*/
//Freak
CVAPI(cv::xfeatures2d::FREAK*) CvFreakCreate(bool orientationNormalized, bool scaleNormalized, float patternScale, int nOctaves, cv::Feature2D** descriptorExtractor);
CVAPI(void) CvFreakRelease(cv::xfeatures2d::FREAK** detector);

//Brief descriptor extractor
CVAPI(cv::xfeatures2d::BriefDescriptorExtractor*) CvBriefDescriptorExtractorCreate(int descriptorSize, cv::Feature2D** feature2D);
CVAPI(void) CvBriefDescriptorExtractorRelease(cv::xfeatures2d::BriefDescriptorExtractor** extractor);

/*
//DenseFeatureDetector
CVAPI(cv::xfeatures2d::DenseFeatureDetector*) CvDenseFeatureDetectorCreate( float initFeatureScale, int featureScaleLevels, float featureScaleMul, int initXyStep, int initImgBound, bool varyXyStepWithScale, bool varyImgBoundWithScale);
CVAPI(void) CvDenseFeatureDetectorRelease(cv::xfeatures2d::DenseFeatureDetector** detector);
*/

//LUCID
CVAPI(cv::xfeatures2d::LUCID*) cveLUCIDCreate(int lucidKernel, int blurKernel, cv::Feature2D** feature2D);
CVAPI(void) cveLUCIDRelease(cv::xfeatures2d::LUCID** lucid);


//LATCH
CVAPI(cv::xfeatures2d::LATCH*) cveLATCHCreate(int bytes, bool rotationInvariance, int halfSsdSize, cv::DescriptorExtractor** extractor);
CVAPI(void) cveLATCHRelease(cv::xfeatures2d::LATCH** lucid);

//DAISY
CVAPI(cv::xfeatures2d::DAISY*) cveDAISYCreate(float radius = 15, int q_radius = 3, int q_theta = 8,
   int q_hist = 8, int norm, cv::_InputArray* H,
   bool interpolation, bool use_orientation = false, cv::DescriptorExtractor** extractor);
CVAPI(void) cveDAISYRelease(cv::xfeatures2d::DAISY** daisy);

#endif