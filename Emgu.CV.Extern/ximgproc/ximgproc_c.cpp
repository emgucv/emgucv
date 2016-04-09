//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
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

void cveFastGlobalSmootherFilter(cv::_InputArray* guide, cv::_InputArray* src, cv::_OutputArray* dst, double lambda, double sigmaColor, double lambdaAttenuation, int numIter)
{
   cv::ximgproc::fastGlobalSmootherFilter(*guide, *src, *dst, lambda, sigmaColor, lambdaAttenuation, numIter);
}

void cveNiBlackThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double maxValue, int type, int blockSize, double delta)
{
   cv::ximgproc::niBlackThreshold(*src, *dst, maxValue, type, blockSize, delta);
}

void cveCovarianceEstimation(cv::_InputArray* src, cv::_OutputArray* dst, int windowRows, int windowCols)
{
   cv::ximgproc::covarianceEstimation(*src, *dst, windowRows, windowCols);
}

cv::ximgproc::DTFilter* cveDTFilterCreate(cv::_InputArray* guide, double sigmaSpatial, double sigmaColor, int mode, int numIters)
{
   cv::Ptr<cv::ximgproc::DTFilter> ptr = cv::ximgproc::createDTFilter(*guide, sigmaSpatial, sigmaColor, mode, numIters);
   ptr.addref();
   return ptr.get();
}

void cveDTFilterFilter(cv::ximgproc::DTFilter* filter, cv::_InputArray* src, cv::_OutputArray* dst, int dDepth)
{
   filter->filter(*src, *dst, dDepth);
}
void cveDTFilterRelease(cv::ximgproc::DTFilter** filter)
{
   delete *filter;
   *filter = 0;
}

cv::ximgproc::RFFeatureGetter* cveRFFeatureGetterCreate()
{
   cv::Ptr<cv::ximgproc::RFFeatureGetter> ptr = cv::ximgproc::createRFFeatureGetter();
   ptr.addref();
   return ptr.get();
}
void cveRFFeatureGetterRelease(cv::ximgproc::RFFeatureGetter** getter)
{
   delete *getter;
   *getter = 0;
}


cv::ximgproc::StructuredEdgeDetection* cveStructuredEdgeDetectionCreate(cv::String* model, cv::ximgproc::RFFeatureGetter* howToGetFeatures)
{
   cv::Ptr<cv::ximgproc::StructuredEdgeDetection> ptr = cv::ximgproc::createStructuredEdgeDetection(*model, howToGetFeatures);
   ptr.addref();
   return ptr.get();
}
void cveStructuredEdgeDetectionDetectEdges(cv::ximgproc::StructuredEdgeDetection* detection, cv::Mat* src, cv::Mat* dst)
{
   detection->detectEdges(*src, *dst);
}
void cveStructuredEdgeDetectionRelease(cv::ximgproc::StructuredEdgeDetection** detection)
{
   delete *detection;
   *detection = 0;
}

cv::ximgproc::SuperpixelSEEDS* cveSuperpixelSEEDSCreate(
   int imageWidth, int imageHeight, int imageChannels,
   int numSuperpixels, int numLevels, int prior,
   int histogramBins, bool doubleStep)
{
   cv::Ptr<cv::ximgproc::SuperpixelSEEDS> ptr = cv::ximgproc::createSuperpixelSEEDS(imageWidth, imageHeight, imageChannels, numSuperpixels, numLevels, prior, histogramBins, doubleStep);
   ptr.addref();
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
void cveSuperpixelSEEDSRelease(cv::ximgproc::SuperpixelSEEDS** seeds)
{
   delete *seeds;
   *seeds = 0;
}


cv::ximgproc::SuperpixelLSC* cveSuperpixelLSCCreate(cv::_InputArray* image, int regionSize, float ratio)
{
   cv::Ptr<cv::ximgproc::SuperpixelLSC> ptr = cv::ximgproc::createSuperpixelLSC(*image, regionSize, ratio);
   ptr.addref();
   return ptr.get();
}
int cveSuperixelLSCGetNumberOfSuperpixels(cv::ximgproc::SuperpixelLSC* lsc)
{
   return lsc->getNumberOfSuperpixels();
}
void cveSuperixelLSCIterate(cv::ximgproc::SuperpixelLSC* lsc, int numIterations)
{
   lsc->iterate(numIterations);
}
void cveSuperixelLSCGetLabels(cv::ximgproc::SuperpixelLSC* lsc, cv::_OutputArray* labelsOut)
{
   lsc->getLabels(*labelsOut);
}
void cveSuperixelLSCGetLabelContourMask(cv::ximgproc::SuperpixelLSC* lsc, cv::_OutputArray* image, bool thickLine)
{
   lsc->getLabelContourMask(*image, thickLine);
}
void cveSuperixelLSCEnforceLabelConnectivity(cv::ximgproc::SuperpixelLSC* lsc, int minElementSize)
{
   lsc->enforceLabelConnectivity(minElementSize);
}
void cveSuperpixelLSCRelease(cv::ximgproc::SuperpixelLSC** lsc)
{
   delete *lsc;
   *lsc = 0;
}


cv::ximgproc::SuperpixelSLIC* cveSuperpixelSLICCreate(cv::_InputArray* image, int algorithm, int regionSize, float ruler)
{
   cv::Ptr<cv::ximgproc::SuperpixelSLIC> ptr = cv::ximgproc::createSuperpixelSLIC(*image, regionSize, ruler);
   ptr.addref();
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
void cveSuperpixelSLICRelease(cv::ximgproc::SuperpixelSLIC** slic)
{
   delete *slic;
   *slic = 0;
}


cv::ximgproc::segmentation::GraphSegmentation* cveGraphSegmentationCreate(double sigma, float k, int minSize)
{
   cv::Ptr<cv::ximgproc::segmentation::GraphSegmentation> ptr = cv::ximgproc::segmentation::createGraphSegmentation(sigma, k, minSize);
   ptr.addref();
   return ptr.get();
}
void cveGraphSegmentationProcessImage(cv::ximgproc::segmentation::GraphSegmentation* segmentation, cv::_InputArray* src, cv::_OutputArray* dst)
{
   segmentation->processImage(*src, *dst);
}
void cveGraphSegmentationRelease(cv::ximgproc::segmentation::GraphSegmentation** segmentation)
{
   delete *segmentation;
   *segmentation = 0;
}