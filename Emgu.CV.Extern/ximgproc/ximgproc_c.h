//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_XIMGPROC_C_H
#define EMGU_XIMGPROC_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/ximgproc.hpp"

CVAPI(void) cveDtFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, double sigmaSpatial, double sigmaColor, int mode, int numIters);

CVAPI(void) cveGuidedFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, int radius, double eps, int dDepth);

CVAPI(void) cveAmFilter(cv::_InputArray* joint, cv::_InputArray* src, cv::_OutputArray* dst, double sigmaS, double sigmaR, bool adjustOutliers);

CVAPI(void) cveJointBilateralFilter(cv::_InputArray* joint, cv::_InputArray* src, cv::_OutputArray* dst, int d, double sigmaColor, double sigmaSpace, int borderType);

CVAPI(void) cveBilateralTextureFilter(cv::_InputArray* src, cv::_OutputArray* dst, int fr, int numIter, double sigmaAlpha, double sigmaAvg);

CVAPI(void) cveRollingGuidanceFilter(cv::_InputArray* src, cv::_OutputArray* dst, int d, double sigmaColor, double sigmaSpace, int numOfIter, int borderType);

CVAPI(void) cveFastGlobalSmootherFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, double lambda, double sigmaColor, double lambdaAttenuation, int numIter);

CVAPI(void) cveNiBlackThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double maxValue, int type, int blockSize, double delta);

CVAPI(void) cveCovarianceEstimation(cv::_InputArray* src, cv::_OutputArray* dst, int windowRows, int windowCols);

CVAPI(cv::ximgproc::DTFilter*) cveDTFilterCreate(cv::_InputArray* guide, double sigmaSpatial, double sigmaColor, int mode, int numIters);
CVAPI(void) cveDTFilterFilter(cv::ximgproc::DTFilter* filter, cv::_InputArray* src, cv::_OutputArray* dst, int dDepth);
CVAPI(void) cveDTFilterRelease(cv::ximgproc::DTFilter** filter);


CVAPI(cv::ximgproc::RFFeatureGetter*) cveRFFeatureGetterCreate();
CVAPI(void) cveRFFeatureGetterRelease(cv::ximgproc::RFFeatureGetter** getter);

CVAPI(cv::ximgproc::StructuredEdgeDetection*) cveStructuredEdgeDetectionCreate(cv::String* model, cv::ximgproc::RFFeatureGetter* howToGetFeatures);
CVAPI(void) cveStructuredEdgeDetectionDetectEdges(cv::ximgproc::StructuredEdgeDetection* detection, cv::Mat* src, cv::Mat* dst);
CVAPI(void) cveStructuredEdgeDetectionRelease(cv::ximgproc::StructuredEdgeDetection** detection);

CVAPI(cv::ximgproc::SuperpixelSEEDS*) cveSuperpixelSEEDSCreate(
   int imageWidth, int imageHeight, int imageChannels,
   int numSuperpixels, int numLevels, int prior,
   int histogramBins, bool doubleStep);
CVAPI(int) cveSuperpixelSEEDSGetNumberOfSuperpixels(cv::ximgproc::SuperpixelSEEDS* seeds);
CVAPI(void) cveSuperpixelSEEDSGetLabels(cv::ximgproc::SuperpixelSEEDS* seeds, cv::_OutputArray* labelsOut);
CVAPI(void) cveSuperpixelSEEDSGetLabelContourMask(cv::ximgproc::SuperpixelSEEDS* seeds, cv::_OutputArray* image, bool thickLine);
CVAPI(void) cveSuperpixelSEEDSIterate(cv::ximgproc::SuperpixelSEEDS* seeds, cv::_InputArray* img, int numIterations);
CVAPI(void) cveSuperpixelSEEDSRelease(cv::ximgproc::SuperpixelSEEDS** seeds);

