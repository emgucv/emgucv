#include "core_c.h"
#include "opencv2\features2d\features2d.hpp"

CVAPI(int) CvHierarchicalClustering(CvMat* features, CvMat* centers, cv::cvflann::KMeansIndexParams* params)
{
   cv::Mat f = cv::cvarrToMat(features);
   cv::Mat c = cv::cvarrToMat(centers);
   return cv::cvflann::hierarchicalClustering(f, c, *params);
}

CVAPI(cv::cvflann::Index*) CvFlannIndexCreateKDTree(CvMat* features, int trees)
{
   cv::cvflann::KDTreeIndexParams param = cv::cvflann::KDTreeIndexParams(trees);
   cv::Mat f = cv::cvarrToMat(features);
   return new cv::cvflann::Index(f, param);
}

CVAPI(cv::cvflann::Index*) CvFlannIndexCreateLinear(CvMat* features)
{
   cv::cvflann::LinearIndexParams param;
   cv::Mat f = cv::cvarrToMat(features);
   return new cv::cvflann::Index(f, param);
}

CVAPI(cv::cvflann::Index*) CvFlannIndexCreateKMeans(CvMat* features, int branching_, int iterations_, cv::cvflann::flann_centers_init_t centers_init_, float cb_index_)
{
   cv::cvflann::KMeansIndexParams param = cv::cvflann::KMeansIndexParams(branching_, iterations_, centers_init_, cb_index_);
   cv::Mat f = cv::cvarrToMat(features);
   return new cv::cvflann::Index(f, param);
}

CVAPI(cv::cvflann::Index*) CvFlannIndexCreateComposite(CvMat* features, int trees, int branching_, int iterations_, cv::cvflann::flann_centers_init_t centers_init_, float cb_index_)
{
   cv::cvflann::CompositeIndexParams param = cv::cvflann::CompositeIndexParams(trees, branching_, iterations_, centers_init_, cb_index_);
   cv::Mat f = cv::cvarrToMat(features);
   return new cv::cvflann::Index(f, param);
}

CVAPI(cv::cvflann::Index*) CvFlannIndexCreateAutotuned(CvMat* features, float target_precision, float build_weight, float memory_weight, float sample_fraction)
{
	cv::cvflann::AutotunedIndexParams param = cv::cvflann::AutotunedIndexParams(target_precision, build_weight, memory_weight, sample_fraction);
	cv::Mat f = cv::cvarrToMat(features);
	return new cv::cvflann::Index(f, param);
}

CVAPI(void) CvFlannIndexKnnSearch(cv::cvflann::Index* index, CvMat* queries, CvMat* indices, CvMat* dists, int knn, int checks)
{
   cv::cvflann::SearchParams p = cv::cvflann::SearchParams(checks);
   cv::Mat queriesMat = cv::cvarrToMat(queries); 
   cv::Mat indicesMat = cv::cvarrToMat(indices);
   cv::Mat distsMat = cv::cvarrToMat(dists);
   index->knnSearch(queriesMat, indicesMat, distsMat, knn, p);
}

CVAPI(int) CvFlannIndexRadiusSearch(cv::cvflann::Index* index, CvMat* queries, CvMat* indices, CvMat* dists, float radius, int checks)
{
   cv::cvflann::SearchParams p = cv::cvflann::SearchParams(checks);
   cv::Mat queriesMat = cv::cvarrToMat(queries); 
   cv::Mat indicesMat = cv::cvarrToMat(indices);
   cv::Mat distsMat = cv::cvarrToMat(dists);
   return index->radiusSearch(queriesMat, indicesMat, distsMat, radius, p);
}

CVAPI(void) CvFlannIndexRelease(cv::cvflann::Index* index) { delete index; }
