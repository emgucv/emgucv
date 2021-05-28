//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CORE_C_H
#define EMGU_CORE_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"
#include "opencv2/core.hpp"
#include "opencv2/core/cuda.hpp"
#include "emgu_c.h"
#include "opencv2/core/affine.hpp"
#include "opencv2/core/utils/logger.hpp"
#include "opencv2/core/utils/logger.hpp"
#include "registry_parallel.hpp"

CVAPI(CvErrorCallback) cveRedirectError(CvErrorCallback error_handler, void* userdata, void** prev_userdata);
CVAPI(int) cveGetErrMode();
CVAPI(int) cveSetErrMode(int mode);
CVAPI(int) cveGetErrStatus();
CVAPI(void) cveSetErrStatus(int status);
CVAPI(const char*) cveErrorStr(int status);

CVAPI(int) cveSetLogLevel(int logLevel);
CVAPI(int) cveGetLogLevel();

CVAPI(int) cveGetNumThreads();
CVAPI(void) cveSetNumThreads(int nthreads);
CVAPI(int) cveGetThreadNum();
CVAPI(int) cveGetNumberOfCPUs();

CVAPI(bool) cveSetParallelForBackend(cv::String* backendName, bool propagateNumThreads);
CVAPI(void) cveGetParallelBackends(std::vector< cv::String >* backendNames);


CVAPI(cv::String*) cveStringCreate();
CVAPI(cv::String*) cveStringCreateFromStr(const char* c);
CVAPI(void) cveStringGetCStr(cv::String* string, const char** c, int* size);
CVAPI(int) cveStringGetLength(cv::String* string);
CVAPI(void) cveStringRelease(cv::String** string);

CVAPI(cv::_InputArray*) cveInputArrayFromDouble(double* scalar);
CVAPI(cv::_InputArray*) cveInputArrayFromScalar(cv::Scalar* scalar);
CVAPI(cv::_InputArray*) cveInputArrayFromMat(cv::Mat* mat);
CVAPI(cv::_InputArray*) cveInputArrayFromGpuMat(cv::cuda::GpuMat* mat);
CVAPI(cv::_InputArray*) cveInputArrayFromUMat(cv::UMat* mat);
CVAPI(int) cveInputArrayGetDims(cv::_InputArray* ia, int i);
CVAPI(void) cveInputArrayGetSize(cv::_InputArray* ia, CvSize* size, int idx);
CVAPI(int) cveInputArrayGetDepth(cv::_InputArray* ia, int idx);
CVAPI(int) cveInputArrayGetChannels(cv::_InputArray* ia, int idx);
CVAPI(bool) cveInputArrayIsEmpty(cv::_InputArray* ia);
CVAPI(void) cveInputArrayRelease(cv::_InputArray** arr);

CVAPI(void) cveInputArrayGetMat(cv::_InputArray* ia, int idx, cv::Mat* mat);
CVAPI(void) cveInputArrayGetUMat(cv::_InputArray* ia, int idx, cv::UMat* umat);
CVAPI(void) cveInputArrayGetGpuMat(cv::_InputArray* ia, cv::cuda::GpuMat* gpuMat);
CVAPI(void) cveInputArrayCopyTo(cv::_InputArray* ia, cv::_OutputArray* arr, cv::_InputArray* mask);

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
CVAPI(void) cveMinMaxLoc(cv::_InputArray* src, double* minVal, double* maxVal, CvPoint* minLoc, CvPoint* macLoc, cv::_InputArray* mask);

CVAPI(void) cveBitwiseAnd(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask);
CVAPI(void) cveBitwiseNot(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* mask);
CVAPI(void) cveBitwiseOr(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask);
CVAPI(void) cveBitwiseXor(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask);

CVAPI(void) cveAdd(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, int dtype);
CVAPI(void) cveSubtract(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, int dtype);
CVAPI(void) cveDivide(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, double scale, int dtype);
CVAPI(void) cveMultiply(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, double scale, int dtype);
CVAPI(void) cveCountNonZero(cv::_InputArray* src);
CVAPI(void) cveFindNonZero(cv::_InputArray* src, cv::_OutputArray* idx);
CVAPI(void) cveMin(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst);
CVAPI(void) cveMax(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst);

CVAPI(void) cveAbsDiff(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst);
CVAPI(void) cveInRange(cv::_InputArray* src1, cv::_InputArray* lowerb, cv::_InputArray* upperb, cv::_OutputArray* dst);

CVAPI(void) cveSqrt(cv::_InputArray* src, cv::_OutputArray* dst);

CVAPI(void) cveCompare(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int compop);

