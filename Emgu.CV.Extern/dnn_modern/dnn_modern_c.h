//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_DNN_MODERN_C_H
#define EMGU_DNN_MODERN_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/dnn_modern.hpp"

/*
CVAPI(cv::dnn::Blob*) cveDnnBlobCreate();
CVAPI(cv::dnn::Blob*) cveDnnBlobCreateFromInputArray(cv::_InputArray* image);
CVAPI(void) cveDnnBlobBatchFromImages(cv::dnn::Blob* blob, cv::_InputArray* image, int dstCn);
CVAPI(void) cveDnnBlobMatRef(cv::dnn::Blob* blob, cv::Mat* outMat);
CVAPI(void) cveDnnBlobRelease(cv::dnn::Blob** blob); 
CVAPI(int) cveDnnBlobDims(cv::dnn::Blob* blob);
CVAPI(int) cveDnnBlobChannels(cv::dnn::Blob* blob);
CVAPI(int) cveDnnBlobCols(cv::dnn::Blob* blob);
CVAPI(int) cveDnnBlobNum(cv::dnn::Blob* blob);
CVAPI(int) cveDnnBlobRows(cv::dnn::Blob* blob);
CVAPI(int) cveDnnBlobType(cv::dnn::Blob* blob);
CVAPI(int) cveDnnBlobElemSize(cv::dnn::Blob* blob);
CVAPI(uchar *) cveDnnBlobGetPtr(cv::dnn::Blob* blob, int n, int cn, int row, int col);*/
#endif