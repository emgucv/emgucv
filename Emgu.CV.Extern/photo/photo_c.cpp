//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
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

void cveTextureFlattening(cv::_InputArray* src, cv::_InputArray* mask, cv::_OutputArray* dst, float lowThreshold, float highThreshold, int kernelSize)
{
   cv::textureFlattening(*src, mask ? *mask : (cv::InputArray) cv::noArray(), *dst, lowThreshold, highThreshold, kernelSize);
}

void cveCalibrateCRFProcess(cv::CalibrateCRF* calibrateCRF, cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* times)
{
   calibrateCRF->process(*src, *dst, *times);
}

cv::CalibrateDebevec* cveCalibrateDebevecCreate(int samples, float lambda, bool random, cv::CalibrateCRF** calibrateCRF)
{
   cv::Ptr<cv::CalibrateDebevec> res = cv::createCalibrateDebevec(samples, lambda, random);
   res.addref();
   *calibrateCRF = dynamic_cast<cv::CalibrateCRF*>(res.get());
   return res.get();
}
void cveCalibrateDebevecRelease(cv::CalibrateDebevec** calibrateDebevec)
{
   delete *calibrateDebevec;
   *calibrateDebevec = 0;
}

cv::CalibrateRobertson* cveCalibrateRobertsonCreate(int maxIter, float threshold, cv::CalibrateCRF** calibrateCRF)
{
   cv::Ptr<cv::CalibrateRobertson> res = cv::createCalibrateRobertson(maxIter, threshold);
   res.addref();
   *calibrateCRF = dynamic_cast<cv::CalibrateCRF*>(res.get());
   return res.get();
}
void cveCalibrateRobertsonRelease(cv::CalibrateRobertson** calibrateRobertson)
{
   delete *calibrateRobertson;
   calibrateRobertson = 0;
}

void cveMergeExposuresProcess(
   cv::MergeExposures* mergeExposures, 
   cv::_InputArray* src, cv::_OutputArray* dst,
   cv::_InputArray* times, cv::_InputArray* response)
{
   mergeExposures->process(*src, *dst, *times, *response);
}

cv::MergeDebevec* cveMergeDebevecCreate(cv::MergeExposures** merge)
{
   cv::Ptr<cv::MergeDebevec> res = cv::createMergeDebevec();
   res.addref();
   *merge = dynamic_cast<cv::MergeExposures*>(res.get());
   return res.get();
}
void cveMergeDebevecRelease(cv::MergeDebevec** merge)
{
   delete *merge;
   *merge = 0;
}

cv::MergeMertens* cveMergeMertensCreate(float contrastWeight, float saturationWeight, float exposureWeight, cv::MergeExposures** merge)
{
   cv::Ptr<cv::MergeMertens> res = cv::createMergeMertens(contrastWeight, saturationWeight, exposureWeight);
   res.addref();
   *merge = dynamic_cast<cv::MergeExposures*>(res.get());
   return res.get();
}

void cveMergeMertensRelease(cv::MergeMertens** merge)
{
   delete *merge;
   *merge = 0;
}

cv::MergeRobertson* cveMergeRobertsonCreate(cv::MergeExposures** merge)
{
   cv::Ptr<cv::MergeRobertson> res = cv::createMergeRobertson();
   res.addref();
   *merge = dynamic_cast<cv::MergeExposures*>(res.get());
   return res.get();
}

void cveMergeRobertsonRelease(cv::MergeRobertson** merge)
{
   delete *merge;
   *merge = 0;
}

void cveDenoiseTVL1(const std::vector< cv::Mat >* observations, cv::Mat* result, double lambda, int niters)
{
   cv::denoise_TVL1(*observations, *result, lambda, niters);
}