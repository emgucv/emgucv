//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "video_c.h"

//BackgroundSubtractorMOG2
cv::BackgroundSubtractorMOG2* cveBackgroundSubtractorMOG2Create(int history,  float varThreshold, bool bShadowDetection, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm)
{
   cv::Ptr<cv::BackgroundSubtractorMOG2> ptr =  cv::createBackgroundSubtractorMOG2(history, varThreshold, bShadowDetection);
   ptr.addref();
   cv::BackgroundSubtractorMOG2* bs = ptr.get();
   *bgSubtractor = dynamic_cast<cv::BackgroundSubtractor*>(bs);
   *algorithm = dynamic_cast<cv::Algorithm*>(bs);
   return bs;
}

void cveBackgroundSubtractorMOG2Release(cv::BackgroundSubtractorMOG2** bgSubtractor)
{
   delete *bgSubtractor;
   *bgSubtractor = 0;
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
cv::BackgroundSubtractorKNN* cveBackgroundSubtractorKNNCreate(int history, double dist2Threshold, bool detectShadows, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm)
{
   cv::Ptr<cv::BackgroundSubtractorKNN> ptr = cv::createBackgroundSubtractorKNN(history, dist2Threshold, detectShadows);
  
   ptr.addref();

   cv::BackgroundSubtractorKNN* bs = ptr.get();
   *bgSubtractor = dynamic_cast<cv::BackgroundSubtractor*>(bs);
   *algorithm = dynamic_cast<cv::Algorithm*>(bs);
   return bs;
}
void cveBackgroundSubtractorKNNRelease(cv::BackgroundSubtractorKNN** bgSubtractor)
{
   delete *bgSubtractor;
   *bgSubtractor = 0;
}


cv::DualTVL1OpticalFlow* cveDenseOpticalFlowCreateDualTVL1(cv::DenseOpticalFlow** denseOpticalFlow, cv::Algorithm** algorithm)
{
   cv::Ptr<cv::DualTVL1OpticalFlow> dof = cv::createOptFlow_DualTVL1();
   dof.addref();
   cv::DualTVL1OpticalFlow* ptr = dof.get();
   *denseOpticalFlow = dynamic_cast<cv::DenseOpticalFlow*>(ptr);
   *algorithm = dynamic_cast<cv::Algorithm*>(ptr);
   return ptr;
}
void cveDualTVL1OpticalFlowRelease(cv::DualTVL1OpticalFlow** flow)
{
   delete *flow;
   *flow = 0;
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
	cv::Algorithm** algorithm)
{
	cv::Ptr<cv::FarnebackOpticalFlow> dof = cv::FarnebackOpticalFlow::create(
	numLevels, pyrScale, fastPyramids, winSize, numIters, polyN, polySigma, flags
	);
	dof.addref();
	cv::FarnebackOpticalFlow* ptr = dof.get();
	*denseOpticalFlow = dynamic_cast<cv::DenseOpticalFlow*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
}
void cveFarnebackOpticalFlowRelease(cv::FarnebackOpticalFlow** flow)
{
	delete *flow;
	*flow = 0;
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
	sof->calc(*prevImg, *nextImg, *prevPts, *nextPts, *status, err ? *err : dynamic_cast<cv::OutputArray>( cv::noArray() ) );
}

cv::SparsePyrLKOpticalFlow* cveSparsePyrLKOpticalFlowCreate(
	CvSize* winSize,
	int maxLevel,
	CvTermCriteria* crit,
	int flags,
	double minEigThreshold,
	cv::SparseOpticalFlow** sparseOpticalFlow,
	cv::Algorithm** algorithm)
{
	cv::Ptr<cv::SparsePyrLKOpticalFlow> sof = cv::SparsePyrLKOpticalFlow::create(
		*winSize, maxLevel, *crit, flags, minEigThreshold
	);
	sof.addref();
	cv::SparsePyrLKOpticalFlow* ptr = sof.get();
	*sparseOpticalFlow = dynamic_cast<cv::SparseOpticalFlow*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
}
void cveSparsePyrLKOpticalFlowRelease(cv::SparsePyrLKOpticalFlow** flow)
{
	delete *flow;
	*flow = 0;
}

void cveDenseOpticalFlowRelease(cv::DenseOpticalFlow** flow)
{
	delete *flow;
	*flow = 0;
}

void cveCalcOpticalFlowFarneback(cv::_InputArray* prev, cv::_InputArray* next, cv::_InputOutputArray* flow, double pyrScale, int levels, int winSize, int iterations, int polyN, double polySigma, int flags)
{
   cv::calcOpticalFlowFarneback(*prev, *next, *flow, pyrScale, levels, winSize, iterations, polyN, polySigma, flags);
}

void cveCalcOpticalFlowPyrLK(cv::_InputArray* prevImg, cv::_InputArray* nextImg, cv::_InputArray* prevPts, cv::_InputOutputArray* nextPts, cv::_OutputArray* status, cv::_OutputArray* err, CvSize* winSize, int maxLevel, CvTermCriteria* criteria, int flags, double minEigenThreshold)
{
   cv::calcOpticalFlowPyrLK(*prevImg, *nextImg, *prevPts, *nextPts, *status, *err, *winSize, maxLevel, *criteria, flags, minEigenThreshold);
}

void cveCamShift( cv::_InputArray* probImage, CvRect* window, CvTermCriteria* criteria, CvBox2D* result)
{
   cv::Rect rect = *window;
   cv::RotatedRect rr = cv::CamShift(*probImage, rect, *criteria);
   *window = rect;
   *result = rr;
}

int cveMeanShift( cv::_InputArray* probImage, CvRect* window, CvTermCriteria* criteria )
{
   cv::Rect rect = *window;
   int result = cv::meanShift(*probImage, rect, *criteria);
   *window = rect;
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

void cveEstimateRigidTransform(cv::_InputArray* src, cv::_InputArray* dst, bool fullAffine, cv::Mat* result)
{
   cv::Mat r = cv::estimateRigidTransform(*src, *dst, fullAffine);
   cv::swap(r, *result);
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