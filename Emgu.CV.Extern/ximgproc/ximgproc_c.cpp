//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "ximgproc_c.h"

void cveDtFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, double sigmaSpatial, double sigmaColor, int mode, int numIters)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::dtFilter(*guide, *src, *dst, sigmaSpatial, sigmaColor, mode, numIters);
#else
	throw_no_ximgproc();
#endif
}

void cveGuidedFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, int radius, double eps, int dDepth)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::guidedFilter(*guide, *src, *dst, radius, eps, dDepth);
#else
	throw_no_ximgproc();
#endif
}

void cveAmFilter(cv::_InputArray* joint, cv::_InputArray* src, cv::_OutputArray* dst, double sigmaS, double sigmaR, bool adjustOutliers)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::amFilter(*joint, *src, *dst, sigmaS, sigmaR, adjustOutliers);
#else
	throw_no_ximgproc();
#endif
}

void cveJointBilateralFilter(cv::_InputArray* joint, cv::_InputArray* src, cv::_OutputArray* dst, int d, double sigmaColor, double sigmaSpace, int borderType)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::jointBilateralFilter(*joint, *src, *dst, d, sigmaColor, sigmaSpace, borderType);
#else
	throw_no_ximgproc();
#endif
}

void cveBilateralTextureFilter(cv::_InputArray* src, cv::_OutputArray* dst, int fr, int numIter, double sigmaAlpha, double sigmaAvg)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::bilateralTextureFilter(*src, *dst, fr, numIter, sigmaAlpha, sigmaAvg);
#else
	throw_no_ximgproc();
#endif
}

void cveRollingGuidanceFilter(cv::_InputArray* src, cv::_OutputArray* dst, int d, double sigmaColor, double sigmaSpace, int numOfIter, int borderType)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::rollingGuidanceFilter(*src, *dst, d, sigmaColor, sigmaSpace, numOfIter, borderType);
#else
	throw_no_ximgproc();
#endif
}

void cveFastGlobalSmootherFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, double lambda, double sigmaColor, double lambdaAttenuation, int numIter)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::fastGlobalSmootherFilter(*guide, *src, *dst, lambda, sigmaColor, lambdaAttenuation, numIter);
#else
	throw_no_ximgproc();
#endif
}

void cveL0Smooth(cv::_InputArray* src, cv::_OutputArray* dst, double lambda, double kappa)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::l0Smooth(*src, *dst, lambda, kappa);
#else
	throw_no_ximgproc();
#endif
}

void cveNiBlackThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double maxValue, int type, int blockSize, double delta)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::niBlackThreshold(*src, *dst, maxValue, type, blockSize, delta);
#else
	throw_no_ximgproc();
#endif
}

void cveCovarianceEstimation(cv::_InputArray* src, cv::_OutputArray* dst, int windowRows, int windowCols)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::covarianceEstimation(*src, *dst, windowRows, windowCols);
#else
	throw_no_ximgproc();
#endif
}

cv::ximgproc::DTFilter* cveDTFilterCreate(cv::_InputArray* guide, double sigmaSpatial, double sigmaColor, int mode, int numIters, cv::Ptr<cv::ximgproc::DTFilter>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::Ptr<cv::ximgproc::DTFilter> ptr = cv::ximgproc::createDTFilter(*guide, sigmaSpatial, sigmaColor, mode, numIters);
	*sharedPtr = new cv::Ptr<cv::ximgproc::DTFilter>(ptr);
	return ptr.get();
#else
	throw_no_ximgproc();
#endif
}

void cveDTFilterFilter(cv::ximgproc::DTFilter* filter, cv::_InputArray* src, cv::_OutputArray* dst, int dDepth)
{
#ifdef HAVE_OPENCV_XIMGPROC
	filter->filter(*src, *dst, dDepth);
#else
	throw_no_ximgproc();
#endif
}
void cveDTFilterRelease(cv::ximgproc::DTFilter** filter, cv::Ptr<cv::ximgproc::DTFilter>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	delete *sharedPtr;
	*filter = 0;
	*sharedPtr = 0;
#else
	throw_no_ximgproc();
#endif
}

