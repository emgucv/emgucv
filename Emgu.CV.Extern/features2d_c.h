//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_FEATURES2D_C_H
#define EMGU_FEATURES2D_C_H

#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/features2d/features2d.hpp"
#include "opencv2/calib3d/calib3d.hpp"
#include "opencv2/contrib/contrib.hpp"
#include "opencv2/legacy/legacy.hpp"
#include "opencv2/legacy/compat.hpp"
#include "opencv2/nonfree/nonfree.hpp"
#include "vectors_c.h"

//FernClassifier
CVAPI(cv::FernClassifier*) CvFernClassifierCreate();
CVAPI(void) CvFernClassifierRelease(cv::FernClassifier* classifier);

CVAPI(void) CvFernClassifierTrainFromSingleView(
                                  cv::FernClassifier* classifier,
                                  IplImage* image,
                                  std::vector<cv::KeyPoint>* keypoints,
                                  int _patchSize,
                                  int _signatureSize,
                                  int _nstructs,
                                  int _structSize,
                                  int _nviews,
                                  int _compressionMethod,
                                  cv::PatchGenerator* patchGenerator);

//Patch Genetator
CVAPI(void) CvPatchGeneratorInit(cv::PatchGenerator* pg);

//LDetector
CVAPI(void) CvLDetectorDetectKeyPoints(cv::LDetector* detector, IplImage* image, std::vector<cv::KeyPoint>* keypoints, int maxCount, bool scaleCoords);

//SelfSimDescriptor
CVAPI(cv::SelfSimDescriptor*) CvSelfSimDescriptorCreate(int smallSize,int largeSize, int startDistanceBucket, int numberOfDistanceBuckets, int numberOfAngles);
CVAPI(void) CvSelfSimDescriptorRelease(cv::SelfSimDescriptor* descriptor);
CVAPI(void) CvSelfSimDescriptorCompute(cv::SelfSimDescriptor* descriptor, IplImage* image, std::vector<float>* descriptors, cv::Size* winStride, cv::Point* locations, int numberOfLocation);
CVAPI(int) CvSelfSimDescriptorGetDescriptorSize(cv::SelfSimDescriptor* descriptor);

//StarDetector
CVAPI(cv::StarDetector*) CvStarDetectorCreate(int maxSize, int responseThreshold, int lineThresholdProjected, int lineThresholdBinarized, int suppressNonmaxSize);
CVAPI(void) CvStarDetectorRelease(cv::StarDetector** detector);

//SIFTDetector
CVAPI(cv::SIFT*) CvSIFTDetectorCreate(
   int nFeatures, int nOctaveLayers, 
   double contrastThreshold, double edgeThreshold, 
   double sigma, cv::FeatureDetector** featureDetector, cv::DescriptorExtractor** descriptorExtractor);

CVAPI(void) CvSIFTDetectorRelease(cv::SIFT** detector);

/*
CVAPI(int) CvSIFTDetectorGetDescriptorSize(cv::SIFT* detector);

CVAPI(void) CvSIFTDetectorDetectFeature(cv::SIFT* detector, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints, std::vector<float>* descriptors)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);
   cv::Mat descriptorsMat;
   (*detector)(mat, maskMat, *keypoints, descriptorsMat, false);

   descriptors->resize(keypoints->size()*detector->descriptorSize());

   if (keypoints->size() > 0)
      memcpy(&(*descriptors)[0], descriptorsMat.ptr<float>(), sizeof(float)* descriptorsMat.rows * descriptorsMat.cols);
}

CVAPI(void) CvSIFTDetectorComputeDescriptors(cv::SIFT* detector, IplImage* image, std::vector<cv::KeyPoint>* keypoints, CvMat* descriptors);
*/
//FeatureDetector
CVAPI(void) CvFeatureDetectorDetectKeyPoints(cv::FeatureDetector* detector, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints);

CVAPI(void) CvFeatureDetectorRelease(cv::FeatureDetector** detector);

//GridAdaptedFeatureDetector
CVAPI(cv::GridAdaptedFeatureDetector*) GridAdaptedFeatureDetectorCreate(   
   cv::FeatureDetector* detector,
   int maxTotalKeypoints,
   int gridRows, int gridCols);

