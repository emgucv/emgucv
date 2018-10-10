//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "ximgproc_c.h"

void cveDtFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, double sigmaSpatial, double sigmaColor, int mode, int numIters)
{
	cv::ximgproc::dtFilter(*guide, *src, *dst, sigmaSpatial, sigmaColor, mode, numIters);
}

void cveGuidedFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, int radius, double eps, int dDepth)
{
	cv::ximgproc::guidedFilter(*guide, *src, *dst, radius, eps, dDepth);
}

void cveAmFilter(cv::_InputArray* joint, cv::_InputArray* src, cv::_OutputArray* dst, double sigmaS, double sigmaR, bool adjustOutliers)
{
	cv::ximgproc::amFilter(*joint, *src, *dst, sigmaS, sigmaR, adjustOutliers);
}

void cveJointBilateralFilter(cv::_InputArray* joint, cv::_InputArray* src, cv::_OutputArray* dst, int d, double sigmaColor, double sigmaSpace, int borderType)
{
	cv::ximgproc::jointBilateralFilter(*joint, *src, *dst, d, sigmaColor, sigmaSpace, borderType);
}

void cveBilateralTextureFilter(cv::_InputArray* src, cv::_OutputArray* dst, int fr, int numIter, double sigmaAlpha, double sigmaAvg)
{
	cv::ximgproc::bilateralTextureFilter(*src, *dst, fr, numIter, sigmaAlpha, sigmaAvg);
}

void cveRollingGuidanceFilter(cv::_InputArray* src, cv::_OutputArray* dst, int d, double sigmaColor, double sigmaSpace, int numOfIter, int borderType)
{
	cv::ximgproc::rollingGuidanceFilter(*src, *dst, d, sigmaColor, sigmaSpace, numOfIter, borderType);
}

void cveFastGlobalSmootherFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, double lambda, double sigmaColor, double lambdaAttenuation, int numIter)
{
	cv::ximgproc::fastGlobalSmootherFilter(*guide, *src, *dst, lambda, sigmaColor, lambdaAttenuation, numIter);
}

void cveL0Smooth(cv::_InputArray* src, cv::_OutputArray* dst, double lambda, double kappa)
{
	cv::ximgproc::l0Smooth(*src, *dst, lambda, kappa);
}

void cveNiBlackThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double maxValue, int type, int blockSize, double delta)
{
	cv::ximgproc::niBlackThreshold(*src, *dst, maxValue, type, blockSize, delta);
}

void cveCovarianceEstimation(cv::_InputArray* src, cv::_OutputArray* dst, int windowRows, int windowCols)
{
	cv::ximgproc::covarianceEstimation(*src, *dst, windowRows, windowCols);
}

cv::ximgproc::DTFilter* cveDTFilterCreate(cv::_InputArray* guide, double sigmaSpatial, double sigmaColor, int mode, int numIters, cv::Ptr<cv::ximgproc::DTFilter>** sharedPtr)
{
	cv::Ptr<cv::ximgproc::DTFilter> ptr = cv::ximgproc::createDTFilter(*guide, sigmaSpatial, sigmaColor, mode, numIters);
	*sharedPtr = new cv::Ptr<cv::ximgproc::DTFilter>(ptr);
	return ptr.get();
}

void cveDTFilterFilter(cv::ximgproc::DTFilter* filter, cv::_InputArray* src, cv::_OutputArray* dst, int dDepth)
{
	filter->filter(*src, *dst, dDepth);
}
void cveDTFilterRelease(cv::ximgproc::DTFilter** filter, cv::Ptr<cv::ximgproc::DTFilter>** sharedPtr)
{
	delete *sharedPtr;
	*filter = 0;
	*sharedPtr = 0;
}

cv::ximgproc::RFFeatureGetter* cveRFFeatureGetterCreate(cv::Ptr<cv::ximgproc::RFFeatureGetter>** sharedPtr)
{
	cv::Ptr<cv::ximgproc::RFFeatureGetter> ptr = cv::ximgproc::createRFFeatureGetter();
	*sharedPtr = new cv::Ptr<cv::ximgproc::RFFeatureGetter>(ptr);
	return ptr.get();
}
void cveRFFeatureGetterRelease(cv::ximgproc::RFFeatureGetter** getter, cv::Ptr<cv::ximgproc::RFFeatureGetter>** sharedPtr)
{
	delete *sharedPtr;
	*getter = 0;
	*sharedPtr = 0;
}


