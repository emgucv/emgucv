//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "dnn_c.h"

cv::dnn::Net* cveReadNetFromDarknet(cv::String* cfgFile, cv::String* darknetModel)
{
	cv::dnn::Net net = cv::dnn::readNetFromDarknet(*cfgFile, *darknetModel);
	return new cv::dnn::Net(net);
}

cv::dnn::Net* cveReadNetFromCaffe(cv::String* prototxt, cv::String* caffeModel)
{
	cv::dnn::Net net = cv::dnn::readNetFromCaffe(*prototxt, *caffeModel);
	return new cv::dnn::Net(net);
}
cv::dnn::Net* cveReadNetFromCaffe2(const char *bufferProto, int lenProto, const char *bufferModel, int lenModel)
{
	cv::dnn::Net net = cv::dnn::readNetFromCaffe(bufferProto, lenProto, bufferModel, lenModel);
	return new cv::dnn::Net(net);
}
cv::dnn::Net* cveReadNetFromTensorflow(cv::String* model, cv::String* config)
{
	cv::dnn::Net net = cv::dnn::readNetFromTensorflow(*model, *config);
	return new cv::dnn::Net(net);
}
cv::dnn::Net* cveReadNetFromTensorflow2(const char *bufferModel, int lenModel, const char *bufferConfig, int lenConfig)
{
	cv::dnn::Net net = cv::dnn::readNetFromTensorflow(bufferModel, lenModel, bufferConfig, lenConfig);
	return new cv::dnn::Net(net);
}

cv::dnn::Net* cveReadNet(cv::String* model, cv::String* config, cv::String* framework)
{
	cv::dnn::Net net = cv::dnn::readNet(*model, *config, *framework);
	return new cv::dnn::Net(net);
}
cv::dnn::Net* cveReadNetFromModelOptimizer(cv::String* xml, cv::String* bin)
{
	cv::dnn::Net net = cv::dnn::readNetFromModelOptimizer(*xml, *bin);
	return new cv::dnn::Net(net);
}


cv::dnn::Net* cveDnnNetCreate()
{
   return new cv::dnn::Net();
}

void cveDnnNetSetInput(cv::dnn::Net* net, cv::Mat* blob, cv::String* name)
{
	net->setInput(*blob, name ? *name : "");
}

void cveDnnNetForward(cv::dnn::Net* net, cv::String* outputName, cv::Mat* output)
{
   cv::Mat m = net->forward(*outputName);
   cv::swap(m, *output);
}
void cveDnnNetRelease(cv::dnn::Net** net)
{
   delete *net;
   *net = 0;
}

std::vector<cv::String>* cveDnnNetGetLayerNames(cv::dnn::Net* net)
{
	return new std::vector<cv::String>(net->getLayerNames());
}

void cveDnnBlobFromImage(
	cv::_InputArray* image,
	cv::_OutputArray* blob,
	double scalefactor,
	CvSize* size,
	CvScalar* mean,
	bool swapRB,
	bool crop
	)
{
	cv::dnn::blobFromImage(*image, *blob, scalefactor, *size, *mean, swapRB, crop);
}

void cveDnnBlobFromImages(
	cv::_InputArray* images,
	cv::_OutputArray* blob,
	double scalefactor,
	CvSize* size,
	CvScalar* mean,
	bool swapRB,
	bool crop)
{
	cv::dnn::blobFromImages(*images, *blob, scalefactor, *size, *mean, swapRB, crop);
}

void cveDnnImagesFromBlob(cv::Mat* blob, cv::_OutputArray* images)
{
	cv::dnn::imagesFromBlob(*blob, *images);
}

void cveDnnShrinkCaffeModel(cv::String* src, cv::String* dst)
{
	cv::dnn::shrinkCaffeModel(*src, *dst);
}

void cveDnnNMSBoxes(
	std::vector<cv::Rect>* bboxes,
	std::vector<float>* scores,
	float scoreThreshold,
	float nmsThreshold,
	std::vector<int>* indices,
	float eta,
	int topK)
{
	cv::dnn::NMSBoxes(*bboxes, *scores, scoreThreshold, nmsThreshold, *indices, eta, topK);
}