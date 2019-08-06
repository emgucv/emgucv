//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_QUALITY_C_H
#define EMGU_QUALITY_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/quality.hpp"

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