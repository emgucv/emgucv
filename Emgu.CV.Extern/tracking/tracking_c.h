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

CVAPI(cv::MultiTracker*) cveMultiTrackerInit(cv::String* trackerType);
CVAPI(bool) cveMultiTrackerAdd(cv::MultiTracker* tracker, cv::Mat* image, CvRect* boundingBox);
CVAPI(bool) cveMultiTrackerAddType(cv::MultiTracker* tracker, cv::String* trackerType, cv::Mat* image, CvRect* boundingBox);
CVAPI(bool) cveMultiTrackerUpdate(cv::MultiTracker* tracker, cv::Mat* image, std::vector<CvRect>* boundingBox);
CVAPI(void) cveMultiTrackerRelease(cv::MultiTracker** tracker);

#endif