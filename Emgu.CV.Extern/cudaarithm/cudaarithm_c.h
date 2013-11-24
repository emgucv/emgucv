//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDAARITHM_C_H
#define EMGU_CUDAARITHM_C_H

#include "opencv2/cuda.hpp"
#include "opencv2/cudaarithm.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "emgu_c.h"

CVAPI(void) cudaExp(const cv::cuda::GpuMat* a, cv::cuda::GpuMat* b, cv::cuda::Stream* stream);

CVAPI(void) cudaPow(const cv::cuda::GpuMat* src, double power, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaLog(const cv::cuda::GpuMat* a, cv::cuda::GpuMat* b, cv::cuda::Stream* stream);

CVAPI(void) cudaMagnitude(const cv::cuda::GpuMat* x, const cv::cuda::GpuMat* y, cv::cuda::GpuMat* magnitude, cv::cuda::Stream* stream);

CVAPI(void) cudaMagnitudeSqr(const cv::cuda::GpuMat* x, const cv::cuda::GpuMat* y, cv::cuda::GpuMat* magnitude, cv::cuda::Stream* stream);

CVAPI(void) cudaPhase(const cv::cuda::GpuMat* x, const cv::cuda::GpuMat* y, cv::cuda::GpuMat* angle, bool angleInDegrees, cv::cuda::Stream* stream);

CVAPI(void) cudaCartToPolar(const cv::cuda::GpuMat* x, const cv::cuda::GpuMat* y, cv::cuda::GpuMat* magnitude, cv::cuda::GpuMat* angle, bool angleInDegrees, cv::cuda::Stream* stream);

CVAPI(void) cudaPolarToCart(const cv::cuda::GpuMat* magnitude, const cv::cuda::GpuMat* angle, cv::cuda::GpuMat* x, cv::cuda::GpuMat* y, bool angleInDegrees, cv::cuda::Stream* stream);
CVAPI(void) cudaMerge(const cv::cuda::GpuMat** src, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream);

//only support single channel gpuMat
CVAPI(void) cudaMinMaxLoc(const cv::cuda::GpuMat* src, 
   double* minVal, double* maxVal, 
   CvPoint* minLoc, CvPoint* maxLoc, 
   const cv::cuda::GpuMat* mask);

CVAPI(void) cudaMeanStdDev(const cv::cuda::GpuMat* mtx, CvScalar* mean, CvScalar* stddev, cv::cuda::GpuMat* buffer);

CVAPI(double) cudaNorm(const cv::cuda::GpuMat* src1, const cv::cuda::GpuMat* src2, int normType);

CVAPI(int) cudaCountNonZero(const cv::cuda::GpuMat* src);

CVAPI(void) cudaReduce(const cv::cuda::GpuMat* mtx, cv::cuda::GpuMat* vec, int dim, int reduceOp, cv::cuda::Stream* stream);

CVAPI(void) cudaBitwiseNot(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaBitwiseAnd(const cv::cuda::GpuMat* src1, const cv::cuda::GpuMat* src2, cv::cuda::GpuMat* dst, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaBitwiseAndS(const cv::cuda::GpuMat* src1, const CvScalar* sc, cv::cuda::GpuMat* dst,  const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaBitwiseOr(const cv::cuda::GpuMat* src1, const cv::cuda::GpuMat* src2, cv::cuda::GpuMat* dst, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaBitwiseOrS(const cv::cuda::GpuMat* src1, const CvScalar* sc, cv::cuda::GpuMat* dst, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaBitwiseXor(const cv::cuda::GpuMat* src1, const cv::cuda::GpuMat* src2, cv::cuda::GpuMat* dst, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaBitwiseXorS(const cv::cuda::GpuMat* src1, const CvScalar* sc, cv::cuda::GpuMat* dst, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaMin(const cv::cuda::GpuMat* src1, const cv::cuda::GpuMat* src2, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaMinS(const cv::cuda::GpuMat* src1, double src2, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaMax(const cv::cuda::GpuMat* src1, const cv::cuda::GpuMat* src2, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaMaxS(const cv::cuda::GpuMat* src1, double src2, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaGemm(const cv::cuda::GpuMat* src1, const cv::cuda::GpuMat* src2, double alpha, 
   const cv::cuda::GpuMat* src3, double beta, cv::cuda::GpuMat* dst, int flags, cv::cuda::Stream* stream);


CVAPI(void) cudaLShift(const cv::cuda::GpuMat* a, CvScalar* scale, cv::cuda::GpuMat* c, cv::cuda::Stream* stream);

CVAPI(void) cudaRShift(const cv::cuda::GpuMat* a, CvScalar* scale, cv::cuda::GpuMat* c, cv::cuda::Stream* stream);

CVAPI(void) cudaAdd(const cv::cuda::GpuMat* a, const cv::cuda::GpuMat* b, cv::cuda::GpuMat* c, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaAddS(const cv::cuda::GpuMat* a, const CvScalar* scale, cv::cuda::GpuMat* c, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaSubtract(const cv::cuda::GpuMat* a, const cv::cuda::GpuMat* b, cv::cuda::GpuMat* c, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaSubtractS(const cv::cuda::GpuMat* a, const CvScalar* scale, cv::cuda::GpuMat* c, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaMultiply(const cv::cuda::GpuMat* a, const cv::cuda::GpuMat* b, cv::cuda::GpuMat* c, double scale, cv::cuda::Stream* stream);

CVAPI(void) cudaMultiplyS(const cv::cuda::GpuMat* a, const CvScalar* s, cv::cuda::GpuMat* c, cv::cuda::Stream* stream);

CVAPI(void) cudaDivide(const cv::cuda::GpuMat* a, const cv::cuda::GpuMat* b, cv::cuda::GpuMat* c, double scale, cv::cuda::Stream* stream);

CVAPI(void) cudaDivideSR(const cv::cuda::GpuMat* a, const CvScalar* s, cv::cuda::GpuMat* c, cv::cuda::Stream* stream);

CVAPI(void) cudaDivideSL(const double s, const cv::cuda::GpuMat* b, cv::cuda::GpuMat* c, cv::cuda::Stream* stream);

CVAPI(void) cudaAddWeighted(const cv::cuda::GpuMat* src1, double alpha, const cv::cuda::GpuMat* src2, double beta, double gamma, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaAbsdiff(const cv::cuda::GpuMat* a, const cv::cuda::GpuMat* b, cv::cuda::GpuMat* c, cv::cuda::Stream* stream);

CVAPI(void) cudaAbs(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaSqr(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaSqrt(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaAbsdiffS(const cv::cuda::GpuMat* a, const CvScalar s, cv::cuda::GpuMat* c, cv::cuda::Stream* stream);

CVAPI(void) cudaCompare(const cv::cuda::GpuMat* a, const cv::cuda::GpuMat* b, cv::cuda::GpuMat* c, int cmpop, cv::cuda::Stream* stream);

CVAPI(double) cudaThreshold(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, double thresh, double maxval, int type, cv::cuda::Stream* stream);

CVAPI(void) cudaCopyMakeBorder(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int top, int bottom, int left, int right, int gpuBorderType, const CvScalar* value, cv::cuda::Stream* stream);

CVAPI(void) cudaIntegral(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* sum, cv::cuda::GpuMat* buffer, cv::cuda::Stream* stream);

CVAPI(void) cudaDft(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int flags, cv::cuda::Stream* stream);

CVAPI(void) cudaFlip(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int flipcode, cv::cuda::Stream* stream);

CVAPI(void) cudaSplit(const cv::cuda::GpuMat* src, cv::cuda::GpuMat** dst, cv::cuda::Stream* stream);

CVAPI(cv::cuda::LookUpTable*) cudaLookUpTableCreate( const CvArr* lut );

CVAPI(void) cudaLookUpTableTransform(cv::cuda::LookUpTable* lut, cv::cuda::GpuMat* image, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaLookUpTableRelease(cv::cuda::LookUpTable** lut);
#endif