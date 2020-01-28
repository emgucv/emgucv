//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_FREETYPE_C_H
#define EMGU_FREETYPE_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_FREETYPE
#include "opencv2/freetype.hpp"
#else

static inline CV_NORETURN void throw_no_freetype() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without Freetype support"); }

namespace cv
{
	namespace freetype
	{
		class FreeType2
		{

		};
	}
}
#endif

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