//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "tracking_c.h"

/*
cv::Tracker* cveTrackerCreate(cv::String* trackerType)
{
   cv::Ptr<cv::Tracker> tracker = cv::Tracker::create(*trackerType);
   tracker.addref();
   return tracker.get();
}*/
bool cveTrackerInit(cv::Tracker* tracker, cv::Mat* image, CvRect* boundingBox)
{
	return tracker->init(*image, *boundingBox);
}
bool cveTrackerUpdate(cv::Tracker* tracker, cv::Mat* image, CvRect* boundingBox)
{
	cv::Rect2d box;
	bool result = tracker->update(*image, box);
	*boundingBox = box;
	return result;
}
/*
void cveTrackerRelease(cv::Tracker** tracker)
{
   delete *tracker;
   *tracker = 0;
}
*/

cv::TrackerBoosting* cveTrackerBoostingCreate(
	int numClassifiers, 
	float samplerOverlap, 
	float samplerSearchFactor, 
	int iterationInit, 
	int featureSetNumFeatures, 
	cv::Tracker** tracker, 
	cv::Ptr<cv::TrackerBoosting>** sharedPtr)
{
	cv::TrackerBoosting::Params p;
	p.numClassifiers = numClassifiers;
	p.samplerOverlap = samplerOverlap;
	p.samplerSearchFactor = samplerSearchFactor;
	p.iterationInit = iterationInit;
	p.featureSetNumFeatures = featureSetNumFeatures;
	cv::Ptr<cv::TrackerBoosting> ptr = cv::TrackerBoosting::create(p);
	*sharedPtr = new cv::Ptr<cv::TrackerBoosting>(ptr);
	*tracker = dynamic_cast<cv::Tracker*>(ptr.get());
	return ptr.get();
}
void cveTrackerBoostingRelease(cv::TrackerBoosting** tracker, cv::Ptr<cv::TrackerBoosting>** sharedPtr)
{
	delete *sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
}

cv::TrackerMedianFlow* cveTrackerMedianFlowCreate(
	int pointsInGrid, 
	CvSize* winSize, 
	int maxLevel, 
	CvTermCriteria* termCriteria, 
	CvSize* winSizeNCC, 
	double maxMedianLengthOfDisplacementDifference, 
	cv::Tracker** tracker,
	cv::Ptr<cv::TrackerMedianFlow>** sharedPtr)
{
	cv::TrackerMedianFlow::Params p;
	p.pointsInGrid = pointsInGrid;
	p.winSize = *winSize;
	p.maxLevel = maxLevel;
	p.termCriteria = *termCriteria;
	p.winSizeNCC = *winSizeNCC;
	p.maxMedianLengthOfDisplacementDifference = maxMedianLengthOfDisplacementDifference;

	cv::Ptr<cv::TrackerMedianFlow> ptr = cv::TrackerMedianFlow::create(p);
	*sharedPtr = new cv::Ptr<cv::TrackerMedianFlow>(ptr);
	*tracker = dynamic_cast<cv::Tracker*>(ptr.get());
	return ptr.get();
}
void cveTrackerMedianFlowRelease(cv::TrackerMedianFlow** tracker, cv::Ptr<cv::TrackerMedianFlow>** sharedPtr)
{
	delete* sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
}

cv::TrackerMIL* cveTrackerMILCreate(
	float samplerInitInRadius,
	int samplerInitMaxNegNum,
	float samplerSearchWinSize,
	float samplerTrackInRadius,
	int samplerTrackMaxPosNum,
	int samplerTrackMaxNegNum,
	int featureSetNumFeatures,
	cv::Tracker** tracker,
	cv::Ptr<cv::TrackerMIL>** sharedPtr)
{
	cv::TrackerMIL::Params p;
	p.samplerInitInRadius = samplerInitInRadius;
	p.samplerInitMaxNegNum = samplerInitMaxNegNum;
	p.samplerSearchWinSize = samplerSearchWinSize;
	p.samplerTrackInRadius = samplerTrackInRadius;
	p.samplerTrackMaxPosNum = samplerTrackMaxPosNum;
	p.samplerTrackMaxNegNum = samplerTrackMaxNegNum;
	p.featureSetNumFeatures = featureSetNumFeatures;

	cv::Ptr<cv::TrackerMIL> ptr = cv::TrackerMIL::create(p);
	*sharedPtr = new cv::Ptr<cv::TrackerMIL>(ptr);
	*tracker = dynamic_cast<cv::Tracker*>(ptr.get());
	return ptr.get();
}
void cveTrackerMILRelease(cv::TrackerMIL** tracker, cv::Ptr<cv::TrackerMIL>** sharedPtr)
{
	delete *sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
}