cv::ximgproc::RFFeatureGetter* cveRFFeatureGetterCreate(cv::Ptr<cv::ximgproc::RFFeatureGetter>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::Ptr<cv::ximgproc::RFFeatureGetter> ptr = cv::ximgproc::createRFFeatureGetter();
	*sharedPtr = new cv::Ptr<cv::ximgproc::RFFeatureGetter>(ptr);
	return ptr.get();
#else
	throw_no_ximgproc();
#endif
}
void cveRFFeatureGetterRelease(cv::ximgproc::RFFeatureGetter** getter, cv::Ptr<cv::ximgproc::RFFeatureGetter>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	delete *sharedPtr;
	*getter = 0;
	*sharedPtr = 0;
#else
	throw_no_ximgproc();
#endif
}


cv::ximgproc::StructuredEdgeDetection* cveStructuredEdgeDetectionCreate(cv::String* model, cv::ximgproc::RFFeatureGetter* howToGetFeatures, cv::Ptr<cv::ximgproc::StructuredEdgeDetection>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::Ptr<cv::ximgproc::RFFeatureGetter> getterPtr(howToGetFeatures, [](cv::ximgproc::RFFeatureGetter*){});
	cv::Ptr<cv::ximgproc::StructuredEdgeDetection> ptr = cv::ximgproc::createStructuredEdgeDetection(*model, getterPtr);
	*sharedPtr = new cv::Ptr<cv::ximgproc::StructuredEdgeDetection>(ptr);
	return ptr.get();
#else
	throw_no_ximgproc();
#endif
}
void cveStructuredEdgeDetectionDetectEdges(cv::ximgproc::StructuredEdgeDetection* detection, cv::_InputArray* src, cv::_OutputArray* dst)
{
#ifdef HAVE_OPENCV_XIMGPROC
	detection->detectEdges(*src, *dst);
#else
	throw_no_ximgproc();
#endif
}
void cveStructuredEdgeDetectionComputeOrientation(cv::ximgproc::StructuredEdgeDetection* detection, cv::_InputArray* src, cv::_OutputArray* dst)
{
#ifdef HAVE_OPENCV_XIMGPROC
	detection->computeOrientation(*src, *dst);
#else
	throw_no_ximgproc();
#endif
}
void cveStructuredEdgeDetectionEdgesNms(cv::ximgproc::StructuredEdgeDetection* detection, cv::_InputArray* edgeImage, cv::_InputArray* orientationImage, cv::_OutputArray* dst, int r, int s, float m, bool isParallel)
{
#ifdef HAVE_OPENCV_XIMGPROC
	detection->edgesNms(*edgeImage, *orientationImage, *dst, r, s, m, isParallel);
#else
	throw_no_ximgproc();
#endif
}
void cveStructuredEdgeDetectionRelease(cv::ximgproc::StructuredEdgeDetection** detection, cv::Ptr<cv::ximgproc::StructuredEdgeDetection>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	delete *sharedPtr;
	*detection = 0;
	*sharedPtr = 0;
#else
	throw_no_ximgproc();
#endif
}

