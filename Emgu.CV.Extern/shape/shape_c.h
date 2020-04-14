//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_SHAPE_C_H
#define EMGU_SHAPE_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/shape/shape.hpp"

CVAPI(cv::HistogramCostExtractor*) cveNormHistogramCostExtractorCreate(int flag, int nDummies, float defaultCost, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr);

CVAPI(cv::HistogramCostExtractor*) cveEMDHistogramCostExtractorCreate(int flag, int nDummies, float defaultCost, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr);

CVAPI(cv::HistogramCostExtractor*) cveChiHistogramCostExtractorCreate(int nDummies, float defaultCost, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr);

CVAPI(cv::HistogramCostExtractor*) cveEMDL1HistogramCostExtractorCreate(int nDummies, float defaultCost, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr);

CVAPI(void) cveHistogramCostExtractorRelease(cv::Ptr<cv::HistogramCostExtractor>** sharedPtr);

CVAPI(cv::ThinPlateSplineShapeTransformer*) cveThinPlateSplineShapeTransformerCreate(double regularizationParameter, cv::ShapeTransformer** shapeTransformer, cv::Ptr<cv::ThinPlateSplineShapeTransformer>** sharedPtr);
CVAPI(void) cveThinPlateSplineShapeTransformerRelease(cv::Ptr<cv::ThinPlateSplineShapeTransformer>** sharedPtr);

CVAPI(cv::AffineTransformer*) cveAffineTransformerCreate(bool fullAffine, cv::ShapeTransformer** transformer, cv::Ptr<cv::AffineTransformer>** sharedPtr);
CVAPI(void) cveAffineTransformerRelease(cv::Ptr<cv::AffineTransformer>** sharedPtr);


CVAPI(float) cveShapeDistanceExtractorComputeDistance(cv::ShapeDistanceExtractor* extractor, cv::_InputArray* contour1, cv::_InputArray* contour2);

CVAPI(cv::ShapeContextDistanceExtractor*) cveShapeContextDistanceExtractorCreate(
   int nAngularBins, int nRadialBins, float innerRadius, float outerRadius, int iterations,
   cv::HistogramCostExtractor* comparer, cv::ShapeTransformer* transformer, cv::ShapeDistanceExtractor** e, cv::Ptr<cv::ShapeContextDistanceExtractor>** sharedPtr);
CVAPI(void) cveShapeContextDistanceExtractorRelease(cv::Ptr<cv::ShapeContextDistanceExtractor>** sharedPtr);

CVAPI(cv::HausdorffDistanceExtractor*) cveHausdorffDistanceExtractorCreate(int distanceFlag, float rankProp, cv::ShapeDistanceExtractor** e, cv::Ptr<cv::HausdorffDistanceExtractor>** sharedPtr);
CVAPI(void) cveHausdorffDistanceExtractorRelease(cv::Ptr<cv::HausdorffDistanceExtractor>** sharedPtr);

#endif