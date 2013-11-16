//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "shape_c.h"

cv::HistogramCostExtractor* cvNormHistogramCostExtractorCreate(int flag, int nDummies, float defaultCost)
{
   cv::Ptr<cv::HistogramCostExtractor> ptr = cv::createNormHistogramCostExtractor(flag, nDummies, defaultCost);
   ptr.addref();
   return ptr.get();
}

cv::HistogramCostExtractor* cvEMDHistogramCostExtractorCreate(int flag, int nDummies, float defaultCost)
{
   cv::Ptr<cv::HistogramCostExtractor> ptr = cv::createEMDHistogramCostExtractor(flag, nDummies, defaultCost);
   ptr.addref();
   return ptr.get();
}

cv::HistogramCostExtractor* cvChiHistogramCostExtractorCreate(int nDummies, float defaultCost)
{
   cv::Ptr<cv::HistogramCostExtractor> ptr = cv::createChiHistogramCostExtractor(nDummies, defaultCost);
   ptr.addref();
   return ptr.get();
}

cv::HistogramCostExtractor* cvEMDL1HistogramCostExtractorCreate(int nDummies, float defaultCost)
{
   cv::Ptr<cv::HistogramCostExtractor> ptr = cv::createEMDL1HistogramCostExtractor(nDummies, defaultCost);
   ptr.addref();
   return ptr.get();
}

void cvHistogramCostExtractorRelease(cv::HistogramCostExtractor** extractor)
{
   delete *extractor;
   *extractor = 0;
}


cv::ShapeTransformer* cvThinPlateSplineShapeTransformerCreate(double regularizationParameter)
{
   cv::Ptr<cv::ShapeTransformer> ptr = cv::createThinPlateSplineShapeTransformer(regularizationParameter);
   ptr.addref();
   return ptr.get();
}

cv::ShapeTransformer* cvAffineTransformerCreate(bool fullAffine)
{
   cv::Ptr<cv::ShapeTransformer> ptr = cv::createThinPlateSplineShapeTransformer(fullAffine);
   ptr.addref();
   return ptr.get();
}

void cvShapeTransformerRelease(cv::ShapeTransformer** transformer)
{
   delete * transformer;
   *transformer = 0;
}


float cvShapeDistanceExtractorComputeDistance(cv::ShapeDistanceExtractor* extractor, std::vector<cv::Point>* contour1, std::vector<cv::Point>* contour2)
{
   return extractor->computeDistance(*contour1, *contour2);
}

cv::ShapeContextDistanceExtractor* cvShapeContextDistanceExtractorCreate(
   int nAngularBins, int nRadialBins, float innerRadius, float outerRadius, int iterations,
   cv::HistogramCostExtractor* comparer, cv::ShapeTransformer* transformer)
{
   cv::Ptr<cv::HistogramCostExtractor> comparerPtr(comparer);
   comparerPtr.addref();
   cv::Ptr<cv::ShapeTransformer> transformerPtr(transformer);
   transformerPtr.addref();
   cv::Ptr<cv::ShapeContextDistanceExtractor> ptr = cv::createShapeContextDistanceExtractor(nAngularBins, nRadialBins, innerRadius, outerRadius, iterations, comparerPtr, transformerPtr);
   ptr.addref();
   return ptr.get();
}

void cvShapeContextDistanceExtractorRelease(cv::ShapeContextDistanceExtractor** extractor)
{
   delete *extractor;
   *extractor = 0;
}


cv::HausdorffDistanceExtractor* cvHausdorffDistanceExtractorCreate(int distanceFlag, float rankProp)
{
   cv::Ptr<cv::HausdorffDistanceExtractor> ptr = cv::createHausdorffDistanceExtractor(distanceFlag, rankProp);
   ptr.addref();
   return ptr.get();
}
void cvHausdorffDistanceExtractorRelease(cv::HausdorffDistanceExtractor** extractor)
{
   delete *extractor;
   *extractor = 0;
}