cv::ximgproc::SuperpixelSEEDS* cveSuperpixelSEEDSCreate(
	int imageWidth, int imageHeight, int imageChannels,
	int numSuperpixels, int numLevels, int prior,
	int histogramBins, bool doubleStep,
	cv::Ptr<cv::ximgproc::SuperpixelSEEDS>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::Ptr<cv::ximgproc::SuperpixelSEEDS> ptr = cv::ximgproc::createSuperpixelSEEDS(imageWidth, imageHeight, imageChannels, numSuperpixels, numLevels, prior, histogramBins, doubleStep);
	*sharedPtr = new cv::Ptr<cv::ximgproc::SuperpixelSEEDS>(ptr);
	return ptr.get();
#else
	throw_no_ximgproc();
#endif
}
int cveSuperpixelSEEDSGetNumberOfSuperpixels(cv::ximgproc::SuperpixelSEEDS* seeds)
{
#ifdef HAVE_OPENCV_XIMGPROC
	return seeds->getNumberOfSuperpixels();
#else
	throw_no_ximgproc();
#endif
}
void cveSuperpixelSEEDSGetLabels(cv::ximgproc::SuperpixelSEEDS* seeds, cv::_OutputArray* labelsOut)
{
#ifdef HAVE_OPENCV_XIMGPROC
	seeds->getLabels(*labelsOut);
#else
	throw_no_ximgproc();
#endif
}
void cveSuperpixelSEEDSGetLabelContourMask(cv::ximgproc::SuperpixelSEEDS* seeds, cv::_OutputArray* image, bool thickLine)
{
#ifdef HAVE_OPENCV_XIMGPROC
	seeds->getLabelContourMask(*image, thickLine);
#else
	throw_no_ximgproc();
#endif
}
void cveSuperpixelSEEDSIterate(cv::ximgproc::SuperpixelSEEDS* seeds, cv::_InputArray* img, int numIterations)
{
#ifdef HAVE_OPENCV_XIMGPROC
	seeds->iterate(*img, numIterations);
#else
	throw_no_ximgproc();
#endif
}
void cveSuperpixelSEEDSRelease(cv::ximgproc::SuperpixelSEEDS** seeds, cv::Ptr<cv::ximgproc::SuperpixelSEEDS>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	delete *sharedPtr;
	*seeds = 0;
	*sharedPtr = 0;
#else
	throw_no_ximgproc();
#endif
}


cv::ximgproc::SuperpixelLSC* cveSuperpixelLSCCreate(cv::_InputArray* image, int regionSize, float ratio, cv::Ptr<cv::ximgproc::SuperpixelLSC>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::Ptr<cv::ximgproc::SuperpixelLSC> ptr = cv::ximgproc::createSuperpixelLSC(*image, regionSize, ratio);
	*sharedPtr = new cv::Ptr < cv::ximgproc::SuperpixelLSC>(ptr);
	return ptr.get();
#else
	throw_no_ximgproc();
#endif
}
int cveSuperpixelLSCGetNumberOfSuperpixels(cv::ximgproc::SuperpixelLSC* lsc)
{
#ifdef HAVE_OPENCV_XIMGPROC
	return lsc->getNumberOfSuperpixels();
#else
	throw_no_ximgproc();
#endif
}
void cveSuperpixelLSCIterate(cv::ximgproc::SuperpixelLSC* lsc, int numIterations)
{
#ifdef HAVE_OPENCV_XIMGPROC
	lsc->iterate(numIterations);
#else
	throw_no_ximgproc();
#endif
}
void cveSuperpixelLSCGetLabels(cv::ximgproc::SuperpixelLSC* lsc, cv::_OutputArray* labelsOut)
{
#ifdef HAVE_OPENCV_XIMGPROC
	lsc->getLabels(*labelsOut);
#else
	throw_no_ximgproc();
#endif
}
void cveSuperpixelLSCGetLabelContourMask(cv::ximgproc::SuperpixelLSC* lsc, cv::_OutputArray* image, bool thickLine)
{
#ifdef HAVE_OPENCV_XIMGPROC
	lsc->getLabelContourMask(*image, thickLine);
#else
	throw_no_ximgproc();
#endif
}
void cveSuperpixelLSCEnforceLabelConnectivity(cv::ximgproc::SuperpixelLSC* lsc, int minElementSize)
{
#ifdef HAVE_OPENCV_XIMGPROC
	lsc->enforceLabelConnectivity(minElementSize);
#else
	throw_no_ximgproc();
#endif
}
void cveSuperpixelLSCRelease(cv::ximgproc::SuperpixelLSC** lsc, cv::Ptr<cv::ximgproc::SuperpixelLSC>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	delete *sharedPtr;
	*lsc = 0;
	*sharedPtr = 0;
#else
	throw_no_ximgproc();
#endif
}


