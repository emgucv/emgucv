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

void cveEdgePreservingFilter(cv::_InputArray* src, cv::_OutputArray* dst, int flags, float sigmaS, float sigmaR)
{
   cv::edgePreservingFilter(*src, *dst, flags, sigmaS, sigmaR);
}

void cveDetailEnhance(cv::_InputArray* src, cv::_OutputArray* dst, float sigmaS, float sigmaR)
{
   cv::detailEnhance(*src, *dst, sigmaS, sigmaR);
}

void cvePencilSketch(cv::_InputArray* src, cv::_OutputArray* dst1, cv::_OutputArray* dst2, float sigmaS, float sigmaR, float shadeFactor)
{
   cv::pencilSketch(*src, *dst1, *dst2, sigmaS, sigmaR, shadeFactor);
}

void cveStylization(cv::_InputArray* src, cv::_OutputArray* dst, float sigmaS, float sigmaR)
{
   cv::stylization(*src, *dst, sigmaS, sigmaR);
}

void cveColorChange(cv::_InputArray* src, cv::_InputArray* mask, cv::_OutputArray* dst, float redMul, float greenMul, float blueMul)
{
   cv::colorChange(*src, mask ? *mask : (cv::InputArray) cv::noArray(), dst ? *dst : (cv::OutputArray) cv::noArray(), redMul, greenMul, blueMul);
}

void cveIlluminationChange(cv::_InputArray* src, cv::_InputArray* mask, cv::_OutputArray* dst, float alpha, float beta)
{
   cv::illuminationChange(*src, mask ? *mask : (cv::InputArray) cv::noArray(), *dst, alpha, beta);
}

void cveTextureFlattening(cv::_InputArray* src, cv::_InputArray* mask, cv::_OutputArray* dst, double lowThreshold, double highThreshold, int kernelSize)
{
   cv::textureFlattening(*src, mask ? *mask : (cv::InputArray) cv::noArray(), *dst, lowThreshold, highThreshold, kernelSize);
}