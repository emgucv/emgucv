//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_XPHOTO_C_H
#define EMGU_XPHOTO_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/xphoto.hpp"

CVAPI(void) cveWhiteBalancerBalanceWhite(cv::xphoto::WhiteBalancer* whiteBalancer, cv::_InputArray* src, cv::_OutputArray* dst);

CVAPI(cv::xphoto::SimpleWB*) cveSimpleWBCreate(cv::xphoto::WhiteBalancer** whiteBalancer);
CVAPI(void) cveSimpleWBRelease(cv::xphoto::SimpleWB** whiteBalancer);

CVAPI(cv::xphoto::GrayworldWB*) cveGrayworldWBCreate(cv::xphoto::WhiteBalancer** whiteBalancer);
CVAPI(void) cveGrayworldWBRelease(cv::xphoto::GrayworldWB** whiteBalancer);

CVAPI(cv::xphoto::LearningBasedWB*) cveLearningBasedWBCreate(cv::xphoto::WhiteBalancer** whiteBalancer);
CVAPI(void) cveLearningBasedWBRelease(cv::xphoto::LearningBasedWB** whiteBalancer);



CVAPI(void) cveDctDenoising(const cv::Mat* src, cv::Mat* dst, const double sigma, const int psize);

CVAPI(void) cveXInpaint(const cv::Mat* src, const cv::Mat* mask, cv::Mat* dst, const int algorithmType);
#endif