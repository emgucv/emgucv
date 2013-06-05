//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_OCL_C_H
#define EMGU_OCL_C_H

#include "opencv2/ocl.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"

namespace emgu
{
   struct size
   {
      int width;
      int height;
   };
}

CVAPI(int) oclGetDevice(int devicetype);

//----------------------------------------------------------------------------
//
//  OclMat
//
//----------------------------------------------------------------------------

CVAPI(cv::ocl::oclMat*) oclMatCreateDefault();

CVAPI(cv::ocl::oclMat*) oclMatCreate(int rows, int cols, int type);

CVAPI(emgu::size) oclMatGetSize(cv::ocl::oclMat* oclMat);

CVAPI(bool) oclMatIsEmpty(cv::ocl::oclMat* oclMat);

CVAPI(bool) oclMatIsContinuous(cv::ocl::oclMat* oclMat);

CVAPI(int) oclMatGetChannels(cv::ocl::oclMat* oclMat);

CVAPI(void) oclMatRelease(cv::ocl::oclMat** mat);

//Pefroms blocking upload data to oclMat.
CVAPI(void) oclMatUpload(cv::ocl::oclMat* oclMat, CvArr* arr);

//Downloads data from device to host memory. Blocking calls.
CVAPI(void) oclMatDownload(cv::ocl::oclMat* oclMat, CvArr* arr);

CVAPI(int) oclCountNonZero(cv::ocl::oclMat* oclMat);

CVAPI(void) oclMatAdd(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatAddS(const cv::ocl::oclMat* a, const CvScalar scale, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatSubtract(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatSubtractS(const cv::ocl::oclMat* a, const CvScalar scale, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatMultiply(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, double scale);

CVAPI(void) oclMatDivide(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, double scale);

CVAPI(void) oclMatFlip(const cv::ocl::oclMat* a, cv::ocl::oclMat* b, int flipCode);

CVAPI(void) oclMatBitwiseNot(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst);

CVAPI(void) oclMatBitwiseAnd(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatBitwiseAndS(const cv::ocl::oclMat* src1, const cv::Scalar sc, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatBitwiseOr(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatBitwiseOrS(const cv::ocl::oclMat* src1, const cv::Scalar sc, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatBitwiseXor(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatBitwiseXorS(const cv::ocl::oclMat* src1, const cv::Scalar sc, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatErode( const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const CvArr* kernel, CvPoint anchor, int iterations, int borderType, cv::Scalar borderValue);

CVAPI(void) oclMatDilate( const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const CvArr* kernel, CvPoint anchor, int iterations, int borderType, cv::Scalar borderValue);

CVAPI(void) oclMatMorphologyEx( const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int op, const CvArr* kernel, CvPoint anchor, int iterations);

CVAPI(void) oclMatCvtColor(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int code);

CVAPI(void) oclMatCopy(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatResize(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, double fx, double fy, int interpolation);

//only support single channel gpuMat
CVAPI(void) oclMatMinMaxLoc(const cv::ocl::oclMat* src, 
   double* minVal, double* maxVal, 
   CvPoint* minLoc, CvPoint* maxLoc, 
   const cv::ocl::oclMat* mask);

CVAPI(void) oclMatSplit(const cv::ocl::oclMat* src, cv::ocl::oclMat** dst);

CVAPI(void) oclMatConvertTo(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, double alpha, double beta);

#endif