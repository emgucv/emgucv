//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "freetype_c.h"

cv::freetype::FreeType2* cveFreeType2Create(cv::Algorithm** algorithmPtr, cv::Ptr<cv::freetype::FreeType2>** sharedPtr)
{
#ifdef HAVE_OPENCV_FREETYPE
	cv::Ptr<cv::freetype::FreeType2> ptr = cv::freetype::createFreeType2();
	*algorithmPtr = dynamic_cast<cv::Algorithm*>(ptr.get());
	
	*sharedPtr = new cv::Ptr<cv::freetype::FreeType2>(ptr);
	return ptr.get();
#else
	throw_no_freetype();
#endif
}
void cveFreeType2Release(cv::Ptr<cv::freetype::FreeType2>** sharedPtr)
{
#ifdef HAVE_OPENCV_FREETYPE
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_freetype();
#endif
}

void cveFreeType2LoadFontData(cv::freetype::FreeType2* freetype, cv::String* fontFileName, int id)
{
#ifdef HAVE_OPENCV_FREETYPE
	freetype->loadFontData(*fontFileName, id);
#else
	throw_no_freetype();
#endif
}

void cveFreeType2SetSplitNumber(cv::freetype::FreeType2* freetype, int num)
{
#ifdef HAVE_OPENCV_FREETYPE
	freetype->setSplitNumber(num);
#else
	throw_no_freetype();
#endif
}

void cveFreeType2PutText(
	cv::freetype::FreeType2* freetype,
	cv::_InputOutputArray* img,
	cv::String* text,
	CvPoint* org,
	int fontHeight, CvScalar* color,
	int thickness, int lineType, bool bottomLeftOrigin
)
{
#ifdef HAVE_OPENCV_FREETYPE
	freetype->putText(*img, *text, *org, fontHeight, *color, thickness, lineType, bottomLeftOrigin);
#else
	throw_no_freetype();
#endif
}

void cveFreeType2GetTextSize(
	cv::freetype::FreeType2* freetype,
	cv::String* text,
	int fontHeight, int thickness,
	int* baseLine,
	CvSize* size)
{
#ifdef HAVE_OPENCV_FREETYPE
	cv::Size s = freetype->getTextSize(*text, fontHeight, thickness, baseLine);
	size->width = s.width;
	size->height = s.height;
#else
	throw_no_freetype();
#endif
}