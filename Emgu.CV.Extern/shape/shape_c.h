//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
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

CVAPI(cv::ShapeTransformer*) cvThinPlateSplineShapeTransformerCreate(double regularizationParameter);

CVAPI(cv::ShapeTransformer*) cvAffineTransformerCreate(bool fullAffine);

CVAPI(void) cvShapeTransformerRelease(cv::ShapeTransformer** transformer);

CVAPI(float) cvShapeDistanceExtractorComputeDistance(cv::ShapeDistanceExtractor* extractor, std::vector<cv::Point>* contour1, std::vector<cv::Point>* contour2);

CVAPI(cv::ShapeContextDistanceExtractor*) cvShapeContextDistanceExtractorCreate(
   int nAngularBins, int nRadialBins, float innerRadius, float outerRadius, int iterations,
   cv::HistogramCostExtractor* comparer, cv::ShapeTransformer* transformer);
CVAPI(void) cvShapeContextDistanceExtractorRelease(cv::ShapeContextDistanceExtractor** extractor);

CVAPI(cv::HausdorffDistanceExtractor*) cvHausdorffDistanceExtractorCreate(int distanceFlag, float rankProp);
CVAPI(void) cvHausdorffDistanceExtractorRelease(cv::HausdorffDistanceExtractor** extractor);

#endif