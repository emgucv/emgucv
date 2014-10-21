//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CORE_C_H
#define EMGU_CORE_C_H

#include "opencv2/core/core_c.h"
//#include "opencv2/core/ocl.hpp"
//#include "opencv2/imgproc/imgproc.hpp"
#include "emgu_c.h"

CVAPI(cv::String*) cveStringCreate();
CVAPI(cv::String*) cveStringCreateFromStr(const char* c);
CVAPI(void) cveStringGetCStr(cv::String* string, const char** c, int* size);
CVAPI(void) cveStringRelease(cv::String** string);

CVAPI(cv::_InputArray*) cveInputArrayFromDouble(double* scalar);
CVAPI(cv::_InputArray*) cveInputArrayFromScalar(cv::Scalar* scalar);
CVAPI(cv::_InputArray*) cveInputArrayFromMat(cv::Mat* mat);
CVAPI(cv::_InputArray*) cveInputArrayFromGpuMat(cv::cuda::GpuMat* mat);
CVAPI(cv::_InputArray*) cveInputArrayFromUMat(cv::UMat* mat);
CVAPI(void) cveInputArrayGetSize(cv::_InputArray* ia, CvSize* size, int idx);
CVAPI(int) cveInputArrayGetDepth(cv::_InputArray* ia, int idx);
CVAPI(int) cveInputArrayGetChannels(cv::_InputArray* ia, int idx);
CVAPI(bool) cveInputArrayIsEmpty(cv::_InputArray* ia);
CVAPI(void) cveInputArrayRelease(cv::_InputArray** arr);

CVAPI(void) cveInputArrayGetMat(cv::_InputArray* ia, int idx, cv::Mat* mat);
CVAPI(void) cveInputArrayGetUMat(cv::_InputArray* ia, int idx, cv::UMat* umat);

CVAPI(cv::_OutputArray*) cveOutputArrayFromMat(cv::Mat* mat);
CVAPI(cv::_OutputArray*) cveOutputArrayFromGpuMat(cv::cuda::GpuMat* mat);
CVAPI(cv::_OutputArray*) cveOutputArrayFromUMat(cv::UMat* mat);
CVAPI(void) cveOutputArrayRelease(cv::_OutputArray** arr);

CVAPI(cv::_InputOutputArray*) cveInputOutputArrayFromMat(cv::Mat* mat);
CVAPI(cv::_InputOutputArray*) cveInputOutputArrayFromUMat(cv::UMat* mat);
CVAPI(cv::_InputOutputArray*) cveInputOutputArrayFromGpuMat(cv::cuda::GpuMat* mat);
CVAPI(void) cveInputOutputArrayRelease(cv::_InputOutputArray** arr);

CVAPI(cv::Scalar*) cveScalarCreate(CvScalar* scalar);
CVAPI(void) cveScalarRelease(cv::Scalar** scalar);

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
CVAPI(void) cveFindNonZero(cv::_InputArray* src, cv::_OutputArray* idx );
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
CVAPI(void) cveMeanStdDev(cv::_InputArray* src, cv::_OutputArray* mean, cv::_OutputArray* stddev, cv::_InputArray* mask);
CVAPI(void) cveTrace(cv::_InputArray* mtx, CvScalar* result);
CVAPI(double) cveDeterminant(cv::_InputArray* mtx);
CVAPI(double) cveNorm(cv::_InputArray* src1, cv::_InputArray* src2, int normType, cv::_InputArray* mask);

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
CVAPI(int) cveSolveCubic(cv::_InputArray* coeffs, cv::_OutputArray* roots);
CVAPI(double) cveSolvePoly(cv::_InputArray* coeffs, cv::_OutputArray* roots, int maxIters);
CVAPI(void) cveSolve(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int flags);
CVAPI(void) cveInvert(cv::_InputArray* src, cv::_OutputArray* dst, int flags);

CVAPI(void) cveDft(cv::_InputArray* src, cv::_OutputArray* dst, int flags, int nonzeroRows);
CVAPI(void) cveDct(cv::_InputArray* src, cv::_OutputArray* dst, int flags);
CVAPI(void) cveMulSpectrums(cv::_InputArray *a, cv::_InputArray* b, cv::_OutputArray* c, int flags, bool conjB);
CVAPI(int) cveGetOptimalDFTSize(int vecsize);

CVAPI(void) cveTransform(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* m);

CVAPI(void) cveMahalanobis(cv::_InputArray* v1, cv::_InputArray* v2, cv::_InputArray* icovar);
CVAPI(void) cveCalcCovarMatrix(cv::_InputArray* samples, cv::_OutputArray* covar, cv::_InputOutputArray* mean, int flags, int ctype);
CVAPI(void) cveNormalize(cv::_InputArray* src, cv::_InputOutputArray* dst, double alpha, double beta, int normType, int dType, cv::_InputArray* mask);

CVAPI(void) cvePerspectiveTransform(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* m);

CVAPI(void) cveMulTransposed(cv::_InputArray* src, cv::_OutputArray* dst, bool aTa, cv::_InputArray* delta, double scale, int dtype);

CVAPI(void) cveSplit(cv::_InputArray* src, cv::_OutputArray* mv);
CVAPI(void) cveMerge(cv::_InputArray* mv, cv::_OutputArray* dst);
CVAPI(void) cveMixChannels(cv::_InputArray* src, cv::_InputOutputArray* dst, const int* fromTo, int npairs);

CVAPI(void) cveExtractChannel(cv::_InputArray* src, cv::_OutputArray* dst, int coi);
CVAPI(void) cveInsertChannel(cv::_InputArray* src, cv::_InputOutputArray* dst, int coi);

CVAPI(double) cveKmeans(cv::_InputArray* data, int k, cv::_InputOutputArray* bestLabels, CvTermCriteria* criteria, int attempts, int flags, cv::_OutputArray* centers);

CVAPI(void) cveHConcat(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst);
CVAPI(void) cveVConcat(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst);

CVAPI(double) cvePSNR(cv::_InputArray* src1, cv::_InputArray* src2);

CVAPI(bool) cveEigen(cv::_InputArray* src, cv::_OutputArray* eigenValues, cv::_OutputArray* eigenVectors);

//Algorithm 
CVAPI(int) cveAlgorithmGetInt(cv::Algorithm* algorithm, cv::String* name);
CVAPI(void) cveAlgorithmSetInt(cv::Algorithm* algorithm, cv::String* name, int value);

CVAPI(double) cveAlgorithmGetDouble(cv::Algorithm* algorithm, cv::String* name);
CVAPI(void) cveAlgorithmSetDouble(cv::Algorithm* algorithm, cv::String* name, double value);

CVAPI(void) cveAlgorithmGetString(cv::Algorithm* algorithm, cv::String* name, cv::String* result);
CVAPI(void) cveAlgorithmSetString(cv::Algorithm* algorithm, cv::String* name, cv::String* value);

CVAPI(void) cveAlgorithmGetParams(cv::Algorithm* algorithm, std::vector<cv::String>* names, std::vector< int >* types, std::vector<cv::String>* help);

CVAPI(void) cveAlgorithmGetList(std::vector< cv::String >* names);

CVAPI(bool) cveClipLine(CvRect* rect, CvPoint* pt1, CvPoint* pt2);

CVAPI(void) cveRandn(cv::_InputOutputArray* dst, cv::_InputArray* mean, cv::_InputArray* stddev);

CVAPI(void) cveRandu(cv::_InputOutputArray* dst, cv::_InputArray* low, cv::_InputArray* high);
#endif