cv::ximgproc::SuperpixelSLIC* cveSuperpixelSLICCreate(cv::_InputArray* image, int algorithm, int regionSize, float ruler, cv::Ptr<cv::ximgproc::SuperpixelSLIC>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::Ptr<cv::ximgproc::SuperpixelSLIC> ptr = cv::ximgproc::createSuperpixelSLIC(*image, algorithm, regionSize, ruler);
	*sharedPtr = new cv::Ptr<cv::ximgproc::SuperpixelSLIC>(ptr);
	return ptr.get();
#else
	throw_no_ximgproc();
#endif
}
int cveSuperpixelSLICGetNumberOfSuperpixels(cv::ximgproc::SuperpixelSLIC* slic)
{
#ifdef HAVE_OPENCV_XIMGPROC
	return slic->getNumberOfSuperpixels();
#else
	throw_no_ximgproc();
#endif
}
void cveSuperpixelSLICIterate(cv::ximgproc::SuperpixelSLIC* slic, int numIterations)
{
#ifdef HAVE_OPENCV_XIMGPROC
	slic->iterate(numIterations);
#else
	throw_no_ximgproc();
#endif
}
void cveSuperpixelSLICGetLabels(cv::ximgproc::SuperpixelSLIC* slic, cv::_OutputArray* labelsOut)
{
#ifdef HAVE_OPENCV_XIMGPROC
	slic->getLabels(*labelsOut);
#else
	throw_no_ximgproc();
#endif
}
void cveSuperpixelSLICGetLabelContourMask(cv::ximgproc::SuperpixelSLIC* slic, cv::_OutputArray* image, bool thickLine)
{
#ifdef HAVE_OPENCV_XIMGPROC
	slic->getLabelContourMask(*image, thickLine);
#else
	throw_no_ximgproc();
#endif
}
void cveSuperpixelSLICEnforceLabelConnectivity(cv::ximgproc::SuperpixelSLIC* slic, int minElementSize)
{
#ifdef HAVE_OPENCV_XIMGPROC
	slic->enforceLabelConnectivity(minElementSize);
#else
	throw_no_ximgproc();
#endif
}
void cveSuperpixelSLICRelease(cv::ximgproc::SuperpixelSLIC** slic, cv::Ptr<cv::ximgproc::SuperpixelSLIC>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	delete *sharedPtr;
	*slic = 0;
	*sharedPtr = 0;
#else
	throw_no_ximgproc();
#endif
}


cv::ximgproc::segmentation::GraphSegmentation* cveGraphSegmentationCreate(double sigma, float k, int minSize, cv::Ptr<cv::ximgproc::segmentation::GraphSegmentation>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::Ptr<cv::ximgproc::segmentation::GraphSegmentation> ptr = cv::ximgproc::segmentation::createGraphSegmentation(sigma, k, minSize);
	*sharedPtr = new cv::Ptr<cv::ximgproc::segmentation::GraphSegmentation>(ptr);
	return ptr.get();
#else
	throw_no_ximgproc();
#endif
}
void cveGraphSegmentationProcessImage(cv::ximgproc::segmentation::GraphSegmentation* segmentation, cv::_InputArray* src, cv::_OutputArray* dst)
{
#ifdef HAVE_OPENCV_XIMGPROC
	segmentation->processImage(*src, *dst);
#else
	throw_no_ximgproc();
#endif
}
void cveGraphSegmentationRelease(cv::ximgproc::segmentation::GraphSegmentation** segmentation, cv::Ptr<cv::ximgproc::segmentation::GraphSegmentation>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	delete *sharedPtr;
	*segmentation = 0;
	*sharedPtr = 0;
#else
	throw_no_ximgproc();
#endif
}

void cveWeightedMedianFilter(cv::_InputArray* joint, cv::_InputArray* src, cv::_OutputArray* dst, int r, double sigma, cv::ximgproc::WMFWeightType weightType, cv::Mat* mask)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::weightedMedianFilter(*joint, *src, *dst, r, sigma, weightType, mask ? *mask : cv::Mat());
#else
	throw_no_ximgproc();
#endif
}


