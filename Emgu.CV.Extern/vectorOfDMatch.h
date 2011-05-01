//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include <vector>
#include "opencv2/core/core_c.h"
#include "opencv2/features2d/features2d.hpp"

CVAPI(std::vector<cv::DMatch>*) VectorOfDMatchCreate();

CVAPI(void) VectorOfDMatchPushMatrix(std::vector<cv::DMatch>* matches, const CvMat* trainIdx, const CvMat* distance = 0, const CvMat* mask = 0);

CVAPI(std::vector<cv::DMatch>*) VectorOfDMatchCreateSize(int size);

CVAPI(int) VectorOfDMatchGetSize(std::vector<cv::DMatch>* v);

CVAPI(void) VectorOfDMatchPushMulti(std::vector<cv::DMatch>* v, cv::DMatch* values, int count);

CVAPI(void) VectorOfDMatchClear(std::vector<cv::DMatch>* v);

CVAPI(void) VectorOfDMatchRelease(std::vector<cv::DMatch>* v);

CVAPI(void) VectorOfDMatchCopyData(std::vector<cv::DMatch>* v, cv::DMatch* data);

CVAPI(cv::DMatch*) VectorOfDMatchGetStartAddress(std::vector<cv::DMatch>* v);