//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_SHAPE_C_H
#define EMGU_SHAPE_C_H

#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_SHAPE
#include "opencv2/shape/shape.hpp"
#else
static inline CV_NORETURN void throw_no_shape() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without shape support"); }
namespace cv {
	class HistogramCostExtractor {};
	class ShapeTransformer {};
	class ThinPlateSplineShapeTransformer {};
	class AffineTransformer {};
	class ShapeDistanceExtractor {};
	class ShapeContextDistanceExtractor {};
	class HausdorffDistanceExtractor {};
}
#endif

CVAPI(cv::HistogramCostExtractor*) cveNormHistogramCostExtractorCreate(int flag, int nDummies, float defaultCost, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr);

CVAPI(cv::HistogramCostExtractor*) cveEMDHistogramCostExtractorCreate(int flag, int nDummies, float defaultCost, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr);

CVAPI(cv::HistogramCostExtractor*) cveChiHistogramCostExtractorCreate(int nDummies, float defaultCost, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr);

CVAPI(cv::HistogramCostExtractor*) cveEMDL1HistogramCostExtractorCreate(int nDummies, float defaultCost, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr);

CVAPI(void) cveHistogramCostExtractorRelease(cv::Ptr<cv::HistogramCostExtractor>** sharedPtr);

CVAPI(cv::ThinPlateSplineShapeTransformer*) cveThinPlateSplineShapeTransformerCreate(double regularizationParameter, cv::ShapeTransformer** shapeTransformer, cv::Ptr<cv::ThinPlateSplineShapeTransformer>** sharedPtr);
CVAPI(void) cveThinPlateSplineShapeTransformerRelease(cv::Ptr<cv::ThinPlateSplineShapeTransformer>** sharedPtr);

CVAPI(cv::AffineTransformer*) cveAffineTransformerCreate(bool fullAffine, cv::ShapeTransformer** transformer, cv::Ptr<cv::AffineTransformer>** sharedPtr);
CVAPI(void) cveAffineTransformerRelease(cv::Ptr<cv::AffineTransformer>** sharedPtr);

CVAPI(void) cveShapeTransformerEstimateTransformation(
	cv::ShapeTransformer* transformer,
	cv::_InputArray* transformingShape,
	cv::_InputArray* targetShape,
	std::vector<cv::DMatch>* matches);

CVAPI(float) cveShapeTransformerApplyTransformation(
	cv::ShapeTransformer* transformer,
	cv::_InputArray* input,
	cv::_OutputArray* output);

CVAPI(void) cveShapeTransformerWarpImage(
	cv::ShapeTransformer* transformer,
	cv::_InputArray* transformingImage,
	cv::_OutputArray* output,
	int flags,
	int borderMode,
	CvScalar* borderValue);

CVAPI(float) cveShapeDistanceExtractorComputeDistance(cv::ShapeDistanceExtractor* extractor, cv::_InputArray* contour1, cv::_InputArray* contour2);

CVAPI(cv::ShapeContextDistanceExtractor*) cveShapeContextDistanceExtractorCreate(
	int nAngularBins, int nRadialBins, float innerRadius, float outerRadius, int iterations,
	cv::HistogramCostExtractor* comparer, cv::ShapeTransformer* transformer, cv::ShapeDistanceExtractor** e, cv::Ptr<cv::ShapeContextDistanceExtractor>** sharedPtr);
CVAPI(void) cveShapeContextDistanceExtractorRelease(cv::Ptr<cv::ShapeContextDistanceExtractor>** sharedPtr);

CVAPI(cv::HausdorffDistanceExtractor*) cveHausdorffDistanceExtractorCreate(int distanceFlag, float rankProp, cv::ShapeDistanceExtractor** e, cv::Ptr<cv::HausdorffDistanceExtractor>** sharedPtr);
CVAPI(void) cveHausdorffDistanceExtractorRelease(cv::Ptr<cv::HausdorffDistanceExtractor>** sharedPtr);

#endif