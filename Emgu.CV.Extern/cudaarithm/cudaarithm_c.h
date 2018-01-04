//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
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


CVAPI(void) cudaMeanStdDev(cv::_InputArray* mtx, CvScalar* mean, CvScalar* stddev);

CVAPI(double) cudaNorm1(cv::_InputArray* src1, int normType, cv::_InputArray* mask);
CVAPI(double) cudaNorm2(cv::_InputArray* src1, cv::_InputArray* src2, int normType);
CVAPI(void) cudaCalcNorm(cv::_InputArray* src, cv::_OutputArray* dst,  int normType, cv::_InputArray* mask, cv::cuda::Stream* stream);
CVAPI(void) cudaCalcNormDiff(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int normType, cv::cuda::Stream* stream);

CVAPI(void) cudaAbsSum(cv::_InputArray* src, CvScalar* sum, cv::_InputArray* mask);
CVAPI(void) cudaCalcAbsSum(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream);
CVAPI(void) cudaSqrSum(cv::_InputArray* src, CvScalar* sqrSum, cv::_InputArray* mask);
CVAPI(void) cudaCalcSqrSum(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream);


//only support single channel gpuMat
CVAPI(void) cudaMinMaxLoc(cv::_InputArray* src, double* minVal, double* maxVal, CvPoint* minLoc, CvPoint* maxLoc, cv::_InputArray* mask);
CVAPI(void) cudaFindMinMaxLoc(cv::_InputArray* src, cv::_OutputArray* minMaxVals, cv::_OutputArray* loc, cv::_InputArray* mask, cv::cuda::Stream* stream);

CVAPI(int) cudaCountNonZero1(cv::_InputArray* src);
CVAPI(void) cudaCountNonZero2(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaReduce(cv::_InputArray* mtx, cv::_OutputArray* vec, int dim, int reduceOp, int dType, cv::cuda::Stream* stream);

CVAPI(void) cudaBitwiseNot(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaBitwiseAnd(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaBitwiseOr(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaBitwiseXor(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream);

CVAPI(void) cudaMin(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaMax(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaGemm(cv::_InputArray* src1, cv::_InputArray* src2, double alpha,
   cv::_InputArray* src3, double beta, cv::_OutputArray* dst, int flags, cv::cuda::Stream* stream);

CVAPI(void) cudaLShift(cv::_InputArray* a, CvScalar* scale, cv::_OutputArray* c, cv::cuda::Stream* stream);

CVAPI(void) cudaRShift(cv::_InputArray* a, CvScalar* scale, cv::_OutputArray* c, cv::cuda::Stream* stream);

CVAPI(void) cudaAdd(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, cv::_InputArray* mask, int dtype, cv::cuda::Stream* stream);

CVAPI(void) cudaSubtract(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, cv::_InputArray* mask, int dtype, cv::cuda::Stream* stream);

CVAPI(void) cudaMultiply(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, double scale, int dtype, cv::cuda::Stream* stream);

CVAPI(void) cudaDivide(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, double scale, int dtype, cv::cuda::Stream* stream);

CVAPI(void) cudaAddWeighted(cv::_InputArray* src1, double alpha, cv::_InputArray* src2, double beta, double gamma, cv::_OutputArray* dst, int dtype, cv::cuda::Stream* stream);

CVAPI(void) cudaAbsdiff(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, cv::cuda::Stream* stream);

CVAPI(void) cudaAbs(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaSqr(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaSqrt(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaCompare(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, int cmpop, cv::cuda::Stream* stream);

CVAPI(double) cudaThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double thresh, double maxval, int type, cv::cuda::Stream* stream);

CVAPI(void) cudaCopyMakeBorder(cv::_InputArray* src, cv::_OutputArray* dst, int top, int bottom, int left, int right, int gpuBorderType, const CvScalar* value, cv::cuda::Stream* stream);

CVAPI(void) cudaIntegral(cv::_InputArray* src, cv::_OutputArray* sum, cv::cuda::Stream* stream);

CVAPI(void) cudaSqrIntegral(cv::_InputArray* src, cv::_OutputArray* sqrSum, cv::cuda::Stream* stream);

CVAPI(void) cudaDft(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* dftSize, int flags, cv::cuda::Stream* stream);

CVAPI(void) cudaMulAndScaleSpectrums(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int flags, float scale, bool conjB, cv::cuda::Stream* stream);

CVAPI(void) cudaMulSpectrums(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int flags, bool conjB, cv::cuda::Stream* stream);

CVAPI(void) cudaFlip(cv::_InputArray* src, cv::_OutputArray* dst, int flipcode, cv::cuda::Stream* stream);

CVAPI(void) cudaSplit(cv::_InputArray* src, std::vector< cv::cuda::GpuMat >* dst, cv::cuda::Stream* stream);

CVAPI(cv::cuda::LookUpTable*) cudaLookUpTableCreate( cv::_InputArray* lut );

CVAPI(void) cudaLookUpTableTransform(cv::cuda::LookUpTable* lut, cv::_InputArray* image, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaLookUpTableRelease(cv::cuda::LookUpTable** lut);

CVAPI(void) cudaTranspose(cv::_InputArray* src1, cv::_OutputArray* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaNormalize(cv::_InputArray* src, cv::_OutputArray* dst, double alpha, double beta,
   int norm_type, int dtype, cv::_InputArray* mask, cv::cuda::Stream* stream);
#endif