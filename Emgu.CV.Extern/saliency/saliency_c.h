//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_SALIENCY_C_H
#define EMGU_SALIENCY_C_H


#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_SALIENCY
#include "opencv2/saliency.hpp"
#else
static inline CV_NORETURN void throw_no_saliency() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without Saliency support"); }
namespace cv
{
	namespace saliency
	{
		class StaticSaliencySpectralResidual
		{
		};
		class StaticSaliency
		{
		};
		class StaticSaliencyFineGrained
		{
		};
		class MotionSaliencyBinWangApr2014
		{
		};
		class Objectness
		{
		};
		class MotionSaliency
		{
		};
		class Saliency
		{
		};
		class ObjectnessBING
		{
		};
	}
}
#endif

CVAPI(cv::saliency::StaticSaliencySpectralResidual*) cveStaticSaliencySpectralResidualCreate(cv::saliency::StaticSaliency** static_saliency, cv::saliency::Saliency** saliency, cv::Algorithm** algorithm, cv::Ptr<cv::saliency::StaticSaliencySpectralResidual>** sharedPtr);
CVAPI(void) cveStaticSaliencySpectralResidualRelease(cv::saliency::StaticSaliencySpectralResidual** saliency, cv::Ptr<cv::saliency::StaticSaliencySpectralResidual>** sharedPtr);

CVAPI(cv::saliency::StaticSaliencyFineGrained*) cveStaticSaliencyFineGrainedCreate(cv::saliency::StaticSaliency** static_saliency, cv::saliency::Saliency** saliency, cv::Algorithm** algorithm, cv::Ptr<cv::saliency::StaticSaliencyFineGrained>** sharedPtr);
CVAPI(void) cveStaticSaliencyFineGrainedRelease(cv::saliency::StaticSaliencyFineGrained** saliency, cv::Ptr<cv::saliency::StaticSaliencyFineGrained>** sharedPtr);

CVAPI(cv::saliency::MotionSaliencyBinWangApr2014*) cveMotionSaliencyBinWangApr2014Create(cv::saliency::MotionSaliency** motion_saliency, cv::saliency::Saliency** saliency, cv::Algorithm** algorithm, cv::Ptr<cv::saliency::MotionSaliencyBinWangApr2014>** sharedPtr);
CVAPI(void) cveMotionSaliencyBinWangApr2014Release(cv::saliency::MotionSaliencyBinWangApr2014** saliency, cv::Ptr<cv::saliency::MotionSaliencyBinWangApr2014>** sharedPtr);

CVAPI(cv::saliency::ObjectnessBING*) cveObjectnessBINGCreate(cv::saliency::Objectness** objectness_saliency, cv::saliency::Saliency** saliency, cv::Algorithm** algorithm, cv::Ptr<cv::saliency::ObjectnessBING>** sharedPtr);
CVAPI(void) cveObjectnessBINGRelease(cv::saliency::ObjectnessBING** saliency, cv::Ptr<cv::saliency::ObjectnessBING>** sharedPtr);


CVAPI(bool) cveSaliencyComputeSaliency(cv::saliency::Saliency* saliency, cv::_InputArray* image, cv::_OutputArray* saliencyMap);

CVAPI(bool) cveStaticSaliencyComputeBinaryMap(cv::saliency::StaticSaliency* saliency, cv::_InputArray* saliencyMap, cv::_OutputArray* binaryMap);

CVAPI(bool) cveSaliencyMotionInit(cv::saliency::Saliency* saliency);
CVAPI(void) cveSaliencyMotionSetImageSize(cv::saliency::Saliency* saliency, int width, int height);

CVAPI(void) cveObjectnessBINGSetTrainingPath(cv::saliency::ObjectnessBING* saliency, cv::String* trainingPath);

CVAPI(void) cveObjectnessBINGGetObjectnessValues(cv::saliency::ObjectnessBING* saliency, std::vector<float>* values);

#endif