cv::ximgproc::StructuredEdgeDetection* cveStructuredEdgeDetectionCreate(cv::String* model, cv::ximgproc::RFFeatureGetter* howToGetFeatures, cv::Ptr<cv::ximgproc::StructuredEdgeDetection>** sharedPtr)
{
	cv::Ptr<cv::ximgproc::RFFeatureGetter> getterPtr(howToGetFeatures, [](cv::ximgproc::RFFeatureGetter*){});
	cv::Ptr<cv::ximgproc::StructuredEdgeDetection> ptr = cv::ximgproc::createStructuredEdgeDetection(*model, getterPtr);
	*sharedPtr = new cv::Ptr<cv::ximgproc::StructuredEdgeDetection>(ptr);
	return ptr.get();
}
void cveStructuredEdgeDetectionDetectEdges(cv::ximgproc::StructuredEdgeDetection* detection, cv::_InputArray* src, cv::_OutputArray* dst)
{
	detection->detectEdges(*src, *dst);
}
void cveStructuredEdgeDetectionComputeOrientation(cv::ximgproc::StructuredEdgeDetection* detection, cv::_InputArray* src, cv::_OutputArray* dst)
{
	detection->computeOrientation(*src, *dst);
}
void cveStructuredEdgeDetectionEdgesNms(cv::ximgproc::StructuredEdgeDetection* detection, cv::_InputArray* edgeImage, cv::_InputArray* orientationImage, cv::_OutputArray* dst, int r, int s, float m, bool isParallel)
{
	detection->edgesNms(*edgeImage, *orientationImage, *dst, r, s, m, isParallel);
}
void cveStructuredEdgeDetectionRelease(cv::ximgproc::StructuredEdgeDetection** detection, cv::Ptr<cv::ximgproc::StructuredEdgeDetection>** sharedPtr)
{
	delete *sharedPtr;
	*detection = 0;
	*sharedPtr = 0;
}

cv::ximgproc::SuperpixelSEEDS* cveSuperpixelSEEDSCreate(
	int imageWidth, int imageHeight, int imageChannels,
	int numSuperpixels, int numLevels, int prior,
	int histogramBins, bool doubleStep,
	cv::Ptr<cv::ximgproc::SuperpixelSEEDS>** sharedPtr)
{
	cv::Ptr<cv::ximgproc::SuperpixelSEEDS> ptr = cv::ximgproc::createSuperpixelSEEDS(imageWidth, imageHeight, imageChannels, numSuperpixels, numLevels, prior, histogramBins, doubleStep);
	*sharedPtr = new cv::Ptr<cv::ximgproc::SuperpixelSEEDS>(ptr);
	return ptr.get();
}
int cveSuperpixelSEEDSGetNumberOfSuperpixels(cv::ximgproc::SuperpixelSEEDS* seeds)
{
	return seeds->getNumberOfSuperpixels();
}
void cveSuperpixelSEEDSGetLabels(cv::ximgproc::SuperpixelSEEDS* seeds, cv::_OutputArray* labelsOut)
{
	seeds->getLabels(*labelsOut);
}
void cveSuperpixelSEEDSGetLabelContourMask(cv::ximgproc::SuperpixelSEEDS* seeds, cv::_OutputArray* image, bool thickLine)
{
	seeds->getLabelContourMask(*image, thickLine);
}
void cveSuperpixelSEEDSIterate(cv::ximgproc::SuperpixelSEEDS* seeds, cv::_InputArray* img, int numIterations)
{
	seeds->iterate(*img, numIterations);
}
void cveSuperpixelSEEDSRelease(cv::ximgproc::SuperpixelSEEDS** seeds, cv::Ptr<cv::ximgproc::SuperpixelSEEDS>** sharedPtr)
{
	delete *sharedPtr;
	*seeds = 0;
	*sharedPtr = 0;
}


