//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_TRACKING_C_H
#define EMGU_TRACKING_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/tracking/tracking.hpp"

//CVAPI(cv::Tracker*) cveTrackerCreate(cv::String* trackerType);
CVAPI(bool) cveTrackerInit(cv::Tracker* tracker, cv::Mat* image, CvRect* boundingBox);
CVAPI(bool) cveTrackerUpdate(cv::Tracker* tracker, cv::Mat* image, CvRect* boundingBox);
//CVAPI(void) cveTrackerRelease(cv::Tracker** tracker);

CVAPI(cv::TrackerBoosting*) cveTrackerBoostingCreate(int numClassifiers, float samplerOverlap, float samplerSearchFactor, int iterationInit, int featureSetNumFeatures, cv::Tracker** tracker);
CVAPI(void) cveTrackerBoostingRelease(cv::TrackerBoosting** tracker);

CVAPI(cv::TrackerMedianFlow*) cveTrackerMedianFlowCreate(int pointsInGrid, CvSize* winSize, int maxLevel, CvTermCriteria* termCriteria, CvSize* winSizeNCC, double maxMedianLengthOfDisplacementDifference, cv::Tracker** tracker);
CVAPI(void) cveTrackerMedianFlowRelease(cv::TrackerMedianFlow** tracker);

CVAPI(cv::TrackerMIL*) cveTrackerMILCreate(
	float samplerInitInRadius,
	int samplerInitMaxNegNum,
	float samplerSearchWinSize,
	float samplerTrackInRadius,
	int samplerTrackMaxPosNum,
	int samplerTrackMaxNegNum,
	int featureSetNumFeatures,
	cv::Tracker** tracker);
CVAPI(void) cveTrackerMILRelease(cv::TrackerMIL** tracker);

CVAPI(cv::TrackerTLD*) cveTrackerTLDCreate(cv::Tracker** tracker);
CVAPI(void) cveTrackerTLDRelease(cv::TrackerTLD** tracker);

CVAPI(cv::TrackerKCF*) cveTrackerKCFCreate(cv::Tracker** tracker);
CVAPI(void) cveTrackerKCFRelease(cv::TrackerKCF** tracker);

CVAPI(cv::TrackerGOTURN*) cveTrackerGOTURNCreate(cv::Tracker** tracker);
CVAPI(void) cveTrackerGOTURNRelease(cv::TrackerGOTURN** tracker);

CVAPI(cv::MultiTracker*) cveMultiTrackerCreate();
CVAPI(bool) cveMultiTrackerAdd(cv::MultiTracker* multiTracker, cv::Tracker* tracker, cv::Mat* image, CvRect* boundingBox);
CVAPI(bool) cveMultiTrackerUpdate(cv::MultiTracker* tracker, cv::Mat* image, std::vector<CvRect>* boundingBox);
CVAPI(void) cveMultiTrackerRelease(cv::MultiTracker** tracker);



#endif