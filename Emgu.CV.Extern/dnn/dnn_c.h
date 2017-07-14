//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_DNN_C_H
#define EMGU_DNN_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/dnn/dnn.hpp"

//CVAPI(void) cveDnnInitModule();

CVAPI(cv::dnn::Importer*) cveDnnCreateCaffeImporter(cv::String* prototxt, cv::String* caffeModel);
CVAPI(cv::dnn::Importer*) cveDnnCreateTensorflowImporter(cv::String* model);
CVAPI(void) cveDnnImporterRelease(cv::dnn::Importer** importer);
CVAPI(void) cveDnnImporterPopulateNet(cv::dnn::Importer* importer, cv::dnn::Net* net);

CVAPI(cv::dnn::Net*) cveDnnNetCreate();
CVAPI(void) cveDnnNetSetInput(cv::dnn::Net* net, cv::Mat* blob, cv::String* name);
//CVAPI(cv::dnn::Blob*) cveDnnNetGetBlob(cv::dnn::Net* net, cv::String* outputName);
CVAPI(void) cveDnnNetForward(cv::dnn::Net* net, cv::String* outputName, cv::Mat* output);
CVAPI(void) cveDnnNetRelease(cv::dnn::Net** net);


CVAPI(void) cveDnnBlobFromImage(
	cv::Mat* image, 
	double scalefactor, 
	CvSize* size,
	CvScalar* mean, 
	bool swapRB, 
	cv::Mat* blob);

CVAPI(void) cveDnnBlobFromImages(
	std::vector<cv::Mat>* images, 
	double scalefactor,
	CvSize* size, 
	CvScalar* mean, 
	bool swapRB,
	cv::Mat* blob);

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