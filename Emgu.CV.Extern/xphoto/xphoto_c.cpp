//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "xphoto_c.h"

void cveBalanceWhite(const cv::Mat* src, cv::Mat* dst, const int algorithmType,
   const float inputMin, const float inputMax,
   const float outputMin, const float outputMax)
{
   cv::xphoto::balanceWhite(*src, *dst, algorithmType, inputMin, inputMax, outputMin, outputMax);
}

void cveDctDenoising(const cv::Mat* src, cv::Mat* dst, const double sigma, const int psize)
{
   cv::xphoto::dctDenoising(*src, *dst, sigma, psize);
}