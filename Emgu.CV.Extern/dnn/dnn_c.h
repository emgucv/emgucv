//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_DNN_C_H
#define EMGU_DNN_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/dnn/dnn.hpp"

CVAPI(cv::dnn::Importer*) cveDnnCreateCaffeImporter(cv::String* prototxt, cv::String* caffeModel);
CVAPI(void) cveDnnImporterRelease(cv::dnn::Importer** importer);
CVAPI(void) cveDnnImporterPopulateNet(cv::dnn::Importer* importer, cv::dnn::Net* net);

CVAPI(cv::dnn::Net*) cveDnnNetCreate();
CVAPI(void) cveDnnNetSetBlob(cv::dnn::Net* net, cv::String* outputName, cv::dnn::Blob* blob);
CVAPI(cv::dnn::Blob*) cveDnnNetGetBlob(cv::dnn::Net* net, cv::String* outputName);
CVAPI(void) cveDnnNetForward(cv::dnn::Net* net);
CVAPI(void) cveDnnNetRelease(cv::dnn::Net** net);

CVAPI(cv::dnn::Blob*) cveDnnBlobCreateFromInputArray(cv::_InputArray* image, int dstCn);
CVAPI(void) cveDnnBlobMatRef(cv::dnn::Blob* blob, cv::Mat* outMat);
CVAPI(void) cveDnnBlobRelease(cv::dnn::Blob** blob); 
CVAPI(int) cveDnnBlobDims(cv::dnn::Blob* blob);
CVAPI(int) cveDnnBlobChannels(cv::dnn::Blob* blob);
CVAPI(int) cveDnnBlobCols(cv::dnn::Blob* blob);
CVAPI(int) cveDnnBlobNum(cv::dnn::Blob* blob);
CVAPI(int) cveDnnBlobRows(cv::dnn::Blob* blob);
#endif