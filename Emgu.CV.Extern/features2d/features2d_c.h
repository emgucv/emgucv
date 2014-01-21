//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_FEATURES2D_C_H
#define EMGU_FEATURES2D_C_H

#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/features2d/features2d.hpp"
#include "opencv2/legacy/compat.hpp"
#include "vectors_c.h"

//StarDetector
CVAPI(cv::StarDetector*) CvStarDetectorCreate(int maxSize, int responseThreshold, int lineThresholdProjected, int lineThresholdBinarized, int suppressNonmaxSize);
CVAPI(void) CvStarDetectorRelease(cv::StarDetector** detector);

//FeatureDetector
CVAPI(void) CvFeatureDetectorDetectKeyPoints(cv::FeatureDetector* detector, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints);

CVAPI(void) CvFeatureDetectorRelease(cv::FeatureDetector** detector);

//GridAdaptedFeatureDetector
CVAPI(cv::GridAdaptedFeatureDetector*) GridAdaptedFeatureDetectorCreate(   
   cv::FeatureDetector* detector,
   int maxTotalKeypoints,
   int gridRows, int gridCols);

CVAPI(void) GridAdaptedFeatureDetectorRelease(cv::GridAdaptedFeatureDetector** detector);

//ORB
CVAPI(cv::ORB*) CvOrbDetectorCreate(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTA_K, int scoreType, int patchSize, cv::FeatureDetector** featureDetector, cv::DescriptorExtractor** descriptorExtractor);
CVAPI(void) CvOrbDetectorRelease(cv::ORB** detector);

//Freak
CVAPI(cv::FREAK*) CvFreakCreate(bool orientationNormalized, bool scaleNormalized, float patternScale, int nOctaves, cv::DescriptorExtractor** descriptorExtractor);
CVAPI(void) CvFreakRelease(cv::FREAK** detector);

//Brisk
CVAPI(cv::BRISK*) CvBriskCreate(int thresh, int octaves, float patternScale, cv::FeatureDetector** featureDetector, cv::DescriptorExtractor** descriptorExtractor);
CVAPI(void) CvBriskRelease(cv::BRISK** detector);

//Brief descriptor extractor
CVAPI(cv::BriefDescriptorExtractor*) CvBriefDescriptorExtractorCreate(int descriptorSize);
CVAPI(void) CvBriefDescriptorExtractorRelease(cv::BriefDescriptorExtractor** extractor);

//FAST algorithm
CVAPI(cv::FastFeatureDetector*) CvFASTGetFeatureDetector(int threshold, bool nonmax_supression);
CVAPI(void) CvFASTFeatureDetectorRelease(cv::FastFeatureDetector** detector);

//GFTT
CVAPI(cv::GFTTDetector*) CvGFTTDetectorCreate( int maxCorners, double qualityLevel, double minDistance, int blockSize, bool useHarrisDetector, double k);
CVAPI(void) CvGFTTDetectorRelease(cv::GFTTDetector** detector);

//DenseFeatureDetector
CVAPI(cv::DenseFeatureDetector*) CvDenseFeatureDetectorCreate( float initFeatureScale, int featureScaleLevels, float featureScaleMul, int initXyStep, int initImgBound, bool varyXyStepWithScale, bool varyImgBoundWithScale);
CVAPI(void) CvDenseFeatureDetectorRelease(cv::DenseFeatureDetector** detector);

// MSER detector
CVAPI(cv::MSER*) CvMserGetFeatureDetector(CvMSERParams* detector);
CVAPI(void) CvMserFeatureDetectorRelease(cv::MSER** detector);

// SimpleBlobDetector
CVAPI(cv::SimpleBlobDetector*) CvSimpleBlobDetectorCreate();
CVAPI(void) CvSimpleBlobDetectorRelease(cv::SimpleBlobDetector** detector);

// Draw keypoints.
CVAPI(void) drawKeypoints(
                          const IplImage* image, 
                          const std::vector<cv::KeyPoint>* keypoints, 
                          IplImage* outImage,
                          const CvScalar* color, 
                          int flags);

