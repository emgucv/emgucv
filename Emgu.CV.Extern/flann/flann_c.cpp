//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "flann_c.h"

cv::flann::LinearIndexParams* cveLinearIndexParamsCreate(cv::flann::IndexParams** ip)
{
   cv::flann::LinearIndexParams* p = new cv::flann::LinearIndexParams();
   *ip = dynamic_cast<cv::flann::IndexParams*>( p );
   return p;
}
void cveLinearIndexParamsRelease( cv::flann::LinearIndexParams** p)
{
   delete *p;
   *p = 0;
}

cv::flann::KDTreeIndexParams* cveKDTreeIndexParamsCreate(cv::flann::IndexParams** ip, int trees)
{
   cv::flann::KDTreeIndexParams* p = new cv::flann::KDTreeIndexParams(trees);
   *ip = dynamic_cast<cv::flann::IndexParams*>( p );
   return p;
}
void cveKDTreeIndexParamsRelease( cv::flann::KDTreeIndexParams** p)
{
   delete *p;
   *p = 0;
}

cv::flann::LshIndexParams* cveLshIndexParamsCreate(cv::flann::IndexParams** ip, int tableNumber, int keySize, int multiProbeLevel)
{
   cv::flann::LshIndexParams* p = new cv::flann::LshIndexParams(tableNumber, keySize, multiProbeLevel);
   *ip = dynamic_cast<cv::flann::IndexParams*>( p );
   return p;
}
void cveLshIndexParamsRelease( cv::flann::LshIndexParams** p)
{
   delete *p;
   *p = 0;
}

cv::flann::KMeansIndexParams* cveKMeansIndexParamsCreate(cv::flann::IndexParams** ip, int branching, int iterations, cvflann::flann_centers_init_t centersInit, float cbIndex)
{
   cv::flann::KMeansIndexParams* p = new cv::flann::KMeansIndexParams(branching, iterations, centersInit, cbIndex);
   *ip = dynamic_cast<cv::flann::IndexParams*> ( p );
   return p;
}
void cveKMeansIndexParamsRelease( cv::flann::KMeansIndexParams** p)
{
   delete *p;
   *p = 0;
}

cv::flann::CompositeIndexParams* cveCompositeIndexParamsCreate(cv::flann::IndexParams** ip, int trees, int branching, int iterations, cvflann::flann_centers_init_t centersInit, float cbIndex)
{
   cv::flann::CompositeIndexParams* p = new cv::flann::CompositeIndexParams(trees, branching, iterations, centersInit, cbIndex);
   *ip = dynamic_cast<cv::flann::IndexParams*>( p );
   return p;
}
void cveCompositeIndexParamsRelease( cv::flann::CompositeIndexParams** p)
{
   delete *p;
   *p = 0;
}

cv::flann::AutotunedIndexParams* cveAutotunedIndexParamsCreate(cv::flann::IndexParams** ip, float targetPrecision, float buildWeight, float memoryWeight, float sampleFraction)
{
   cv::flann::AutotunedIndexParams* p = new cv::flann::AutotunedIndexParams(targetPrecision, buildWeight, memoryWeight, sampleFraction);
   *ip = dynamic_cast<cv::flann::IndexParams*>( p );
   return p;
}
void cveAutotunedIndexParamsRelease( cv::flann::AutotunedIndexParams** p)
{
   delete *p;
   *p = 0;
}

cv::flann::HierarchicalClusteringIndexParams* cveHierarchicalClusteringIndexParamsCreate(cv::flann::IndexParams** ip, int branching, cvflann::flann_centers_init_t centersInit, int trees, int leafSize )
{
   cv::flann::HierarchicalClusteringIndexParams* p = new cv::flann::HierarchicalClusteringIndexParams(branching, centersInit, trees, leafSize);
   *ip = dynamic_cast<cv::flann::IndexParams*>( p );
   return p;
}
void cveHierarchicalClusteringIndexParamsRelease( cv::flann::HierarchicalClusteringIndexParams** p)
{
   delete *p;
   *p = 0;
}

cv::flann::SearchParams* cveSearchParamsCreate(cv::flann::IndexParams** ip, int checks, float eps, bool sorted )
{
   cv::flann::SearchParams* p = new cv::flann::SearchParams(checks, eps, sorted);
   *ip = dynamic_cast<cv::flann::IndexParams*>( p );
   return p;
}
void cveSearchParamsRelease( cv::flann::SearchParams** p)
{
   delete *p;
   *p = 0;
}

cv::flann::Index* cveFlannIndexCreate(cv::_InputArray* features, cv::flann::IndexParams* p)
{
   return new cv::flann::Index(*features, *p);
}

void cveFlannIndexKnnSearch(cv::flann::Index* index, cv::_InputArray* queries, cv::_OutputArray* indices, cv::_OutputArray* dists, int knn, int checks, float eps, bool sorted)
{
   cv::flann::SearchParams p(checks, eps, sorted);
   index->knnSearch(*queries, *indices, *dists, knn, p);
}

int cveFlannIndexRadiusSearch(cv::flann::Index* index, cv::_InputArray* queries, cv::_OutputArray* indices, cv::_OutputArray* dists, double radius, int maxResults, int checks, float eps, bool sorted)
{
   cv::flann::SearchParams p(checks, eps, sorted);
   return index->radiusSearch(*queries, *indices, *dists, radius, maxResults, p);
}

void cveFlannIndexRelease(cv::flann::Index** index) 
{ 
   delete *index; 
   *index = 0;
}