CVAPI(void) cveFlip(cv::_InputArray* src, cv::_OutputArray* dst, int flipCode);
CVAPI(void) cveRotate(cv::_InputArray* src, cv::_OutputArray* dst, int rotateCode);
CVAPI(void) cveTranspose(cv::_InputArray* src, cv::_OutputArray* dst);
CVAPI(void) cveLUT(cv::_InputArray* src, cv::_InputArray* lut, cv::_OutputArray* dst);
CVAPI(void) cveSum(cv::_InputArray* src, CvScalar* result);
CVAPI(void) cveMean(cv::_InputArray* src, cv::_InputArray* mask, CvScalar* result);
CVAPI(void) cveMeanStdDev(cv::_InputArray* src, cv::_OutputArray* mean, cv::_OutputArray* stddev, cv::_InputArray* mask);
CVAPI(void) cveTrace(cv::_InputArray* mtx, CvScalar* result);
CVAPI(double) cveDeterminant(cv::_InputArray* mtx);
CVAPI(double) cveNorm(cv::_InputArray* src1, cv::_InputArray* src2, int normType, cv::_InputArray* mask);

CVAPI(bool) cveCheckRange(cv::_InputArray* arr, bool quiet, CvPoint* index, double minVal, double maxVal);
CVAPI(void) cvePatchNaNs(cv::_InputOutputArray* a, double val);
CVAPI(void) cveGemm(cv::_InputArray* src1, cv::_InputArray* src2, double alpha, cv::_InputArray* src3, double beta, cv::_OutputArray* dst, int flags);
CVAPI(void) cveScaleAdd(cv::_InputArray* src1, double alpha, cv::_InputArray* src2, cv::_OutputArray* dst);
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
CVAPI(void) cveSort(cv::_InputArray* src, cv::_OutputArray* dst, int flags);
CVAPI(void) cveSortIdx(cv::_InputArray* src, cv::_OutputArray* dst, int flags);
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
CVAPI(void) cveHConcat2(cv::_InputArray* src, cv::_OutputArray* dst);
CVAPI(void) cveVConcat2(cv::_InputArray* src, cv::_OutputArray* dst);


CVAPI(double) cvePSNR(cv::_InputArray* src1, cv::_InputArray* src2);

CVAPI(bool) cveEigen(cv::_InputArray* src, cv::_OutputArray* eigenValues, cv::_OutputArray* eigenVectors);

//Algorithm 
CVAPI(void) cveAlgorithmRead(cv::Algorithm* algorithm, cv::FileNode* node);
CVAPI(void) cveAlgorithmWrite(cv::Algorithm* algorithm, cv::FileStorage* storage);
CVAPI(void) cveAlgorithmWrite2(cv::Algorithm* algorithm, cv::FileStorage* storage, cv::String* name);
CVAPI(void) cveAlgorithmSave(cv::Algorithm* algorithm, cv::String* filename);
CVAPI(void) cveAlgorithmClear(cv::Algorithm* algorithm);
CVAPI(bool) cveAlgorithmEmpty(cv::Algorithm* algorithm);
CVAPI(void) cveAlgorithmGetDefaultName(cv::Algorithm* algorithm, cv::String* defaultName);

CVAPI(bool) cveClipLine(CvRect* rect, CvPoint* pt1, CvPoint* pt2);

CVAPI(void) cveRandn(cv::_InputOutputArray* dst, cv::_InputArray* mean, cv::_InputArray* stddev);

CVAPI(void) cveRandu(cv::_InputOutputArray* dst, cv::_InputArray* low, cv::_InputArray* high);

//File Storage
CVAPI(cv::FileStorage*) cveFileStorageCreate(const cv::String* source, int flags, const cv::String* encoding);
CVAPI(bool) cveFileStorageIsOpened(cv::FileStorage* storage);
CVAPI(void) cveFileStorageReleaseAndGetString(cv::FileStorage* storage, cv::String* result);
CVAPI(void) cveFileStorageRelease(cv::FileStorage** storage);
CVAPI(void) cveFileStorageWriteMat(cv::FileStorage* fs, cv::String* name, cv::Mat* value);
CVAPI(void) cveFileStorageWriteInt(cv::FileStorage* fs, cv::String* name, int value);
CVAPI(void) cveFileStorageWriteFloat(cv::FileStorage* fs, cv::String* name, float value);
CVAPI(void) cveFileStorageWriteDouble(cv::FileStorage* fs, cv::String* name, double value);
CVAPI(void) cveFileStorageWriteString(cv::FileStorage* fs, cv::String* name, cv::String* value);
CVAPI(void) cveFileStorageInsertString(cv::FileStorage* fs, cv::String* value);

