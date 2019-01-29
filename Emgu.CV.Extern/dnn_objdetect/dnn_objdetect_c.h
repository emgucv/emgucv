//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_DNN_OBJDETECT_C_H
#define EMGU_DNN_OBJDETECT_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/core_detect.hpp"

CVAPI(cv::dnn_objdetect::InferBbox*) cveInferBboxCreate(cv::Mat* deltaBbox, cv::Mat* classScores, cv::Mat* confScores);
CVAPI(void) cveInferBboxFilter(cv::dnn_objdetect::InferBbox* inferBbox, double thresh);
CVAPI(void) cveInferBboxRelease(cv::dnn_objdetect::InferBbox** inferBbox);

#endif