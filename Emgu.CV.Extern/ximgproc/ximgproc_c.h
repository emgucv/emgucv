//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_XIMGPROC_C_H
#define EMGU_XIMGPROC_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_XIMGPROC
#include "opencv2/ximgproc.hpp"
#else
static inline CV_NORETURN void throw_no_ximgproc() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without ximgproc support"); }
namespace cv {
	namespace ximgproc {
		class DTFilter {};
		class RFFeatureGetter {};
		class StructuredEdgeDetection {};
		class SuperpixelSEEDS {};
		class SuperpixelLSC {};
		class SuperpixelSLIC {};
		class FastLineDetector {};
		enum WMFWeightType {};
		class DisparityFilter {};
		class DisparityWLSFilter {};
		class RidgeDetectionFilter {};
		class EdgeBoxes {};
		namespace  segmentation {
			class GraphSegmentation {};
				class SelectiveSearchSegmentation {};
		}
	}
class StereoMatcher {};
}
#endif

CVAPI(void) cveDtFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, double sigmaSpatial, double sigmaColor, int mode, int numIters);

CVAPI(void) cveGuidedFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, int radius, double eps, int dDepth);

CVAPI(void) cveAmFilter(cv::_InputArray* joint, cv::_InputArray* src, cv::_OutputArray* dst, double sigmaS, double sigmaR, bool adjustOutliers);

CVAPI(void) cveJointBilateralFilter(cv::_InputArray* joint, cv::_InputArray* src, cv::_OutputArray* dst, int d, double sigmaColor, double sigmaSpace, int borderType);

CVAPI(void) cveBilateralTextureFilter(cv::_InputArray* src, cv::_OutputArray* dst, int fr, int numIter, double sigmaAlpha, double sigmaAvg);

CVAPI(void) cveRollingGuidanceFilter(cv::_InputArray* src, cv::_OutputArray* dst, int d, double sigmaColor, double sigmaSpace, int numOfIter, int borderType);

CVAPI(void) cveFastGlobalSmootherFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, double lambda, double sigmaColor, double lambdaAttenuation, int numIter);

CVAPI(void) cveL0Smooth(cv::_InputArray* src, cv::_OutputArray* dst, double lambda, double kappa);

CVAPI(void) cveNiBlackThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double maxValue, int type, int blockSize, double delta);

CVAPI(void) cveCovarianceEstimation(cv::_InputArray* src, cv::_OutputArray* dst, int windowRows, int windowCols);

CVAPI(cv::ximgproc::DTFilter*) cveDTFilterCreate(cv::_InputArray* guide, double sigmaSpatial, double sigmaColor, int mode, int numIters, cv::Ptr<cv::ximgproc::DTFilter>** sharedPtr);
CVAPI(void) cveDTFilterFilter(cv::ximgproc::DTFilter* filter, cv::_InputArray* src, cv::_OutputArray* dst, int dDepth);
CVAPI(void) cveDTFilterRelease(cv::ximgproc::DTFilter** filter, cv::Ptr<cv::ximgproc::DTFilter>** sharedPtr);


CVAPI(cv::ximgproc::RFFeatureGetter*) cveRFFeatureGetterCreate(cv::Ptr<cv::ximgproc::RFFeatureGetter>** sharedPtr);
CVAPI(void) cveRFFeatureGetterRelease(cv::ximgproc::RFFeatureGetter** getter, cv::Ptr<cv::ximgproc::RFFeatureGetter>** sharedPtr);

CVAPI(cv::ximgproc::StructuredEdgeDetection*) cveStructuredEdgeDetectionCreate(cv::String* model, cv::ximgproc::RFFeatureGetter* howToGetFeatures, cv::Ptr<cv::ximgproc::StructuredEdgeDetection>** sharedPtr);
CVAPI(void) cveStructuredEdgeDetectionDetectEdges(cv::ximgproc::StructuredEdgeDetection* detection, cv::_InputArray* src, cv::_OutputArray* dst);
CVAPI(void) cveStructuredEdgeDetectionComputeOrientation(cv::ximgproc::StructuredEdgeDetection* detection, cv::_InputArray* src, cv::_OutputArray* dst);
CVAPI(void) cveStructuredEdgeDetectionEdgesNms(cv::ximgproc::StructuredEdgeDetection* detection, cv::_InputArray* edgeImage, cv::_InputArray* orientationImage, cv::_OutputArray* dst, int r, int s, float m, bool isParallel);
CVAPI(void) cveStructuredEdgeDetectionRelease(cv::ximgproc::StructuredEdgeDetection** detection, cv::Ptr<cv::ximgproc::StructuredEdgeDetection>** sharedPtr);

CVAPI(cv::ximgproc::SuperpixelSEEDS*) cveSuperpixelSEEDSCreate(
	int imageWidth, int imageHeight, int imageChannels,
	int numSuperpixels, int numLevels, int prior,
	int histogramBins, bool doubleStep,
	cv::Ptr<cv::ximgproc::SuperpixelSEEDS>** sharedPtr);
