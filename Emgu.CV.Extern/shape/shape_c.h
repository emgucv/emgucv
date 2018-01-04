//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_SHAPE_C_H
#define EMGU_SHAPE_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/shape/shape.hpp"

CVAPI(cv::HistogramCostExtractor*) cvNormHistogramCostExtractorCreate(int flag, int nDummies, float defaultCost);

CVAPI(cv::HistogramCostExtractor*) cvEMDHistogramCostExtractorCreate(int flag, int nDummies, float defaultCost);

CVAPI(cv::HistogramCostExtractor*) cvChiHistogramCostExtractorCreate(int nDummies, float defaultCost);

CVAPI(cv::HistogramCostExtractor*) cvEMDL1HistogramCostExtractorCreate(int nDummies, float defaultCost);

CVAPI(void) cvHistogramCostExtractorRelease(cv::HistogramCostExtractor** extractor);

CVAPI(cv::ThinPlateSplineShapeTransformer*) cvThinPlateSplineShapeTransformerCreate(double regularizationParameter, cv::ShapeTransformer** shapeTransformer);
CVAPI(void) cvThinPlateSplineShapeTransformerRelease(cv::ThinPlateSplineShapeTransformer** transformer);
CVAPI(cv::AffineTransformer*) cvAffineTransformerCreate(bool fullAffine, cv::ShapeTransformer** transformer);
CVAPI(void) cvAffineTransformerRelease(cv::AffineTransformer** transformer);


CVAPI(float) cvShapeDistanceExtractorComputeDistance(cv::ShapeDistanceExtractor* extractor, cv::_InputArray* contour1, cv::_InputArray* contour2);

CVAPI(cv::ShapeContextDistanceExtractor*) cvShapeContextDistanceExtractorCreate(
   int nAngularBins, int nRadialBins, float innerRadius, float outerRadius, int iterations,
   cv::HistogramCostExtractor* comparer, cv::ShapeTransformer* transformer, cv::ShapeDistanceExtractor** e);
CVAPI(void) cvShapeContextDistanceExtractorRelease(cv::ShapeContextDistanceExtractor** extractor);

CVAPI(cv::HausdorffDistanceExtractor*) cvHausdorffDistanceExtractorCreate(int distanceFlag, float rankProp, cv::ShapeDistanceExtractor** e);
CVAPI(void) cvHausdorffDistanceExtractorRelease(cv::HausdorffDistanceExtractor** extractor);

#endif