cv::ximgproc::segmentation::SelectiveSearchSegmentation* cveSelectiveSearchSegmentationCreate(cv::Ptr<cv::ximgproc::segmentation::SelectiveSearchSegmentation>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::Ptr<cv::ximgproc::segmentation::SelectiveSearchSegmentation> ptr = cv::ximgproc::segmentation::createSelectiveSearchSegmentation();
	*sharedPtr = new cv::Ptr<cv::ximgproc::segmentation::SelectiveSearchSegmentation>(ptr);
	return ptr.get();
#else
	throw_no_ximgproc();
#endif
}
void cveSelectiveSearchSegmentationSetBaseImage(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, cv::_InputArray* image)
{
#ifdef HAVE_OPENCV_XIMGPROC
	segmentation->setBaseImage(*image);
#else
	throw_no_ximgproc();
#endif
}
void cveSelectiveSearchSegmentationSwitchToSingleStrategy(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, int k, float sigma)
{
#ifdef HAVE_OPENCV_XIMGPROC
	segmentation->switchToSingleStrategy(k, sigma);
#else
	throw_no_ximgproc();
#endif
}
void cveSelectiveSearchSegmentationSwitchToSelectiveSearchFast(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, int baseK, int incK, float sigma)
{
#ifdef HAVE_OPENCV_XIMGPROC
	segmentation->switchToSelectiveSearchFast(baseK, incK, sigma);
#else
	throw_no_ximgproc();
#endif
}
void cveSelectiveSearchSegmentationSwitchToSelectiveSearchQuality(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, int baseK, int incK, float sigma)
{
#ifdef HAVE_OPENCV_XIMGPROC
	segmentation->switchToSelectiveSearchQuality(baseK, incK, sigma);
#else
	throw_no_ximgproc();
#endif
}
void cveSelectiveSearchSegmentationAddImage(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, cv::_InputArray* img)
{
#ifdef HAVE_OPENCV_XIMGPROC
	segmentation->addImage(*img);
#else
	throw_no_ximgproc();
#endif
}
void cveSelectiveSearchSegmentationProcess(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, std::vector<cv::Rect>* rects)
{
#ifdef HAVE_OPENCV_XIMGPROC
	segmentation->process(*rects);
#else
	throw_no_ximgproc();
#endif
}
void cveSelectiveSearchSegmentationRelease(cv::ximgproc::segmentation::SelectiveSearchSegmentation** segmentation, cv::Ptr<cv::ximgproc::segmentation::SelectiveSearchSegmentation>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	delete *sharedPtr;
	*segmentation = 0;
	*sharedPtr = 0;
#else
	throw_no_ximgproc();
#endif
}

void cveGradientPaillouY(cv::_InputArray* op, cv::_OutputArray* dst, double alpha, double omega)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::GradientPaillouY(*op, *dst, alpha, omega);
#else
	throw_no_ximgproc();
#endif
}
void cveGradientPaillouX(cv::_InputArray* op, cv::_OutputArray* dst, double alpha, double omega)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::GradientPaillouX(*op, *dst, alpha, omega);
#else
	throw_no_ximgproc();
#endif
}
void cveGradientDericheY(cv::_InputArray* op, cv::_OutputArray* dst, double alphaDerive, double alphaMean)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::GradientDericheY(*op, *dst, alphaDerive, alphaMean);
#else
	throw_no_ximgproc();
#endif
}
void cveGradientDericheX(cv::_InputArray* op, cv::_OutputArray* dst, double alphaDerive, double alphaMean)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::GradientDericheX(*op, *dst, alphaDerive, alphaMean);
#else
	throw_no_ximgproc();
#endif
}


void cveThinning(cv::_InputArray* src, cv::_OutputArray* dst, int thinningType)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::thinning(*src, *dst, thinningType);
#else
	throw_no_ximgproc();
#endif
}

void cveAnisotropicDiffusion(cv::_InputArray* src, cv::_OutputArray* dst, float alpha, float K, int niters)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::anisotropicDiffusion(*src, *dst, alpha, K, niters);
#else
	throw_no_ximgproc();
#endif
}


