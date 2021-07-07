//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "video_c.h"

//BackgroundSubtractorMOG2
cv::BackgroundSubtractorMOG2* cveBackgroundSubtractorMOG2Create(int history, float varThreshold, bool bShadowDetection, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm, cv::Ptr<cv::BackgroundSubtractorMOG2>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	cv::Ptr<cv::BackgroundSubtractorMOG2> ptr = cv::createBackgroundSubtractorMOG2(history, varThreshold, bShadowDetection);
	*sharedPtr = new cv::Ptr<cv::BackgroundSubtractorMOG2>(ptr);
	cv::BackgroundSubtractorMOG2* bs = ptr.get();
	*bgSubtractor = dynamic_cast<cv::BackgroundSubtractor*>(bs);
	*algorithm = dynamic_cast<cv::Algorithm*>(bs);
	return bs;
#else
	throw_no_video();
#endif
}

void cveBackgroundSubtractorMOG2Release(cv::BackgroundSubtractorMOG2** bgSubtractor, cv::Ptr<cv::BackgroundSubtractorMOG2>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	delete *sharedPtr;
	*bgSubtractor = 0;
	*sharedPtr = 0;
#else
	throw_no_video();
#endif
}

//BackgroundSubtractor
void cveBackgroundSubtractorUpdate(cv::BackgroundSubtractor* bgSubtractor, cv::_InputArray* image, cv::_OutputArray* fgmask, double learningRate)
{
#ifdef HAVE_OPENCV_VIDEO
	//cv::Mat imgMat = cv::cvarrToMat(image);
	//cv::Mat fgMat = cv::cvarrToMat(fgmask);
	bgSubtractor->apply(*image, *fgmask, learningRate);
#else
	throw_no_video();
#endif
}

void cveBackgroundSubtractorGetBackgroundImage(cv::BackgroundSubtractor* bgSubtractor, cv::_OutputArray* backgroundImage)
{
#ifdef HAVE_OPENCV_VIDEO
	bgSubtractor->getBackgroundImage(*backgroundImage);
#else
	throw_no_video();
#endif
}

//BackgroundSubtractorKNN
cv::BackgroundSubtractorKNN* cveBackgroundSubtractorKNNCreate(int history, double dist2Threshold, bool detectShadows, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm, cv::Ptr<cv::BackgroundSubtractorKNN>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	cv::Ptr<cv::BackgroundSubtractorKNN> ptr = cv::createBackgroundSubtractorKNN(history, dist2Threshold, detectShadows);

	*sharedPtr = new cv::Ptr<cv::BackgroundSubtractorKNN>(ptr);

	cv::BackgroundSubtractorKNN* bs = ptr.get();
	*bgSubtractor = dynamic_cast<cv::BackgroundSubtractor*>(bs);
	*algorithm = dynamic_cast<cv::Algorithm*>(bs);
	return bs;
#else
	throw_no_video();
#endif
}
void cveBackgroundSubtractorKNNRelease(cv::BackgroundSubtractorKNN** bgSubtractor, cv::Ptr<cv::BackgroundSubtractorKNN>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	delete *sharedPtr;
	*bgSubtractor = 0;
	*sharedPtr = 0;
#else
	throw_no_video();
#endif
}


cv::FarnebackOpticalFlow* cveFarnebackOpticalFlowCreate(
	int numLevels,
	double pyrScale,
	bool fastPyramids,
	int winSize,
	int numIters,
	int polyN,
	double polySigma,
	int flags,
	cv::DenseOpticalFlow** denseOpticalFlow,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::FarnebackOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	cv::Ptr<cv::FarnebackOpticalFlow> dof = cv::FarnebackOpticalFlow::create(
		numLevels, pyrScale, fastPyramids, winSize, numIters, polyN, polySigma, flags
	);
	*sharedPtr = new cv::Ptr<cv::FarnebackOpticalFlow>(dof);
	cv::FarnebackOpticalFlow* ptr = dof.get();
	*denseOpticalFlow = dynamic_cast<cv::DenseOpticalFlow*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
#else
	throw_no_video();
#endif
}
void cveFarnebackOpticalFlowRelease(cv::FarnebackOpticalFlow** flow, cv::Ptr<cv::FarnebackOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	delete *sharedPtr;
	*flow = 0;
	*sharedPtr = 0;
#else
	throw_no_video();
#endif
}


