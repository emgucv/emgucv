//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_PHOTO_C_H
#define EMGU_PHOTO_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/photo/photo.hpp"

#include "opencv2/photo/cuda.hpp"

CVAPI(void) cveInpaint( cv::_InputArray* src, cv::_InputArray* inpaintMask, cv::_OutputArray* dst, double inpaintRadius, int flags );

CVAPI(void) cveFastNlMeansDenoising(cv::_InputArray* src, cv::_OutputArray* dst, float h, int templateWindowSize, int searchWindowSize);

CVAPI(void) cveFastNlMeansDenoisingColored(cv::_InputArray* src, cv::_OutputArray* dst, float h, float hColor, int templateWindowSize, int searchWindowSize);

CVAPI(void) cudaNonLocalMeans(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, float h, int searchWindow, int blockSize, int borderMode, cv::cuda::Stream* stream);

CVAPI(void) cveEdgePreservingFilter(cv::_InputArray* src, cv::_OutputArray* dst, int flags, float sigmaS, float sigmaR);

CVAPI(void) cveDetailEnhance(cv::_InputArray* src, cv::_OutputArray* dst, float sigmaS, float sigmaR);

CVAPI(void) cvePencilSketch(cv::_InputArray* src, cv::_OutputArray* dst1, cv::_OutputArray* dst2, float sigmaS, float sigmaR, float shadeFactor);

CVAPI(void) cveStylization(cv::_InputArray* src, cv::_OutputArray* dst, float sigmaS, float sigmaR);

CVAPI(void) cveColorChange(cv::_InputArray* src, cv::_InputArray* mask, cv::_OutputArray* dst, float redMul, float greenMul, float blueMul);

CVAPI(void) cveIlluminationChange(cv::_InputArray* src, cv::_InputArray* mask, cv::_OutputArray* dst, float alpha, float beta);

CVAPI(void) cveTextureFlattening(cv::_InputArray* src, cv::_InputArray* mask, cv::_OutputArray* dst, float lowThreshold, float highThreshold, int kernelSize);

CVAPI(void) cveDecolor(cv::_InputArray* src, cv::_OutputArray* grayscale, cv::_OutputArray* colorBoost);

CVAPI(void) cveSeamlessClone(cv::_InputArray* src, cv::_InputArray* dst, cv::_InputArray* mask, CvPoint* p, cv::_OutputArray* blend, int flags);

CVAPI(void) cveDenoiseTVL1(const std::vector< cv::Mat >* observations, cv::Mat* result, double lambda, int niters);

///CalibrateCRF
CVAPI(void) cveCalibrateCRFProcess(cv::CalibrateCRF* calibrateCRF, cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* times);

CVAPI(cv::CalibrateDebevec*) cveCalibrateDebevecCreate(int samples, float lambda, bool random, cv::CalibrateCRF** calibrateCRF, cv::Ptr<cv::CalibrateDebevec>** sharedPtr);
CVAPI(void) cveCalibrateDebevecRelease(cv::CalibrateDebevec** calibrateDebevec, cv::Ptr<cv::CalibrateDebevec>** sharedPtr);

CVAPI(cv::CalibrateRobertson*) cveCalibrateRobertsonCreate(int maxIter, float threshold, cv::CalibrateCRF** calibrateCRF, cv::Ptr<cv::CalibrateRobertson>** sharedPtr);
CVAPI(void) cveCalibrateRobertsonRelease(cv::CalibrateRobertson** calibrateRobertson, cv::Ptr<cv::CalibrateRobertson>** sharedPtr);

//MergeExposures
CVAPI(void) cveMergeExposuresProcess(
   cv::MergeExposures* mergeExposures, 
   cv::_InputArray* src, cv::_OutputArray* dst,
   cv::_InputArray* times, cv::_InputArray* response);

CVAPI(cv::MergeDebevec*) cveMergeDebevecCreate(cv::MergeExposures** merge, cv::Ptr<cv::MergeDebevec>** sharedPtr);
CVAPI(void) cveMergeDebevecRelease(cv::MergeDebevec** merge, cv::Ptr<cv::MergeDebevec>** sharedPtr);

CVAPI(cv::MergeMertens*) cveMergeMertensCreate(float contrastWeight, float saturationWeight, float exposureWeight, cv::MergeExposures** merge, cv::Ptr<cv::MergeMertens>** sharedPtr);
CVAPI(void) cveMergeMertensRelease(cv::MergeMertens** merge, cv::Ptr<cv::MergeMertens>** sharedPtr);

CVAPI(cv::MergeRobertson*) cveMergeRobertsonCreate(cv::MergeExposures** merge, cv::Ptr<cv::MergeRobertson>** sharedPtr);
CVAPI(void) cveMergeRobertsonRelease(cv::MergeRobertson** merge, cv::Ptr<cv::MergeRobertson>** sharedPtr);

//Tonemap
CVAPI(void) cveTonemapProcess(cv::Tonemap* tonemap, cv::_InputArray* src, cv::_OutputArray* dst);
CVAPI(cv::Tonemap*) cveTonemapCreate(float gamma, cv::Algorithm** algorithm, cv::Ptr<cv::Tonemap>** sharedPtr);
CVAPI(void) cveTonemapRelease(cv::Tonemap** tonemap, cv::Ptr<cv::Tonemap>** sharedPtr);

CVAPI(cv::TonemapDrago*) cveTonemapDragoCreate(float gamma, float saturation, float bias, cv::Tonemap** tonemap, cv::Algorithm** algorithm, cv::Ptr<cv::TonemapDrago>** sharedPtr);
CVAPI(void) cveTonemapDragoRelease(cv::TonemapDrago** tonemap, cv::Ptr<cv::TonemapDrago>** sharedPtr);

CVAPI(cv::TonemapReinhard*) cveTonemapReinhardCreate(float gamma, float intensity, float lightAdapt, float colorAdapt, cv::Tonemap** tonemap, cv::Algorithm** algorithm, cv::Ptr<cv::TonemapReinhard>** sharedPtr);
CVAPI(void) cveTonemapReinhardRelease(cv::TonemapReinhard** tonemap, cv::Ptr<cv::TonemapReinhard>** sharedPtr);

CVAPI(cv::TonemapMantiuk*) cveTonemapMantiukCreate(float gamma, float scale, float saturation, cv::Tonemap** tonemap, cv::Algorithm** algorithm, cv::Ptr<cv::TonemapMantiuk>** sharedPtr);
CVAPI(void) cveTonemapMantiukRelease(cv::TonemapMantiuk** tonemap, cv::Ptr<cv::TonemapMantiuk>** sharedPtr);

//AlignExposures
CVAPI(void) cveAlignExposuresProcess(cv::AlignExposures* alignExposures, cv::_InputArray* src, std::vector<cv::Mat>* dst, cv::_InputArray* times, cv::_InputArray* response);

CVAPI(cv::AlignMTB*) cveAlignMTBCreate(int maxBits, int excludeRange, bool cut, cv::AlignExposures** alignExposures, cv::Ptr<cv::AlignMTB>** sharedPtr);
CVAPI(void) cveAlignMTBRelease(cv::AlignMTB** alignExposures, cv::Ptr<cv::AlignMTB>** sharedPtr);
#endif