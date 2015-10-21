//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "xphoto_c.h"

void cveBalanceWhite(const cv::Mat* src, cv::Mat* dst, const int algorithmType,
   const float inputMin, const float inputMax,
   const float outputMin, const float outputMax)
{
   cv::xphoto::balanceWhite(*src, *dst, algorithmType, inputMin, inputMax, outputMin, outputMax);
}

void cveAutowbGrayworld(cv::_InputArray* src, cv::_OutputArray* dst, float thresh)
{
   cv::xphoto::autowbGrayworld(*src, *dst, thresh);
}

void cveDctDenoising(const cv::Mat* src, cv::Mat* dst, const double sigma, const int psize)
{
   cv::xphoto::dctDenoising(*src, *dst, sigma, psize);
}

void cveXInpaint(const cv::Mat* src, const cv::Mat* mask, cv::Mat* dst, const int algorithmType)
{
   cv::xphoto::inpaint(*src, *mask, *dst, algorithmType);
}