void cveDenseOpticalFlowCalc(cv::DenseOpticalFlow* dof, cv::_InputArray* i0, cv::_InputArray* i1, cv::_InputOutputArray* flow)
{
#ifdef HAVE_OPENCV_VIDEO
	dof->calc(*i0, *i1, *flow);
#else
	throw_no_video();
#endif
}

void cveSparseOpticalFlowCalc(
	cv::SparseOpticalFlow* sof,
	cv::_InputArray* prevImg, cv::_InputArray* nextImg,
	cv::_InputArray* prevPts, cv::_InputOutputArray* nextPts,
	cv::_OutputArray* status,
	cv::_OutputArray* err)
{
#ifdef HAVE_OPENCV_VIDEO
	sof->calc(*prevImg, *nextImg, *prevPts, *nextPts, *status, err ? *err : dynamic_cast<cv::OutputArray>(cv::noArray()));
#else
	throw_no_video();
#endif
}

cv::SparsePyrLKOpticalFlow* cveSparsePyrLKOpticalFlowCreate(
	CvSize* winSize,
	int maxLevel,
	CvTermCriteria* crit,
	int flags,
	double minEigThreshold,
	cv::SparseOpticalFlow** sparseOpticalFlow,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::SparsePyrLKOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	cv::Ptr<cv::SparsePyrLKOpticalFlow> sof = cv::SparsePyrLKOpticalFlow::create(
		*winSize, maxLevel, *crit, flags, minEigThreshold
	);
	*sharedPtr = new cv::Ptr<cv::SparsePyrLKOpticalFlow>(sof);
	cv::SparsePyrLKOpticalFlow* ptr = sof.get();
	*sparseOpticalFlow = dynamic_cast<cv::SparseOpticalFlow*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
#else
	throw_no_video();
#endif
}
void cveSparsePyrLKOpticalFlowRelease(cv::SparsePyrLKOpticalFlow** flow, cv::Ptr<cv::SparsePyrLKOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	delete *sharedPtr;
	*flow = 0;
	*sharedPtr = 0;
#else
	throw_no_video();
#endif
}

void cveDenseOpticalFlowRelease(cv::Ptr<cv::DenseOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_video();
#endif
}

void cveCalcOpticalFlowFarneback(cv::_InputArray* prev, cv::_InputArray* next, cv::_InputOutputArray* flow, double pyrScale, int levels, int winSize, int iterations, int polyN, double polySigma, int flags)
{
#ifdef HAVE_OPENCV_VIDEO
	cv::calcOpticalFlowFarneback(*prev, *next, *flow, pyrScale, levels, winSize, iterations, polyN, polySigma, flags);
#else
	throw_no_video();
#endif
}

void cveCalcOpticalFlowPyrLK(cv::_InputArray* prevImg, cv::_InputArray* nextImg, cv::_InputArray* prevPts, cv::_InputOutputArray* nextPts, cv::_OutputArray* status, cv::_OutputArray* err, CvSize* winSize, int maxLevel, CvTermCriteria* criteria, int flags, double minEigenThreshold)
{
#ifdef HAVE_OPENCV_VIDEO
	cv::calcOpticalFlowPyrLK(*prevImg, *nextImg, *prevPts, *nextPts, *status, *err, *winSize, maxLevel, *criteria, flags, minEigenThreshold);
#else
	throw_no_video();
#endif
}

