//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_VIDEO_C_H
#define EMGU_VIDEO_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/video/video.hpp"

//BackgroundSubtractorMOG2
CVAPI(cv::BackgroundSubtractorMOG2*) cveBackgroundSubtractorMOG2Create(int history,  float varThreshold, bool bShadowDetection, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm, cv::Ptr<cv::BackgroundSubtractorMOG2>** sharedPtr);
CVAPI(void) cveBackgroundSubtractorMOG2Release(cv::BackgroundSubtractorMOG2** bgSubtractor, cv::Ptr<cv::BackgroundSubtractorMOG2>** sharedPtr);

//BackgroundSubtractor
CVAPI(void) cveBackgroundSubtractorUpdate(cv::BackgroundSubtractor* bgSubtractor, cv::_InputArray* image, cv::_OutputArray* fgmask, double learningRate);
CVAPI(void) cveBackgroundSubtractorGetBackgroundImage(cv::BackgroundSubtractor* bgSubtractor, cv::_OutputArray* backgroundImage);


//BackgroundSubtractorKNN
CVAPI(cv::BackgroundSubtractorKNN*) cveBackgroundSubtractorKNNCreate(int history, double dist2Threshold, bool detectShadows, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm, cv::Ptr<cv::BackgroundSubtractorKNN>** sharedPtr);
CVAPI(void) cveBackgroundSubtractorKNNRelease(cv::BackgroundSubtractorKNN** bgSubtractor, cv::Ptr<cv::BackgroundSubtractorKNN>** sharedPtr);


CVAPI(cv::FarnebackOpticalFlow*) cveFarnebackOpticalFlowCreate(
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
	cv::Ptr<cv::FarnebackOpticalFlow>** sharedPtr);
CVAPI(void) cveFarnebackOpticalFlowRelease(cv::FarnebackOpticalFlow** flow, cv::Ptr<cv::FarnebackOpticalFlow>** sharedPtr);

CVAPI(void) cveDenseOpticalFlowCalc(cv::DenseOpticalFlow* dof, cv::_InputArray* i0, cv::_InputArray* i1, cv::_InputOutputArray* flow);
CVAPI(void) cveDenseOpticalFlowRelease(cv::Ptr<cv::DenseOpticalFlow>** sharedPtr);

CVAPI(void) cveSparseOpticalFlowCalc(
	cv::SparseOpticalFlow* sof,  
	cv::_InputArray* prevImg, cv::_InputArray* nextImg,
	cv::_InputArray* prevPts, cv::_InputOutputArray* nextPts,
	cv::_OutputArray* status,
	cv::_OutputArray* err);

CVAPI(cv::SparsePyrLKOpticalFlow*) cveSparsePyrLKOpticalFlowCreate(
	CvSize* winSize,
	int maxLevel, 
    CvTermCriteria* crit,
	int flags,
	double minEigThreshold,
	cv::SparseOpticalFlow** sparseOpticalFlow,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::SparsePyrLKOpticalFlow>** sharedPtr);
CVAPI(void) cveSparsePyrLKOpticalFlowRelease(cv::SparsePyrLKOpticalFlow** flow, cv::Ptr<cv::SparsePyrLKOpticalFlow>** sharedPtr);


CVAPI(void) cveCalcOpticalFlowFarneback(cv::_InputArray* prev, cv::_InputArray* next, cv::_InputOutputArray* flow, double pyrScale, int levels, int winSize, int iterations, int polyN, double polySigma, int flags);
CVAPI(void) cveCalcOpticalFlowPyrLK(cv::_InputArray* prevImg, cv::_InputArray* nextImg, cv::_InputArray* prevPts, cv::_InputOutputArray* nextPts, cv::_OutputArray* status, cv::_OutputArray* err, CvSize* winSize, int maxLevel, CvTermCriteria* criteria, int flags, double minEigenThreshold);

CVAPI(void) cveCamShift( cv::_InputArray* probImage, CvRect* window, CvTermCriteria* criteria, CvBox2D* result);

CVAPI(int) cveMeanShift( cv::_InputArray* probImage, CvRect* window, CvTermCriteria* criteria );

CVAPI(int) cveBuildOpticalFlowPyramid(
	cv::_InputArray* img, 
	cv::_OutputArray* pyramid,
	CvSize* winSize, 
	int maxLevel, 
	bool withDerivatives,
	int pyrBorder,
	int derivBorder,
	bool tryReuseInputImage);

//CVAPI(void) cveEstimateRigidTransform(cv::_InputArray* src, cv::_InputArray* dst, bool fullAffine, cv::Mat* result);

CVAPI(double) cveFindTransformECC(cv::_InputArray* templateImage, cv::_InputArray* inputImage,
	cv::_InputOutputArray* warpMatrix, int motionType,
	CvTermCriteria* criteria,
	cv::_InputArray* inputMask);

CVAPI(cv::KalmanFilter*) cveKalmanFilterCreate(int dynamParams, int measureParams, int controlParams, int type);

CVAPI(void) cveKalmanFilterRelease(cv::KalmanFilter** filter);

CVAPI(const cv::Mat*) cveKalmanFilterPredict(cv::KalmanFilter* kalman, cv::Mat* control);

CVAPI(const cv::Mat*) cveKalmanFilterCorrect(cv::KalmanFilter* kalman, cv::Mat* measurement);

CVAPI(cv::DISOpticalFlow*) cveDISOpticalFlowCreate(int preset, cv::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm, cv::Ptr<cv::DISOpticalFlow>** sharedPtr);
CVAPI(void) cveDISOpticalFlowRelease(cv::DISOpticalFlow** flow, cv::Ptr<cv::DISOpticalFlow>** sharedPtr);

CVAPI(cv::VariationalRefinement*) cveVariationalRefinementCreate(cv::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm, cv::Ptr<cv::VariationalRefinement>** sharedPtr);
CVAPI(void) cveVariationalRefinementRelease(cv::VariationalRefinement** flow, cv::Ptr<cv::VariationalRefinement>** sharedPtr);

#endif