//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "photo_c.h"

void cvFastNlMeansDenoising(CvArr* src, CvArr* dst, float h, int templateWindowSize, int searchWindowSize)
{
   cv::Mat srcMat = cv::cvarrToMat(src);
   cv::Mat dstMat = cv::cvarrToMat(dst);
   cv::fastNlMeansDenoising(srcMat, dstMat, h, templateWindowSize, searchWindowSize);
}

void cvFastNlMeansDenoisingColored(CvArr* src, CvArr* dst, float h, float hColor, int templateWindowSize, int searchWindowSize)
{
   cv::Mat srcMat = cv::cvarrToMat(src);
   cv::Mat dstMat = cv::cvarrToMat(dst);
   cv::fastNlMeansDenoisingColored(srcMat, dstMat, h, hColor, templateWindowSize, searchWindowSize);
}