void cveCamShift(cv::_InputArray* probImage, CvRect* window, CvTermCriteria* criteria, CvBox2D* result)
{
#ifdef HAVE_OPENCV_VIDEO
	cv::Rect rect = *window;
	cv::RotatedRect rr = cv::CamShift(*probImage, rect, *criteria);
	*window = cvRect(rect);
	result->center = cvPoint2D32f(rr.center.x, rr.center.y);
	result->size = cvSize2D32f(rr.size.width, rr.size.height);
	result->angle = rr.angle;
#else
	throw_no_video();
#endif
}

int cveMeanShift(cv::_InputArray* probImage, CvRect* window, CvTermCriteria* criteria)
{
#ifdef HAVE_OPENCV_VIDEO
	cv::Rect rect = *window;
	int result = cv::meanShift(*probImage, rect, *criteria);
	*window = cvRect(rect);
	return result;
#else
	throw_no_video();
#endif
}

int cveBuildOpticalFlowPyramid(
	cv::_InputArray* img,
	cv::_OutputArray* pyramid,
	CvSize* winSize,
	int maxLevel,
	bool withDerivatives,
	int pyrBorder,
	int derivBorder,
	bool tryReuseInputImage)
{
#ifdef HAVE_OPENCV_VIDEO
	return cv::buildOpticalFlowPyramid(*img, *pyramid, *winSize, maxLevel, withDerivatives, pyrBorder, derivBorder, tryReuseInputImage);
#else
	throw_no_video();
#endif
}

/*
void cveEstimateRigidTransform(cv::_InputArray* src, cv::_InputArray* dst, bool fullAffine, cv::Mat* result)
{
	cv::Mat r = cv::estimateRigidTransform(*src, *dst, fullAffine);
	cv::swap(r, *result);
}*/

double cveFindTransformECC(
	cv::_InputArray* templateImage, cv::_InputArray* inputImage,
	cv::_InputOutputArray* warpMatrix, int motionType,
	CvTermCriteria* criteria,
	cv::_InputArray* inputMask)
{
#ifdef HAVE_OPENCV_VIDEO
	return cv::findTransformECC(
		*templateImage, *inputImage,
		*warpMatrix, motionType,
		*criteria,
		inputMask ? *inputMask : (cv::InputArray) cv::noArray());
#else
	throw_no_video();
#endif
}


cv::KalmanFilter* cveKalmanFilterCreate(int dynamParams, int measureParams, int controlParams, int type)
{
#ifdef HAVE_OPENCV_VIDEO
	return new cv::KalmanFilter(dynamParams, measureParams, controlParams, type);
#else
	throw_no_video();
#endif
}

void cveKalmanFilterRelease(cv::KalmanFilter** filter)
{
#ifdef HAVE_OPENCV_VIDEO
	delete *filter;
	*filter = 0;
#else
	throw_no_video();
#endif
}

const cv::Mat* cveKalmanFilterPredict(cv::KalmanFilter* kalman, cv::Mat* control)
{
#ifdef HAVE_OPENCV_VIDEO
	return &(kalman->predict(control ? *control : cv::Mat()));
#else
	throw_no_video();
#endif
}

const cv::Mat* cveKalmanFilterCorrect(cv::KalmanFilter* kalman, cv::Mat* measurement)
{
#ifdef HAVE_OPENCV_VIDEO
	return &(kalman->correct(*measurement));
#else
	throw_no_video();
#endif
}

