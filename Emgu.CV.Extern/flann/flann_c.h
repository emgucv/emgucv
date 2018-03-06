//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_FLANN_C_H
#define EMGU_FLANN_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/flann/flann.hpp"


//flann index
CVAPI(cv::flann::LinearIndexParams*) cveLinearIndexParamsCreate(cv::flann::IndexParams** ip);
CVAPI(void) cveLinearIndexParamsRelease( cv::flann::LinearIndexParams** p);

CVAPI(cv::flann::KDTreeIndexParams*) cveKDTreeIndexParamsCreate(cv::flann::IndexParams** ip, int trees);
CVAPI(void) cveKDTreeIndexParamsRelease( cv::flann::KDTreeIndexParams** p);

CVAPI(cv::flann::LshIndexParams*) cveLshIndexParamsCreate(cv::flann::IndexParams** ip, int tableNumber, int keySize, int multiProbeLevel);
CVAPI(void) cveLshIndexParamsRelease( cv::flann::LshIndexParams** p);

CVAPI(cv::flann::KMeansIndexParams*) cveKMeansIndexParamsCreate(cv::flann::IndexParams** ip, int branching, int iterations, cvflann::flann_centers_init_t centersInit, float cbIndex);
CVAPI(void) cveKMeansIndexParamsRelease( cv::flann::KMeansIndexParams** p);

CVAPI(cv::flann::CompositeIndexParams*) cveCompositeIndexParamsCreate(cv::flann::IndexParams** ip, int trees, int branching, int iterations, cvflann::flann_centers_init_t centersInit, float cbIndex);
CVAPI(void) cveCompositeIndexParamsRelease( cv::flann::CompositeIndexParams** p);

CVAPI(cv::flann::AutotunedIndexParams*) cveAutotunedIndexParamsCreate(cv::flann::IndexParams** ip, float targetPrecision, float buildWeight, float memoryWeight, float sampleFraction);
CVAPI(void) cveAutotunedIndexParamsRelease( cv::flann::AutotunedIndexParams** p);

CVAPI(cv::flann::HierarchicalClusteringIndexParams*) cveHierarchicalClusteringIndexParamsCreate(cv::flann::IndexParams** ip, int branching, cvflann::flann_centers_init_t centersInit, int trees, int leafSize );
CVAPI(void) cveHierarchicalClusteringIndexParamsRelease( cv::flann::HierarchicalClusteringIndexParams** p);

CVAPI(cv::flann::SearchParams*) cveSearchParamsCreate(cv::flann::IndexParams** ip, int checks, float eps, bool sorted );
CVAPI(void) cveSearchParamsRelease( cv::flann::SearchParams** p);

CVAPI(cv::flann::Index*) cveFlannIndexCreate(cv::_InputArray* features, cv::flann::IndexParams* p);

CVAPI(void) cveFlannIndexKnnSearch(cv::flann::Index* index, cv::_InputArray* queries, cv::_OutputArray* indices, cv::_OutputArray* dists, int knn, int checks, float eps, bool sorted);

CVAPI(int) cveFlannIndexRadiusSearch(cv::flann::Index* index, cv::_InputArray* queries, cv::_OutputArray* indices, cv::_OutputArray* dists, double radius, int maxResults, int checks, float eps, bool sorted);

CVAPI(void) cveFlannIndexRelease(cv::flann::Index** index);

#endif