cv::ximgproc::SuperpixelLSC* cveSuperpixelLSCCreate(cv::_InputArray* image, int regionSize, float ratio, cv::Ptr<cv::ximgproc::SuperpixelLSC>** sharedPtr)
{
	cv::Ptr<cv::ximgproc::SuperpixelLSC> ptr = cv::ximgproc::createSuperpixelLSC(*image, regionSize, ratio);
	*sharedPtr = new cv::Ptr < cv::ximgproc::SuperpixelLSC>(ptr);
	return ptr.get();
}
int cveSuperpixelLSCGetNumberOfSuperpixels(cv::ximgproc::SuperpixelLSC* lsc)
{
	return lsc->getNumberOfSuperpixels();
}
void cveSuperpixelLSCIterate(cv::ximgproc::SuperpixelLSC* lsc, int numIterations)
{
	lsc->iterate(numIterations);
}
void cveSuperpixelLSCGetLabels(cv::ximgproc::SuperpixelLSC* lsc, cv::_OutputArray* labelsOut)
{
	lsc->getLabels(*labelsOut);
}
void cveSuperpixelLSCGetLabelContourMask(cv::ximgproc::SuperpixelLSC* lsc, cv::_OutputArray* image, bool thickLine)
{
	lsc->getLabelContourMask(*image, thickLine);
}
void cveSuperpixelLSCEnforceLabelConnectivity(cv::ximgproc::SuperpixelLSC* lsc, int minElementSize)
{
	lsc->enforceLabelConnectivity(minElementSize);
}
void cveSuperpixelLSCRelease(cv::ximgproc::SuperpixelLSC** lsc, cv::Ptr<cv::ximgproc::SuperpixelLSC>** sharedPtr)
{
	delete *sharedPtr;
	*lsc = 0;
	*sharedPtr = 0;
}


cv::ximgproc::SuperpixelSLIC* cveSuperpixelSLICCreate(cv::_InputArray* image, int algorithm, int regionSize, float ruler, cv::Ptr<cv::ximgproc::SuperpixelSLIC>** sharedPtr)
{
	cv::Ptr<cv::ximgproc::SuperpixelSLIC> ptr = cv::ximgproc::createSuperpixelSLIC(*image, algorithm, regionSize, ruler);
	*sharedPtr = new cv::Ptr<cv::ximgproc::SuperpixelSLIC>(ptr);
	return ptr.get();
}
int cveSuperpixelSLICGetNumberOfSuperpixels(cv::ximgproc::SuperpixelSLIC* slic)
{
	return slic->getNumberOfSuperpixels();
}
void cveSuperpixelSLICIterate(cv::ximgproc::SuperpixelSLIC* slic, int numIterations)
{
	slic->iterate(numIterations);
}
void cveSuperpixelSLICGetLabels(cv::ximgproc::SuperpixelSLIC* slic, cv::_OutputArray* labelsOut)
{
	slic->getLabels(*labelsOut);
}
void cveSuperpixelSLICGetLabelContourMask(cv::ximgproc::SuperpixelSLIC* slic, cv::_OutputArray* image, bool thickLine)
{
	slic->getLabelContourMask(*image, thickLine);
}
void cveSuperpixelSLICEnforceLabelConnectivity(cv::ximgproc::SuperpixelSLIC* slic, int minElementSize)
{
	slic->enforceLabelConnectivity(minElementSize);
}
void cveSuperpixelSLICRelease(cv::ximgproc::SuperpixelSLIC** slic, cv::Ptr<cv::ximgproc::SuperpixelSLIC>** sharedPtr)
{
	delete *sharedPtr;
	*slic = 0;
	*sharedPtr = 0;
}


cv::ximgproc::segmentation::GraphSegmentation* cveGraphSegmentationCreate(double sigma, float k, int minSize, cv::Ptr<cv::ximgproc::segmentation::GraphSegmentation>** sharedPtr)
{
	cv::Ptr<cv::ximgproc::segmentation::GraphSegmentation> ptr = cv::ximgproc::segmentation::createGraphSegmentation(sigma, k, minSize);
	*sharedPtr = new cv::Ptr<cv::ximgproc::segmentation::GraphSegmentation>(ptr);
	return ptr.get();
}
void cveGraphSegmentationProcessImage(cv::ximgproc::segmentation::GraphSegmentation* segmentation, cv::_InputArray* src, cv::_OutputArray* dst)
{
	segmentation->processImage(*src, *dst);
}
void cveGraphSegmentationRelease(cv::ximgproc::segmentation::GraphSegmentation** segmentation, cv::Ptr<cv::ximgproc::segmentation::GraphSegmentation>** sharedPtr)
{
	delete *sharedPtr;
	*segmentation = 0;
	*sharedPtr = 0;
}

