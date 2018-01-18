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
cv::dnn::Net* cveReadNetFromTensorflow(cv::String* model, cv::String* config)
{
	cv::dnn::Net net = cv::dnn::readNetFromTensorflow(*model, *config);
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
bool cveDnnNetEmpty(cv::dnn::Net* net)
{
	return net->empty();
}

std::vector<cv::String>* cveDnnNetGetLayerNames(cv::dnn::Net* net)
{
	return new std::vector<cv::String>(net->getLayerNames());
}


void cveDnnBlobFromImage(
	cv::Mat* image,
	double scalefactor,
	CvSize* size,
	CvScalar* mean,
	bool swapRB,
	bool crop,
	cv::Mat* blob)
{
	cv::Mat b = cv::dnn::blobFromImage(*image, scalefactor, *size, *mean, swapRB, crop);
	cv::swap(*blob, b);
}

void cveDnnBlobFromImages(
	std::vector<cv::Mat>* images,
	double scalefactor,
	CvSize* size,
	CvScalar* mean,
	bool swapRB,
	bool crop,
	cv::Mat* blob)
{
	cv::Mat b = cv::dnn::blobFromImages(*images, scalefactor, *size, *mean, swapRB, crop);
	cv::swap(*blob, b);
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