cv::DISOpticalFlow* cveDISOpticalFlowCreate(int preset, cv::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm, cv::Ptr<cv::DISOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	cv::Ptr<cv::DISOpticalFlow> ptr = cv::DISOpticalFlow::create(preset);
	*sharedPtr = new cv::Ptr<cv::DISOpticalFlow>(ptr);
	*denseFlow = dynamic_cast<cv::DenseOpticalFlow*>(ptr.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
#else
	throw_no_video();
#endif
}

void cveDISOpticalFlowRelease(cv::DISOpticalFlow** flow, cv::Ptr<cv::DISOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	delete *sharedPtr;
	*flow = 0;
	*sharedPtr = 0;
#else
	throw_no_video();
#endif
}

cv::VariationalRefinement* cveVariationalRefinementCreate(cv::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm, cv::Ptr<cv::VariationalRefinement>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	cv::Ptr<cv::VariationalRefinement> ptr = cv::VariationalRefinement::create();
	*sharedPtr = new cv::Ptr<cv::VariationalRefinement>(ptr);
	*denseFlow = dynamic_cast<cv::DenseOpticalFlow*>(ptr.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
#else
	throw_no_video();
#endif
}

void cveVariationalRefinementRelease(cv::VariationalRefinement** flow, cv::Ptr<cv::VariationalRefinement>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	delete *sharedPtr;
	*flow = 0;
	*sharedPtr = 0;
#else
	throw_no_video();
#endif
}

/*
cv::Tracker* cveTrackerCreate(cv::String* trackerType)
{
   cv::Ptr<cv::Tracker> tracker = cv::Tracker::create(*trackerType);
   tracker.addref();
   return tracker.get();
}*/
void cveTrackerInit(cv::Tracker* tracker, cv::Mat* image, CvRect* boundingBox)
{
#ifdef HAVE_OPENCV_VIDEO
	return tracker->init(*image, *boundingBox);
#else
	throw_no_video();
#endif
}
bool cveTrackerUpdate(cv::Tracker* tracker, cv::Mat* image, CvRect* boundingBox)
{
#ifdef HAVE_OPENCV_VIDEO
	cv::Rect box;
	bool result = tracker->update(*image, box);
	*boundingBox = cvRect(box);
	return result;
#else
	throw_no_video();
#endif
}
/*
void cveTrackerRelease(cv::Tracker** tracker)
{
   delete *tracker;
   *tracker = 0;
}
*/

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
#ifdef HAVE_OPENCV_VIDEO
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
#else
	throw_no_video();
#endif
}
void cveTrackerMILRelease(cv::TrackerMIL** tracker, cv::Ptr<cv::TrackerMIL>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	delete* sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
#else
	throw_no_video();
#endif
}

cv::TrackerGOTURN* cveTrackerGOTURNCreate(cv::Tracker** tracker, cv::Ptr<cv::TrackerGOTURN>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	cv::Ptr<cv::TrackerGOTURN> ptr = cv::TrackerGOTURN::create();
	*sharedPtr = new cv::Ptr<cv::TrackerGOTURN>(ptr);
	*tracker = dynamic_cast<cv::Tracker*>(ptr.get());
	return ptr.get();
#else
	throw_no_video();
#endif
}
void cveTrackerGOTURNRelease(cv::TrackerGOTURN** tracker, cv::Ptr<cv::TrackerGOTURN>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	delete* sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
#else
	throw_no_video();
#endif
}

cv::TrackerDaSiamRPN* cveTrackerDaSiamRPNCreate(
	cv::String* model,
	cv::String* kernel_cls1,
	cv::String* kernel_r1,
	int backend,
	int target,
	cv::Tracker** tracker,
	cv::Ptr<cv::TrackerDaSiamRPN>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	cv::TrackerDaSiamRPN::Params p;
	p.model = *model;
	p.kernel_cls1 = *kernel_cls1;
	p.kernel_r1 = *kernel_r1;
	p.backend = backend;
	p.target = target;

	cv::Ptr<cv::TrackerDaSiamRPN> ptr = cv::TrackerDaSiamRPN::create(p);
	*sharedPtr = new cv::Ptr<cv::TrackerDaSiamRPN>(ptr);
	*tracker = dynamic_cast<cv::Tracker*>(ptr.get());
	return ptr.get();
#else
	throw_no_video();
#endif
}


void cveTrackerDaSiamRPNRelease(cv::TrackerDaSiamRPN** tracker, cv::Ptr<cv::TrackerDaSiamRPN>** sharedPtr)
{
#ifdef HAVE_OPENCV_VIDEO
	delete* sharedPtr;
	*tracker = 0;
	*sharedPtr = 0;
#else
	throw_no_video();
#endif	
}