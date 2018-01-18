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
CVAPI(cv::dnn::Net*) cveReadNetFromTensorflow(cv::String* model, cv::String* config);

CVAPI(cv::dnn::Net*) cveDnnNetCreate();
CVAPI(void) cveDnnNetSetInput(cv::dnn::Net* net, cv::Mat* blob, cv::String* name);

CVAPI(void) cveDnnNetForward(cv::dnn::Net* net, cv::String* outputName, cv::Mat* output);
CVAPI(void) cveDnnNetRelease(cv::dnn::Net** net);
CVAPI(bool) cveDnnNetEmpty(cv::dnn::Net* net);
CVAPI(std::vector<cv::String>*) cveDnnNetGetLayerNames(cv::dnn::Net* net);

CVAPI(void) cveDnnBlobFromImage(
	cv::Mat* image, 
	double scalefactor, 
	CvSize* size,
	CvScalar* mean, 
	bool swapRB,
	bool crop,
	cv::Mat* blob);

CVAPI(void) cveDnnBlobFromImages(
	std::vector<cv::Mat>* images, 
	double scalefactor,
	CvSize* size, 
	CvScalar* mean, 
	bool swapRB,
	bool crop,
	cv::Mat* blob);

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