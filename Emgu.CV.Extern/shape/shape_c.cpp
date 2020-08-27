//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "shape_c.h"

cv::HistogramCostExtractor* cveNormHistogramCostExtractorCreate(int flag, int nDummies, float defaultCost, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr)
{
#ifdef HAVE_OPENCV_SHAPE
	cv::Ptr<cv::HistogramCostExtractor> ptr = cv::createNormHistogramCostExtractor(flag, nDummies, defaultCost);
	*sharedPtr = new cv::Ptr<cv::HistogramCostExtractor>(ptr);
	return ptr.get();
#else
	throw_no_shape();
#endif
}

cv::HistogramCostExtractor* cveEMDHistogramCostExtractorCreate(int flag, int nDummies, float defaultCost, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr)
{
#ifdef HAVE_OPENCV_SHAPE
	cv::Ptr<cv::HistogramCostExtractor> ptr = cv::createEMDHistogramCostExtractor(flag, nDummies, defaultCost);
	*sharedPtr = new cv::Ptr<cv::HistogramCostExtractor>(ptr);
	return ptr.get();
#else
	throw_no_shape();
#endif
}

cv::HistogramCostExtractor* cveChiHistogramCostExtractorCreate(int nDummies, float defaultCost, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr)
{
#ifdef HAVE_OPENCV_SHAPE
	cv::Ptr<cv::HistogramCostExtractor> ptr = cv::createChiHistogramCostExtractor(nDummies, defaultCost);
	*sharedPtr = new cv::Ptr<cv::HistogramCostExtractor>(ptr);
	return ptr.get();
#else
	throw_no_shape();
#endif
}

cv::HistogramCostExtractor* cveEMDL1HistogramCostExtractorCreate(int nDummies, float defaultCost, cv::Ptr<cv::HistogramCostExtractor>** sharedPtr)
{
#ifdef HAVE_OPENCV_SHAPE
	cv::Ptr<cv::HistogramCostExtractor> ptr = cv::createEMDL1HistogramCostExtractor(nDummies, defaultCost);
	*sharedPtr = new cv::Ptr<cv::HistogramCostExtractor>(ptr);
	return ptr.get();
#else
	throw_no_shape();
#endif
}

void cveHistogramCostExtractorRelease(cv::Ptr<cv::HistogramCostExtractor>** sharedPtr)
{
#ifdef HAVE_OPENCV_SHAPE
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_shape();
#endif
}

cv::ThinPlateSplineShapeTransformer* cveThinPlateSplineShapeTransformerCreate(double regularizationParameter, cv::ShapeTransformer** transformer, cv::Ptr<cv::ThinPlateSplineShapeTransformer>** sharedPtr)
{
#ifdef HAVE_OPENCV_SHAPE
	cv::Ptr<cv::ThinPlateSplineShapeTransformer> ptr = cv::createThinPlateSplineShapeTransformer(regularizationParameter);
	*sharedPtr = new cv::Ptr<cv::ThinPlateSplineShapeTransformer>(ptr);
	*transformer = dynamic_cast<cv::ShapeTransformer*>(ptr.get());
	return (*sharedPtr)->get();
#else
	throw_no_shape();
#endif
}

void cveThinPlateSplineShapeTransformerRelease(cv::Ptr<cv::ThinPlateSplineShapeTransformer>** sharedPtr)
{
#ifdef HAVE_OPENCV_SHAPE
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_shape();
#endif
}

cv::AffineTransformer* cveAffineTransformerCreate(bool fullAffine, cv::ShapeTransformer** transformer, cv::Ptr<cv::AffineTransformer>** sharedPtr)
{
#ifdef HAVE_OPENCV_SHAPE
	cv::Ptr<cv::AffineTransformer> ptr = cv::createAffineTransformer(fullAffine);
	*sharedPtr = new cv::Ptr<cv::AffineTransformer>(ptr);
	*transformer = dynamic_cast<cv::ShapeTransformer*>(ptr.get());
	return (*sharedPtr)->get();
#else
	throw_no_shape();
#endif
}