CVAPI(cv::FileNode*) cveFileStorageRoot(cv::FileStorage* fs, int streamIdx);
CVAPI(cv::FileNode*) cveFileStorageGetFirstTopLevelNode(cv::FileStorage* fs);
CVAPI(cv::FileNode*) cveFileStorageGetNode(cv::FileStorage* fs, cv::String* nodeName);

CVAPI(int) cveFileNodeGetType(cv::FileNode* node);
CVAPI(void) cveFileNodeGetName(cv::FileNode* node, cv::String* name);
CVAPI(void) cveFileNodeGetKeys(cv::FileNode* node, std::vector< cv::String >* keys);
//CVAPI(bool) cveFileNodeIsEmpty(cv::FileNode* node);
CVAPI(void) cveFileNodeReadMat(cv::FileNode* node, cv::Mat* mat, cv::Mat* defaultMat);
CVAPI(void) cveFileNodeReadString(cv::FileNode* node, cv::String* str, cv::String* defaultStr);
CVAPI(int) cveFileNodeReadInt(cv::FileNode* node, int defaultInt);
CVAPI(double) cveFileNodeReadDouble(cv::FileNode* node, double defaultDouble);
CVAPI(float) cveFileNodeReadFloat(cv::FileNode* node, float defaultFloat);
CVAPI(void) cveFileNodeRelease(cv::FileNode** node);

CVAPI(cv::FileNodeIterator*) cveFileNodeIteratorCreate();
CVAPI(cv::FileNodeIterator*) cveFileNodeIteratorCreateFromNode(cv::FileNode* node, bool seekEnd);
CVAPI(bool) cveFileNodeIteratorEqualTo(cv::FileNodeIterator* iterator, cv::FileNodeIterator* otherIterator);
CVAPI(void) cveFileNodeIteratorNext(cv::FileNodeIterator* iterator);
CVAPI(cv::FileNode*) cveFileNodeIteratorGetFileNode(cv::FileNodeIterator* iterator);
CVAPI(void) cveFileNodeIteratorRelease(cv::FileNodeIterator** iterator);

CVAPI(IplImage*) cveCreateImage(CvSize* size, int depth, int channels);
CVAPI(IplImage*) cveCreateImageHeader(CvSize* size, int depth, int channels);
CVAPI(IplImage*) cveInitImageHeader(IplImage* image, CvSize* size, int depth, int channels, int origin, int align);
CVAPI(void) cveSetData(CvArr* arr, void* data, int step);
CVAPI(void) cveReleaseImageHeader(IplImage** image);
CVAPI(void) cveSetImageCOI(IplImage* image, int coi);
CVAPI(int) cveGetImageCOI(IplImage* image);
CVAPI(void) cveResetImageROI(IplImage* image);
CVAPI(void) cveSetImageROI(IplImage* image, CvRect* rect);
CVAPI(void) cveGetImageROI(IplImage* image, CvRect* rect);

CVAPI(CvMat*) cveInitMatHeader(CvMat* mat, int rows, int cols, int type, void* data, int step);
CVAPI(CvMat*) cveCreateMat(int rows, int cols, int type);
CVAPI(CvMatND*) cveInitMatNDHeader(CvMatND* mat, int dims, int* sizes, int type, void* data);
CVAPI(void) cveReleaseMat(CvMat** mat);

CVAPI(CvSparseMat*) cveCreateSparseMat(int dim, int* sizes, int type);
CVAPI(void) cveReleaseSparseMat(CvSparseMat** mat);

CVAPI(void) cveSet2D(CvArr* arr, int idx0, int idx1, CvScalar* value);
CVAPI(CvMat*) cveGetSubRect(CvArr* arr, CvMat* submat, CvRect* rect);
CVAPI(CvMat*) cveGetRows(CvArr* arr, CvMat* submat, int startRow, int endRow, int deltaRow);
CVAPI(CvMat*) cveGetCols(CvArr* arr, CvMat* submat, int startCol, int endCol);

CVAPI(void) cveGetSize(CvArr* arr, int* width, int* height);

CVAPI(void) cveCopy(CvArr* src, CvArr* dst, CvArr* mask);
CVAPI(void) cveRange(CvArr* mat, double start, double end);

