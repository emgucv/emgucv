//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "flann_c.h"

cv::flann::LinearIndexParams* cveLinearIndexParamsCreate(cv::flann::IndexParams** ip)
{
#ifdef HAVE_OPENCV_FLANN
	cv::flann::LinearIndexParams* p = new cv::flann::LinearIndexParams();
	*ip = dynamic_cast<cv::flann::IndexParams*>(p);
	return p;
#else
	throw_no_flann();
#endif
}
void cveLinearIndexParamsRelease(cv::flann::LinearIndexParams** p)
{
#ifdef HAVE_OPENCV_FLANN
	delete* p;
	*p = 0;
#else
	throw_no_flann();
#endif
}

cv::flann::KDTreeIndexParams* cveKDTreeIndexParamsCreate(cv::flann::IndexParams** ip, int trees)
{
#ifdef HAVE_OPENCV_FLANN
	cv::flann::KDTreeIndexParams* p = new cv::flann::KDTreeIndexParams(trees);
	*ip = dynamic_cast<cv::flann::IndexParams*>(p);
	return p;
#else
	throw_no_flann();
#endif
}
void cveKDTreeIndexParamsRelease(cv::flann::KDTreeIndexParams** p)
{
#ifdef HAVE_OPENCV_FLANN
	delete* p;
	*p = 0;
#else
	throw_no_flann();
#endif
}

cv::flann::LshIndexParams* cveLshIndexParamsCreate(cv::flann::IndexParams** ip, int tableNumber, int keySize, int multiProbeLevel)
{
#ifdef HAVE_OPENCV_FLANN
	cv::flann::LshIndexParams* p = new cv::flann::LshIndexParams(tableNumber, keySize, multiProbeLevel);
	*ip = dynamic_cast<cv::flann::IndexParams*>(p);
	return p;
#else
	throw_no_flann();
#endif
}
void cveLshIndexParamsRelease(cv::flann::LshIndexParams** p)
{
#ifdef HAVE_OPENCV_FLANN
	delete* p;
	*p = 0;
#else
	throw_no_flann();
#endif
}

cv::flann::KMeansIndexParams* cveKMeansIndexParamsCreate(cv::flann::IndexParams** ip, int branching, int iterations, cvflann::flann_centers_init_t centersInit, float cbIndex)
{
#ifdef HAVE_OPENCV_FLANN
	cv::flann::KMeansIndexParams* p = new cv::flann::KMeansIndexParams(branching, iterations, centersInit, cbIndex);
	*ip = dynamic_cast<cv::flann::IndexParams*> (p);
	return p;
#else
	throw_no_flann();
#endif
}
void cveKMeansIndexParamsRelease(cv::flann::KMeansIndexParams** p)
{
#ifdef HAVE_OPENCV_FLANN
	delete* p;
	*p = 0;
#else
	throw_no_flann();
#endif
}

cv::flann::CompositeIndexParams* cveCompositeIndexParamsCreate(cv::flann::IndexParams** ip, int trees, int branching, int iterations, cvflann::flann_centers_init_t centersInit, float cbIndex)
{
#ifdef HAVE_OPENCV_FLANN
	cv::flann::CompositeIndexParams* p = new cv::flann::CompositeIndexParams(trees, branching, iterations, centersInit, cbIndex);
	*ip = dynamic_cast<cv::flann::IndexParams*>(p);
	return p;
#else
	throw_no_flann();
#endif
}
void cveCompositeIndexParamsRelease(cv::flann::CompositeIndexParams** p)
{
#ifdef HAVE_OPENCV_FLANN
	delete* p;
	*p = 0;
#else
	throw_no_flann();
#endif
}

cv::flann::AutotunedIndexParams* cveAutotunedIndexParamsCreate(cv::flann::IndexParams** ip, float targetPrecision, float buildWeight, float memoryWeight, float sampleFraction)
{
#ifdef HAVE_OPENCV_FLANN
	cv::flann::AutotunedIndexParams* p = new cv::flann::AutotunedIndexParams(targetPrecision, buildWeight, memoryWeight, sampleFraction);
	*ip = dynamic_cast<cv::flann::IndexParams*>(p);
	return p;
#else
	throw_no_flann();
#endif
}
void cveAutotunedIndexParamsRelease(cv::flann::AutotunedIndexParams** p)
{
#ifdef HAVE_OPENCV_FLANN
	delete* p;
	*p = 0;
#else
	throw_no_flann();
#endif
}

cv::flann::HierarchicalClusteringIndexParams* cveHierarchicalClusteringIndexParamsCreate(cv::flann::IndexParams** ip, int branching, cvflann::flann_centers_init_t centersInit, int trees, int leafSize)
{
#ifdef HAVE_OPENCV_FLANN
	cv::flann::HierarchicalClusteringIndexParams* p = new cv::flann::HierarchicalClusteringIndexParams(branching, centersInit, trees, leafSize);
	*ip = dynamic_cast<cv::flann::IndexParams*>(p);
	return p;
#else
	throw_no_flann();
#endif
}
void cveHierarchicalClusteringIndexParamsRelease(cv::flann::HierarchicalClusteringIndexParams** p)
{
#ifdef HAVE_OPENCV_FLANN
	delete* p;
	*p = 0;
#else
	throw_no_flann();
#endif
}

cv::flann::SearchParams* cveSearchParamsCreate(cv::flann::IndexParams** ip, int checks, float eps, bool sorted)
{
#ifdef HAVE_OPENCV_FLANN
	cv::flann::SearchParams* p = new cv::flann::SearchParams(checks, eps, sorted);
	*ip = dynamic_cast<cv::flann::IndexParams*>(p);
	return p;
#else
	throw_no_flann();
#endif
}
void cveSearchParamsRelease(cv::flann::SearchParams** p)
{
#ifdef HAVE_OPENCV_FLANN
	delete* p;
	*p = 0;
#else
	throw_no_flann();
#endif
}

cv::flann::Index* cveFlannIndexCreate(cv::_InputArray* features, cv::flann::IndexParams* p, int distType)
{
#ifdef HAVE_OPENCV_FLANN
	return new cv::flann::Index(*features, *p, (cvflann::flann_distance_t)distType);
#else
	throw_no_flann();
#endif
}

void cveFlannIndexKnnSearch(cv::flann::Index* index, cv::_InputArray* queries, cv::_OutputArray* indices, cv::_OutputArray* dists, int knn, int checks, float eps, bool sorted)
{
#ifdef HAVE_OPENCV_FLANN
	cv::flann::SearchParams p(checks, eps, sorted);
	index->knnSearch(*queries, *indices, *dists, knn, p);
#else
	throw_no_flann();
#endif
}

int cveFlannIndexRadiusSearch(cv::flann::Index* index, cv::_InputArray* queries, cv::_OutputArray* indices, cv::_OutputArray* dists, double radius, int maxResults, int checks, float eps, bool sorted)
{
#ifdef HAVE_OPENCV_FLANN
	cv::flann::SearchParams p(checks, eps, sorted);
	return index->radiusSearch(*queries, *indices, *dists, radius, maxResults, p);
#else
	throw_no_flann();
#endif
}

void cveFlannIndexRelease(cv::flann::Index** index)
{
#ifdef HAVE_OPENCV_FLANN
	delete* index;
	*index = 0;
#else
	throw_no_flann();
#endif
}
