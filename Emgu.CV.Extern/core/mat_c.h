//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
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

CVAPI(cv::MatAllocator*) emguMatAllocatorCreate(MatAllocateCallback allocator, MatDeallocateCallback deallocator, void* allocateDataActionPtr, void* freeDataActionPtr);
CVAPI(void) cveMatAllocatorRelease(cv::MatAllocator** allocator);

CVAPI(cv::Mat*) cveMatCreate();
CVAPI(cv::MatAllocator*) cveMatUseCustomAllocator(cv::Mat* mat, MatAllocateCallback allocator, MatDeallocateCallback deallocator, void* allocateDataActionPtr, void* freeDataActionPtr);
CVAPI(void) cveMatCreateData(cv::Mat* mat, int row, int cols, int type);
CVAPI(cv::Mat*) cveMatCreateWithData(int rows, int cols, int type, void* data, size_t step);
CVAPI(cv::Mat*) cveMatCreateMultiDimWithData(int ndims, const int* sizes, int type, void* data, size_t* steps);
CVAPI(cv::Mat*) cveMatCreateFromRect(cv::Mat* mat, CvRect* roi);

CVAPI(void) cveMatRelease(cv::Mat** mat);
CVAPI(emgu::size) cveMatGetSize(cv::Mat* mat);
CVAPI(void) cveMatCopyTo(cv::Mat* mat, cv::_OutputArray* m, cv::_InputArray* mask);
CVAPI(cv::Mat*) cveArrToMat(CvArr* cvArray, bool copyData, bool allowND, int coiMode);
CVAPI(IplImage*) cveMatToIplImage(cv::Mat* mat);
CVAPI(int) cveMatGetElementSize(cv::Mat* mat);
//CVAPI(int) cveMatGetChannels(cv::Mat* mat);

CVAPI(uchar*) cveMatGetDataPointer(cv::Mat* mat);
CVAPI(size_t) cveMatGetStep(cv::Mat* mat);

CVAPI(void) cvMatSetTo(cv::Mat* mat, cv::_InputArray* value, cv::_InputArray* mask);
CVAPI(cv::UMat*) cvMatGetUMat(cv::Mat* mat, int access);
CVAPI(void) cveMatConvertTo( cv::Mat* mat, cv::_OutputArray* out, int rtype, double alpha, double beta );
CVAPI(cv::Mat*) cveMatReshape(cv::Mat* mat, int cn, int rows);

CVAPI(double) cveMatDot(cv::Mat* mat, cv::_InputArray* m);
CVAPI(void) cveMatCross(cv::Mat* mat, cv::_InputArray* m, cv::Mat* result);

CVAPI(void) cveMatCopyDataTo(cv::Mat* mat, unsigned char* dest);
CVAPI(void) cveMatCopyDataFrom(cv::Mat* mat, unsigned char* source);

CVAPI(void) cveMatGetSizeOfDimension(cv::Mat* mat, int* sizes);
#endif