void cveWeightedMedianFilter(cv::_InputArray* joint, cv::_InputArray* src, cv::_OutputArray* dst, int r, double sigma, cv::ximgproc::WMFWeightType weightType, cv::Mat* mask)
{
	cv::ximgproc::weightedMedianFilter(*joint, *src, *dst, r, sigma, weightType, mask ? *mask : cv::Mat());
}


cv::ximgproc::segmentation::SelectiveSearchSegmentation* cveSelectiveSearchSegmentationCreate(cv::Ptr<cv::ximgproc::segmentation::SelectiveSearchSegmentation>** sharedPtr)
{
	cv::Ptr<cv::ximgproc::segmentation::SelectiveSearchSegmentation> ptr = cv::ximgproc::segmentation::createSelectiveSearchSegmentation();
	*sharedPtr = new cv::Ptr<cv::ximgproc::segmentation::SelectiveSearchSegmentation>(ptr);
	return ptr.get();
}
void cveSelectiveSearchSegmentationSetBaseImage(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, cv::_InputArray* image)
{
	segmentation->setBaseImage(*image);
}
void cveSelectiveSearchSegmentationSwitchToSingleStrategy(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, int k, float sigma)
{
	segmentation->switchToSingleStrategy(k, sigma);
}
void cveSelectiveSearchSegmentationSwitchToSelectiveSearchFast(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, int baseK, int incK, float sigma)
{
	segmentation->switchToSelectiveSearchFast(baseK, incK, sigma);
}
void cveSelectiveSearchSegmentationSwitchToSelectiveSearchQuality(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, int baseK, int incK, float sigma)
{
	segmentation->switchToSelectiveSearchQuality(baseK, incK, sigma);
}
void cveSelectiveSearchSegmentationAddImage(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, cv::_InputArray* img)
{
	segmentation->addImage(*img);
}
void cveSelectiveSearchSegmentationProcess(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, std::vector<cv::Rect>* rects)
{
	segmentation->process(*rects);
}
void cveSelectiveSearchSegmentationRelease(cv::ximgproc::segmentation::SelectiveSearchSegmentation** segmentation, cv::Ptr<cv::ximgproc::segmentation::SelectiveSearchSegmentation>** sharedPtr)
{
	delete *sharedPtr;
	*segmentation = 0;
	*sharedPtr = 0;
}

void cveGradientPaillouY(cv::_InputArray* op, cv::_OutputArray* dst, double alpha, double omega)
{
	cv::ximgproc::GradientPaillouY(*op, *dst, alpha, omega);
}
void cveGradientPaillouX(cv::_InputArray* op, cv::_OutputArray* dst, double alpha, double omega)
{
	cv::ximgproc::GradientPaillouX(*op, *dst, alpha, omega);
}
void cveGradientDericheY(cv::_InputArray* op, cv::_OutputArray* dst, double alphaDerive, double alphaMean)
{
	cv::ximgproc::GradientDericheY(*op, *dst, alphaDerive, alphaMean);
}
void cveGradientDericheX(cv::_InputArray* op, cv::_OutputArray* dst, double alphaDerive, double alphaMean)
{
	cv::ximgproc::GradientDericheX(*op, *dst, alphaDerive, alphaMean);
}


void cveThinning(cv::_InputArray* src, cv::_OutputArray* dst, int thinningType)
{
	cv::ximgproc::thinning(*src, *dst, thinningType);
}

void cveAnisotropicDiffusion(cv::_InputArray* src, cv::_OutputArray* dst, float alpha, float K, int niters)
{
	cv::ximgproc::anisotropicDiffusion(*src, *dst, alpha, K, niters);
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
}

void cveFastLineDetectorDetect(cv::ximgproc::FastLineDetector* fld, cv::_InputArray* image, cv::_OutputArray* lines)
{
	fld->detect(*image, *lines);
}

