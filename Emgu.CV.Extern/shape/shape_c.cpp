//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "shape_c.h"

cv::HistogramCostExtractor* cvNormHistogramCostExtractorCreate(int flag, int nDummies, float defaultCost, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr)
{
	cv::Ptr<cv::HistogramCostExtractor> ptr = cv::createNormHistogramCostExtractor(flag, nDummies, defaultCost);
	*sharedPtr = new cv::Ptr<cv::HistogramCostExtractor>(ptr);
	return ptr.get();
}

cv::HistogramCostExtractor* cvEMDHistogramCostExtractorCreate(int flag, int nDummies, float defaultCost, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr)
{
	cv::Ptr<cv::HistogramCostExtractor> ptr = cv::createEMDHistogramCostExtractor(flag, nDummies, defaultCost);
	*sharedPtr = new cv::Ptr<cv::HistogramCostExtractor>(ptr);
	return ptr.get();
}

cv::HistogramCostExtractor* cvChiHistogramCostExtractorCreate(int nDummies, float defaultCost, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr)
{
	cv::Ptr<cv::HistogramCostExtractor> ptr = cv::createChiHistogramCostExtractor(nDummies, defaultCost);
	*sharedPtr = new cv::Ptr<cv::HistogramCostExtractor>(ptr);
	return ptr.get();
}

cv::HistogramCostExtractor* cvEMDL1HistogramCostExtractorCreate(int nDummies, float defaultCost, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr)
{
	cv::Ptr<cv::HistogramCostExtractor> ptr = cv::createEMDL1HistogramCostExtractor(nDummies, defaultCost);
	*sharedPtr = new cv::Ptr<cv::HistogramCostExtractor>(ptr);
	return ptr.get();
}

void cvHistogramCostExtractorRelease(cv::HistogramCostExtractor** extractor, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr)
{
	delete *sharedPtr;
	*extractor = 0;
	*sharedPtr = 0;
}


cv::ThinPlateSplineShapeTransformer* cvThinPlateSplineShapeTransformerCreate(double regularizationParameter, cv::ShapeTransformer** transformer, cv::Ptr<cv::ThinPlateSplineShapeTransformer>** sharedPtr)
{
	cv::Ptr<cv::ThinPlateSplineShapeTransformer> ptr = cv::createThinPlateSplineShapeTransformer(regularizationParameter);
	*sharedPtr = new cv::Ptr<cv::ThinPlateSplineShapeTransformer>(ptr);
	*transformer = dynamic_cast<cv::ShapeTransformer*>(ptr.get());
	return ptr.get();
}

void cvThinPlateSplineShapeTransformerRelease(cv::ThinPlateSplineShapeTransformer** transformer, cv::Ptr<cv::ThinPlateSplineShapeTransformer>** sharedPtr)
{
	delete *sharedPtr;
	*transformer = 0;
	*sharedPtr = 0;
}

cv::AffineTransformer* cvAffineTransformerCreate(bool fullAffine, cv::ShapeTransformer** transformer, cv::Ptr<cv::AffineTransformer>** sharedPtr)
{
	cv::Ptr<cv::AffineTransformer> ptr = cv::createAffineTransformer(fullAffine);
	*sharedPtr = new cv::Ptr<cv::AffineTransformer>(ptr);
	*transformer = dynamic_cast<cv::ShapeTransformer*>(ptr.get());
	return ptr.get();
}

void cvAffineTransformerRelease(cv::AffineTransformer** transformer, cv::Ptr<cv::AffineTransformer>** sharedPtr)
{
	delete *sharedPtr;
	*transformer = 0;
	*sharedPtr = 0;
}

float cvShapeDistanceExtractorComputeDistance(cv::ShapeDistanceExtractor* extractor, cv::_InputArray* contour1, cv::_InputArray* contour2)
{
	return extractor->computeDistance(*contour1, *contour2);
}

cv::ShapeContextDistanceExtractor* cvShapeContextDistanceExtractorCreate(
	int nAngularBins, int nRadialBins, float innerRadius, float outerRadius, int iterations,
	cv::HistogramCostExtractor* comparer, cv::ShapeTransformer* transformer, cv::ShapeDistanceExtractor** e,
	cv::Ptr<cv::ShapeContextDistanceExtractor>** sharedPtr)
{
	cv::Ptr<cv::HistogramCostExtractor> comparerPtr(comparer, [] (cv::HistogramCostExtractor*){});
	cv::Ptr<cv::ShapeTransformer> transformerPtr(transformer, [] (cv::ShapeTransformer*) {});
	cv::Ptr<cv::ShapeContextDistanceExtractor> ptr = cv::createShapeContextDistanceExtractor(nAngularBins, nRadialBins, innerRadius, outerRadius, iterations, comparerPtr, transformerPtr);
	*sharedPtr = new cv::Ptr<cv::ShapeContextDistanceExtractor>(ptr);

	*e = dynamic_cast<cv::ShapeDistanceExtractor*>(ptr.get());
	return ptr.get();
}

void cvShapeContextDistanceExtractorRelease(cv::ShapeContextDistanceExtractor** extractor, cv::Ptr<cv::ShapeContextDistanceExtractor>** sharedPtr)
{
	delete *sharedPtr;
	*extractor = 0;
	*sharedPtr = 0;
}

cv::HausdorffDistanceExtractor* cvHausdorffDistanceExtractorCreate(int distanceFlag, float rankProp, cv::ShapeDistanceExtractor** e, cv::Ptr<cv::HausdorffDistanceExtractor>** sharedPtr)
{
	cv::Ptr<cv::HausdorffDistanceExtractor> ptr = cv::createHausdorffDistanceExtractor(distanceFlag, rankProp);
	*sharedPtr = new cv::Ptr<cv::HausdorffDistanceExtractor>(ptr);
	*e = dynamic_cast<cv::ShapeDistanceExtractor*>(ptr.get());
	return ptr.get();
}
void cvHausdorffDistanceExtractorRelease(cv::HausdorffDistanceExtractor** extractor, cv::Ptr<cv::HausdorffDistanceExtractor>** sharedPtr)
{
	delete *sharedPtr;
	*extractor = 0;
	*sharedPtr = 0;
}