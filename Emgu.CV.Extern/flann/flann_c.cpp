//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "flann_c.h"

cv::flann::Index* CvFlannIndexCreateKDTree(CvMat* features, int trees)
{
   cv::flann::KDTreeIndexParams param = cv::flann::KDTreeIndexParams(trees);
   cv::Mat f = cv::cvarrToMat(features);
   return new cv::flann::Index(f, param);
}

cv::flann::Index* CvFlannIndexCreateLinear(CvMat* features)
{
   cv::flann::LinearIndexParams param;
   cv::Mat f = cv::cvarrToMat(features);
   return new cv::flann::Index(f, param);
}

cv::flann::Index* CvFlannIndexCreateKMeans(CvMat* features, int branching_, int iterations_, cvflann::flann_centers_init_t centers_init_, float cb_index_)
{
   cv::flann::KMeansIndexParams param = cv::flann::KMeansIndexParams(branching_, iterations_, centers_init_, cb_index_);
   cv::Mat f = cv::cvarrToMat(features);
   return new cv::flann::Index(f, param);
}

cv::flann::Index* CvFlannIndexCreateComposite(CvMat* features, int trees, int branching_, int iterations_, cvflann::flann_centers_init_t centers_init_, float cb_index_)
{
   cv::flann::CompositeIndexParams param = cv::flann::CompositeIndexParams(trees, branching_, iterations_, centers_init_, cb_index_);
   cv::Mat f = cv::cvarrToMat(features);
   return new cv::flann::Index(f, param);
}

cv::flann::Index* CvFlannIndexCreateAutotuned(CvMat* features, float target_precision, float build_weight, float memory_weight, float sample_fraction)
{
	cv::flann::AutotunedIndexParams param = cv::flann::AutotunedIndexParams(target_precision, build_weight, memory_weight, sample_fraction);
	cv::Mat f = cv::cvarrToMat(features);
	return new cv::flann::Index(f, param);
}

void CvFlannIndexKnnSearch(cv::flann::Index* index, CvMat* queries, CvMat* indices, CvMat* dists, int knn, int checks)
{
   cv::flann::SearchParams p = cv::flann::SearchParams(checks);
   cv::Mat queriesMat = cv::cvarrToMat(queries); 
   cv::Mat indicesMat = cv::cvarrToMat(indices);
   cv::Mat distsMat = cv::cvarrToMat(dists);
   index->knnSearch(queriesMat, indicesMat, distsMat, knn, p);
}

int CvFlannIndexRadiusSearch(cv::flann::Index* index, CvMat* queries, CvMat* indices, CvMat* dists, float radius, int checks)
{
   cv::flann::SearchParams p = cv::flann::SearchParams(checks);
   cv::Mat queriesMat = cv::cvarrToMat(queries); 
   cv::Mat indicesMat = cv::cvarrToMat(indices);
   cv::Mat distsMat = cv::cvarrToMat(dists);
   return index->radiusSearch(queriesMat, indicesMat, distsMat, radius, indicesMat.cols, p);
}

void CvFlannIndexRelease(cv::flann::Index* index) { delete index; }
