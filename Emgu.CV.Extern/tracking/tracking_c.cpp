//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "tracking_c.h"

cv::TrackerKCF* cveTrackerKCFCreate(
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
	cv::Ptr<cv::TrackerKCF>** sharedPtr)
{
#ifdef HAVE_OPENCV_TRACKING
	cv::TrackerKCF::Params p;
	p.detect_thresh = detect_thresh;
	p.sigma = sigma;
	p.lambda = lambda;
	p.interp_factor = interp_factor;
	p.output_sigma_factor = output_sigma_factor;
	p.pca_learning_rate = pca_learning_rate;
	p.resize = resize;
	p.split_coeff = split_coeff;
	p.wrap_kernel = wrap_kernel;
	p.compress_feature = compress_feature;
	p.max_patch_size = max_patch_size;
	p.compressed_size = compressed_size;
	p.desc_pca = desc_pca;
	p.desc_npca = desc_npca;

	cv::Ptr<cv::TrackerKCF> ptr = cv::TrackerKCF::create(p);
	*sharedPtr = new cv::Ptr<cv::TrackerKCF>(ptr);
	*tracker = dynamic_cast<cv::Tracker*>(ptr.get());
	return ptr.get();
#else
	throw_no_tracking();
#endif
}
void cveTrackerKCFRelease(cv::TrackerKCF** tracker, cv::Ptr<cv::TrackerKCF>** sharedPtr)
{
#ifdef HAVE_OPENCV_TRACKING
	delete* sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
#else
	throw_no_tracking();
#endif
}


cv::TrackerCSRT* cveTrackerCSRTCreate(
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
	cv::Ptr<cv::TrackerCSRT>** sharedPtr)
{
#ifdef HAVE_OPENCV_TRACKING
	cv::TrackerCSRT::Params p;
	p.use_hog = use_hog;
	p.use_color_names = use_color_names;
	p.use_gray = use_gray;
	p.use_rgb = use_rgb;
	p.use_channel_weights = use_channel_weights;
	p.use_segmentation = use_segmentation;
	if (window_function && !window_function->empty())
		p.window_function = *window_function;
	p.kaiser_alpha = kaiser_alpha;
	p.cheb_attenuation = cheb_attenuation;
	p.template_size = template_size;
	p.gsl_sigma = gsl_sigma;
	p.hog_orientations = hog_orientations;
	p.hog_clip = hog_clip;
	p.padding = padding;
	p.filter_lr = filter_lr;
	p.weights_lr = weights_lr;
	p.num_hog_channels_used = num_hog_channels_used;
	p.admm_iterations = admm_iterations;
	p.histogram_bins = histogram_bins;
	p.histogram_lr = histogram_lr;
	p.background_ratio = background_ratio;
	p.number_of_scales = number_of_scales;
	p.scale_sigma_factor = scale_sigma_factor;
	p.scale_model_max_area = scale_model_max_area;
	p.scale_lr = scale_lr;
	p.scale_step = scale_step;

	cv::Ptr<cv::TrackerCSRT> ptr = cv::TrackerCSRT::create(p);
	*sharedPtr = new cv::Ptr<cv::TrackerCSRT>(ptr);
	*tracker = dynamic_cast<cv::Tracker*>(ptr.get());
	return ptr.get();
#else
	throw_no_tracking();
#endif
}
void cveTrackerCSRTRelease(cv::TrackerCSRT** tracker, cv::Ptr<cv::TrackerCSRT>** sharedPtr)
{
#ifdef HAVE_OPENCV_TRACKING
	delete* sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
#else
	throw_no_tracking();
#endif
}

bool cveLegacyTrackerInit(cv::legacy::Tracker* tracker, cv::Mat* image, CvRect* boundingBox)
{
#ifdef HAVE_OPENCV_TRACKING
	return tracker->init(*image, *boundingBox);
#else
	throw_no_tracking();
#endif
}
bool cveLegacyTrackerUpdate(cv::legacy::Tracker* tracker, cv::Mat* image, CvRect* boundingBox)
{
#ifdef HAVE_OPENCV_TRACKING
	cv::Rect2d box;
	bool result = tracker->update(*image, box);
	*boundingBox = cvRect(box);
	return result;
#else
	throw_no_tracking();
#endif
}

cv::legacy::TrackerBoosting* cveTrackerBoostingCreate(
	int numClassifiers, 
	float samplerOverlap, 
	float samplerSearchFactor, 
	int iterationInit, 
	int featureSetNumFeatures, 
	cv::legacy::Tracker** tracker,
	cv::Ptr<cv::legacy::TrackerBoosting>** sharedPtr)
{
#ifdef HAVE_OPENCV_TRACKING
	cv::legacy::TrackerBoosting::Params p;
	p.numClassifiers = numClassifiers;
	p.samplerOverlap = samplerOverlap;
	p.samplerSearchFactor = samplerSearchFactor;
	p.iterationInit = iterationInit;
	p.featureSetNumFeatures = featureSetNumFeatures;
	cv::Ptr<cv::legacy::TrackerBoosting> ptr = cv::legacy::TrackerBoosting::create(p);
	*sharedPtr = new cv::Ptr<cv::legacy::TrackerBoosting>(ptr);
	*tracker = dynamic_cast<cv::legacy::Tracker*>(ptr.get());
	return ptr.get();
#else
	throw_no_tracking();
#endif
}
void cveTrackerBoostingRelease(cv::legacy::TrackerBoosting** tracker, cv::Ptr<cv::legacy::TrackerBoosting>** sharedPtr)
{
#ifdef HAVE_OPENCV_TRACKING
	delete *sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
#else
	throw_no_tracking();
#endif
}