cv::ximgproc::FastLineDetector* cveFastLineDetectorCreate(
	int length_threshold,
	float distance_threshold,
	double canny_th1,
	double canny_th2,
	int canny_aperture_size,
	bool do_merge,
	cv::Ptr<cv::ximgproc::FastLineDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::Ptr<cv::ximgproc::FastLineDetector> ptr =
		cv::ximgproc::createFastLineDetector(
			length_threshold,
			distance_threshold,
			canny_th1,
			canny_th2,
			canny_aperture_size,
			do_merge);
	*sharedPtr = new cv::Ptr<cv::ximgproc::FastLineDetector>(ptr);
	return ptr.get();
#else
	throw_no_ximgproc();
#endif
}

void cveFastLineDetectorDetect(cv::ximgproc::FastLineDetector* fld, cv::_InputArray* image, cv::_OutputArray* lines)
{
#ifdef HAVE_OPENCV_XIMGPROC
	fld->detect(*image, *lines);
#else
	throw_no_ximgproc();
#endif
}

void cveFastLineDetectorDrawSegments(cv::ximgproc::FastLineDetector* fld, cv::_InputOutputArray* image, cv::_InputArray* lines, bool draw_arrow)
{
#ifdef HAVE_OPENCV_XIMGPROC
	fld->drawSegments(*image, *lines, draw_arrow);
#else
	throw_no_ximgproc();
#endif
}

void cveFastLineDetectorRelease(cv::Ptr<cv::ximgproc::FastLineDetector>** fld)
{
#ifdef HAVE_OPENCV_XIMGPROC
	delete *fld;
	*fld = 0;
#else
	throw_no_ximgproc();
#endif
}

void cveBrightEdges(cv::Mat* original, cv::Mat* edgeview, int contrast, int shortrange, int longrange)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::ximgproc::BrightEdges(*original, *edgeview, contrast, shortrange, longrange);
#else
	throw_no_ximgproc();
#endif
}

cv::ximgproc::DisparityWLSFilter* cveCreateDisparityWLSFilter(cv::StereoMatcher* matcherLeft, cv::ximgproc::DisparityFilter** disparityFilter, cv::Algorithm** algorithm, cv::Ptr<cv::ximgproc::DisparityWLSFilter>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::Ptr<cv::StereoMatcher> sm = cv::Ptr<cv::StereoMatcher>(matcherLeft, [](cv::StereoMatcher*) {});
	cv::Ptr<cv::ximgproc::DisparityWLSFilter> filter = cv::ximgproc::createDisparityWLSFilter(sm);
	*sharedPtr = new cv::Ptr<cv::ximgproc::DisparityWLSFilter>(filter);
	*disparityFilter = (*sharedPtr)->dynamicCast<cv::ximgproc::DisparityFilter>();
	*algorithm = (*sharedPtr)->dynamicCast<cv::Algorithm>();
	return (*sharedPtr)->get();
#else
	throw_no_ximgproc();
#endif
}
cv::StereoMatcher* cveCreateRightMatcher(cv::StereoMatcher* matcherLeft, cv::Ptr<cv::StereoMatcher>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::Ptr<cv::StereoMatcher> ml = cv::Ptr<cv::StereoMatcher>(matcherLeft, [](cv::StereoMatcher*) {});
	cv::Ptr<cv::StereoMatcher> sm = cv::ximgproc::createRightMatcher(ml);
	*sharedPtr = new cv::Ptr<cv::StereoMatcher>(sm);
	return (*sharedPtr)->get();
#else
	throw_no_ximgproc();
#endif
}
cv::ximgproc::DisparityWLSFilter* cveCreateDisparityWLSFilterGeneric(bool use_confidence, cv::ximgproc::DisparityFilter** disparityFilter, cv::Algorithm** algorithm, cv::Ptr<cv::ximgproc::DisparityWLSFilter>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::Ptr<cv::ximgproc::DisparityWLSFilter> filter = cv::ximgproc::createDisparityWLSFilterGeneric(use_confidence);
	*sharedPtr = new cv::Ptr<cv::ximgproc::DisparityWLSFilter>(filter);
	*disparityFilter = (*sharedPtr)->dynamicCast<cv::ximgproc::DisparityFilter>();
	*algorithm = (*sharedPtr)->dynamicCast<cv::Algorithm>();
	return (*sharedPtr)->get();
#else
	throw_no_ximgproc();
#endif
}
void cveDisparityWLSFilterRelease(cv::Ptr<cv::ximgproc::DisparityWLSFilter>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_ximgproc();
#endif
}

