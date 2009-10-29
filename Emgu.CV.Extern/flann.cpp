#include "cvaux.h"

CVAPI(int) CvHierarchicalClustering(CvMat* features, CvMat* centers, cv::flann::KMeansIndexParams* params)
{
   cv::Mat f = cv::cvarrToMat(features);
   cv::Mat c = cv::cvarrToMat(centers);
   return cv::flann::hierarchicalClustering(f, c, *params);
}

CVAPI(cv::flann::Index*) CvFlannIndexCreateKDTree(CvMat* features, int trees)
{
   cv::flann::KDTreeIndexParams param = cv::flann::KDTreeIndexParams(trees);
   cv::Mat f = cv::cvarrToMat(features);
   return new cv::flann::Index(f, param);
}

CVAPI(cv::flann::Index*) CvFlannIndexCreateLinear(CvMat* features)
{
   cv::flann::LinearIndexParams param;
   cv::Mat f = cv::cvarrToMat(features);
   return new cv::flann::Index(f, param);
}

CVAPI(cv::flann::Index*) CvFlannIndexCreateKMeans(CvMat* features, int branching_, int iterations_, cv::flann::flann_centers_init_t centers_init_, float cb_index_)
{
   cv::flann::KMeansIndexParams param = cv::flann::KMeansIndexParams(branching_, iterations_, centers_init_, cb_index_);
   cv::Mat f = cv::cvarrToMat(features);
   return new cv::flann::Index(f, param);
}

CVAPI(cv::flann::Index*) CvFlannIndexCreateComposite(CvMat* features, int trees, int branching_, int iterations_, cv::flann::flann_centers_init_t centers_init_, float cb_index_)
{
   cv::flann::CompositeIndexParams param = cv::flann::CompositeIndexParams(trees, branching_, iterations_, centers_init_, cb_index_);
   cv::Mat f = cv::cvarrToMat(features);
   return new cv::flann::Index(f, param);
}

CVAPI(cv::flann::Index*) CvFlannIndexCreateAutotuned(CvMat* features, float target_precision, float build_weight, float memory_weight, float sample_fraction)
{
	cv::flann::AutotunedIndexParams param = cv::flann::AutotunedIndexParams(target_precision, build_weight, memory_weight, sample_fraction);
	cv::Mat f = cv::cvarrToMat(features);
	return new cv::flann::Index(f, param);
}

CVAPI(void) CvFlannIndexKnnSearch(cv::flann::Index* index, CvMat* queries, CvMat* indices, CvMat* dists, int knn, int checks)
{
   cv::flann::SearchParams p = cv::flann::SearchParams(checks);
   cv::Mat queriesMat = cv::cvarrToMat(queries); 
   cv::Mat indicesMat = cv::cvarrToMat(indices);
   cv::Mat distsMat = cv::cvarrToMat(dists);
   index->knnSearch(queriesMat, indicesMat, distsMat, knn, p);
}

CVAPI(void) CvFlannIndexRadiusSearch(cv::flann::Index* index, CvMat* queries, CvMat* indices, CvMat* dists, float radius, int checks)
{
   cv::flann::SearchParams p = cv::flann::SearchParams(checks);
   cv::Mat queriesMat = cv::cvarrToMat(queries); 
   cv::Mat indicesMat = cv::cvarrToMat(indices);
   cv::Mat distsMat = cv::cvarrToMat(dists);
   index->radiusSearch(queriesMat, indicesMat, distsMat, radius, p);
}

CVAPI(void) CvFlannIndexRelease(cv::flann::Index* index) { delete index; }