/*
CVAPI(void) GridAdaptedFeatureDetectorDetect(
   cv::GridAdaptedFeatureDetector* detector, 
   const cv::Mat* image, std::vector<cv::KeyPoint>* keypoints, const cv::Mat* mask)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat = mask? cv::cvarrToMat(mask) : cv::Mat();
   detector->detect(mat, *keypoints, maskMat);
}*/

CVAPI(void) GridAdaptedFeatureDetectorRelease(cv::GridAdaptedFeatureDetector** detector);

//SURFDetector
CVAPI(cv::SURF*) CvSURFDetectorCreate(CvSURFParams* detector, cv::FeatureDetector** featureDetector, cv::DescriptorExtractor** descriptorExtractor);

CVAPI(void) CvSURFDetectorRelease(cv::SURF** detector);

/*
CVAPI(void) CvSURFDetectorDetectFeature(cv::SURF* detector, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints, std::vector<float>* descriptors)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);
   (*detector)(mat, maskMat, *keypoints, *descriptors, false);
}

CVAPI(void) CvSURFDetectorComputeDescriptors(cv::SURF* detector, IplImage* image, std::vector<cv::KeyPoint>* keypoints, CvMat* descriptors);
*/
//ORB
CVAPI(cv::ORB*) CvOrbDetectorCreate(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTA_K, int scoreType, int patchSize, cv::FeatureDetector** featureDetector, cv::DescriptorExtractor** descriptorExtractor);

CVAPI(void) CvOrbDetectorRelease(cv::ORB** detector);

CVAPI(cv::FREAK*) CvFreakCreate(bool orientationNormalized, bool scaleNormalized, float patternScale, int nOctaves);

CVAPI(void) CvFreakRelease(cv::FREAK** detector);

/*
CVAPI(int) CvOrbDetectorGetDescriptorSize(cv::ORB* detector);

CVAPI(void) CvOrbDetectorComputeDescriptors(cv::ORB* detector, IplImage* image, std::vector<cv::KeyPoint>* keypoints, CvMat* descriptors);
*/

//Brief descriptor extractor
CVAPI(cv::BriefDescriptorExtractor*) CvBriefDescriptorExtractorCreate(int descriptorSize);

/*
CVAPI(int) CvBriefDescriptorExtractorGetDescriptorSize(cv::BriefDescriptorExtractor* extractor);

CVAPI(void) CvBriefDescriptorComputeDescriptors(cv::BriefDescriptorExtractor* extractor, IplImage* image, std::vector<cv::KeyPoint>* keypoints, cv::Mat* descriptors);
*/
CVAPI(void) CvBriefDescriptorExtractorRelease(cv::BriefDescriptorExtractor** extractor);

// detect corners using FAST algorithm
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

//Plannar Object Detector
CVAPI(cv::PlanarObjectDetector*) CvPlanarObjectDetectorDefaultCreate();
CVAPI(void) CvPlanarObjectDetectorRelease(cv::PlanarObjectDetector* detector);
CVAPI(void) CvPlanarObjectDetectorTrain(
   cv::PlanarObjectDetector* objectDetector, 
   IplImage* image, 
   int _npoints,
   int _patchSize,
   int _nstructs,
   int _structSize,
   int _nviews,
   cv::LDetector* detector,
   cv::PatchGenerator* patchGenerator);
CVAPI(void) CvPlanarObjectDetectorDetect(cv::PlanarObjectDetector* detector, IplImage* image, CvMat* homography, CvSeq* corners);

CVAPI(void) CvPlanarObjectDetectorGetModelPoints(cv::PlanarObjectDetector* detector, CvSeq* modelPoints);

// Draw keypoints.
CVAPI(void) drawKeypoints(
                          const IplImage* image, 
                          const std::vector<cv::KeyPoint>* keypoints, 
                          IplImage* outImage,
                          const CvScalar color, 
                          int flags);

// Draws matches of keypints from two images on output image.
CVAPI(void) drawMatchedFeatures(
                                const IplImage* img1, const std::vector<cv::KeyPoint>* keypoints1,
                                const IplImage* img2, const std::vector<cv::KeyPoint>* keypoints2,
                                const CvMat* matchIndicies, 
                                IplImage* outImg,
                                const CvScalar matchColor, const CvScalar singlePointColor,
                                const CvMat* matchesMask, 
                                int flags);

//DescriptorMatcher
CVAPI(void) CvDescriptorMatcherAdd(cv::DescriptorMatcher* matcher, CvMat* trainDescriptor);

