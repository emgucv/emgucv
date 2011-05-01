//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include <vector>
#include "opencv2/core/core_c.h"
#include "opencv2/features2d/features2d.hpp"

CVAPI(std::vector<cv::KeyPoint>*) VectorOfKeyPointCreate();

CVAPI(std::vector<cv::KeyPoint>*) VectorOfKeyPointCreateSize(int size);

CVAPI(int) VectorOfKeyPointGetSize(std::vector<cv::KeyPoint>* v);

CVAPI(void) VectorOfKeyPointPushMulti(std::vector<cv::KeyPoint>* v, cv::KeyPoint* values, int count);

CVAPI(void) VectorOfKeyPointClear(std::vector<cv::KeyPoint>* v);

CVAPI(void) VectorOfKeyPointRelease(std::vector<cv::KeyPoint>* v);

CVAPI(void) VectorOfKeyPointCopyData(std::vector<cv::KeyPoint>* v, cv::KeyPoint* data);

CVAPI(cv::KeyPoint*) VectorOfKeyPointGetStartAddress(std::vector<cv::KeyPoint>* v);