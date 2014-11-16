//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_XFEATURES2D_C_H
#define EMGU_XFEATURES2D_C_H

#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/xfeatures2d.hpp"
//#include "opencv2/legacy/compat.hpp"
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
#endif