CVAPI(void) CvDescriptorMatcherKnnMatch(cv::DescriptorMatcher* matcher, const CvMat* queryDescriptors, 
                   CvMat* trainIdx, CvMat* distance, int k,
                   const CvMat* mask);

/*
CVAPI(void) CvDescriptorMatcherRadiusMatch(cv::DescriptorMatcher* matcher, const CvMat* queryDescriptors, 
                   CvMat* trainIdx, CvMat* distance, int k,
                   const CvMat* mask);*/

//BruteForceMatcher
CVAPI(cv::DescriptorMatcher*) CvBruteForceMatcherCreate(int distanceType, bool crossCheck);

CVAPI(void) CvBruteForceMatcherRelease(cv::DescriptorMatcher** matcher);

//RTreeClassifier
CVAPI(cv::RTreeClassifier*) CvRTreeClassifierCreate();
CVAPI(void) CvRTreeClassifierRelease(cv::RTreeClassifier* classifier);
CVAPI(void) CvRTreeClassifierTrain(
      cv::RTreeClassifier* classifier, 
      IplImage* train_image,
      CvPoint* train_points,
      int numberOfPoints,
		cv::RNG* rng, 
      int num_trees, int depth,
		int views, size_t reduced_num_dim,
		int num_quant_bits);

CVAPI(int) CvRTreeClassifierGetOriginalNumClasses(cv::RTreeClassifier* classifier);
CVAPI(int) CvRTreeClassifierGetNumClasses(cv::RTreeClassifier* classifier);

CVAPI(int) CvRTreeClassifierGetSigniture(
   cv::RTreeClassifier* classifier, 
   IplImage* image, 
   CvPoint* point,
   int patchSize,
   float* signiture);

//flann index
CVAPI(cv::flann::Index*) CvFlannIndexCreateKDTree(CvMat* features, int trees);

CVAPI(cv::flann::Index*) CvFlannIndexCreateLinear(CvMat* features);

CVAPI(cv::flann::Index*) CvFlannIndexCreateKMeans(CvMat* features, int branching_, int iterations_, cvflann::flann_centers_init_t centers_init_, float cb_index_);

CVAPI(cv::flann::Index*) CvFlannIndexCreateComposite(CvMat* features, int trees, int branching_, int iterations_, cvflann::flann_centers_init_t centers_init_, float cb_index_);

CVAPI(cv::flann::Index*) CvFlannIndexCreateAutotuned(CvMat* features, float target_precision, float build_weight, float memory_weight, float sample_fraction);

CVAPI(void) CvFlannIndexKnnSearch(cv::flann::Index* index, CvMat* queries, CvMat* indices, CvMat* dists, int knn, int checks);

CVAPI(int) CvFlannIndexRadiusSearch(cv::flann::Index* index, CvMat* queries, CvMat* indices, CvMat* dists, float radius, int checks);

CVAPI(void) CvFlannIndexRelease(cv::flann::Index* index);

//2D Tracker
CVAPI(bool) getHomographyMatrixFromMatchedFeatures(std::vector<cv::KeyPoint>* model, std::vector<cv::KeyPoint>* observed, CvArr* indices, CvArr* mask, double randsacThreshold, CvMat* homography);

CVAPI(int) voteForSizeAndOrientation(std::vector<cv::KeyPoint>* modelKeyPoints, std::vector<cv::KeyPoint>* observedKeyPoints, CvArr* indices, CvArr* mask, double scaleIncrement, int rotationBins);

//Feature2D
CVAPI(void) CvFeature2DDetectAndCompute(cv::Feature2D* feature2D, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints, cv::Mat* descriptors, bool useProvidedKeyPoints);

//OpponentColorDescriptorExtractor
CVAPI(cv::OpponentColorDescriptorExtractor*) CvOpponentColorDescriptorExtractorCreate(cv::DescriptorExtractor* extractor);
CVAPI(void) CvOpponentColorDescriptorExtractorRelease(cv::OpponentColorDescriptorExtractor** extractor);

//DescriptorExtractor
CVAPI(void) CvDescriptorExtractorCompute(cv::DescriptorExtractor* extractor, const IplImage* image,  std::vector<cv::KeyPoint>* keypoints, cv::Mat* descriptors );
CVAPI(int) CvDescriptorExtractorGetDescriptorSize(cv::DescriptorExtractor* extractor);


#endif