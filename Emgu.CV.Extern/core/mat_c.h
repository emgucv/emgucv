//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_MAT_C_H
#define EMGU_MAT_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/core/ocl.hpp"
#include "emgu_c.h"

typedef uchar* (CV_CDECL *MatAllocateCallback)(int depthType, int channel, int totalInBytes, void* allocateDataActionPtr);
typedef void (CV_CDECL *MatDeallocateCallback)(void* freeDataActionPtr);

//CVAPI(cv::MatAllocator*) emguMatAllocatorCreate(MatAllocateCallback allocator, MatDeallocateCallback deallocator, void* allocateDataActionPtr, void* freeDataActionPtr);
//CVAPI(void) cveMatAllocatorRelease(cv::MatAllocator** allocator);

CVAPI(cv::Mat*) cveMatCreate();
//CVAPI(cv::MatAllocator*) cveMatUseCustomAllocator(cv::Mat* mat, MatAllocateCallback allocator, MatDeallocateCallback deallocator, void* allocateDataActionPtr, void* freeDataActionPtr);
CVAPI(void) cveMatCreateData(cv::Mat* mat, int row, int cols, int type);
CVAPI(cv::Mat*) cveMatCreateWithData(int rows, int cols, int type, void* data, size_t step);
CVAPI(cv::Mat*) cveMatCreateMultiDimWithData(int ndims, const int* sizes, int type, void* data, size_t* steps);
CVAPI(cv::Mat*) cveMatCreateFromRect(cv::Mat* mat, CvRect* roi);
CVAPI(cv::Mat*) cveMatCreateFromRange(cv::Mat* mat, cv::Range* rowRange, cv::Range* colRange);
 
CVAPI(void) cveMatRelease(cv::Mat** mat);
CVAPI(void) cveMatGetSize(cv::Mat* mat, CvSize* size);
CVAPI(void) cveMatCopyTo(cv::Mat* mat, cv::_OutputArray* m, cv::_InputArray* mask);
CVAPI(cv::Mat*) cveArrToMat(CvArr* cvArray, bool copyData, bool allowND, int coiMode);
CVAPI(IplImage*) cveMatToIplImage(cv::Mat* mat);
CVAPI(int) cveMatGetElementSize(cv::Mat* mat);
//CVAPI(int) cveMatGetChannels(cv::Mat* mat);

CVAPI(uchar*) cveMatGetDataPointer(cv::Mat* mat);
CVAPI(uchar*) cveMatGetDataPointer2(cv::Mat* mat, int* indices);
CVAPI(size_t) cveMatGetStep(cv::Mat* mat);

CVAPI(void) cveMatSetTo(cv::Mat* mat, cv::_InputArray* value, cv::_InputArray* mask);
CVAPI(cv::UMat*) cveMatGetUMat(cv::Mat* mat, int access, cv::UMatUsageFlags usageFlags);
CVAPI(void) cveMatConvertTo( cv::Mat* mat, cv::_OutputArray* out, int rtype, double alpha, double beta );
CVAPI(cv::Mat*) cveMatReshape(cv::Mat* mat, int cn, int rows);

CVAPI(double) cveMatDot(cv::Mat* mat, cv::_InputArray* m);
CVAPI(void) cveMatCross(cv::Mat* mat, cv::_InputArray* m, cv::Mat* result);

CVAPI(void) cveMatCopyDataTo(cv::Mat* mat, unsigned char* dest);
CVAPI(void) cveMatCopyDataFrom(cv::Mat* mat, unsigned char* source);

CVAPI(void) cveMatGetSizeOfDimension(cv::Mat* mat, int* sizes);

CVAPI(void) cveSwapMat(cv::Mat* mat1, cv::Mat* mat2);

CVAPI(void) cveMatEye(int rows, int cols, int type, cv::Mat* m);
CVAPI(void) cveMatDiag(cv::Mat* src, int d, cv::Mat* dst);
CVAPI(void) cveMatT(cv::Mat* src, cv::Mat* dst);
CVAPI(void) cveMatZeros(int rows, int cols, int type, cv::Mat* dst);
CVAPI(void) cveMatOnes(int rows, int cols, int type, cv::Mat* dst);

#endif