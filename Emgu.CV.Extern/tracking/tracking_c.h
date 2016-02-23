//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_TRACKING_C_H
#define EMGU_TRACKING_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/tracking/tracking.hpp"

CVAPI(cv::Tracker*) cveTrackerCreate(cv::String* trackerType);
CVAPI(bool) cveTrackerInit(cv::Tracker* tracker, cv::Mat* image, CvRect* boundingBox);
CVAPI(bool) cveTrackerUpdate(cv::Tracker* tracker, cv::Mat* image, CvRect* boundingBox);
CVAPI(void) cveTrackerRelease(cv::Tracker** tracker);
#endif