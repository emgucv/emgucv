//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_XPHOTO_C_H
#define EMGU_XPHOTO_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_XPHOTO
#include "opencv2/xphoto.hpp"
#else
static inline CV_NORETURN void throw_no_xphoto() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without xphoto module support"); }
namespace cv {
	class Tonemap {};
	namespace xphoto {
		class WhiteBalancer {};
		class SimpleWB {};
		class GrayworldWB {};
		class LearningBasedWB {};
		class TonemapDurand {};
	}
}
#endif

CVAPI(void) cveWhiteBalancerBalanceWhite(cv::xphoto::WhiteBalancer* whiteBalancer, cv::_InputArray* src, cv::_OutputArray* dst);

CVAPI(cv::xphoto::SimpleWB*) cveSimpleWBCreate(cv::xphoto::WhiteBalancer** whiteBalancer, cv::Ptr<cv::xphoto::SimpleWB>** sharedPtr);
CVAPI(void) cveSimpleWBRelease(cv::Ptr<cv::xphoto::SimpleWB>** sharedPtr);

CVAPI(cv::xphoto::GrayworldWB*) cveGrayworldWBCreate(cv::xphoto::WhiteBalancer** whiteBalancer, cv::Ptr<cv::xphoto::GrayworldWB>** sharedPtr);
CVAPI(void) cveGrayworldWBRelease(cv::Ptr<cv::xphoto::GrayworldWB>** sharedPtr);

CVAPI(cv::xphoto::LearningBasedWB*) cveLearningBasedWBCreate(cv::xphoto::WhiteBalancer** whiteBalancer, cv::Ptr<cv::xphoto::LearningBasedWB>** sharedPtr);
CVAPI(void) cveLearningBasedWBRelease(cv::Ptr<cv::xphoto::LearningBasedWB>** sharedPtr);

CVAPI(void) cveApplyChannelGains(cv::_InputArray* src, cv::_OutputArray* dst, float gainB, float gainG, float gainR);

CVAPI(void) cveDctDenoising(const cv::Mat* src, cv::Mat* dst, const double sigma, const int psize);

CVAPI(void) cveXInpaint(const cv::Mat* src, const cv::Mat* mask, cv::Mat* dst, const int algorithmType);

CVAPI(void) cveBm3dDenoising1(
	cv::_InputArray* src,
	cv::_InputOutputArray* dstStep1,
	cv::_OutputArray* dstStep2,
	float h,
	int templateWindowSize,
	int searchWindowSize,
	int blockMatchingStep1,
	int blockMatchingStep2,
	int groupSize,
	int slidingStep,
	float beta,
	int normType,
	int step,
	int transformType);

CVAPI(void) cveBm3dDenoising2(
	cv::_InputArray* src,
	cv::_OutputArray* dst,
	float h,
	int templateWindowSize,
	int searchWindowSize,
	int blockMatchingStep1,
	int blockMatchingStep2,
	int groupSize,
	int slidingStep,
	float beta,
	int normType,
	int step,
	int transformType);


CVAPI(void) cveOilPainting(
	cv::_InputArray* src, 
	cv::_OutputArray* dst, 
	int size, 
	int dynRatio, 
	int code);

CVAPI(cv::xphoto::TonemapDurand*) cveTonemapDurandCreate(float gamma, float contrast, float saturation, float sigmaSpace, float sigmaColor, cv::Tonemap** tonemap, cv::Algorithm** algorithm, cv::Ptr<cv::xphoto::TonemapDurand>** sharedPtr);
CVAPI(void) cveTonemapDurandRelease(cv::Ptr<cv::xphoto::TonemapDurand>** sharedPtr);

#endif