//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_FLANN_C_H
#define EMGU_FLANN_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/flann/flann.hpp"

//flann index
CVAPI(cv::flann::Index*) CvFlannIndexCreateKDTree(cv::_InputArray* features, int trees);

CVAPI(cv::flann::Index*) CvFlannIndexCreateLinear(cv::_InputArray* features);

CVAPI(cv::flann::Index*) CvFlannIndexCreateLSH(cv::_InputArray* features, int tableNumber, int keySize, int multiProbeLevel);

CVAPI(cv::flann::Index*) CvFlannIndexCreateKMeans(cv::_InputArray* features, int branching_, int iterations_, cvflann::flann_centers_init_t centers_init_, float cb_index_);

CVAPI(cv::flann::Index*) CvFlannIndexCreateComposite(cv::_InputArray* features, int trees, int branching_, int iterations_, cvflann::flann_centers_init_t centers_init_, float cb_index_);

CVAPI(cv::flann::Index*) CvFlannIndexCreateAutotuned(cv::_InputArray* features, float target_precision, float build_weight, float memory_weight, float sample_fraction);

CVAPI(void) CvFlannIndexKnnSearch(cv::flann::Index* index, cv::_InputArray* queries, cv::_OutputArray* indices, cv::_OutputArray* dists, int knn, int checks);

CVAPI(int) CvFlannIndexRadiusSearch(cv::flann::Index* index, cv::_InputArray* queries, cv::_OutputArray* indices, cv::_OutputArray* dists, float radius, int maxResults, int checks);

CVAPI(void) CvFlannIndexRelease(cv::flann::Index* index);

#endif