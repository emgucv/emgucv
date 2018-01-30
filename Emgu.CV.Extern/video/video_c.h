//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_VIDEO_C_H
#define EMGU_VIDEO_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/video/video.hpp"

//BackgroundSubtractorMOG2
CVAPI(cv::BackgroundSubtractorMOG2*) cveBackgroundSubtractorMOG2Create(int history, float varThreshold, bool bShadowDetection);
CVAPI(void) cveBackgroundSubtractorMOG2Release(cv::BackgroundSubtractorMOG2** bgSubtractor);

//BackgroundSubtractor
CVAPI(void) cveBackgroundSubtractorUpdate(cv::BackgroundSubtractor* bgSubtractor, cv::_InputArray* image, cv::_OutputArray* fgmask, double learningRate);
CVAPI(void) cveBackgroundSubtractorGetBackgroundImage(cv::BackgroundSubtractor* bgSubtractor, cv::_OutputArray* backgroundImage);


//BackgroundSubtractorKNN
CVAPI(cv::BackgroundSubtractorKNN*) cveBackgroundSubtractorKNNCreate(int history, double dist2Threshold, bool detectShadows);
CVAPI(void) cveBackgroundSubtractorKNNRelease(cv::BackgroundSubtractorKNN** bgSubtractor);

CVAPI(cv::DualTVL1OpticalFlow*) cveDenseOpticalFlowCreateDualTVL1(cv::DenseOpticalFlow** denseOpticalFlow, cv::Algorithm** algorithm);
CVAPI(void) cveDualTVL1OpticalFlowRelease(cv::DualTVL1OpticalFlow** flow);

CVAPI(void) cveDenseOpticalFlowCalc(cv::DenseOpticalFlow* dof, cv::_InputArray* i0, cv::_InputArray* i1, cv::_InputOutputArray* flow);
CVAPI(void) cveDenseOpticalFlowRelease(cv::DenseOpticalFlow** flow);

CVAPI(void) cveCalcOpticalFlowFarneback(cv::_InputArray* prev, cv::_InputArray* next, cv::_InputOutputArray* flow, double pyrScale, int levels, int winSize, int iterations, int polyN, double polySigma, int flags);

CVAPI(int) cveBuildOpticalFlowPyramid(
   cv::_InputArray *img, cv::_OutputArray *pyramidVec,
   CvSize* winSize, int maxLevel, int withDerivatives,
   int pyrBorder, int derivBorder, int tryReuseInputImage);

CVAPI(void) cveCalcOpticalFlowPyrLK(cv::_InputArray* prevImg, cv::_InputArray* nextImg, cv::_InputArray* prevPts, cv::_InputOutputArray* nextPts, cv::_OutputArray* status, cv::_OutputArray* err, CvSize* winSize, int maxLevel, CvTermCriteria* criteria, int flags, double minEigenThreshold);

CVAPI(void) cveCamShift(cv::_InputArray* probImage, CvRect* window, CvTermCriteria* criteria, CvBox2D* result);

CVAPI(int) cveMeanShift(cv::_InputArray* probImage, CvRect* window, CvTermCriteria* criteria);

CVAPI(void) cveEstimateRigidTransform(cv::_InputArray* src, cv::_InputArray* dst, bool fullAffine, cv::Mat* result);

CVAPI(cv::KalmanFilter*) cveKalmanFilterCreate(int dynamParams, int measureParams, int controlParams, int type);

CVAPI(void) cveKalmanFilterRelease(cv::KalmanFilter** filter);

CVAPI(const cv::Mat*) cveKalmanFilterPredict(cv::KalmanFilter* kalman, cv::Mat* control);

CVAPI(const cv::Mat*) cveKalmanFilterCorrect(cv::KalmanFilter* kalman, cv::Mat* measurement);

#endif