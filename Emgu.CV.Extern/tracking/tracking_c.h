//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_TRACKING_C_H
#define EMGU_TRACKING_C_H

#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_VIDEO
#include "opencv2/video/video.hpp"
#else
namespace cv {
	class Tracker {};
	}
#endif

#ifdef HAVE_OPENCV_TRACKING
#include "opencv2/tracking/tracking.hpp"
#include "opencv2/tracking/tracking_legacy.hpp"
#else
static inline CV_NORETURN void throw_no_tracking() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without tracking support"); }
namespace cv {
	class TrackerCSRT {};
	class TrackerKCF {};

  namespace legacy {
	  class Tracker {};
	  class TrackerBoosting {};
	  class TrackerMedianFlow {};
	  class TrackerTLD {};
	  class TrackerMOSSE {};
	  class MultiTracker {};
  }
}
#endif

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

CVAPI(bool) cveLegacyTrackerInit(cv::legacy::Tracker* tracker, cv::Mat* image, CvRect* boundingBox);
CVAPI(bool) cveLegacyTrackerUpdate(cv::legacy::Tracker* tracker, cv::Mat* image, CvRect* boundingBox);

CVAPI(cv::legacy::TrackerBoosting*) cveTrackerBoostingCreate(int numClassifiers, float samplerOverlap, float samplerSearchFactor, int iterationInit, int featureSetNumFeatures, cv::legacy::Tracker** tracker, cv::Ptr<cv::legacy::TrackerBoosting>** sharedPtr);
CVAPI(void) cveTrackerBoostingRelease(cv::legacy::TrackerBoosting** tracker, cv::Ptr<cv::legacy::TrackerBoosting>** sharedPtr);

CVAPI(cv::legacy::TrackerMedianFlow*) cveTrackerMedianFlowCreate(int pointsInGrid, CvSize* winSize, int maxLevel, CvTermCriteria* termCriteria, CvSize* winSizeNCC, double maxMedianLengthOfDisplacementDifference, cv::legacy::Tracker** tracker, cv::Ptr<cv::legacy::TrackerMedianFlow>** sharedPtr);
CVAPI(void) cveTrackerMedianFlowRelease(cv::legacy::TrackerMedianFlow** tracker, cv::Ptr<cv::legacy::TrackerMedianFlow>** sharedPtr);

CVAPI(cv::legacy::TrackerTLD*) cveTrackerTLDCreate(cv::legacy::Tracker** tracker, cv::Ptr<cv::legacy::TrackerTLD>** sharedPtr);
CVAPI(void) cveTrackerTLDRelease(cv::legacy::TrackerTLD** tracker, cv::Ptr<cv::legacy::TrackerTLD>** sharedPtr);

CVAPI(cv::legacy::TrackerMOSSE*) cveTrackerMOSSECreate(cv::legacy::Tracker** tracker, cv::Ptr<cv::legacy::TrackerMOSSE>** sharedPtr);
CVAPI(void) cveTrackerMOSSERelease(cv::legacy::TrackerMOSSE** tracker, cv::Ptr<cv::legacy::TrackerMOSSE>** sharedPtr);

CVAPI(cv::legacy::MultiTracker*) cveMultiTrackerCreate();
CVAPI(bool) cveMultiTrackerAdd(cv::legacy::MultiTracker* multiTracker, cv::legacy::Tracker* tracker, cv::_InputArray* image, CvRect* boundingBox);
CVAPI(bool) cveMultiTrackerUpdate(cv::legacy::MultiTracker* tracker, cv::Mat* image, std::vector<CvRect>* boundingBox);
CVAPI(void) cveMultiTrackerRelease(cv::legacy::MultiTracker** tracker);
CVAPI(void) cveMultiTrackerGetObjects(cv::legacy::MultiTracker* tracker, std::vector<CvRect>* boundingBox);

#endif