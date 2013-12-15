//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "photo_c.h"

void cveInpaint( cv::_InputArray* src, cv::_InputArray* inpaintMask, cv::_OutputArray* dst, double inpaintRadius, int flags )
{
   cv::inpaint(*src, *inpaintMask, *dst, inpaintRadius, flags);
}
void cveFastNlMeansDenoising(cv::_InputArray* src, cv::_OutputArray* dst, float h, int templateWindowSize, int searchWindowSize)
{
   cv::fastNlMeansDenoising(*src, *dst, h, templateWindowSize, searchWindowSize);
}

void cveFastNlMeansDenoisingColored(cv::_InputArray* src, cv::_OutputArray* dst, float h, float hColor, int templateWindowSize, int searchWindowSize)
{
   cv::fastNlMeansDenoisingColored(*src, *dst, h, hColor, templateWindowSize, searchWindowSize);
}

void cudaNonLocalMeans(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, float h, int searchWindow, int blockSize, int borderMode, cv::cuda::Stream* stream)
{
   cv::cuda::nonLocalMeans(*src, *dst, h, searchWindow, blockSize, borderMode, stream ? *stream : cv::cuda::Stream::Null());
}