//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "xfeatures2d_c.h"

//StarDetector
cv::xfeatures2d::StarDetector* CvStarDetectorCreate(int maxSize, int responseThreshold, int lineThresholdProjected, int lineThresholdBinarized, int suppressNonmaxSize)
{
   cv::xfeatures2d::StarDetector* detector = new cv::xfeatures2d::StarDetector(maxSize, responseThreshold, lineThresholdProjected, lineThresholdBinarized, suppressNonmaxSize);
   return detector;
}

void CvStarDetectorRelease(cv::xfeatures2d::StarDetector** detector)
{
   delete *detector;
   *detector = 0;
}

/*
//GridAdaptedFeatureDetector
cv::GridAdaptedFeatureDetector* GridAdaptedFeatureDetectorCreate(   
   cv::FeatureDetector* detector,
   int maxTotalKeypoints,
   int gridRows, int gridCols)
{
   cv::Ptr<cv::FeatureDetector> detectorPtr(detector);
   detectorPtr.addref(); //increment the counter such that it should never be release by the grid adapeted feature detector
   return new cv::GridAdaptedFeatureDetector(detectorPtr, maxTotalKeypoints, gridRows, gridCols);
}

void GridAdaptedFeatureDetectorRelease(cv::GridAdaptedFeatureDetector** detector)
{
   delete *detector;
   *detector = 0;
}*/

//FREAK
cv::xfeatures2d::FREAK* CvFreakCreate(bool orientationNormalized, bool scaleNormalized, float patternScale, int nOctaves, cv::DescriptorExtractor** descriptorExtractor)
{
   cv::xfeatures2d::FREAK* freak = new cv::xfeatures2d::FREAK(orientationNormalized, scaleNormalized, patternScale, nOctaves);
   *descriptorExtractor = (cv::DescriptorExtractor*) freak;
   return freak;
}

void CvFreakRelease(cv::xfeatures2d::FREAK** detector)
{
   delete * detector;
   *detector = 0;
}

//Brief
cv::xfeatures2d::BriefDescriptorExtractor* CvBriefDescriptorExtractorCreate(int descriptorSize)
{
   return new cv::xfeatures2d::BriefDescriptorExtractor(descriptorSize);
}

/*
int CvBriefDescriptorExtractorGetDescriptorSize(cv::BriefDescriptorExtractor* extractor)
{
   return extractor->descriptorSize();
}

void CvBriefDescriptorComputeDescriptors(cv::BriefDescriptorExtractor* extractor, IplImage* image, std::vector<cv::KeyPoint>* keypoints, cv::Mat* descriptors)
{
   if (keypoints->size() <= 0) return;
   cv::Mat img = cv::cvarrToMat(image);
   if (img.channels() == 1)
   {
     extractor->compute(img, *keypoints, *descriptors);
   } else //opponent brief
   {
      cv::Ptr<cv::DescriptorExtractor> briefExtractor = new cv::BriefDescriptorExtractor(extractor->descriptorSize());
      cv::OpponentColorDescriptorExtractor colorDescriptorExtractor(briefExtractor);
      colorDescriptorExtractor.compute(img, *keypoints, *descriptors);
   }
}*/

void CvBriefDescriptorExtractorRelease(cv::xfeatures2d::BriefDescriptorExtractor** extractor)
{
   delete *extractor;
   *extractor = 0;
}

/*
//DenseFeatureDetector
cv::DenseFeatureDetector* CvDenseFeatureDetectorCreate( float initFeatureScale, int featureScaleLevels, float featureScaleMul, int initXyStep, int initImgBound, bool varyXyStepWithScale, bool varyImgBoundWithScale)
{
   return new cv::DenseFeatureDetector(initFeatureScale, featureScaleLevels, featureScaleMul, initXyStep, initImgBound, varyXyStepWithScale, varyImgBoundWithScale);
}
void CvDenseFeatureDetectorRelease(cv::DenseFeatureDetector** detector)
{
   delete * detector;
   *detector = 0;
}*/
