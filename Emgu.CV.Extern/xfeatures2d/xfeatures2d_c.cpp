//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "xfeatures2d_c.h"

//BEBLID
cv::xfeatures2d::BEBLID* cveBEBLIDCreate(
	float scaleFactor,
	int nBits,
	cv::Feature2D** feature2D,
	cv::Ptr<cv::xfeatures2d::BEBLID>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::Ptr<cv::xfeatures2d::BEBLID> detectorPtr = cv::xfeatures2d::BEBLID::create(
			scaleFactor,
			nBits);
		*sharedPtr = new cv::Ptr<cv::xfeatures2d::BEBLID>(detectorPtr);
		*feature2D = dynamic_cast<cv::Feature2D*>(detectorPtr.get());
		return detectorPtr.get();
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveBEBLIDRelease(cv::Ptr<cv::xfeatures2d::BEBLID>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete* sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//TBMR
cv::xfeatures2d::TBMR* cveTBMRCreate(
	int minArea,
	float maxAreaRelative,
	float scaleFactor,
	int nScales,
	cv::Feature2D** feature2D,
	cv::Ptr<cv::xfeatures2d::TBMR>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::Ptr<cv::xfeatures2d::TBMR> detectorPtr = cv::xfeatures2d::TBMR::create(
			minArea,
			maxAreaRelative,
			scaleFactor,
			nScales);
		*sharedPtr = new cv::Ptr<cv::xfeatures2d::TBMR>(detectorPtr);
		*feature2D = dynamic_cast<cv::Feature2D*>(detectorPtr.get());
		return detectorPtr.get();
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveTBMRRelease(cv::Ptr<cv::xfeatures2d::TBMR>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete* sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//StarDetector
cv::xfeatures2d::StarDetector* cveStarDetectorCreate(int maxSize, int responseThreshold, int lineThresholdProjected, int lineThresholdBinarized, int suppressNonmaxSize, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::StarDetector>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveStarDetectorRelease(cv::Ptr<cv::xfeatures2d::StarDetector>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete *sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
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
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveFreakRelease(cv::Ptr<cv::xfeatures2d::FREAK>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete *sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//Brief
cv::xfeatures2d::BriefDescriptorExtractor* cveBriefDescriptorExtractorCreate(int descriptorSize, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::BriefDescriptorExtractor>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveBriefDescriptorExtractorRelease(cv::Ptr<cv::xfeatures2d::BriefDescriptorExtractor>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete *sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
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
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveLUCIDRelease(cv::Ptr<cv::xfeatures2d::LUCID>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete *sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//LATCH
cv::xfeatures2d::LATCH* cveLATCHCreate(int bytes, bool rotationInvariance, int halfSsdSize, cv::Feature2D** extractor, cv::Ptr<cv::xfeatures2d::LATCH>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveLATCHRelease(cv::Ptr<cv::xfeatures2d::LATCH>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete *sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//DAISY
cv::xfeatures2d::DAISY* cveDAISYCreate(
	float radius, int qRadius, int qTheta,
	int qHist, int norm, cv::_InputArray* H,
	bool interpolation, bool useOrientation, 
	cv::Feature2D** extractor,
	cv::Ptr<cv::xfeatures2d::DAISY>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveDAISYRelease(cv::Ptr<cv::xfeatures2d::DAISY>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete* sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//BoostDesc
cv::xfeatures2d::BoostDesc* cveBoostDescCreate(int desc, bool useScaleOrientation, float scalefactor, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::BoostDesc>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveBoostDescRelease(cv::Ptr<cv::xfeatures2d::BoostDesc>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete *sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::xfeatures2d::MSDDetector* cveMSDDetectorCreate(
	int m_patch_radius,
	int m_search_area_radius,
	int m_nms_radius,
	int m_nms_scale_radius,
	float m_th_saliency,
	int m_kNN,
	float m_scale_factor,
	int m_n_scales,
	bool m_compute_orientation,
	cv::Feature2D** feature2D,
	cv::Ptr<cv::xfeatures2d::MSDDetector>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::Ptr<cv::xfeatures2d::MSDDetector> ptr = cv::xfeatures2d::MSDDetector::create(
			m_patch_radius,
			m_search_area_radius,
			m_nms_radius,
			m_nms_scale_radius,
			m_th_saliency,
			m_kNN,
			m_scale_factor,
			m_n_scales,
			m_compute_orientation);
		*sharedPtr = new cv::Ptr<cv::xfeatures2d::MSDDetector>(ptr);
		*feature2D = dynamic_cast<cv::Feature2D*>(ptr.get());
		return ptr.get();
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveMSDDetectorRelease(cv::Ptr<cv::xfeatures2d::MSDDetector>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete* sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//VGG
cv::xfeatures2d::VGG* cveVGGCreate(
	int desc, float isigma, bool imgNormalize, bool useScaleOrientation,
	float scaleFactor, bool dscNormalize, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::VGG>** sharedPtr)
{
	try
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
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveVGGRelease(cv::Ptr<cv::xfeatures2d::VGG>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete *sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::xfeatures2d::PCTSignatures* cvePCTSignaturesCreate(int initSampleCount, int initSeedCount, int pointDistribution, cv::Ptr<cv::xfeatures2d::PCTSignatures>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::Ptr<cv::xfeatures2d::PCTSignatures> ptr = cv::xfeatures2d::PCTSignatures::create(initSampleCount, initSeedCount, pointDistribution);
		*sharedPtr = new cv::Ptr<cv::xfeatures2d::PCTSignatures>(ptr);
		return ptr.get();
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::xfeatures2d::PCTSignatures* cvePCTSignaturesCreate2(std::vector<cv::Point2f>* initSamplingPoints, int initSeedCount, cv::Ptr<cv::xfeatures2d::PCTSignatures>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::Ptr<cv::xfeatures2d::PCTSignatures> ptr = cv::xfeatures2d::PCTSignatures::create(*initSamplingPoints, initSeedCount);
		*sharedPtr = new cv::Ptr<cv::xfeatures2d::PCTSignatures>(ptr);
		return ptr.get();
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::xfeatures2d::PCTSignatures* cvePCTSignaturesCreate3(std::vector<cv::Point2f>* initSamplingPoints, std::vector<int>* initClusterSeedIndexes, cv::Ptr<cv::xfeatures2d::PCTSignatures>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::Ptr<cv::xfeatures2d::PCTSignatures> ptr = cv::xfeatures2d::PCTSignatures::create(*initSamplingPoints, *initClusterSeedIndexes);
		*sharedPtr = new cv::Ptr<cv::xfeatures2d::PCTSignatures>(ptr);
		return ptr.get();
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cvePCTSignaturesRelease(cv::Ptr<cv::xfeatures2d::PCTSignatures>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete *sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cvePCTSignaturesComputeSignature(cv::xfeatures2d::PCTSignatures* pct, cv::_InputArray* image, cv::_OutputArray* signature)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		pct->computeSignature(*image, *signature);
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cvePCTSignaturesDrawSignature(cv::_InputArray* source, cv::_InputArray* signature, cv::_OutputArray* result, float radiusToShorterSideRatio, int borderThickness)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::xfeatures2d::PCTSignatures::drawSignature(*source, *signature, *result, radiusToShorterSideRatio, borderThickness);
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::xfeatures2d::PCTSignaturesSQFD* cvePCTSignaturesSQFDCreate(
	int distanceFunction,
	int similarityFunction,
	float similarityParameter, 
	cv::Ptr<cv::xfeatures2d::PCTSignaturesSQFD>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::Ptr<cv::xfeatures2d::PCTSignaturesSQFD> ptr = cv::xfeatures2d::PCTSignaturesSQFD::create(distanceFunction, similarityFunction, similarityParameter);
		*sharedPtr = new cv::Ptr<cv::xfeatures2d::PCTSignaturesSQFD>(ptr);
		return ptr.get();
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
float cvePCTSignaturesSQFDComputeQuadraticFormDistance(
	cv::xfeatures2d::PCTSignaturesSQFD* sqfd,
	cv::_InputArray* signature0,
	cv::_InputArray* signature1)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		return sqfd->computeQuadraticFormDistance(*signature0, *signature1);
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cvePCTSignaturesSQFDComputeQuadraticFormDistances(
	cv::xfeatures2d::PCTSignaturesSQFD* sqfd,
	cv::Mat* sourceSignature,
	std::vector<cv::Mat>* imageSignatures,
	std::vector<float>* distances)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		sqfd->computeQuadraticFormDistances(*sourceSignature, *imageSignatures, *distances);
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cvePCTSignaturesSQFDRelease(cv::Ptr<cv::xfeatures2d::PCTSignaturesSQFD>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete *sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::xfeatures2d::HarrisLaplaceFeatureDetector* cveHarrisLaplaceFeatureDetectorCreate(
	int numOctaves,
	float corn_thresh,
	float DOG_thresh,
	int maxCorners,
	int num_layers, 
	cv::Ptr<cv::xfeatures2d::HarrisLaplaceFeatureDetector>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::Ptr<cv::xfeatures2d::HarrisLaplaceFeatureDetector> ptr = cv::xfeatures2d::HarrisLaplaceFeatureDetector::create(numOctaves, corn_thresh, DOG_thresh, maxCorners, num_layers);
		*sharedPtr = new cv::Ptr<cv::xfeatures2d::HarrisLaplaceFeatureDetector>(ptr);
		return ptr.get();
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveHarrisLaplaceFeatureDetectorRelease(cv::Ptr<cv::xfeatures2d::HarrisLaplaceFeatureDetector>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete *sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveMatchGMS(
	cv::Size* size1, cv::Size* size2,
	std::vector< cv::KeyPoint >* keypoints1, std::vector< cv::KeyPoint >* keypoints2,
	std::vector< cv::DMatch >* matches1to2, std::vector< cv::DMatch >* matchesGMS,
	bool withRotation, bool withScale, double thresholdFactor)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::xfeatures2d::matchGMS(*size1, *size2, *keypoints1, *keypoints2, *matches1to2, *matchesGMS, withRotation, withScale, thresholdFactor);
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveMatchLOGOS(
	std::vector< cv::KeyPoint >*keypoints1,
	std::vector< cv::KeyPoint >*keypoints2,
	std::vector< int >* nn1,
	std::vector< int >* nn2,
	std::vector< cv::DMatch >* matches1to2)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::xfeatures2d::matchLOGOS(*keypoints1, *keypoints2, *nn1, *nn2, *matches1to2);
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}



//Brisk
cv::xfeatures2d::BRISK* cveBriskCreate(int thresh, int octaves, float patternScale, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::BRISK>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::Ptr<cv::xfeatures2d::BRISK> briskPtr = cv::xfeatures2d::BRISK::create(thresh, octaves, patternScale);
		*sharedPtr = new cv::Ptr<cv::xfeatures2d::BRISK>(briskPtr);
		*feature2D = dynamic_cast<cv::Feature2D*>(briskPtr.get());
		return briskPtr.get();
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveBriskRelease(cv::Ptr<cv::xfeatures2d::BRISK>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete* sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


//AKAZEDetector
cv::xfeatures2d::AKAZE* cveAKAZEDetectorCreate(
	int descriptorType, int descriptorSize, int descriptorChannels,
	float threshold, int octaves, int sublevels, int diffusivity,
	cv::Feature2D** feature2D,
	cv::Ptr<cv::xfeatures2d::AKAZE>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::Ptr<cv::xfeatures2d::AKAZE> akazePtr = cv::xfeatures2d::AKAZE::create(static_cast<cv::xfeatures2d::AKAZE::DescriptorType>(descriptorType), descriptorSize, descriptorChannels, threshold, octaves, sublevels, static_cast<cv::xfeatures2d::KAZE::DiffusivityType>(diffusivity));
		*sharedPtr = new cv::Ptr<cv::xfeatures2d::AKAZE>(akazePtr);
		*feature2D = dynamic_cast<cv::Feature2D*>(akazePtr.get());
		return akazePtr.get();
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveAKAZEDetectorRelease(cv::Ptr<cv::xfeatures2d::AKAZE>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete* sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


//KAZEDetector
cv::xfeatures2d::KAZE* cveKAZEDetectorCreate(
	bool extended, bool upright, float threshold,
	int octaves, int sublevels, int diffusivity,
	cv::Feature2D** feature2D,
	cv::Ptr<cv::xfeatures2d::KAZE>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::Ptr<cv::xfeatures2d::KAZE> kazePtr = cv::xfeatures2d::KAZE::create(extended, upright, threshold, octaves, sublevels, static_cast<cv::xfeatures2d::KAZE::DiffusivityType>(diffusivity));
		*sharedPtr = new cv::Ptr<cv::xfeatures2d::KAZE>(kazePtr);
		*feature2D = dynamic_cast<cv::Feature2D*>(kazePtr.get());

		return kazePtr.get();
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveKAZEDetectorRelease(cv::Ptr<cv::xfeatures2d::KAZE>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete* sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//Agast
cv::xfeatures2d::AgastFeatureDetector* cveAgastFeatureDetectorCreate(int threshold, bool nonmaxSuppression, int type, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::AgastFeatureDetector>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::Ptr<cv::xfeatures2d::AgastFeatureDetector> agastPtr = cv::xfeatures2d::AgastFeatureDetector::create(threshold, nonmaxSuppression, static_cast<cv::xfeatures2d::AgastFeatureDetector::DetectorType>(type));
		*sharedPtr = new cv::Ptr<cv::xfeatures2d::AgastFeatureDetector>(agastPtr);
		*feature2D = dynamic_cast<cv::Feature2D*>(agastPtr.get());
		return agastPtr.get();
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveAgastFeatureDetectorRelease(cv::Ptr<cv::xfeatures2d::AgastFeatureDetector>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete* sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


//BowKMeansTrainer
cv::xfeatures2d::BOWKMeansTrainer* cveBOWKMeansTrainerCreate(int clusterCount, const cv::TermCriteria* termcrit, int attempts, int flags)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		return new cv::xfeatures2d::BOWKMeansTrainer(clusterCount, *termcrit, attempts, flags);
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveBOWKMeansTrainerRelease(cv::xfeatures2d::BOWKMeansTrainer** trainer)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete* trainer;
		*trainer = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int cveBOWKMeansTrainerGetDescriptorCount(cv::xfeatures2d::BOWKMeansTrainer* trainer)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		return trainer->descriptorsCount();
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveBOWKMeansTrainerAdd(cv::xfeatures2d::BOWKMeansTrainer* trainer, cv::Mat* descriptors)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		trainer->add(*descriptors);
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveBOWKMeansTrainerCluster(cv::xfeatures2d::BOWKMeansTrainer* trainer, cv::_OutputArray* cluster)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::Mat m = trainer->cluster();
		m.copyTo(*cluster);
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//BOWImgDescriptorExtractor
cv::xfeatures2d::BOWImgDescriptorExtractor* cveBOWImgDescriptorExtractorCreate(cv::Feature2D* descriptorExtractor, cv::DescriptorMatcher* descriptorMatcher)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::Ptr<cv::Feature2D> extractorPtr(descriptorExtractor, [](cv::Feature2D*) {});

		cv::Ptr<cv::DescriptorMatcher> matcherPtr(descriptorMatcher, [](cv::DescriptorMatcher*) {});

		return new cv::xfeatures2d::BOWImgDescriptorExtractor(extractorPtr, matcherPtr);
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveBOWImgDescriptorExtractorRelease(cv::xfeatures2d::BOWImgDescriptorExtractor** descriptorExtractor)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete* descriptorExtractor;
		*descriptorExtractor = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveBOWImgDescriptorExtractorSetVocabulary(cv::xfeatures2d::BOWImgDescriptorExtractor* bowImgDescriptorExtractor, cv::Mat* vocabulary)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		bowImgDescriptorExtractor->setVocabulary(*vocabulary);
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveBOWImgDescriptorExtractorCompute(cv::xfeatures2d::BOWImgDescriptorExtractor* bowImgDescriptorExtractor, cv::_InputArray* image, std::vector<cv::KeyPoint>* keypoints, cv::Mat* imgDescriptor)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		bowImgDescriptorExtractor->compute(*image, *keypoints, *imgDescriptor);
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


//TEBLID
cv::xfeatures2d::TEBLID* cveTEBLIDCreate(
	float scaleFactor,
	int nBits,
	cv::Feature2D** feature2D,
	cv::Ptr<cv::xfeatures2d::TEBLID>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		cv::Ptr<cv::xfeatures2d::TEBLID> detectorPtr = cv::xfeatures2d::TEBLID::create(
			scaleFactor,
			nBits);
		*sharedPtr = new cv::Ptr<cv::xfeatures2d::TEBLID>(detectorPtr);
		*feature2D = dynamic_cast<cv::Feature2D*>(detectorPtr.get());
		return detectorPtr.get();
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveTEBLIDRelease(cv::Ptr<cv::xfeatures2d::TEBLID>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_XFEATURES2D
		delete *sharedPtr;
		*sharedPtr = 0;
	#else
		throw_no_xfeatures2d();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