cv::TrackerTLD* cveTrackerTLDCreate(cv::Tracker** tracker, cv::Ptr<cv::TrackerTLD>** sharedPtr)
{
	cv::Ptr<cv::TrackerTLD> ptr = cv::TrackerTLD::create();
	*sharedPtr = new cv::Ptr<cv::TrackerTLD>(ptr);
	*tracker = dynamic_cast<cv::Tracker*>(ptr.get());
	return ptr.get();
}
void cveTrackerTLDRelease(cv::TrackerTLD** tracker, cv::Ptr<cv::TrackerTLD>** sharedPtr)
{
	delete *sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
}

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
}
void cveTrackerKCFRelease(cv::TrackerKCF** tracker, cv::Ptr<cv::TrackerKCF>** sharedPtr)
{
	delete *sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
}

cv::TrackerGOTURN* cveTrackerGOTURNCreate(cv::Tracker** tracker, cv::Ptr<cv::TrackerGOTURN>** sharedPtr)
{
	cv::Ptr<cv::TrackerGOTURN> ptr = cv::TrackerGOTURN::create();
	*sharedPtr = new cv::Ptr<cv::TrackerGOTURN>(ptr);
	*tracker = dynamic_cast<cv::Tracker*>(ptr.get());
	return ptr.get();
}
void cveTrackerGOTURNRelease(cv::TrackerGOTURN** tracker, cv::Ptr<cv::TrackerGOTURN>** sharedPtr)
{
	delete *sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
}

cv::TrackerMOSSE* cveTrackerMOSSECreate(cv::Tracker** tracker, cv::Ptr<cv::TrackerMOSSE>** sharedPtr)
{
	cv::Ptr<cv::TrackerMOSSE> ptr = cv::TrackerMOSSE::create();
	*sharedPtr = new cv::Ptr<cv::TrackerMOSSE>(ptr);
	*tracker = dynamic_cast<cv::Tracker*>(ptr.get());
	return ptr.get();
}
void cveTrackerMOSSERelease(cv::TrackerMOSSE** tracker, cv::Ptr<cv::TrackerMOSSE>** sharedPtr)
{
	delete *sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
}

cv::MultiTracker* cveMultiTrackerCreate()
{
	return new cv::MultiTracker();
}
bool cveMultiTrackerAdd(cv::MultiTracker* multiTracker, cv::Tracker* tracker, cv::_InputArray* image, CvRect* boundingBox)
{
	cv::Ptr<cv::Tracker> trackerPtr(tracker, [](cv::Tracker*) {});	
	return multiTracker->add(trackerPtr, *image, *boundingBox);
}

bool cveMultiTrackerUpdate(cv::MultiTracker* tracker, cv::Mat* image, std::vector<CvRect>* boundingBox)
{
	std::vector<cv::Rect2d> bb;
	bool result = tracker->update(*image, bb);
	boundingBox->clear();
	for (std::vector<cv::Rect2d>::iterator it = bb.begin(); it != bb.end(); ++it)
	{
		boundingBox->push_back(*it);
	}
	return result;
}
void cveMultiTrackerRelease(cv::MultiTracker** tracker)
{
	delete* tracker;
	*tracker = 0;
}

void cveMultiTrackerGetObjects(cv::MultiTracker* tracker, std::vector<CvRect>* boundingBox)
{
	std::vector<cv::Rect2d> bb = tracker->getObjects();
	boundingBox->clear();
	for (std::vector<cv::Rect2d>::iterator it = bb.begin(); it != bb.end(); ++it)
	{
		boundingBox->push_back(*it);
	}
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
}
void cveTrackerCSRTRelease(cv::TrackerCSRT** tracker, cv::Ptr<cv::TrackerCSRT>** sharedPtr)
{
	delete* sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
}