CVAPI(cv::ximgproc::SuperpixelLSC*) cveSuperpixelLSCCreate(cv::_InputArray* image, int regionSize, float ratio);
CVAPI(int) cveSuperpixelLSCGetNumberOfSuperpixels(cv::ximgproc::SuperpixelLSC* lsc);
CVAPI(void) cveSuperpixelLSCIterate(cv::ximgproc::SuperpixelLSC* lsc, int numIterations);
CVAPI(void) cveSuperpixelLSCGetLabels(cv::ximgproc::SuperpixelLSC* lsc, cv::_OutputArray* labelsOut);
CVAPI(void) cveSuperpixelLSCGetLabelContourMask(cv::ximgproc::SuperpixelLSC* lsc, cv::_OutputArray* image, bool thickLine);
CVAPI(void) cveSuperpixelLSCEnforceLabelConnectivity(cv::ximgproc::SuperpixelLSC* lsc, int minElementSize);
CVAPI(void) cveSuperpixelLSCRelease(cv::ximgproc::SuperpixelLSC** lsc);

CVAPI(cv::ximgproc::SuperpixelSLIC*) cveSuperpixelSLICCreate(cv::_InputArray* image, int algorithm, int regionSize, float ruler);
CVAPI(int) cveSuperpixelSLICGetNumberOfSuperpixels(cv::ximgproc::SuperpixelSLIC* slic);
CVAPI(void) cveSuperpixelSLICIterate(cv::ximgproc::SuperpixelSLIC* slic, int numIterations);
CVAPI(void) cveSuperpixelSLICGetLabels(cv::ximgproc::SuperpixelSLIC* slic, cv::_OutputArray* labelsOut);
CVAPI(void) cveSuperpixelSLICGetLabelContourMask(cv::ximgproc::SuperpixelSLIC* slic, cv::_OutputArray* image, bool thickLine);
CVAPI(void) cveSuperpixelSLICEnforceLabelConnectivity(cv::ximgproc::SuperpixelSLIC* slic, int minElementSize);
CVAPI(void) cveSuperpixelSLICRelease(cv::ximgproc::SuperpixelSLIC** slic);
 
CVAPI(cv::ximgproc::segmentation::GraphSegmentation*) cveGraphSegmentationCreate(double sigma, float k, int minSize);
CVAPI(void) cveGraphSegmentationProcessImage(cv::ximgproc::segmentation::GraphSegmentation* segmentation, cv::_InputArray* src, cv::_OutputArray* dst);
CVAPI(void) cveGraphSegmentationRelease(cv::ximgproc::segmentation::GraphSegmentation** segmentation);

CVAPI(void) cveWeightedMedianFilter(cv::_InputArray* joint, cv::_InputArray* src, cv::_OutputArray* dst, int r, double sigma, cv::ximgproc::WMFWeightType weightType, cv::Mat* mask);


CVAPI(cv::ximgproc::segmentation::SelectiveSearchSegmentation*) cveSelectiveSearchSegmentationCreate();
CVAPI(void) cveSelectiveSearchSegmentationSetBaseImage(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, cv::_InputArray* image);
CVAPI(void) cveSelectiveSearchSegmentationSwitchToSingleStrategy(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, int k, float sigma);
CVAPI(void) cveSelectiveSearchSegmentationSwitchToSelectiveSearchFast(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, int baseK, int incK, float sigma);
CVAPI(void) cveSelectiveSearchSegmentationSwitchToSelectiveSearchQuality(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, int baseK, int incK, float sigma);
CVAPI(void) cveSelectiveSearchSegmentationAddImage(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, cv::_InputArray* img);
CVAPI(void) cveSelectiveSearchSegmentationProcess(cv::ximgproc::segmentation::SelectiveSearchSegmentation* segmentation, std::vector<cv::Rect>* rects);
CVAPI(void) cveSelectiveSearchSegmentationRelease(cv::ximgproc::segmentation::SelectiveSearchSegmentation** segmentation);

CVAPI(void) cveGradientPaillouY(cv::_InputArray* op, cv::_OutputArray* dst, double alpha, double omega);
CVAPI(void) cveGradientPaillouX(cv::_InputArray* op, cv::_OutputArray* dst, double alpha, double omega);
CVAPI(void) cveGradientDericheY(cv::_InputArray* op, cv::_OutputArray* dst, double alphaDerive, double alphaMean);
CVAPI(void) cveGradientDericheX(cv::_InputArray* op, cv::_OutputArray* dst, double alphaDerive, double alphaMean);


CVAPI(void) cveThinning(cv::_InputArray* src, cv::_OutputArray* dst, int thinningType);

CVAPI(void) cveAnisotropicDiffusion(cv::_InputArray* src, cv::_OutputArray* dst, float alpha, float K, int niters);
#endif