CVAPI(void) cveSetReal1D(CvArr* arr, int idx0, double value);
CVAPI(void) cveSetReal2D(CvArr* arr, int idx0, int idx1, double value);
CVAPI(void) cveSetReal3D(CvArr* arr, int idx0, int idx1, int idx2, double value);
CVAPI(void) cveSetRealND(CvArr* arr, int* idx, double value);
CVAPI(void) cveGet1D(CvArr* arr, int idx0, CvScalar* value);
CVAPI(void) cveGet2D(CvArr* arr, int idx0, int idx1, CvScalar* value);
CVAPI(void) cveGet3D(CvArr* arr, int idx0, int idx1, int idx2, CvScalar* value);
CVAPI(double) cveGetReal1D(CvArr* arr, int idx0);
CVAPI(double) cveGetReal2D(CvArr* arr, int idx0, int idx1);
CVAPI(double) cveGetReal3D(CvArr* arr, int idx0, int idx1, int idx2);
CVAPI(void) cveClearND(CvArr* arr, int* idx);

CVAPI(bool) cveUseOptimized();
CVAPI(void) cveSetUseOptimized(bool onoff);

CVAPI(bool) cveHaveOpenVX();
CVAPI(bool) cveUseOpenVX();
CVAPI(void) cveSetUseOpenVX(bool flag);

CVAPI(void) cveGetBuildInformation(cv::String* buildInformation);

CVAPI(void) cveGetRawData(CvArr* arr, uchar** data, int* step, CvSize* roiSize);
CVAPI(CvMat*) cveGetMat(CvArr* arr, CvMat* header, int* coi, int allowNd);
CVAPI(IplImage*) cveGetImage(CvArr* arr, IplImage* imageHeader);

CVAPI(int) cveCheckArr(CvArr* arr, int flags, double minVal, double maxVal);
CVAPI(CvMat*) cveReshape(CvArr* arr, CvMat* header, int newCn, int newRows);
CVAPI(CvMat*) cveGetDiag(CvArr* arr, CvMat* submat, int diag);
CVAPI(void) cveConvertScale(CvArr* arr, CvArr* dst, double scale, double shift);

CVAPI(void) cveReleaseImage(IplImage** image);

CVAPI(void) cveSVDecomp(cv::_InputArray* src, cv::_OutputArray* w, cv::_OutputArray* u, cv::_OutputArray* vt, int flags);
CVAPI(void) cveSVBackSubst(cv::_InputArray* w, cv::_InputArray* u, cv::_InputArray* vt, cv::_InputArray* rhs, cv::_OutputArray* dst);

CVAPI(void) cvePCACompute1(cv::_InputArray* data, cv::_InputOutputArray* mean, cv::_OutputArray* eigenvectors, int maxComponents);
CVAPI(void) cvePCACompute2(cv::_InputArray* data, cv::_InputOutputArray* mean, cv::_OutputArray* eigenvectors, double retainedVariance);
CVAPI(void) cvePCAProject(cv::_InputArray* data, cv::_InputArray* mean, cv::_InputArray* eigenvectors, cv::_OutputArray* result);
CVAPI(void) cvePCABackProject(cv::_InputArray* data, cv::_InputArray* mean, cv::_InputArray* eigenvectors, cv::_OutputArray* result);

CVAPI(void) cveGetRangeAll(cv::Range* range);


CVAPI(cv::Affine3d*) cveAffine3dCreate();
CVAPI(cv::Affine3d*) cveAffine3dGetIdentity();
CVAPI(cv::Affine3d*) cveAffine3dRotate(cv::Affine3d* affine, double r0, double r1, double r2);
CVAPI(cv::Affine3d*) cveAffine3dTranslate(cv::Affine3d* affine, double t0, double t1, double t2);
CVAPI(void) cveAffine3dGetValues(cv::Affine3d* affine, double* values);
CVAPI(void) cveAffine3dRelease(cv::Affine3d** affine);

CVAPI(cv::RNG*) cveRngCreate();
CVAPI(cv::RNG*) cveRngCreateWithSeed(uint64 state);
CVAPI(void) cveRngFill(cv::RNG* rng, cv::_InputOutputArray* mat, int distType, cv::_InputArray* a, cv::_InputArray* b, bool saturateRange);
CVAPI(double) cveRngGaussian(cv::RNG* rng, double sigma);
CVAPI(unsigned) cveRngNext(cv::RNG* rng);
CVAPI(int) cveRngUniformInt(cv::RNG* rng, int a, int b);
CVAPI(float) cveRngUniformFloat(cv::RNG* rng, float a, float b);
CVAPI(double) cveRngUniformDouble(cv::RNG* rng, double a, double b);
CVAPI(void) cveRngRelease(cv::RNG** rng);


CVAPI(cv::Moments*) cveMomentsCreate();
CVAPI(void) cveMomentsRelease(cv::Moments** moments);

CVAPI(void) cveGetConfigDict(std::vector<cv::String>* key, std::vector<double>* value);
#endif