void cveDisparityFilterFilter(
	cv::ximgproc::DisparityFilter* disparityFilter,
	cv::_InputArray* disparity_map_left, cv::_InputArray* left_view, cv::_OutputArray* filtered_disparity_map,
	cv::_InputArray* disparity_map_right, CvRect* ROI, cv::_InputArray* right_view)
{
#ifdef HAVE_OPENCV_XIMGPROC
	disparityFilter->filter(
		*disparity_map_left, 
		*left_view, 
		*filtered_disparity_map, 
		disparity_map_right->empty() ? cv::Mat() : *disparity_map_right, 
		*ROI, 
		right_view->empty() ? cv::Mat() : *right_view);
#else
	throw_no_ximgproc();
#endif
}

cv::ximgproc::RidgeDetectionFilter* cveRidgeDetectionFilterCreate(
	int ddepth,
	int dx,
	int dy,
	int ksize,
	int outDtype,
	double scale,
	double delta,
	int borderType,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::ximgproc::RidgeDetectionFilter>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::Ptr<cv::ximgproc::RidgeDetectionFilter> filter = cv::ximgproc::RidgeDetectionFilter::create(
		ddepth,
		dx,
		dy,
		ksize,
		outDtype,
		scale,
		delta,
		borderType);
	*sharedPtr = new cv::Ptr<cv::ximgproc::RidgeDetectionFilter>(filter);
	*algorithm = (*sharedPtr)->dynamicCast<cv::Algorithm>();
	return (*sharedPtr)->get();
#else
	throw_no_ximgproc();
#endif
}
void cveRidgeDetectionFilterRelease(cv::Ptr<cv::ximgproc::RidgeDetectionFilter>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_ximgproc();
#endif
}
void cveRidgeDetectionFilterGetRidgeFilteredImage(cv::ximgproc::RidgeDetectionFilter* ridgeDetection, cv::_InputArray* img, cv::_OutputArray* out)
{
#ifdef HAVE_OPENCV_XIMGPROC
	ridgeDetection->getRidgeFilteredImage(*img, *out);
#else
	throw_no_ximgproc();
#endif
}

cv::ximgproc::EdgeBoxes* cveEdgeBoxesCreate(
	float alpha,
	float beta,
	float eta,
	float minScore,
	int   maxBoxes,
	float edgeMinMag,
	float edgeMergeThr,
	float clusterMinMag,
	float maxAspectRatio,
	float minBoxArea,
	float gamma,
	float kappa,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::ximgproc::EdgeBoxes>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	cv::Ptr<cv::ximgproc::EdgeBoxes> ptr = cv::ximgproc::createEdgeBoxes(
		alpha, beta, eta, minScore, maxBoxes, edgeMinMag, edgeMergeThr, clusterMinMag, maxAspectRatio, minBoxArea, gamma, kappa);
	*sharedPtr = new cv::Ptr<cv::ximgproc::EdgeBoxes>(ptr);
	*algorithm = (*sharedPtr)->dynamicCast<cv::Algorithm>();
	return (*sharedPtr)->get();
#else
	throw_no_ximgproc();
#endif
}
void cveEdgeBoxesGetBoundingBoxes(cv::ximgproc::EdgeBoxes* edgeBoxes, cv::_InputArray* edgeMap, cv::_InputArray* orientationMap, std::vector<cv::Rect>* boxes)
{
#ifdef HAVE_OPENCV_XIMGPROC
	//std::vector<cv::Rect> b;
	//for (std::vector<CvRect>::iterator it = boxes->begin(); it != boxes->end(); it++)
	//{
	//	b.push_back(*it);
	//}
	edgeBoxes->getBoundingBoxes(*edgeMap, *orientationMap, *boxes);
#else
	throw_no_ximgproc();
#endif
}
void cveEdgeBoxesRelease(cv::Ptr<cv::ximgproc::EdgeBoxes>** sharedPtr)
{
#ifdef HAVE_OPENCV_XIMGPROC
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_ximgproc();
#endif
}