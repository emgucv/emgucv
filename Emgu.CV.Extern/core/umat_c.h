//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_UMAT_C_H
#define EMGU_UMAT_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/core/ocl.hpp"
#include "emgu_c.h"
#include "mat_c.h"

CVAPI(cv::UMat*) cvUMatCreate();
CVAPI(void) cvUMatUseCustomAllocator(cv::UMat* mat, MatAllocateCallback allocator, MatDeallocateCallback deallocator, void* allocateDataActionPtr, void* freeDataActionPtr, cv::MatAllocator** matAllocator, cv::MatAllocator** oclAllocator);
CVAPI(void) cvUMatCreateData(cv::UMat* mat, int row, int cols, int type);
CVAPI(cv::UMat*) cvUMatCreateFromROI(cv::UMat* mat, CvRect* roi);
CVAPI(void) cvUMatRelease(cv::UMat** mat);
CVAPI(emgu::size) cvUMatGetSize(cv::UMat* mat);
CVAPI(void) cvUMatCopyTo(cv::UMat* mat, cv::_OutputArray* m, cv::_InputArray* mask);
CVAPI(int) cvUMatGetElementSize(cv::UMat* mat);

CVAPI(void) cvUMatSetTo(cv::UMat* mat, cv::_InputArray* value, cv::_InputArray* mask);
CVAPI(cv::Mat*) cvUMatGetMat(cv::UMat* mat, int access);
CVAPI(void) cvUMatConvertTo( cv::UMat* mat, cv::_OutputArray* out, int rtype, double alpha, double beta );
CVAPI(cv::UMat*) cvUMatReshape(cv::UMat* mat, int cn, int rows);

#endif