cv::legacy::TrackerMedianFlow* cveTrackerMedianFlowCreate(
	int pointsInGrid, 
	CvSize* winSize, 
	int maxLevel, 
	CvTermCriteria* termCriteria, 
	CvSize* winSizeNCC, 
	double maxMedianLengthOfDisplacementDifference, 
	cv::legacy::Tracker** tracker,
	cv::Ptr<cv::legacy::TrackerMedianFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_TRACKING
	cv::legacy::TrackerMedianFlow::Params p;
	p.pointsInGrid = pointsInGrid;
	p.winSize = *winSize;
	p.maxLevel = maxLevel;
	p.termCriteria = *termCriteria;
	p.winSizeNCC = *winSizeNCC;
	p.maxMedianLengthOfDisplacementDifference = maxMedianLengthOfDisplacementDifference;

	cv::Ptr<cv::legacy::TrackerMedianFlow> ptr = cv::legacy::TrackerMedianFlow::create(p);
	*sharedPtr = new cv::Ptr<cv::legacy::TrackerMedianFlow>(ptr);
	*tracker = dynamic_cast<cv::legacy::Tracker*>(ptr.get());
	return ptr.get();
#else
	throw_no_tracking();
#endif
}
void cveTrackerMedianFlowRelease(cv::legacy::TrackerMedianFlow** tracker, cv::Ptr<cv::legacy::TrackerMedianFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_TRACKING
	delete* sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
#else
	throw_no_tracking();
#endif
}


cv::legacy::TrackerTLD* cveTrackerTLDCreate(cv::legacy::Tracker** tracker, cv::Ptr<cv::legacy::TrackerTLD>** sharedPtr)
{
#ifdef HAVE_OPENCV_TRACKING
	cv::Ptr<cv::legacy::TrackerTLD> ptr = cv::legacy::TrackerTLD::create();
	*sharedPtr = new cv::Ptr<cv::legacy::TrackerTLD>(ptr);
	*tracker = dynamic_cast<cv::legacy::Tracker*>(ptr.get());
	return ptr.get();
#else
	throw_no_tracking();
#endif
}
void cveTrackerTLDRelease(cv::legacy::TrackerTLD** tracker, cv::Ptr<cv::legacy::TrackerTLD>** sharedPtr)
{
#ifdef HAVE_OPENCV_TRACKING
	delete *sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
#else
	throw_no_tracking();
#endif
}

cv::legacy::TrackerMOSSE* cveTrackerMOSSECreate(cv::legacy::Tracker** tracker, cv::Ptr<cv::legacy::TrackerMOSSE>** sharedPtr)
{
#ifdef HAVE_OPENCV_TRACKING
	cv::Ptr<cv::legacy::TrackerMOSSE> ptr = cv::legacy::TrackerMOSSE::create();
	*sharedPtr = new cv::Ptr<cv::legacy::TrackerMOSSE>(ptr);
	*tracker = dynamic_cast<cv::legacy::Tracker*>(ptr.get());
	return ptr.get();
#else
	throw_no_tracking();
#endif
}
void cveTrackerMOSSERelease(cv::legacy::TrackerMOSSE** tracker, cv::Ptr<cv::legacy::TrackerMOSSE>** sharedPtr)
{
#ifdef HAVE_OPENCV_TRACKING
	delete *sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
#else
	throw_no_tracking();
#endif
}

cv::legacy::MultiTracker* cveMultiTrackerCreate()
{
#ifdef HAVE_OPENCV_TRACKING
	return new cv::legacy::MultiTracker();
#else
	throw_no_tracking();
#endif
}
bool cveMultiTrackerAdd(cv::legacy::MultiTracker* multiTracker, cv::legacy::Tracker* tracker, cv::_InputArray* image, CvRect* boundingBox)
{
#ifdef HAVE_OPENCV_TRACKING
	cv::Ptr<cv::legacy::Tracker> trackerPtr(tracker, [](cv::legacy::Tracker*) {});
	return multiTracker->add(trackerPtr, *image, *boundingBox);
#else
	throw_no_tracking();
#endif
}

bool cveMultiTrackerUpdate(cv::legacy::MultiTracker* tracker, cv::Mat* image, std::vector<CvRect>* boundingBox)
{
#ifdef HAVE_OPENCV_TRACKING
	std::vector<cv::Rect2d> bb;
	bool result = tracker->update(*image, bb);
	boundingBox->clear();
	for (std::vector<cv::Rect2d>::iterator it = bb.begin(); it != bb.end(); ++it)
	{
		boundingBox->push_back(cvRect(*it));
	}
	return result;
#else
	throw_no_tracking();
#endif
}
void cveMultiTrackerRelease(cv::legacy::MultiTracker** tracker)
{
#ifdef HAVE_OPENCV_TRACKING
	delete* tracker;
	*tracker = 0;
#else
	throw_no_tracking();
#endif
}

void cveMultiTrackerGetObjects(cv::legacy::MultiTracker* tracker, std::vector<CvRect>* boundingBox)
{
#ifdef HAVE_OPENCV_TRACKING
	std::vector<cv::Rect2d> bb = tracker->getObjects();
	boundingBox->clear();
	for (std::vector<cv::Rect2d>::iterator it = bb.begin(); it != bb.end(); ++it)
	{
		boundingBox->push_back(cvRect(*it));
	}
#else
	throw_no_tracking();
#endif
}
