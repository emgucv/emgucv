//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "freetype_c.h"

cv::freetype::FreeType2* cveFreeType2Create(cv::Algorithm** algorithmPtr, cv::Ptr<cv::freetype::FreeType2>** sharedPtr)
{
	cv::Ptr<cv::freetype::FreeType2> ptr = cv::freetype::createFreeType2();
	*algorithmPtr = dynamic_cast<cv::Algorithm*>(ptr.get());
	
	*sharedPtr = new cv::Ptr<cv::freetype::FreeType2>(ptr);
	return ptr.get();
}
void cveFreeType2Release(cv::Ptr<cv::freetype::FreeType2>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
}

void cveFreeType2LoadFontData(cv::freetype::FreeType2* freetype, cv::String* fontFileName, int id)
{
	freetype->loadFontData(*fontFileName, id);
}
void cveFreeType2SetSplitNumber(cv::freetype::FreeType2* freetype, int num)
{
	freetype->setSplitNumber(num);
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
	freetype->putText(*img, *text, *org, fontHeight, *color, thickness, lineType, bottomLeftOrigin);
}

void cveFreeType2GetTextSize(
	cv::freetype::FreeType2* freetype,
	cv::String* text,
	int fontHeight, int thickness,
	int* baseLine,
	CvSize* size)
{
	cv::Size s = freetype->getTextSize(*text, fontHeight, thickness, baseLine);
	size->width = s.width;
	size->height = s.height;
}