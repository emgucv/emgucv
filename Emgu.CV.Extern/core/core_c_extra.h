//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CORE_C_H
#define EMGU_CORE_C_H

#include "opencv2/core/core_c.h"
//#include "opencv2/imgproc/imgproc.hpp"
#include "emgu_c.h"

CVAPI(cv::UMat*) cvUMatCreate();
CVAPI(cv::UMat*) cvUMatCreateWithType(int row, int cols, int type);
CVAPI(void) cvUMatRelease(cv::UMat** mat);
CVAPI(emgu::size) cvUMatGetSize(cv::UMat* mat);
CVAPI(void) cvUMatCopyTo(cv::UMat* mat, cv::_OutputArray* m);
CVAPI(int) cvUMatGetElementSize(cv::UMat* mat);
CVAPI(int) cvUMatGetChannels(cv::UMat* mat);
CVAPI(bool) cvUMatIsEmpty(cv::UMat* mat);
CVAPI(void) cvUMatSetTo(cv::UMat* mat, cv::_InputArray* value, cv::_InputArray* mask);

CVAPI(cv::Mat*) cvMatCreate();
CVAPI(cv::Mat*) cvMatCreateWithType(int row, int cols, int type);
CVAPI(cv::Mat*) cvMatCreateWithData(int rows, int cols, int type, void* data, size_t step);

CVAPI(void) cvMatRelease(cv::Mat** mat);
CVAPI(emgu::size) cvMatGetSize(cv::Mat* mat);
CVAPI(void) cvMatCopyTo(cv::Mat* mat, cv::_OutputArray* m, cv::_InputArray* mask);
CVAPI(void) cveArrToMat(CvArr* cvArray, cv::Mat* mat);
CVAPI(int) cvMatGetElementSize(cv::Mat* mat);
CVAPI(int) cvMatGetChannels(cv::Mat* mat);
CVAPI(uchar*) cvMatGetDataPointer(cv::Mat* mat);
CVAPI(size_t) cvMatGetStep(cv::Mat* mat);
CVAPI(bool) cvMatIsEmpty(cv::Mat* mat);
CVAPI(void) cvMatSetTo(cv::Mat* mat, cv::_InputArray* value, cv::_InputArray* mask);
CVAPI(cv::UMat*) cvMatGetUMat(cv::Mat* mat, int access);

CVAPI(cv::_InputArray*) cvInputArrayFromDouble(double* scalar);
CVAPI(cv::_InputArray*) cvInputArrayFromScalar(cv::Scalar* scalar);
CVAPI(cv::_InputArray*) cvInputArrayFromMat(cv::Mat* mat);
CVAPI(cv::_InputArray*) cvInputArrayFromGpuMat(cv::cuda::GpuMat* mat);
CVAPI(cv::_InputArray*) cvInputArrayFromUMat(cv::UMat* mat);
CVAPI(void) cvInputArrayRelease(cv::_InputArray** arr);

CVAPI(cv::_OutputArray*) cvOutputArrayFromMat(cv::Mat* mat);
CVAPI(cv::_OutputArray*) cvOutputArrayFromGpuMat(cv::cuda::GpuMat* mat);
CVAPI(cv::_OutputArray*) cvOutputArrayFromUMat(cv::UMat* mat);
CVAPI(void) cvOutputArrayRelease(cv::_OutputArray** arr);

CVAPI(cv::_InputOutputArray*) cvInputOutputArrayFromMat(cv::Mat* mat);
CVAPI(cv::_InputOutputArray*) cvInputOutputArrayFromUMat(cv::UMat* mat);
CVAPI(cv::_InputOutputArray*) cvInputOutputArrayFromGpuMat(cv::Mat* mat);
CVAPI(void) cvInputOutputArrayRelease(cv::_InputOutputArray** arr);

CVAPI(cv::Scalar*) cvScalarCreate(CvScalar* scalar);
CVAPI(void) cvScalarRelease(cv::Scalar** scalar);

CVAPI(void) cveMinMaxIdx(cv::_InputArray* src, double* minVal, double* maxVal, int* minIdx, int* maxIdx, cv::_InputArray* mask);
CVAPI(void) cveMinMaxLoc(cv::_InputArray* src, double* minVal, double* maxVal, CvPoint* minLoc, CvPoint* macLoc, cv::_InputArray *mask);

CVAPI(void) cveBitwiseAnd(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask);
CVAPI(void) cveBitwiseNot(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* mask);
CVAPI(void) cveBitwiseOr(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask);
CVAPI(void) cveBitwiseXor(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask);

