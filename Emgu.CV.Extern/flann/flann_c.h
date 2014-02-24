//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_FLANN_C_H
#define EMGU_FLANN_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/flann/flann.hpp"

//flann index
CVAPI(cv::flann::Index*) CvFlannIndexCreateKDTree(CvMat* features, int trees);

CVAPI(cv::flann::Index*) CvFlannIndexCreateLinear(CvMat* features);

CVAPI(cv::flann::Index*) CvFlannIndexCreateLSH(CvMat* features, int tableNumber, int keySize, int multiProbeLevel);

CVAPI(cv::flann::Index*) CvFlannIndexCreateKMeans(CvMat* features, int branching_, int iterations_, cvflann::flann_centers_init_t centers_init_, float cb_index_);

CVAPI(cv::flann::Index*) CvFlannIndexCreateComposite(CvMat* features, int trees, int branching_, int iterations_, cvflann::flann_centers_init_t centers_init_, float cb_index_);

CVAPI(cv::flann::Index*) CvFlannIndexCreateAutotuned(CvMat* features, float target_precision, float build_weight, float memory_weight, float sample_fraction);

CVAPI(void) CvFlannIndexKnnSearch(cv::flann::Index* index, CvMat* queries, CvMat* indices, CvMat* dists, int knn, int checks);

CVAPI(int) CvFlannIndexRadiusSearch(cv::flann::Index* index, CvMat* queries, CvMat* indices, CvMat* dists, float radius, int checks);

CVAPI(void) CvFlannIndexRelease(cv::flann::Index* index);

#endif