// Draws matches of keypints from two images on output image.
CVAPI(void) drawMatchedFeatures(
                                const IplImage* img1, const std::vector<cv::KeyPoint>* keypoints1,
                                const IplImage* img2, const std::vector<cv::KeyPoint>* keypoints2,
                                const CvMat* matchIndicies, 
                                IplImage* outImg,
                                const CvScalar* matchColor, const CvScalar* singlePointColor,
                                const CvMat* matchesMask, 
                                int flags);

//DescriptorMatcher
CVAPI(void) CvDescriptorMatcherAdd(cv::DescriptorMatcher* matcher, cv::_InputArray* trainDescriptors);

CVAPI(void) CvDescriptorMatcherKnnMatch(cv::DescriptorMatcher* matcher, cv::_InputArray* queryDescriptors, 
                   CvMat* trainIdx, CvMat* distance, int k,
                   const CvMat* mask);

/*
CVAPI(void) CvDescriptorMatcherRadiusMatch(cv::DescriptorMatcher* matcher, const CvMat* queryDescriptors, 
                   CvMat* trainIdx, CvMat* distance, int k,
                   const CvMat* mask);*/

//BruteForceMatcher
CVAPI(cv::DescriptorMatcher*) CvBruteForceMatcherCreate(int distanceType, bool crossCheck);

CVAPI(void) CvBruteForceMatcherRelease(cv::DescriptorMatcher** matcher);



//2D Tracker
CVAPI(int) voteForSizeAndOrientation(std::vector<cv::KeyPoint>* modelKeyPoints, std::vector<cv::KeyPoint>* observedKeyPoints, CvArr* indices, CvArr* mask, double scaleIncrement, int rotationBins);

//Feature2D
CVAPI(void) CvFeature2DDetectAndCompute(cv::Feature2D* feature2D, cv::_InputArray* image, cv::_InputArray* mask, std::vector<cv::KeyPoint>* keypoints, cv::_OutputArray* descriptors, bool useProvidedKeyPoints);

//OpponentColorDescriptorExtractor
CVAPI(cv::OpponentColorDescriptorExtractor*) CvOpponentColorDescriptorExtractorCreate(cv::DescriptorExtractor* extractor);
CVAPI(void) CvOpponentColorDescriptorExtractorRelease(cv::OpponentColorDescriptorExtractor** extractor);

//DescriptorExtractor
CVAPI(void) CvDescriptorExtractorCompute(cv::DescriptorExtractor* extractor, const IplImage* image,  std::vector<cv::KeyPoint>* keypoints, cv::Mat* descriptors );
CVAPI(int) CvDescriptorExtractorGetDescriptorSize(cv::DescriptorExtractor* extractor);

//BowKMeansTrainer
CVAPI(cv::BOWKMeansTrainer*) CvBOWKMeansTrainerCreate(int clusterCount, const CvTermCriteria* termcrit, int attempts, int flags);
CVAPI(void) CvBOWKMeansTrainerRelease(cv::BOWKMeansTrainer** trainer);
CVAPI(int) CvBOWKMeansTrainerGetDescriptorCount(cv::BOWKMeansTrainer* trainer);
CVAPI(void) CvBOWKMeansTrainerAdd(cv::BOWKMeansTrainer* trainer, cv::Mat* descriptors);
CVAPI(void) CvBOWKMeansTrainerCluster(cv::BOWKMeansTrainer* trainer, cv::_OutputArray* cluster);

//BOWImgDescriptorExtractor
CVAPI(cv::BOWImgDescriptorExtractor*) CvBOWImgDescriptorExtractorCreate(cv::DescriptorExtractor* descriptorExtractor, cv::DescriptorMatcher* descriptorMatcher);
CVAPI(void) CvBOWImgDescriptorExtractorRelease(cv::BOWImgDescriptorExtractor** descriptorExtractor);
CVAPI(void) CvBOWImgDescriptorExtractorSetVocabulary(cv::BOWImgDescriptorExtractor* bowImgDescriptorExtractor, cv::Mat* vocabulary);
CVAPI(void) CvBOWImgDescriptorExtractorCompute(cv::BOWImgDescriptorExtractor* bowImgDescriptorExtractor, const cv::Mat* image, std::vector<cv::KeyPoint>* keypoints, cv::Mat* imgDescriptor);

#endif