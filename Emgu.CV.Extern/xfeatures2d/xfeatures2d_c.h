//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
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

#endif