CVAPI(int) cveSuperpixelSEEDSGetNumberOfSuperpixels(cv::ximgproc::SuperpixelSEEDS* seeds);
CVAPI(void) cveSuperpixelSEEDSGetLabels(cv::ximgproc::SuperpixelSEEDS* seeds, cv::_OutputArray* labelsOut);
CVAPI(void) cveSuperpixelSEEDSGetLabelContourMask(cv::ximgproc::SuperpixelSEEDS* seeds, cv::_OutputArray* image, bool thickLine);
CVAPI(void) cveSuperpixelSEEDSIterate(cv::ximgproc::SuperpixelSEEDS* seeds, cv::_InputArray* img, int numIterations);
CVAPI(void) cveSuperpixelSEEDSRelease(cv::ximgproc::SuperpixelSEEDS** seeds, cv::Ptr<cv::ximgproc::SuperpixelSEEDS>** sharedPtr);

CVAPI(cv::ximgproc::SuperpixelLSC*) cveSuperpixelLSCCreate(cv::_InputArray* image, int regionSize, float ratio, cv::Ptr<cv::ximgproc::SuperpixelLSC>** sharedPtr);
CVAPI(int) cveSuperpixelLSCGetNumberOfSuperpixels(cv::ximgproc::SuperpixelLSC* lsc);
CVAPI(void) cveSuperpixelLSCIterate(cv::ximgproc::SuperpixelLSC* lsc, int numIterations);
CVAPI(void) cveSuperpixelLSCGetLabels(cv::ximgproc::SuperpixelLSC* lsc, cv::_OutputArray* labelsOut);
CVAPI(void) cveSuperpixelLSCGetLabelContourMask(cv::ximgproc::SuperpixelLSC* lsc, cv::_OutputArray* image, bool thickLine);
CVAPI(void) cveSuperpixelLSCEnforceLabelConnectivity(cv::ximgproc::SuperpixelLSC* lsc, int minElementSize);
CVAPI(void) cveSuperpixelLSCRelease(cv::ximgproc::SuperpixelLSC** lsc, cv::Ptr<cv::ximgproc::SuperpixelLSC>** sharedPtr);

CVAPI(cv::ximgproc::SuperpixelSLIC*) cveSuperpixelSLICCreate(cv::_InputArray* image, int algorithm, int regionSize, float ruler, cv::Ptr<cv::ximgproc::SuperpixelSLIC>** sharedPtr);
CVAPI(int) cveSuperpixelSLICGetNumberOfSuperpixels(cv::ximgproc::SuperpixelSLIC* slic);
CVAPI(void) cveSuperpixelSLICIterate(cv::ximgproc::SuperpixelSLIC* slic, int numIterations);
CVAPI(void) cveSuperpixelSLICGetLabels(cv::ximgproc::SuperpixelSLIC* slic, cv::_OutputArray* labelsOut);
CVAPI(void) cveSuperpixelSLICGetLabelContourMask(cv::ximgproc::SuperpixelSLIC* slic, cv::_OutputArray* image, bool thickLine);
CVAPI(void) cveSuperpixelSLICEnforceLabelConnectivity(cv::ximgproc::SuperpixelSLIC* slic, int minElementSize);
CVAPI(void) cveSuperpixelSLICRelease(cv::ximgproc::SuperpixelSLIC** slic, cv::Ptr<cv::ximgproc::SuperpixelSLIC>** sharedPtr);

CVAPI(cv::ximgproc::segmentation::GraphSegmentation*) cveGraphSegmentationCreate(double sigma, float k, int minSize, cv::Ptr<cv::ximgproc::segmentation::GraphSegmentation>** sharedPtr);
CVAPI(void) cveGraphSegmentationProcessImage(cv::ximgproc::segmentation::GraphSegmentation* segmentation, cv::_InputArray* src, cv::_OutputArray* dst);
CVAPI(void) cveGraphSegmentationRelease(cv::ximgproc::segmentation::GraphSegmentation** segmentation, cv::Ptr<cv::ximgproc::segmentation::GraphSegmentation>** sharedPtr);

CVAPI(void) cveWeightedMedianFilter(cv::_InputArray* joint, cv::_InputArray* src, cv::_OutputArray* dst, int r, double sigma, cv::ximgproc::WMFWeightType weightType, cv::Mat* mask);


CVAPI(cv::ximgproc::segmentation::SelectiveSearchSegmentation*) cveSelectiveSearchSegmentationCreate(cv::Ptr<cv::ximgproc::segmentation::SelectiveSearchSegmentation>** sharedPtr);
CVAPI(void) cveSelectiveSearchSegmentationSetBaseImage(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, cv::_InputArray* image);
CVAPI(void) cveSelectiveSearchSegmentationSwitchToSingleStrategy(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, int k, float sigma);
CVAPI(void) cveSelectiveSearchSegmentationSwitchToSelectiveSearchFast(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, int baseK, int incK, float sigma);
CVAPI(void) cveSelectiveSearchSegmentationSwitchToSelectiveSearchQuality(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, int baseK, int incK, float sigma);
CVAPI(void) cveSelectiveSearchSegmentationAddImage(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, cv::_InputArray* img);
CVAPI(void) cveSelectiveSearchSegmentationProcess(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, std::vector<cv::Rect>* rects);
CVAPI(void) cveSelectiveSearchSegmentationRelease(cv::ximgproc::segmentation::SelectiveSearchSegmentation** segmentation, cv::Ptr<cv::ximgproc::segmentation::SelectiveSearchSegmentation>** sharedPtr);