void cveAffineTransformerRelease(cv::Ptr<cv::AffineTransformer>** sharedPtr)
{
#ifdef HAVE_OPENCV_SHAPE
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_shape();
#endif
}

void cveShapeTransformerEstimateTransformation(
	cv::ShapeTransformer* transformer,
	cv::_InputArray* transformingShape,
	cv::_InputArray* targetShape,
	std::vector<cv::DMatch>* matches)
{
#ifdef HAVE_OPENCV_SHAPE
	transformer->estimateTransformation(*transformingShape, *targetShape, *matches);
#else
	throw_no_shape();
#endif
}

float cveShapeTransformerApplyTransformation(
	cv::ShapeTransformer* transformer,
	cv::_InputArray* input,
	cv::_OutputArray* output)
{
#ifdef HAVE_OPENCV_SHAPE
	return transformer->applyTransformation(*input, output ? *output : static_cast<cv::OutputArray>(cv::noArray()));
#else
	throw_no_shape();
#endif
}

void cveShapeTransformerWarpImage(
	cv::ShapeTransformer* transformer,
	cv::_InputArray* transformingImage,
	cv::_OutputArray* output,
	int flags,
	int borderMode,
	CvScalar* borderValue)
{
#ifdef HAVE_OPENCV_SHAPE
	transformer->warpImage(*transformingImage, *output, flags, borderMode, *borderValue);
#else
	throw_no_shape();
#endif
}

float cveShapeDistanceExtractorComputeDistance(cv::ShapeDistanceExtractor* extractor, cv::_InputArray* contour1, cv::_InputArray* contour2)
{
#ifdef HAVE_OPENCV_SHAPE
	return extractor->computeDistance(*contour1, *contour2);
#else
	throw_no_shape();
#endif
}

cv::ShapeContextDistanceExtractor* cveShapeContextDistanceExtractorCreate(
	int nAngularBins, int nRadialBins, float innerRadius, float outerRadius, int iterations,
	cv::HistogramCostExtractor* comparer, cv::ShapeTransformer* transformer, cv::ShapeDistanceExtractor** e,
	cv::Ptr<cv::ShapeContextDistanceExtractor>** sharedPtr)
{
#ifdef HAVE_OPENCV_SHAPE
	cv::Ptr<cv::HistogramCostExtractor> comparerPtr(comparer, [] (cv::HistogramCostExtractor*){});
	cv::Ptr<cv::ShapeTransformer> transformerPtr(transformer, [] (cv::ShapeTransformer*) {});
	cv::Ptr<cv::ShapeContextDistanceExtractor> ptr = cv::createShapeContextDistanceExtractor(nAngularBins, nRadialBins, innerRadius, outerRadius, iterations, comparerPtr, transformerPtr);
	*sharedPtr = new cv::Ptr<cv::ShapeContextDistanceExtractor>(ptr);

	*e = dynamic_cast<cv::ShapeDistanceExtractor*>(ptr.get());
	return ptr.get();
#else
	throw_no_shape();
#endif
}

void cveShapeContextDistanceExtractorRelease(cv::Ptr<cv::ShapeContextDistanceExtractor>** sharedPtr)
{
#ifdef HAVE_OPENCV_SHAPE
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_shape();
#endif
}

cv::HausdorffDistanceExtractor* cveHausdorffDistanceExtractorCreate(int distanceFlag, float rankProp, cv::ShapeDistanceExtractor** e, cv::Ptr<cv::HausdorffDistanceExtractor>** sharedPtr)
{
#ifdef HAVE_OPENCV_SHAPE
	cv::Ptr<cv::HausdorffDistanceExtractor> ptr = cv::createHausdorffDistanceExtractor(distanceFlag, rankProp);
	*sharedPtr = new cv::Ptr<cv::HausdorffDistanceExtractor>(ptr);
	*e = dynamic_cast<cv::ShapeDistanceExtractor*>(ptr.get());
	return ptr.get();
#else
	throw_no_shape();
#endif
}
void cveHausdorffDistanceExtractorRelease(cv::Ptr<cv::HausdorffDistanceExtractor>** sharedPtr)
{
#ifdef HAVE_OPENCV_SHAPE
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_shape();
#endif
}