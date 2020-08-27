//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "xfeatures2d_c.h"

//StarDetector
cv::xfeatures2d::StarDetector* cveStarDetectorCreate(int maxSize, int responseThreshold, int lineThresholdProjected, int lineThresholdBinarized, int suppressNonmaxSize, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::StarDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::Ptr<cv::xfeatures2d::StarDetector> detectorPtr = cv::xfeatures2d::StarDetector::create(maxSize, responseThreshold, lineThresholdProjected, lineThresholdBinarized, suppressNonmaxSize);
	*sharedPtr = new cv::Ptr<cv::xfeatures2d::StarDetector>(detectorPtr);
	*feature2D = dynamic_cast<cv::Feature2D*>(detectorPtr.get());
	return detectorPtr.get();
#else
	throw_no_xfeatures2d();
#endif
}

void cveStarDetectorRelease(cv::Ptr<cv::xfeatures2d::StarDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xfeatures2d();
#endif
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
cv::xfeatures2d::FREAK* cveFreakCreate(bool orientationNormalized, bool scaleNormalized, float patternScale, int nOctaves, cv::Feature2D** descriptorExtractor, cv::Ptr<cv::xfeatures2d::FREAK>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::Ptr<cv::xfeatures2d::FREAK> freakPtr = cv::xfeatures2d::FREAK::create(orientationNormalized, scaleNormalized, patternScale, nOctaves);
	*sharedPtr = new cv::Ptr<cv::xfeatures2d::FREAK>(freakPtr);
	*descriptorExtractor = dynamic_cast<cv::Feature2D*>(freakPtr.get());
	return freakPtr.get();
#else
	throw_no_xfeatures2d();
#endif
}

void cveFreakRelease(cv::Ptr<cv::xfeatures2d::FREAK>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xfeatures2d();
#endif
}

//Brief
cv::xfeatures2d::BriefDescriptorExtractor* cveBriefDescriptorExtractorCreate(int descriptorSize, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::BriefDescriptorExtractor>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::Ptr<cv::xfeatures2d::BriefDescriptorExtractor> briefPtr = cv::xfeatures2d::BriefDescriptorExtractor::create(descriptorSize);
	*sharedPtr = new cv::Ptr<cv::xfeatures2d::BriefDescriptorExtractor>(briefPtr);
	*feature2D = dynamic_cast<cv::Feature2D*>(briefPtr.get());
	return briefPtr.get();
#else
	throw_no_xfeatures2d();
#endif
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

void cveBriefDescriptorExtractorRelease(cv::Ptr<cv::xfeatures2d::BriefDescriptorExtractor>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xfeatures2d();
#endif
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
cv::xfeatures2d::LUCID* cveLUCIDCreate(int lucidKernel, int blurKernel, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::LUCID>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::Ptr<cv::xfeatures2d::LUCID> lucidPtr = cv::xfeatures2d::LUCID::create(lucidKernel, blurKernel);
	*sharedPtr = new cv::Ptr<cv::xfeatures2d::LUCID>(lucidPtr);
	*feature2D = dynamic_cast<cv::Feature2D*>(lucidPtr.get());
	return lucidPtr.get();
#else
	throw_no_xfeatures2d();
#endif
}
void cveLUCIDRelease(cv::Ptr<cv::xfeatures2d::LUCID>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xfeatures2d();
#endif
}

//LATCH
cv::xfeatures2d::LATCH* cveLATCHCreate(int bytes, bool rotationInvariance, int halfSsdSize, cv::Feature2D** extractor, cv::Ptr<cv::xfeatures2d::LATCH>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::Ptr<cv::xfeatures2d::LATCH> latchPtr = cv::xfeatures2d::LATCH::create(bytes, rotationInvariance, halfSsdSize);
	*sharedPtr = new cv::Ptr<cv::xfeatures2d::LATCH>(latchPtr);
	*extractor = dynamic_cast<cv::Feature2D*>(latchPtr.get());
	return latchPtr.get();
#else
	throw_no_xfeatures2d();
#endif
}
void cveLATCHRelease(cv::Ptr<cv::xfeatures2d::LATCH>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xfeatures2d();
#endif
}

//DAISY
cv::xfeatures2d::DAISY* cveDAISYCreate(
	float radius, int qRadius, int qTheta,
	int qHist, int norm, cv::_InputArray* H,
	bool interpolation, bool useOrientation, 
	cv::Feature2D** extractor,
	cv::Ptr<cv::xfeatures2d::DAISY>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::Ptr<cv::xfeatures2d::DAISY> daisyPtr = cv::xfeatures2d::DAISY::create(radius, qRadius, qTheta, qHist, static_cast<cv::xfeatures2d::DAISY::NormalizationType>( norm ), H ? *H : (cv::_InputArray) cv::noArray(), interpolation, useOrientation);
	*sharedPtr = new cv::Ptr<cv::xfeatures2d::DAISY>(daisyPtr);
	*extractor = dynamic_cast<cv::Feature2D*>(daisyPtr.get());
	return daisyPtr.get();
#else
	throw_no_xfeatures2d();
#endif
}
void cveDAISYRelease(cv::Ptr<cv::xfeatures2d::DAISY>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xfeatures2d();
#endif
}

//BoostDesc
cv::xfeatures2d::BoostDesc* cveBoostDescCreate(int desc, bool useScaleOrientation, float scalefactor, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::BoostDesc>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::Ptr<cv::xfeatures2d::BoostDesc> ptr = cv::xfeatures2d::BoostDesc::create(desc, useScaleOrientation, scalefactor);
	*sharedPtr = new cv::Ptr<cv::xfeatures2d::BoostDesc>(ptr);
	*feature2D = dynamic_cast<cv::Feature2D*>(ptr.get());
	return ptr.get();
#else
	throw_no_xfeatures2d();
#endif
}
void cveBoostDescRelease(cv::Ptr<cv::xfeatures2d::BoostDesc>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xfeatures2d();
#endif
}

//MSD
cv::xfeatures2d::MSDDetector* cveMSDDetectorCreate(int m_patch_radius, int m_search_area_radius,
	int m_nms_radius, int m_nms_scale_radius, float m_th_saliency, int m_kNN,
	float m_scale_factor, int m_n_scales, bool m_compute_orientation, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::MSDDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::Ptr<cv::xfeatures2d::MSDDetector> ptr = cv::xfeatures2d::MSDDetector::create(
		m_patch_radius, m_search_area_radius, m_nms_radius, m_nms_scale_radius, m_th_saliency,
		m_kNN, m_scale_factor, m_n_scales, m_compute_orientation);
	*sharedPtr = new cv::Ptr<cv::xfeatures2d::MSDDetector>(ptr);
	*feature2D = dynamic_cast<cv::Feature2D*>(ptr.get());
	return ptr.get();
#else
	throw_no_xfeatures2d();
#endif
}
void cveMSDDetectorRelease(cv::Ptr<cv::xfeatures2d::MSDDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xfeatures2d();
#endif
}

//VGG
cv::xfeatures2d::VGG* cveVGGCreate(
	int desc, float isigma, bool imgNormalize, bool useScaleOrientation,
	float scaleFactor, bool dscNormalize, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::VGG>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::Ptr<cv::xfeatures2d::VGG> ptr = cv::xfeatures2d::VGG::create(desc, isigma, imgNormalize, useScaleOrientation, scaleFactor, dscNormalize);
	*sharedPtr = new cv::Ptr<cv::xfeatures2d::VGG>(ptr);
	*feature2D = dynamic_cast<cv::Feature2D*>(ptr.get());
	return ptr.get();
#else
	throw_no_xfeatures2d();
#endif
}
void cveVGGRelease(cv::Ptr<cv::xfeatures2d::VGG>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xfeatures2d();
#endif
}

cv::xfeatures2d::PCTSignatures* cvePCTSignaturesCreate(int initSampleCount, int initSeedCount, int pointDistribution, cv::Ptr<cv::xfeatures2d::PCTSignatures>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::Ptr<cv::xfeatures2d::PCTSignatures> ptr = cv::xfeatures2d::PCTSignatures::create(initSampleCount, initSeedCount, pointDistribution);
	*sharedPtr = new cv::Ptr<cv::xfeatures2d::PCTSignatures>(ptr);
	return ptr.get();
#else
	throw_no_xfeatures2d();
#endif
}
cv::xfeatures2d::PCTSignatures* cvePCTSignaturesCreate2(std::vector<cv::Point2f>* initSamplingPoints, int initSeedCount, cv::Ptr<cv::xfeatures2d::PCTSignatures>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::Ptr<cv::xfeatures2d::PCTSignatures> ptr = cv::xfeatures2d::PCTSignatures::create(*initSamplingPoints, initSeedCount);
	*sharedPtr = new cv::Ptr<cv::xfeatures2d::PCTSignatures>(ptr);
	return ptr.get();
#else
	throw_no_xfeatures2d();
#endif
}
cv::xfeatures2d::PCTSignatures* cvePCTSignaturesCreate3(std::vector<cv::Point2f>* initSamplingPoints, std::vector<int>* initClusterSeedIndexes, cv::Ptr<cv::xfeatures2d::PCTSignatures>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::Ptr<cv::xfeatures2d::PCTSignatures> ptr = cv::xfeatures2d::PCTSignatures::create(*initSamplingPoints, *initClusterSeedIndexes);
	*sharedPtr = new cv::Ptr<cv::xfeatures2d::PCTSignatures>(ptr);
	return ptr.get();
#else
	throw_no_xfeatures2d();
#endif
}
void cvePCTSignaturesRelease(cv::Ptr<cv::xfeatures2d::PCTSignatures>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xfeatures2d();
#endif
}
void cvePCTSignaturesComputeSignature(cv::xfeatures2d::PCTSignatures* pct, cv::_InputArray* image, cv::_OutputArray* signature)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	pct->computeSignature(*image, *signature);
#else
	throw_no_xfeatures2d();
#endif
}
void cvePCTSignaturesDrawSignature(cv::_InputArray* source, cv::_InputArray* signature, cv::_OutputArray* result, float radiusToShorterSideRatio, int borderThickness)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::xfeatures2d::PCTSignatures::drawSignature(*source, *signature, *result, radiusToShorterSideRatio, borderThickness);
#else
	throw_no_xfeatures2d();
#endif
}

cv::xfeatures2d::PCTSignaturesSQFD* cvePCTSignaturesSQFDCreate(
	int distanceFunction,
	int similarityFunction,
	float similarityParameter, 
	cv::Ptr<cv::xfeatures2d::PCTSignaturesSQFD>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::Ptr<cv::xfeatures2d::PCTSignaturesSQFD> ptr = cv::xfeatures2d::PCTSignaturesSQFD::create(distanceFunction, similarityFunction, similarityParameter);
	*sharedPtr = new cv::Ptr<cv::xfeatures2d::PCTSignaturesSQFD>(ptr);
	return ptr.get();
#else
	throw_no_xfeatures2d();
#endif
}
float cvePCTSignaturesSQFDComputeQuadraticFormDistance(
	cv::xfeatures2d::PCTSignaturesSQFD* sqfd,
	cv::_InputArray* signature0,
	cv::_InputArray* signature1)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	return sqfd->computeQuadraticFormDistance(*signature0, *signature1);
#else
	throw_no_xfeatures2d();
#endif
}
void cvePCTSignaturesSQFDComputeQuadraticFormDistances(
	cv::xfeatures2d::PCTSignaturesSQFD* sqfd,
	cv::Mat* sourceSignature,
	std::vector<cv::Mat>* imageSignatures,
	std::vector<float>* distances)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	sqfd->computeQuadraticFormDistances(*sourceSignature, *imageSignatures, *distances);
