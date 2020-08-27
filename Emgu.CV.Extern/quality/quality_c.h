//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_QUALITY_C_H
#define EMGU_QUALITY_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_QUALITY
#include "opencv2/quality.hpp"
#else
static inline CV_NORETURN void throw_no_quality() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without quality module support"); }
namespace cv {
namespace quality {
class QualityBase {};
class QualityMSE {};
class QualityBRISQUE {};
class QualityPSNR {};
class QualitySSIM {};
class QualityGMSD {};
}
}
#endif

CVAPI(void) cveQualityBaseCompute(cv::quality::QualityBase* qualityBase, cv::_InputArray* cmpImgs, CvScalar* score);
CVAPI(void) cveQualityBaseGetQualityMap(cv::quality::QualityBase* qualityBase, cv::_OutputArray* dst);

CVAPI(cv::quality::QualityMSE*) cveQualityMSECreate(
	cv::_InputArray* refImgs,
	cv::quality::QualityBase** qualityBase,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::quality::QualityMSE>** sharedPtr);

CVAPI(void) cveQualityMSERelease(cv::Ptr<cv::quality::QualityMSE>** sharedPtr);

CVAPI(cv::quality::QualityBRISQUE*) cveQualityBRISQUECreate(
	cv::String* modelFilePath, 
	cv::String* rangeFilePath,
	cv::quality::QualityBase** qualityBase,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::quality::QualityBRISQUE>** sharedPtr);

CVAPI(void) cveQualityBRISQUERelease(cv::Ptr<cv::quality::QualityBRISQUE>** sharedPtr);

CVAPI(cv::quality::QualityPSNR*) cveQualityPSNRCreate(
	cv::_InputArray* refImgs, 
	double maxPixelValue,
	cv::quality::QualityBase** qualityBase,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::quality::QualityPSNR>** sharedPtr);

CVAPI(void) cveQualityPSNRRelease(cv::Ptr<cv::quality::QualityPSNR>** sharedPtr);

CVAPI(cv::quality::QualitySSIM*) cveQualitySSIMCreate(
	cv::_InputArray* refImgs,
	cv::quality::QualityBase** qualityBase,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::quality::QualitySSIM>** sharedPtr);

CVAPI(void) cveQualitySSIMRelease(cv::Ptr<cv::quality::QualitySSIM>** sharedPtr);

CVAPI(cv::quality::QualityGMSD*) cveQualityGMSDCreate(
	cv::_InputArray* refImgs,
	cv::quality::QualityBase** qualityBase,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::quality::QualityGMSD>** sharedPtr);

CVAPI(void) cveQualityGMSDRelease(cv::Ptr<cv::quality::QualityGMSD>** sharedPtr);

#endif