//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_TRACKING_C_H
#define EMGU_TRACKING_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_TRACKING
#include "opencv2/tracking/tracking.hpp"
#else
static inline CV_NORETURN void throw_no_tracking() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without tracking support"); }
namespace cv {
class Tracker {};
class TrackerBoosting {};
class TrackerMedianFlow {};
class TrackerMIL {};
class TrackerTLD {};
class TrackerKCF {};
class TrackerGOTURN {};
class TrackerMOSSE {};
class MultiTracker {};
class TrackerCSRT {};
}
#endif
//CVAPI(cv::Tracker*) cveTrackerCreate(cv::String* trackerType);
CVAPI(bool) cveTrackerInit(cv::Tracker* tracker, cv::Mat* image, CvRect* boundingBox);
CVAPI(bool) cveTrackerUpdate(cv::Tracker* tracker, cv::Mat* image, CvRect* boundingBox);
//CVAPI(void) cveTrackerRelease(cv::Tracker** tracker);

CVAPI(cv::TrackerBoosting*) cveTrackerBoostingCreate(int numClassifiers, float samplerOverlap, float samplerSearchFactor, int iterationInit, int featureSetNumFeatures, cv::Tracker** tracker, cv::Ptr<cv::TrackerBoosting>** sharedPtr);
CVAPI(void) cveTrackerBoostingRelease(cv::TrackerBoosting** tracker, cv::Ptr<cv::TrackerBoosting>** sharedPtr);

CVAPI(cv::TrackerMedianFlow*) cveTrackerMedianFlowCreate(int pointsInGrid, CvSize* winSize, int maxLevel, CvTermCriteria* termCriteria, CvSize* winSizeNCC, double maxMedianLengthOfDisplacementDifference, cv::Tracker** tracker, cv::Ptr<cv::TrackerMedianFlow>** sharedPtr);
CVAPI(void) cveTrackerMedianFlowRelease(cv::TrackerMedianFlow** tracker, cv::Ptr<cv::TrackerMedianFlow>** sharedPtr);

CVAPI(cv::TrackerMIL*) cveTrackerMILCreate(
	float samplerInitInRadius,
	int samplerInitMaxNegNum,
	float samplerSearchWinSize,
	float samplerTrackInRadius,
	int samplerTrackMaxPosNum,
	int samplerTrackMaxNegNum,
	int featureSetNumFeatures,
	cv::Tracker** tracker, 
	cv::Ptr<cv::TrackerMIL>** sharedPtr);
CVAPI(void) cveTrackerMILRelease(cv::TrackerMIL** tracker, cv::Ptr<cv::TrackerMIL>** sharedPtr);

CVAPI(cv::TrackerTLD*) cveTrackerTLDCreate(cv::Tracker** tracker, cv::Ptr<cv::TrackerTLD>** sharedPtr);
CVAPI(void) cveTrackerTLDRelease(cv::TrackerTLD** tracker, cv::Ptr<cv::TrackerTLD>** sharedPtr);

CVAPI(cv::TrackerKCF*) cveTrackerKCFCreate(
	float detect_thresh,
	float sigma,
	float lambda,
	float interp_factor,
	float output_sigma_factor,
	float pca_learning_rate,
	bool resize,
	bool split_coeff,
	bool wrap_kernel,
	bool compress_feature,
	int max_patch_size,
	int compressed_size,
	int desc_pca,
	int desc_npca,
	cv::Tracker** tracker,
	cv::Ptr<cv::TrackerKCF>** sharedPtr);
CVAPI(void) cveTrackerKCFRelease(cv::TrackerKCF** tracker, cv::Ptr<cv::TrackerKCF>** sharedPtr);

CVAPI(cv::TrackerGOTURN*) cveTrackerGOTURNCreate(cv::Tracker** tracker, cv::Ptr<cv::TrackerGOTURN>** sharedPtr);
CVAPI(void) cveTrackerGOTURNRelease(cv::TrackerGOTURN** tracker, cv::Ptr<cv::TrackerGOTURN>** sharedPtr);

CVAPI(cv::TrackerMOSSE*) cveTrackerMOSSECreate(cv::Tracker** tracker, cv::Ptr<cv::TrackerMOSSE>** sharedPtr);
CVAPI(void) cveTrackerMOSSERelease(cv::TrackerMOSSE** tracker, cv::Ptr<cv::TrackerMOSSE>** sharedPtr);

CVAPI(cv::MultiTracker*) cveMultiTrackerCreate();
CVAPI(bool) cveMultiTrackerAdd(cv::MultiTracker* multiTracker, cv::Tracker* tracker, cv::_InputArray* image, CvRect* boundingBox);
CVAPI(bool) cveMultiTrackerUpdate(cv::MultiTracker* tracker, cv::Mat* image, std::vector<CvRect>* boundingBox);
CVAPI(void) cveMultiTrackerRelease(cv::MultiTracker** tracker);
CVAPI(void) cveMultiTrackerGetObjects(cv::MultiTracker* tracker, std::vector<CvRect>* boundingBox);

CVAPI(cv::TrackerCSRT*) cveTrackerCSRTCreate(
	bool use_hog,
	bool use_color_names,
	bool use_gray,
	bool use_rgb,
	bool use_channel_weights,
	bool use_segmentation,
	cv::String* window_function, 
	float kaiser_alpha,
	float cheb_attenuation,
	float template_size,
	float gsl_sigma,
	float hog_orientations,
	float hog_clip,
	float padding,
	float filter_lr,
	float weights_lr,
	int num_hog_channels_used,
	int admm_iterations,
	int histogram_bins,
	float histogram_lr,
	int background_ratio,
	int number_of_scales,
	float scale_sigma_factor,
	float scale_model_max_area,
	float scale_lr,
	float scale_step,
	cv::Tracker** tracker,
	cv::Ptr<cv::TrackerCSRT>** sharedPtr);
CVAPI(void) cveTrackerCSRTRelease(cv::TrackerCSRT** tracker, cv::Ptr<cv::TrackerCSRT>** sharedPtr);

#endif