//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
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


cv::ThinPlateSplineShapeTransformer* cvThinPlateSplineShapeTransformerCreate(double regularizationParameter, cv::ShapeTransformer** transformer)
{
   cv::Ptr<cv::ThinPlateSplineShapeTransformer> ptr = cv::createThinPlateSplineShapeTransformer(regularizationParameter);
   ptr.addref();
   *transformer = dynamic_cast<cv::ShapeTransformer*>(ptr.get());
   return ptr.get();
}

cv::AffineTransformer* cvAffineTransformerCreate(bool fullAffine, cv::ShapeTransformer** transformer)
{
   cv::Ptr<cv::AffineTransformer> ptr = cv::createAffineTransformer(fullAffine);
   ptr.addref();
   *transformer = dynamic_cast<cv::ShapeTransformer*>(ptr.get());
   return ptr.get();
}

void cvThinPlateSplineShapeTransformerRelease(cv::ThinPlateSplineShapeTransformer** transformer)
{
   delete * transformer;
   *transformer = 0;
}

void cvAffineTransformerRelease(cv::AffineTransformer** transformer)
{
   delete * transformer;
   *transformer = 0;
}

float cvShapeDistanceExtractorComputeDistance(cv::ShapeDistanceExtractor* extractor, cv::_InputArray* contour1, cv::_InputArray* contour2)
{
   return extractor->computeDistance(*contour1, *contour2);
}

cv::ShapeContextDistanceExtractor* cvShapeContextDistanceExtractorCreate(
   int nAngularBins, int nRadialBins, float innerRadius, float outerRadius, int iterations,
   cv::HistogramCostExtractor* comparer, cv::ShapeTransformer* transformer, cv::ShapeDistanceExtractor** e)
{
   cv::Ptr<cv::HistogramCostExtractor> comparerPtr(comparer);
   comparerPtr.addref();
   cv::Ptr<cv::ShapeTransformer> transformerPtr(transformer);
   transformerPtr.addref();
   cv::Ptr<cv::ShapeContextDistanceExtractor> ptr = cv::createShapeContextDistanceExtractor(nAngularBins, nRadialBins, innerRadius, outerRadius, iterations, comparerPtr, transformerPtr);
   ptr.addref();
   *e = dynamic_cast<cv::ShapeDistanceExtractor*>(ptr.get());
   return ptr.get();
}

void cvShapeContextDistanceExtractorRelease(cv::ShapeContextDistanceExtractor** extractor)
{
   delete *extractor;
   *extractor = 0;
}

cv::HausdorffDistanceExtractor* cvHausdorffDistanceExtractorCreate(int distanceFlag, float rankProp, cv::ShapeDistanceExtractor** e)
{
   cv::Ptr<cv::HausdorffDistanceExtractor> ptr = cv::createHausdorffDistanceExtractor(distanceFlag, rankProp);
   ptr.addref();
   *e = dynamic_cast<cv::ShapeDistanceExtractor*>(ptr.get());
   return ptr.get();
}
void cvHausdorffDistanceExtractorRelease(cv::HausdorffDistanceExtractor** extractor)
{
   delete *extractor;
   *extractor = 0;
}