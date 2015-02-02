//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDAARITHM_C_H
#define EMGU_CUDAARITHM_C_H

//#include "opencv2/cuda.hpp"
#include "opencv2/cudaarithm.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "emgu_c.h"

CVAPI(void) cudaExp(cv::_InputArray* a, cv::_OutputArray* b, cv::cuda::Stream* stream);

CVAPI(void) cudaPow(cv::_InputArray* src, double power, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaLog(cv::_InputArray* a, cv::_OutputArray* b, cv::cuda::Stream* stream);

CVAPI(void) cudaMagnitude(cv::_InputArray* x, cv::_InputArray* y, cv::_OutputArray* magnitude, cv::cuda::Stream* stream);

CVAPI(void) cudaMagnitudeSqr(cv::_InputArray* x, cv::_InputArray* y, cv::_OutputArray* magnitude, cv::cuda::Stream* stream);

CVAPI(void) cudaPhase(cv::_InputArray* x, cv::_InputArray* y, cv::_OutputArray* angle, bool angleInDegrees, cv::cuda::Stream* stream);

CVAPI(void) cudaCartToPolar(cv::_InputArray* x, cv::_InputArray* y, cv::_OutputArray*, cv::_OutputArray*, bool angleInDegrees, cv::cuda::Stream* stream);

CVAPI(void) cudaPolarToCart(cv::_InputArray* magnitude, cv::_InputArray* angle, cv::_OutputArray* x, cv::_OutputArray* y, bool angleInDegrees, cv::cuda::Stream* stream);

CVAPI(void) cudaMerge(std::vector< cv::cuda::GpuMat >* src, cv::_OutputArray* dst, cv::cuda::Stream* stream);

//only support single channel gpuMat
CVAPI(void) cudaMinMaxLoc(cv::_InputArray* src, double* minVal, double* maxVal, CvPoint* minLoc, CvPoint* maxLoc, cv::_InputArray* mask);

CVAPI(void) cudaMeanStdDev(cv::_InputArray* mtx, CvScalar* mean, CvScalar* stddev, cv::cuda::GpuMat* buffer);

CVAPI(double) cudaNorm(cv::_InputArray* src1, cv::_InputArray* src2, int normType);

CVAPI(int) cudaCountNonZero(cv::_InputArray* src);

CVAPI(void) cudaReduce(cv::_InputArray* mtx, cv::_OutputArray* vec, int dim, int reduceOp, int dType, cv::cuda::Stream* stream);

CVAPI(void) cudaBitwiseNot(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaBitwiseAnd(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaBitwiseOr(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaBitwiseXor(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaMin(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaMax(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaGemm(const cv::cuda::GpuMat* src1, const cv::cuda::GpuMat* src2, double alpha, 
   const cv::cuda::GpuMat* src3, double beta, cv::cuda::GpuMat* dst, int flags, cv::cuda::Stream* stream);

CVAPI(void) cudaLShift(const cv::cuda::GpuMat* a, CvScalar* scale, cv::cuda::GpuMat* c, cv::cuda::Stream* stream);

CVAPI(void) cudaRShift(const cv::cuda::GpuMat* a, CvScalar* scale, cv::cuda::GpuMat* c, cv::cuda::Stream* stream);

CVAPI(void) cudaAdd(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, cv::_InputArray* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaSubtract(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, cv::_InputArray* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaMultiply(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, double scale, cv::cuda::Stream* stream);

CVAPI(void) cudaDivide(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, double scale, cv::cuda::Stream* stream);

CVAPI(void) cudaAddWeighted(cv::_InputArray* src1, double alpha, cv::_InputArray* src2, double beta, double gamma, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaAbsdiff(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, cv::cuda::Stream* stream);

CVAPI(void) cudaAbs(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaSqr(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaSqrt(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaCompare(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, int cmpop, cv::cuda::Stream* stream);

CVAPI(double) cudaThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double thresh, double maxval, int type, cv::cuda::Stream* stream);

CVAPI(void) cudaCopyMakeBorder(cv::_InputArray* src, cv::_OutputArray* dst, int top, int bottom, int left, int right, int gpuBorderType, const CvScalar* value, cv::cuda::Stream* stream);

CVAPI(void) cudaIntegral(cv::_InputArray* src, cv::_OutputArray* sum, cv::cuda::GpuMat* buffer, cv::cuda::Stream* stream);

CVAPI(void) cudaSqrIntegral(cv::_InputArray* src, cv::_OutputArray* sqrSum, cv::cuda::GpuMat* buffer,  cv::cuda::Stream* stream);

CVAPI(void) cudaDft(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* dftSize, int flags, cv::cuda::Stream* stream);

CVAPI(void) cudaFlip(cv::_InputArray* src, cv::_OutputArray* dst, int flipcode, cv::cuda::Stream* stream);

CVAPI(void) cudaSplit(cv::_InputArray* src, std::vector< cv::cuda::GpuMat >* dst, cv::cuda::Stream* stream);

CVAPI(cv::cuda::LookUpTable*) cudaLookUpTableCreate( cv::_InputArray* lut );

CVAPI(void) cudaLookUpTableTransform(cv::cuda::LookUpTable* lut, cv::_InputArray* image, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaLookUpTableRelease(cv::cuda::LookUpTable** lut);
#endif