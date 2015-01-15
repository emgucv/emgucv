//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_VIDEO_C_H
#define EMGU_VIDEO_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/video/video.hpp"

//BackgroundSubtractorMOG2
CVAPI(cv::BackgroundSubtractorMOG2*) CvBackgroundSubtractorMOG2Create(int history,  float varThreshold, bool bShadowDetection);
CVAPI(void) CvBackgroundSubtractorMOG2Release(cv::BackgroundSubtractorMOG2** bgSubtractor);

//BackgroundSubtractor
CVAPI(void) CvBackgroundSubtractorUpdate(cv::BackgroundSubtractor* bgSubtractor, cv::_InputArray* image, cv::_OutputArray* fgmask, double learningRate);



//BackgroundSubtractorKNN
CVAPI(cv::BackgroundSubtractorKNN*) CvBackgroundSubtractorKNNCreate(int history, double dist2Threshold, bool detectShadows);
CVAPI(void) CvBackgroundSubtractorKNNRelease(cv::BackgroundSubtractorKNN** bgSubtractor);



CVAPI(cv::DenseOpticalFlow*) cveDenseOpticalFlowCreateDualTVL1();
CVAPI(void) cveDenseOpticalFlowRelease(cv::DenseOpticalFlow** flow);
CVAPI(void) cveDenseOpticalFlowCalc(cv::DenseOpticalFlow* dof, cv::_InputArray* i0, cv::_InputArray* i1, cv::_InputOutputArray* flow);

CVAPI(void) cveCalcOpticalFlowFarneback(cv::_InputArray* prev, cv::_InputArray* next, cv::_InputOutputArray* flow, double pyrScale, int levels, int winSize, int iterations, int polyN, double polySigma, int flags);
CVAPI(void) cveCalcOpticalFlowPyrLK(cv::_InputArray* prevImg, cv::_InputArray* nextImg, cv::_InputArray* prevPts, cv::_InputOutputArray* nextPts, cv::_OutputArray* status, cv::_OutputArray* err, CvSize* winSize, int maxLevel, CvTermCriteria* criteria, int flags, double minEigenThreshold);

CVAPI(void) cveCamShift( cv::_InputArray* probImage, CvRect* window, CvTermCriteria* criteria, CvBox2D* result);

CVAPI(int) cveMeanShift( cv::_InputArray* probImage, CvRect* window, CvTermCriteria* criteria );

CVAPI(void) cveEstimateRigidTransform(cv::_InputArray* src, cv::_InputArray* dst, bool fullAffine, cv::Mat* result);
#endif