//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_DNN_C_H
#define EMGU_DNN_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_DNN
#include "opencv2/dnn/dnn.hpp"
#else
static inline CV_NORETURN void throw_no_dnn() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without DNN support"); }

namespace cv
{
	namespace dnn
	{
		class Net
		{
		};

		class Layer
		{
		};
	}
}
#endif

CVAPI(cv::dnn::Net*) cveReadNetFromDarknet(cv::String* cfgFile, cv::String* darknetModel);
CVAPI(cv::dnn::Net*) cveReadNetFromDarknet2(const char *bufferCfg, int lenCfg, const char *bufferModel, int lenModel);

CVAPI(cv::dnn::Net*) cveReadNetFromCaffe(cv::String* prototxt, cv::String* caffeModel);
CVAPI(cv::dnn::Net*) cveReadNetFromCaffe2(const char *bufferProto, int lenProto, const char *bufferModel, int lenModel);

CVAPI(cv::dnn::Net*) cveReadNetFromTensorflow(cv::String* model, cv::String* config);
CVAPI(cv::dnn::Net*) cveReadNetFromTensorflow2(const char *bufferModel, int lenModel, const char *bufferConfig, int lenConfig);

CVAPI(cv::dnn::Net*) cveReadNetFromONNX(cv::String* onnxFile);
CVAPI(void) cveReadTensorFromONNX(cv::String* path, cv::Mat* tensor);

CVAPI(cv::dnn::Net*) cveReadNet(cv::String* model, cv::String* config, cv::String* framework);
CVAPI(cv::dnn::Net*) cveReadNetFromModelOptimizer(cv::String* xml, cv::String* bin);

CVAPI(cv::dnn::Net*) cveDnnNetCreate();
CVAPI(void) cveDnnNetSetInput(cv::dnn::Net* net, cv::_InputArray* blob, cv::String* name, double scalefactor, CvScalar* mean);

CVAPI(void) cveDnnNetForward(cv::dnn::Net* net, cv::String* outputName, cv::Mat* output);
CVAPI(void) cveDnnNetForward2(cv::dnn::Net* net, cv::_OutputArray* outputBlobs, cv::String* outputName);
CVAPI(void) cveDnnNetForward3(cv::dnn::Net* net, cv::_OutputArray* outputBlobs,	std::vector<cv::String>* outBlobNames);
CVAPI(void) cveDnnNetRelease(cv::dnn::Net** net);

CVAPI(void) cveDnnNetGetUnconnectedOutLayers(cv::dnn::Net* net, std::vector<int>* layerIds);
CVAPI(void) cveDnnNetGetUnconnectedOutLayersNames(cv::dnn::Net* net, std::vector<cv::String>* layerNames);
CVAPI(int64) cveDnnNetGetPerfProfile(cv::dnn::Net* net, std::vector<double>* timings);
CVAPI(void) cveDnnNetDump(cv::dnn::Net* net, cv::String* string);
CVAPI(void) cveDnnNetDumpToFile(cv::dnn::Net* net, cv::String* path);
CVAPI(std::vector<cv::String>*) cveDnnNetGetLayerNames(cv::dnn::Net* net);

CVAPI(int) cveDnnGetLayerId(cv::dnn::Net* net, cv::String* layer);
CVAPI(cv::dnn::Layer*) cveDnnGetLayerByName(cv::dnn::Net* net, cv::String* layerName, cv::Ptr<cv::dnn::Layer>** sharedPtr);
CVAPI(cv::dnn::Layer*) cveDnnGetLayerById(cv::dnn::Net* net, int layerId, cv::Ptr<cv::dnn::Layer>** sharedPtr);
CVAPI(void) cveDnnLayerRelease(cv::Ptr<cv::dnn::Layer>** layer);
CVAPI(std::vector<cv::Mat>*) cveDnnLayerGetBlobs(cv::dnn::Layer* layer);


CVAPI(void) cveDnnBlobFromImage(
	cv::_InputArray* image, 
	cv::_OutputArray* blob,
	double scalefactor, 
	CvSize* size,
	CvScalar* mean, 
	bool swapRB,
	bool crop,
	int ddepth);

CVAPI(void) cveDnnBlobFromImages(
	cv::_InputArray* images,
	cv::_OutputArray* blob,
	double scalefactor,
	CvSize* size, 
	CvScalar* mean, 
	bool swapRB,
	bool crop,
	int ddepth);

CVAPI(void) cveDnnImagesFromBlob(cv::Mat* blob, cv::_OutputArray* images);

CVAPI(void) cveDnnShrinkCaffeModel(cv::String* src, cv::String* dst);

CVAPI(void) cveDnnWriteTextGraph(cv::String* model, cv::String* output);

CVAPI(void) cveDnnNMSBoxes(
	std::vector<cv::Rect>* bboxes, 
	std::vector<float>* scores,
	float scoreThreshold, 
	float nmsThreshold,
	std::vector<int>* indices,
	float eta,
	int topK);

CVAPI(void) cveDNNGetAvailableBackends(std::vector<int>* backends, std::vector<int>* targets);
#endif