CVAPI(void) cveGradientPaillouY(cv::_InputArray* op, cv::_OutputArray* dst, double alpha, double omega);
CVAPI(void) cveGradientPaillouX(cv::_InputArray* op, cv::_OutputArray* dst, double alpha, double omega);
CVAPI(void) cveGradientDericheY(cv::_InputArray* op, cv::_OutputArray* dst, double alphaDerive, double alphaMean);
CVAPI(void) cveGradientDericheX(cv::_InputArray* op, cv::_OutputArray* dst, double alphaDerive, double alphaMean);


CVAPI(void) cveThinning(cv::_InputArray* src, cv::_OutputArray* dst, int thinningType);

CVAPI(void) cveAnisotropicDiffusion(cv::_InputArray* src, cv::_OutputArray* dst, float alpha, float K, int niters);

CVAPI(cv::ximgproc::FastLineDetector*) cveFastLineDetectorCreate(
	int length_threshold,
	float distance_threshold,
	double canny_th1,
	double canny_th2,
	int canny_aperture_size,
	bool do_merge,
	cv::Ptr<cv::ximgproc::FastLineDetector>** sharedPtr);
CVAPI(void) cveFastLineDetectorDetect(cv::ximgproc::FastLineDetector* fld, cv::_InputArray* image, cv::_OutputArray* lines);
CVAPI(void) cveFastLineDetectorDrawSegments(cv::ximgproc::FastLineDetector* fld, cv::_InputOutputArray* image, cv::_InputArray* lines, bool draw_arrow);
CVAPI(void) cveFastLineDetectorRelease(cv::Ptr<cv::ximgproc::FastLineDetector>** fld);

CVAPI(void) cveBrightEdges(cv::Mat* original, cv::Mat* edgeview, int contrast, int shortrange, int longrange);

CVAPI(cv::ximgproc::DisparityWLSFilter*) cveCreateDisparityWLSFilter(cv::StereoMatcher* matcherLeft, cv::ximgproc::DisparityFilter** disparityFilter, cv::Algorithm** algorithm, cv::Ptr<cv::ximgproc::DisparityWLSFilter>** sharedPtr);
CVAPI(cv::StereoMatcher*) cveCreateRightMatcher(cv::StereoMatcher* matcherLeft, cv::Ptr<cv::StereoMatcher>** sharedPtr);
CVAPI(cv::ximgproc::DisparityWLSFilter*) cveCreateDisparityWLSFilterGeneric(bool use_confidence, cv::ximgproc::DisparityFilter** disparityFilter, cv::Algorithm** algorithm, cv::Ptr<cv::ximgproc::DisparityWLSFilter>** sharedPtr);
CVAPI(void) cveDisparityWLSFilterRelease(cv::Ptr<cv::ximgproc::DisparityWLSFilter>** sharedPtr);
CVAPI(void) cveDisparityFilterFilter(
	cv::ximgproc::DisparityFilter* disparityFilter,
	cv::_InputArray* disparity_map_left, cv::_InputArray* left_view, cv::_OutputArray* filtered_disparity_map,
	cv::_InputArray* disparity_map_right, CvRect* ROI, cv::_InputArray* right_view);


CVAPI(cv::ximgproc::RidgeDetectionFilter*) cveRidgeDetectionFilterCreate(
	int ddepth,
	int dx,
	int dy,
	int ksize,
	int outDtype,
	double scale,
	double delta,
	int borderType,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::ximgproc::RidgeDetectionFilter>** sharedPtr);
CVAPI(void) cveRidgeDetectionFilterRelease(cv::Ptr<cv::ximgproc::RidgeDetectionFilter>** sharedPtr);
CVAPI(void) cveRidgeDetectionFilterGetRidgeFilteredImage(cv::ximgproc::RidgeDetectionFilter* ridgeDetection, cv::_InputArray* img, cv::_OutputArray* out);

CVAPI(cv::ximgproc::EdgeBoxes*) cveEdgeBoxesCreate(
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
	cv::Ptr<cv::ximgproc::EdgeBoxes>** sharedPtr);
CVAPI(void) cveEdgeBoxesGetBoundingBoxes(cv::ximgproc::EdgeBoxes* edgeBoxes, cv::_InputArray* edgeMap, cv::_InputArray* orientationMap, std::vector<cv::Rect>* boxes);
CVAPI(void) cveEdgeBoxesRelease(cv::Ptr<cv::ximgproc::EdgeBoxes>** sharedPtr);
#endif