CVAPI(void) cveAdd(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, int dtype);
CVAPI(void) cveSubtract(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, int dtype);
CVAPI(void) cveDivide(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, double scale, int dtype);
CVAPI(void) cveMultiply(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, double scale, int dtype);
CVAPI(void) cveCountNonZero(cv::_InputArray* src);
CVAPI(void) cveMin(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst);
CVAPI(void) cveMax(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst);

CVAPI(void) cveAbsDiff(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst);
CVAPI(void) cveInRange(cv::_InputArray* src1, cv::_InputArray* lowerb, cv::_InputArray* upperb, cv::_OutputArray* dst);

CVAPI(void) cveSqrt(cv::_InputArray* src, cv::_OutputArray* dst);

CVAPI(void) cveCompare(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int compop);

CVAPI(void) cveFlip(cv::_InputArray* src, cv::_OutputArray* dst, int flipCode);
CVAPI(void) cveTranspose(cv::_InputArray* src, cv::_OutputArray* dst);
CVAPI(void) cveLUT(cv::_InputArray* src, cv::_InputArray* lut, cv::_OutputArray* dst);
CVAPI(void) cveSum(cv::_InputArray* src, CvScalar* result);
CVAPI(void) cveMean(cv::_InputArray* src, cv::_InputArray* mask, CvScalar* result);
CVAPI(void) cveTrace(cv::_InputArray* mtx, CvScalar* result);
CVAPI(double) cveDeterminant(cv::_InputArray* mtx);

CVAPI(bool) cveCheckRange(cv::_InputArray* arr, bool quiet, CvPoint* index, double minVal, double maxVal);
CVAPI(void) cveGemm(cv::_InputArray* src1, cv::_InputArray* src2, double alpha, cv::_InputArray* src3, double beta, cv::_OutputArray* dst, int flags);
CVAPI(void) cveAddWeighted(cv::_InputArray* src1, double alpha, cv::_InputArray* src2, double beta, double gamma, cv::_OutputArray* dst, int dtype);
CVAPI(void) cveConvertScaleAbs(cv::_InputArray* src, cv::_OutputArray* dst, double alpha, double beta);
CVAPI(void) cveReduce(cv::_InputArray* src, cv::_OutputArray* dst, int dim, int rtype, int dtype);
CVAPI(void) cveRandShuffle(cv::_InputOutputArray* dst, double iterFactor, uint64 rng);

CVAPI(void) cvePow(cv::_InputArray* src, double power, cv::_OutputArray* dst);
CVAPI(void) cveExp(cv::_InputArray* src, cv::_OutputArray* dst);
CVAPI(void) cveLog(cv::_InputArray* src, cv::_OutputArray* dst);
CVAPI(void) cveCartToPolar(cv::_InputArray* x, cv::_InputArray* y, cv::_OutputArray* magnitude, cv::_OutputArray* angle, bool angleInDegrees);
CVAPI(void) cvePolarToCart(cv::_InputArray* magnitude, cv::_InputArray* angle, cv::_OutputArray* x, cv::_OutputArray* y, bool angleInDegrees);

CVAPI(void) cveSetIdentity(cv::_InputOutputArray* mtx, CvScalar* scalar);
CVAPI(void) cveSolve(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int flags);
CVAPI(void) cveInvert(cv::_InputArray* src, cv::_OutputArray* dst, int flags);

CVAPI(void) cveDft(cv::_InputArray* src, cv::_OutputArray* dst, int flags, int nonzeroRows);
CVAPI(void) cveDct(cv::_InputArray* src, cv::_OutputArray* dst, int flags);
CVAPI(void) cveMulSpectrums(cv::_InputArray *a, cv::_InputArray* b, cv::_OutputArray* c, int flags, bool conjB);

CVAPI(void) cveTransform(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* m);

CVAPI(void) cveMahalanobis(cv::_InputArray* v1, cv::_InputArray* v2, cv::_InputArray* icovar);

CVAPI(void) cveNormalize(cv::_InputArray* src, cv::_OutputArray* dst, double alpha, double beta, int normType, int dType, cv::_InputArray* mask);

CVAPI(void) cvePerspectiveTransform(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* m);

CVAPI(void) cveMulTransposed(cv::_InputArray* src, cv::_OutputArray* dst, bool aTa, cv::_InputArray* delta, double scale, int dtype);

CVAPI(void) cveSplit(cv::_InputArray* src, cv::_OutputArray* mv);
CVAPI(void) cveMerge(cv::_InputArray* mv, cv::_OutputArray* dst);
CVAPI(void) cveMixChannels(cv::_InputArray* src, cv::_InputOutputArray* dst, const int* fromTo, int npairs);
#endif