//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_FREETYPE_C_H
#define EMGU_FREETYPE_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/freetype.hpp"

CVAPI(cv::freetype::FreeType2*) cveFreeType2Create(cv::Algorithm** algorithmPtr, cv::Ptr<cv::freetype::FreeType2>** sharedPtr);
CVAPI(void) cveFreeType2Release(cv::Ptr<cv::freetype::FreeType2>** sharedPtr);
CVAPI(void) cveFreeType2LoadFontData(cv::freetype::FreeType2* freetype, cv::String* fontFileName, int id);
CVAPI(void) cveFreeType2SetSplitNumber(cv::freetype::FreeType2* freetype, int num);
CVAPI(void) cveFreeType2PutText(
	cv::freetype::FreeType2* freetype,
	cv::_InputOutputArray* img, 
	cv::String* text, 
	CvPoint* org,
	int fontHeight, CvScalar* color,
	int thickness, int lineType, bool bottomLeftOrigin
);
CVAPI(void) cveFreeType2GetTextSize(
	cv::freetype::FreeType2* freetype,
	cv::String* text,
	int fontHeight, int thickness,
	int* baseLine,
	CvSize* size);
#endif