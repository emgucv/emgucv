//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_DNN_C_H
#define EMGU_DNN_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/dnn/dnn.hpp"


CVAPI(cv::dnn::Net*) cveReadNetFromDarknet(cv::String* cfgFile, cv::String* darknetModel);
CVAPI(cv::dnn::Net*) cveReadNetFromCaffe(cv::String* prototxt, cv::String* caffeModel);
CVAPI(cv::dnn::Net*) cveReadNetFromCaffe2(const char *bufferProto, int lenProto, const char *bufferModel, int lenModel);

CVAPI(cv::dnn::Net*) cveReadNetFromTensorflow(cv::String* model, cv::String* config);
CVAPI(cv::dnn::Net*) cveReadNetFromTensorflow2(const char *bufferModel, int lenModel, const char *bufferConfig, int lenConfig);

CVAPI(cv::dnn::Net*) cveReadNet(cv::String* model, cv::String* config, cv::String* framework);
CVAPI(cv::dnn::Net*) cveReadNetFromModelOptimizer(cv::String* xml, cv::String* bin);

CVAPI(cv::dnn::Net*) cveDnnNetCreate();
CVAPI(void) cveDnnNetSetInput(cv::dnn::Net* net, cv::Mat* blob, cv::String* name);

CVAPI(void) cveDnnNetForward(cv::dnn::Net* net, cv::String* outputName, cv::Mat* output);
CVAPI(void) cveDnnNetRelease(cv::dnn::Net** net);

CVAPI(std::vector<cv::String>*) cveDnnNetGetLayerNames(cv::dnn::Net* net);

CVAPI(void) cveDnnBlobFromImage(
	cv::_InputArray* image, 
	cv::_OutputArray* blob,
	double scalefactor, 
	CvSize* size,
	CvScalar* mean, 
	bool swapRB,
	bool crop);

CVAPI(void) cveDnnBlobFromImages(
	cv::_InputArray* images,
	cv::_OutputArray* blob,
	double scalefactor,
	CvSize* size, 
	CvScalar* mean, 
	bool swapRB,
	bool crop);

CVAPI(void) cveDnnImagesFromBlob(cv::Mat* blob, cv::_OutputArray* images);

CVAPI(void) cveDnnShrinkCaffeModel(cv::String* src, cv::String* dst);

CVAPI(void) cveDnnNMSBoxes(
	std::vector<cv::Rect>* bboxes, 
	std::vector<float>* scores,
	float scoreThreshold, 
	float nmsThreshold,
	std::vector<int>* indices,
	float eta,
	int topK);
#endif