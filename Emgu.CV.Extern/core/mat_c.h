//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_MAT_C_H
#define EMGU_MAT_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/core/ocl.hpp"
#include "emgu_c.h"

typedef uchar* (CV_CDECL *MatAllocateCallback)(int depthType, int channel, int totalInBytes);
typedef void (CV_CDECL *MatDeallocateCallback)(uchar* data);

CVAPI(cv::MatAllocator*) emguMatAllocatorCreate(MatAllocateCallback allocator, MatDeallocateCallback deallocator);
CVAPI(void) cvMatAllocatorRelease(cv::MatAllocator** allocator);

CVAPI(cv::Mat*) cvMatCreate();
CVAPI(cv::MatAllocator*) cvMatUseCustomAllocator(cv::Mat* mat, MatAllocateCallback allocator, MatDeallocateCallback deallocator);
CVAPI(void) cvMatCreateData(cv::Mat* mat, int row, int cols, int type);
CVAPI(cv::Mat*) cvMatCreateWithData(int rows, int cols, int type, void* data, size_t step);

CVAPI(void) cvMatRelease(cv::Mat** mat);
CVAPI(emgu::size) cvMatGetSize(cv::Mat* mat);
CVAPI(void) cvMatCopyTo(cv::Mat* mat, cv::_OutputArray* m, cv::_InputArray* mask);
CVAPI(void) cveArrToMat(CvArr* cvArray, cv::Mat* mat);
CVAPI(int) cvMatGetElementSize(cv::Mat* mat);
CVAPI(int) cvMatGetChannels(cv::Mat* mat);
CVAPI(int) cvMatGetDepth(cv::Mat* mat);
CVAPI(uchar*) cvMatGetDataPointer(cv::Mat* mat);
CVAPI(size_t) cvMatGetStep(cv::Mat* mat);
CVAPI(bool) cvMatIsEmpty(cv::Mat* mat);
CVAPI(void) cvMatSetTo(cv::Mat* mat, cv::_InputArray* value, cv::_InputArray* mask);
CVAPI(cv::UMat*) cvMatGetUMat(cv::Mat* mat, int access);

#endif