#else
	throw_no_xfeatures2d();
#endif
}
void cvePCTSignaturesSQFDRelease(cv::Ptr<cv::xfeatures2d::PCTSignaturesSQFD>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xfeatures2d();
#endif
}

cv::xfeatures2d::HarrisLaplaceFeatureDetector* cveHarrisLaplaceFeatureDetectorCreate(
	int numOctaves,
	float corn_thresh,
	float DOG_thresh,
	int maxCorners,
	int num_layers, 
	cv::Ptr<cv::xfeatures2d::HarrisLaplaceFeatureDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::Ptr<cv::xfeatures2d::HarrisLaplaceFeatureDetector> ptr = cv::xfeatures2d::HarrisLaplaceFeatureDetector::create(numOctaves, corn_thresh, DOG_thresh, maxCorners, num_layers);
	*sharedPtr = new cv::Ptr<cv::xfeatures2d::HarrisLaplaceFeatureDetector>(ptr);
	return ptr.get();
#else
	throw_no_xfeatures2d();
#endif
}
void cveHarrisLaplaceFeatureDetectorRelease(cv::Ptr<cv::xfeatures2d::HarrisLaplaceFeatureDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xfeatures2d();
#endif
}

void cveMatchGMS(
	CvSize* size1, CvSize* size2,
	std::vector< cv::KeyPoint >* keypoints1, std::vector< cv::KeyPoint >* keypoints2,
	std::vector< cv::DMatch >* matches1to2, std::vector< cv::DMatch >* matchesGMS,
	bool withRotation, bool withScale, double thresholdFactor)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::xfeatures2d::matchGMS(*size1, *size2, *keypoints1, *keypoints2, *matches1to2, *matchesGMS, withRotation, withScale, thresholdFactor);
#else
	throw_no_xfeatures2d();
#endif
}