void cveFastLineDetectorDrawSegments(cv::ximgproc::FastLineDetector* fld, cv::_InputOutputArray* image, cv::_InputArray* lines, bool draw_arrow)
{
	fld->drawSegments(*image, *lines, draw_arrow);
}

void cveFastLineDetectorRelease(cv::Ptr<cv::ximgproc::FastLineDetector>** fld)
{
	delete *fld;
	*fld = 0;
}

void cveBrightEdges(cv::Mat* original, cv::Mat* edgeview, int contrast, int shortrange, int longrange)
{
	cv::ximgproc::BrightEdges(*original, *edgeview, contrast, shortrange, longrange);
}

cv::ximgproc::DisparityWLSFilter* cveCreateDisparityWLSFilter(cv::StereoMatcher* matcherLeft, cv::ximgproc::DisparityFilter** disparityFilter, cv::Algorithm** algorithm, cv::Ptr<cv::ximgproc::DisparityWLSFilter>** sharedPtr)
{
	cv::Ptr<cv::StereoMatcher> sm = cv::Ptr<cv::StereoMatcher>(matcherLeft, [](cv::StereoMatcher*) {});
	cv::Ptr<cv::ximgproc::DisparityWLSFilter> filter = cv::ximgproc::createDisparityWLSFilter(sm);
	*sharedPtr = new cv::Ptr<cv::ximgproc::DisparityWLSFilter>(filter);
	*disparityFilter = (*sharedPtr)->dynamicCast<cv::ximgproc::DisparityFilter>();
	*algorithm = (*sharedPtr)->dynamicCast<cv::Algorithm>();
	return (*sharedPtr)->get();
}
cv::StereoMatcher* cveCreateRightMatcher(cv::StereoMatcher* matcherLeft, cv::Ptr<cv::StereoMatcher>** sharedPtr)
{
	cv::Ptr<cv::StereoMatcher> ml = cv::Ptr<cv::StereoMatcher>(matcherLeft, [](cv::StereoMatcher*) {});
	cv::Ptr<cv::StereoMatcher> sm = cv::ximgproc::createRightMatcher(ml);
	*sharedPtr = new cv::Ptr<cv::StereoMatcher>(sm);
	return (*sharedPtr)->get();
}
cv::ximgproc::DisparityWLSFilter* cveCreateDisparityWLSFilterGeneric(bool use_confidence, cv::ximgproc::DisparityFilter** disparityFilter, cv::Algorithm** algorithm, cv::Ptr<cv::ximgproc::DisparityWLSFilter>** sharedPtr)
{
	cv::Ptr<cv::ximgproc::DisparityWLSFilter> filter = cv::ximgproc::createDisparityWLSFilterGeneric(use_confidence);
	*sharedPtr = new cv::Ptr<cv::ximgproc::DisparityWLSFilter>(filter);
	*disparityFilter = (*sharedPtr)->dynamicCast<cv::ximgproc::DisparityFilter>();
	*algorithm = (*sharedPtr)->dynamicCast<cv::Algorithm>();
	return (*sharedPtr)->get();
}
void cveDisparityWLSFilterRelease(cv::Ptr<cv::ximgproc::DisparityWLSFilter>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
}

void cveDisparityFilterFilter(
	cv::ximgproc::DisparityFilter* disparityFilter,
	cv::_InputArray* disparity_map_left, cv::_InputArray* left_view, cv::_OutputArray* filtered_disparity_map,
	cv::_InputArray* disparity_map_right, CvRect* ROI, cv::_InputArray* right_view)
{
	disparityFilter->filter(
		*disparity_map_left, 
		*left_view, 
		*filtered_disparity_map, 
		disparity_map_right->empty() ? cv::Mat() : *disparity_map_right, 
		*ROI, 
		right_view->empty() ? cv::Mat() : *right_view);
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
}
void cveRidgeDetectionFilterRelease(cv::Ptr<cv::ximgproc::RidgeDetectionFilter>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
}
void cveRidgeDetectionFilterGetRidgeFilteredImage(cv::ximgproc::RidgeDetectionFilter* ridgeDetection, cv::_InputArray* img, cv::_OutputArray* out)
{
	ridgeDetection->getRidgeFilteredImage(*img, *out);
}