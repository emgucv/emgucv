//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_UMAT_C_H
#define EMGU_UMAT_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/core/ocl.hpp"
#include "emgu_c.h"
#include "mat_c.h"

CVAPI(cv::UMat*) cveUMatCreate(cv::UMatUsageFlags flags);
//CVAPI(void) cveUMatUseCustomAllocator(cv::UMat* mat, MatAllocateCallback allocator, MatDeallocateCallback deallocator, void* allocateDataActionPtr, void* freeDataActionPtr, cv::MatAllocator** matAllocator, cv::MatAllocator** oclAllocator);
CVAPI(void) cveUMatCreateData(cv::UMat* mat, int row, int cols, int type, cv::UMatUsageFlags flags);
CVAPI(cv::UMat*) cveUMatCreateFromRect(cv::UMat* mat, CvRect* roi);
CVAPI(cv::UMat*) cveUMatCreateFromRange(cv::UMat* mat, cv::Range* rowRange, cv::Range* colRange);
CVAPI(void) cveUMatRelease(cv::UMat** mat);
CVAPI(void) cveUMatGetSize(cv::UMat* mat, CvSize* size);
CVAPI(void) cveUMatCopyTo(cv::UMat* mat, cv::_OutputArray* m, cv::_InputArray* mask);
CVAPI(int) cveUMatGetElementSize(cv::UMat* mat);

CVAPI(void) cveUMatSetTo(cv::UMat* mat, cv::_InputArray* value, cv::_InputArray* mask);
CVAPI(cv::Mat*) cveUMatGetMat(cv::UMat* mat, int access);
CVAPI(void) cveUMatConvertTo( cv::UMat* mat, cv::_OutputArray* out, int rtype, double alpha, double beta );
CVAPI(cv::UMat*) cveUMatReshape(cv::UMat* mat, int cn, int rows);

CVAPI(void) cveUMatCopyDataTo(cv::UMat* mat, unsigned char* dest);
CVAPI(void) cveUMatCopyDataFrom(cv::UMat* mat, unsigned char* source);

CVAPI(double) cveUMatDot(cv::UMat* mat, cv::_InputArray* m);

CVAPI(void) cveSwapUMat(cv::UMat* mat1, cv::UMat* mat2);
#endif