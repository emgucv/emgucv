//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
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
CVAPI(void) cvMatAllocatorRelease(cv::MatAllocator** allocator);

CVAPI(cv::Mat*) cvMatCreate();
CVAPI(cv::MatAllocator*) cvMatUseCustomAllocator(cv::Mat* mat, MatAllocateCallback allocator, MatDeallocateCallback deallocator, void* allocateDataActionPtr, void* freeDataActionPtr);
CVAPI(void) cvMatCreateData(cv::Mat* mat, int row, int cols, int type);
CVAPI(cv::Mat*) cvMatCreateWithData(int rows, int cols, int type, void* data, size_t step);
CVAPI(cv::Mat*) cvMatCreateFromRect(cv::Mat* mat, CvRect* roi);

CVAPI(void) cvMatRelease(cv::Mat** mat);
CVAPI(emgu::size) cvMatGetSize(cv::Mat* mat);
CVAPI(void) cvMatCopyTo(cv::Mat* mat, cv::_OutputArray* m, cv::_InputArray* mask);
CVAPI(cv::Mat*) cveArrToMat(CvArr* cvArray, bool copyData, bool allowND, int coiMode);
CVAPI(IplImage*) cveMatToIplImage(cv::Mat* mat);
CVAPI(int) cvMatGetElementSize(cv::Mat* mat);
CVAPI(int) cvMatGetChannels(cv::Mat* mat);
CVAPI(int) cvMatGetDepth(cv::Mat* mat);
CVAPI(uchar*) cvMatGetDataPointer(cv::Mat* mat);
CVAPI(size_t) cvMatGetStep(cv::Mat* mat);
CVAPI(bool) cvMatIsEmpty(cv::Mat* mat);
CVAPI(void) cvMatSetTo(cv::Mat* mat, cv::_InputArray* value, cv::_InputArray* mask);
CVAPI(cv::UMat*) cvMatGetUMat(cv::Mat* mat, int access);
CVAPI(void) cvMatConvertTo( cv::Mat* mat, cv::_OutputArray* out, int rtype, double alpha, double beta );
CVAPI(cv::Mat*) cvMatReshape(cv::Mat* mat, int cn, int rows);

CVAPI(double) cvMatDot(cv::Mat* mat, cv::_InputArray* m);
CVAPI(void) cvMatCross(cv::Mat* mat, cv::_InputArray* m, cv::Mat* result);
#endif