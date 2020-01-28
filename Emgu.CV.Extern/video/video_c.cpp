//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "video_c.h"

//BackgroundSubtractorMOG2
cv::BackgroundSubtractorMOG2* cveBackgroundSubtractorMOG2Create(int history, float varThreshold, bool bShadowDetection, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm, cv::Ptr<cv::BackgroundSubtractorMOG2>** sharedPtr)
{
	cv::Ptr<cv::BackgroundSubtractorMOG2> ptr = cv::createBackgroundSubtractorMOG2(history, varThreshold, bShadowDetection);
	*sharedPtr = new cv::Ptr<cv::BackgroundSubtractorMOG2>(ptr);
	cv::BackgroundSubtractorMOG2* bs = ptr.get();
	*bgSubtractor = dynamic_cast<cv::BackgroundSubtractor*>(bs);
	*algorithm = dynamic_cast<cv::Algorithm*>(bs);
	return bs;
}

void cveBackgroundSubtractorMOG2Release(cv::BackgroundSubtractorMOG2** bgSubtractor, cv::Ptr<cv::BackgroundSubtractorMOG2>** sharedPtr)
{
	delete *sharedPtr;
	*bgSubtractor = 0;
	*sharedPtr = 0;
}

//BackgroundSubtractor
void cveBackgroundSubtractorUpdate(cv::BackgroundSubtractor* bgSubtractor, cv::_InputArray* image, cv::_OutputArray* fgmask, double learningRate)
{
	//cv::Mat imgMat = cv::cvarrToMat(image);
	//cv::Mat fgMat = cv::cvarrToMat(fgmask);
	bgSubtractor->apply(*image, *fgmask, learningRate);
}

void cveBackgroundSubtractorGetBackgroundImage(cv::BackgroundSubtractor* bgSubtractor, cv::_OutputArray* backgroundImage)
{
	bgSubtractor->getBackgroundImage(*backgroundImage);
}

//BackgroundSubtractorKNN
cv::BackgroundSubtractorKNN* cveBackgroundSubtractorKNNCreate(int history, double dist2Threshold, bool detectShadows, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm, cv::Ptr<cv::BackgroundSubtractorKNN>** sharedPtr)
{
	cv::Ptr<cv::BackgroundSubtractorKNN> ptr = cv::createBackgroundSubtractorKNN(history, dist2Threshold, detectShadows);

	*sharedPtr = new cv::Ptr<cv::BackgroundSubtractorKNN>(ptr);

	cv::BackgroundSubtractorKNN* bs = ptr.get();
	*bgSubtractor = dynamic_cast<cv::BackgroundSubtractor*>(bs);
	*algorithm = dynamic_cast<cv::Algorithm*>(bs);
	return bs;
}
void cveBackgroundSubtractorKNNRelease(cv::BackgroundSubtractorKNN** bgSubtractor, cv::Ptr<cv::BackgroundSubtractorKNN>** sharedPtr)
{
	delete *sharedPtr;
	*bgSubtractor = 0;
	*sharedPtr = 0;
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
	cv::Ptr<cv::FarnebackOpticalFlow> dof = cv::FarnebackOpticalFlow::create(
		numLevels, pyrScale, fastPyramids, winSize, numIters, polyN, polySigma, flags
	);
	*sharedPtr = new cv::Ptr<cv::FarnebackOpticalFlow>(dof);
	cv::FarnebackOpticalFlow* ptr = dof.get();
	*denseOpticalFlow = dynamic_cast<cv::DenseOpticalFlow*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
}
void cveFarnebackOpticalFlowRelease(cv::FarnebackOpticalFlow** flow, cv::Ptr<cv::FarnebackOpticalFlow>** sharedPtr)
{
	delete *sharedPtr;
	*flow = 0;
	*sharedPtr = 0;
}


void cveDenseOpticalFlowCalc(cv::DenseOpticalFlow* dof, cv::_InputArray* i0, cv::_InputArray* i1, cv::_InputOutputArray* flow)
{
	dof->calc(*i0, *i1, *flow);
}

void cveSparseOpticalFlowCalc(
	cv::SparseOpticalFlow* sof,
	cv::_InputArray* prevImg, cv::_InputArray* nextImg,
	cv::_InputArray* prevPts, cv::_InputOutputArray* nextPts,
	cv::_OutputArray* status,
	cv::_OutputArray* err)
{
	sof->calc(*prevImg, *nextImg, *prevPts, *nextPts, *status, err ? *err : dynamic_cast<cv::OutputArray>(cv::noArray()));
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
	cv::Ptr<cv::SparsePyrLKOpticalFlow> sof = cv::SparsePyrLKOpticalFlow::create(
		*winSize, maxLevel, *crit, flags, minEigThreshold
	);
	*sharedPtr = new cv::Ptr<cv::SparsePyrLKOpticalFlow>(sof);
	cv::SparsePyrLKOpticalFlow* ptr = sof.get();
	*sparseOpticalFlow = dynamic_cast<cv::SparseOpticalFlow*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
}
void cveSparsePyrLKOpticalFlowRelease(cv::SparsePyrLKOpticalFlow** flow, cv::Ptr<cv::SparsePyrLKOpticalFlow>** sharedPtr)
{
	delete *sharedPtr;
	*flow = 0;
	*sharedPtr = 0;
}

