//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "xfeatures2d_c.h"

//StarDetector
cv::xfeatures2d::StarDetector* cveStarDetectorCreate(int maxSize, int responseThreshold, int lineThresholdProjected, int lineThresholdBinarized, int suppressNonmaxSize, cv::Feature2D** feature2D)
{
   cv::Ptr<cv::xfeatures2d::StarDetector> detectorPtr = cv::xfeatures2d::StarDetector::create(maxSize, responseThreshold, lineThresholdProjected, lineThresholdBinarized, suppressNonmaxSize);
   detectorPtr.addref();
   *feature2D = dynamic_cast<cv::Feature2D*>(detectorPtr.get());
   return detectorPtr.get();
}

void cveStarDetectorRelease(cv::xfeatures2d::StarDetector** detector)
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
cv::xfeatures2d::FREAK* cveFreakCreate(bool orientationNormalized, bool scaleNormalized, float patternScale, int nOctaves, cv::Feature2D** descriptorExtractor)
{
   cv::Ptr<cv::xfeatures2d::FREAK> freakPtr = cv::xfeatures2d::FREAK::create(orientationNormalized, scaleNormalized, patternScale, nOctaves);
   freakPtr.addref();
   *descriptorExtractor = dynamic_cast<cv::Feature2D*>( freakPtr.get());
   return freakPtr.get();
}

void cveFreakRelease(cv::xfeatures2d::FREAK** detector)
{
   delete * detector;
   *detector = 0;
}

//Brief
cv::xfeatures2d::BriefDescriptorExtractor* cveBriefDescriptorExtractorCreate(int descriptorSize, cv::Feature2D** feature2D)
{
   cv::Ptr<cv::xfeatures2d::BriefDescriptorExtractor> briefPtr = cv::xfeatures2d::BriefDescriptorExtractor::create(descriptorSize);
   briefPtr.addref();
   *feature2D = dynamic_cast<cv::Feature2D*>(briefPtr.get());
   return briefPtr.get();
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

void cveBriefDescriptorExtractorRelease(cv::xfeatures2d::BriefDescriptorExtractor** extractor)
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


//LUCID
cv::xfeatures2d::LUCID* cveLUCIDCreate(int lucidKernel, int blurKernel, cv::Feature2D** feature2D)
{
   cv::Ptr<cv::xfeatures2d::LUCID> lucidPtr = cv::xfeatures2d::LUCID::create(lucidKernel, blurKernel);
   lucidPtr.addref();
   *feature2D = dynamic_cast<cv::Feature2D*>(lucidPtr.get());
   return lucidPtr.get();
}
void cveLUCIDRelease(cv::xfeatures2d::LUCID** lucid)
{
   delete *lucid;
   *lucid = 0;
}

//LATCH
cv::xfeatures2d::LATCH* cveLATCHCreate(int bytes, bool rotationInvariance, int halfSsdSize, cv::Feature2D** extractor)
{
   cv::Ptr<cv::xfeatures2d::LATCH> latchPtr = cv::xfeatures2d::LATCH::create(bytes, rotationInvariance, halfSsdSize);
   latchPtr.addref();
   *extractor = dynamic_cast<cv::Feature2D*>(latchPtr.get());
   return latchPtr.get();
}
void cveLATCHRelease(cv::xfeatures2d::LATCH** latch)
{
   delete *latch;
   *latch = 0;
}

//DAISY
cv::xfeatures2d::DAISY* cveDAISYCreate(float radius, int qRadius, int qTheta,
   int qHist, int norm, cv::_InputArray* H,
   bool interpolation, bool useOrientation, cv::Feature2D** extractor)
{
   cv::Ptr<cv::xfeatures2d::DAISY> daisyPtr = cv::xfeatures2d::DAISY::create(radius, qRadius, qTheta, qHist, norm, H ? *H : (cv::_InputArray) cv::noArray(), interpolation, useOrientation);
   daisyPtr.addref();
   *extractor = dynamic_cast<cv::Feature2D*>(daisyPtr.get());
   return daisyPtr.get();
}
void cveDAISYRelease(cv::xfeatures2d::DAISY** daisy)
{
   delete* daisy;
   *daisy = 0;
}