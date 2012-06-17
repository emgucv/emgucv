//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CORE_C_H
#define EMGU_CORE_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/imgproc/imgproc.hpp"

CVAPI(cv::Mat*) cvMatCreate();
CVAPI(void) cvMatRelease(cv::Mat** mat);
CVAPI(CvSize) cvMatGetSize(cv::Mat* mat);
CVAPI(void) cvMatCopyToCvArr(cv::Mat* mat, CvArr* cvArray);

#endif