void cveDenseOpticalFlowRelease(cv::Ptr<cv::DenseOpticalFlow>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
}

void cveCalcOpticalFlowFarneback(cv::_InputArray* prev, cv::_InputArray* next, cv::_InputOutputArray* flow, double pyrScale, int levels, int winSize, int iterations, int polyN, double polySigma, int flags)
{
	cv::calcOpticalFlowFarneback(*prev, *next, *flow, pyrScale, levels, winSize, iterations, polyN, polySigma, flags);
}

void cveCalcOpticalFlowPyrLK(cv::_InputArray* prevImg, cv::_InputArray* nextImg, cv::_InputArray* prevPts, cv::_InputOutputArray* nextPts, cv::_OutputArray* status, cv::_OutputArray* err, CvSize* winSize, int maxLevel, CvTermCriteria* criteria, int flags, double minEigenThreshold)
{
	cv::calcOpticalFlowPyrLK(*prevImg, *nextImg, *prevPts, *nextPts, *status, *err, *winSize, maxLevel, *criteria, flags, minEigenThreshold);
}

void cveCamShift(cv::_InputArray* probImage, CvRect* window, CvTermCriteria* criteria, CvBox2D* result)
{
	cv::Rect rect = *window;
	cv::RotatedRect rr = cv::CamShift(*probImage, rect, *criteria);
	*window = cvRect(rect);
	result->center = cvPoint2D32f(rr.center.x, rr.center.y);
	result->size = cvSize2D32f(rr.size.width, rr.size.height);
	result->angle = rr.angle;
}

int cveMeanShift(cv::_InputArray* probImage, CvRect* window, CvTermCriteria* criteria)
{
	cv::Rect rect = *window;
	int result = cv::meanShift(*probImage, rect, *criteria);
	*window = cvRect(rect);
	return result;
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
	return cv::buildOpticalFlowPyramid(*img, *pyramid, *winSize, maxLevel, withDerivatives, pyrBorder, derivBorder, tryReuseInputImage);
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
	return cv::findTransformECC(
		*templateImage, *inputImage,
		*warpMatrix, motionType,
		*criteria,
		inputMask ? *inputMask : (cv::InputArray) cv::noArray());
}


cv::KalmanFilter* cveKalmanFilterCreate(int dynamParams, int measureParams, int controlParams, int type)
{
	return new cv::KalmanFilter(dynamParams, measureParams, controlParams, type);
}

void cveKalmanFilterRelease(cv::KalmanFilter** filter)
{
	delete *filter;
	*filter = 0;
}

const cv::Mat* cveKalmanFilterPredict(cv::KalmanFilter* kalman, cv::Mat* control)
{
	return &(kalman->predict(control ? *control : cv::Mat()));
}

const cv::Mat* cveKalmanFilterCorrect(cv::KalmanFilter* kalman, cv::Mat* measurement)
{
	return &(kalman->correct(*measurement));
}

cv::DISOpticalFlow* cveDISOpticalFlowCreate(int preset, cv::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm, cv::Ptr<cv::DISOpticalFlow>** sharedPtr)
{
	cv::Ptr<cv::DISOpticalFlow> ptr = cv::DISOpticalFlow::create(preset);
	*sharedPtr = new cv::Ptr<cv::DISOpticalFlow>(ptr);
	*denseFlow = dynamic_cast<cv::DenseOpticalFlow*>(ptr.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
}

void cveDISOpticalFlowRelease(cv::DISOpticalFlow** flow, cv::Ptr<cv::DISOpticalFlow>** sharedPtr)
{
	delete *sharedPtr;
	*flow = 0;
	*sharedPtr = 0;
}

cv::VariationalRefinement* cveVariationalRefinementCreate(cv::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm, cv::Ptr<cv::VariationalRefinement>** sharedPtr)
{
	cv::Ptr<cv::VariationalRefinement> ptr = cv::VariationalRefinement::create();
	*sharedPtr = new cv::Ptr<cv::VariationalRefinement>(ptr);
	*denseFlow = dynamic_cast<cv::DenseOpticalFlow*>(ptr.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
}

void cveVariationalRefinementRelease(cv::VariationalRefinement** flow, cv::Ptr<cv::VariationalRefinement>** sharedPtr)
{
	delete *sharedPtr;
	*flow = 0;
	*sharedPtr = 0;
}
