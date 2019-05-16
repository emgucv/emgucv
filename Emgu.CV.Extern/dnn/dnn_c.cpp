//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "dnn_c.h"

cv::dnn::Net* cveReadNetFromDarknet(cv::String* cfgFile, cv::String* darknetModel)
{
	cv::dnn::Net net = cv::dnn::readNetFromDarknet(*cfgFile, *darknetModel);
	return new cv::dnn::Net(net);
}
cv::dnn::Net* cveReadNetFromDarknet2(const char *bufferCfg, int lenCfg, const char *bufferModel, int lenModel)
{
	cv::dnn::Net net = cv::dnn::readNetFromDarknet(bufferCfg, lenCfg, bufferModel, lenModel);
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

cv::dnn::Net* cveReadNetFromONNX(cv::String* onnxFile)
{
	cv::dnn::Net net = cv::dnn::readNetFromONNX(*onnxFile);
	return new cv::dnn::Net(net);
}
void cveReadTensorFromONNX(cv::String* path, cv::Mat* tensor)
{
	cv::Mat t = cv::dnn::readTensorFromONNX(*path);
	cv::swap(t, *tensor);
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

void cveDnnNetSetInput(cv::dnn::Net* net, cv::_InputArray* blob, cv::String* name, double scalefactor, CvScalar* mean)
{
	net->setInput(*blob, name ? *name : "", scalefactor, *mean);
}

void cveDnnNetForward(cv::dnn::Net* net, cv::String* outputName, cv::Mat* output)
{
   cv::Mat m = net->forward(*outputName);
   cv::swap(m, *output);
}
void cveDnnNetForward2(cv::dnn::Net* net, cv::_OutputArray* outputBlobs, cv::String* outputName)
{
	net->forward(*outputBlobs, *outputName);
}
void cveDnnNetForward3(cv::dnn::Net* net, cv::_OutputArray* outputBlobs, std::vector<cv::String>* outBlobNames)
{
	net->forward(*outputBlobs, *outBlobNames);
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

int cveDnnGetLayerId(cv::dnn::Net* net, cv::String* layer)
{
	return net->getLayerId(*layer);
}
cv::dnn::Layer* cveDnnGetLayerByName(cv::dnn::Net* net, cv::String* layerName, cv::Ptr<cv::dnn::Layer>** sharedPtr)
{
	cv::Ptr<cv::dnn::Layer> layerPtr = net->getLayer(*layerName);
	*sharedPtr = new cv::Ptr<cv::dnn::Layer>(layerPtr);
	return (*sharedPtr)->get();
}
cv::dnn::Layer* cveDnnGetLayerById(cv::dnn::Net* net, int layerId, cv::Ptr<cv::dnn::Layer>** sharedPtr)
{
	cv::Ptr<cv::dnn::Layer> layerPtr = net->getLayer(layerId);
	*sharedPtr = new cv::Ptr<cv::dnn::Layer>(layerPtr);
	return (*sharedPtr)->get();
}
void cveDnnLayerRelease(cv::Ptr<cv::dnn::Layer>** layer)
{
	delete *layer;
	layer = 0;
}
std::vector<cv::Mat>* cveDnnLayerGetBlobs(cv::dnn::Layer* layer)
{
	return &(layer->blobs);
}

void cveDnnNetGetUnconnectedOutLayers(cv::dnn::Net* net, std::vector<int>* layerIds)
{
	std::vector<int> v = net->getUnconnectedOutLayers();
	*layerIds = v;
	//layerIds->clear();

	//layerIds->resize(v.size());
	//memccpy(&layerIds[0], &v[0], 0, sizeof(int) * v.size());
}
void cveDnnNetGetUnconnectedOutLayersNames(cv::dnn::Net* net, std::vector<cv::String>* layerNames)
{
	std::vector<cv::String> v = net->getUnconnectedOutLayersNames();
	*layerNames = v;
	//layerNames->clear();
	//for (std::vector<cv::String>::iterator it = v.begin(); it != v.end(); ++it)
	//{
	//	layerNames->push_back(*it);
	//}
}

int64 cveDnnNetGetPerfProfile(cv::dnn::Net* net, std::vector<double>* timings)
{
	return net->getPerfProfile(*timings);
}


void cveDnnBlobFromImage(
	cv::_InputArray* image,
	cv::_OutputArray* blob,
	double scalefactor,
	CvSize* size,
	CvScalar* mean,
	bool swapRB,
	bool crop,
	int ddepth
	)
{
	cv::dnn::blobFromImage(*image, *blob, scalefactor, *size, *mean, swapRB, crop, ddepth);
}

void cveDnnBlobFromImages(
	cv::_InputArray* images,
	cv::_OutputArray* blob,
	double scalefactor,
	CvSize* size,
	CvScalar* mean,
	bool swapRB,
	bool crop,
	int ddepth)
{
	cv::dnn::blobFromImages(*images, *blob, scalefactor, *size, *mean, swapRB, crop, ddepth);
}

void cveDnnImagesFromBlob(cv::Mat* blob, cv::_OutputArray* images)
{
	cv::dnn::imagesFromBlob(*blob, *images);
}

void cveDnnShrinkCaffeModel(cv::String* src, cv::String* dst)
{
	cv::dnn::shrinkCaffeModel(*src, *dst);
}

void cveDnnWriteTextGraph(cv::String* model, cv::String* output)
{
	cv::dnn::writeTextGraph(*model, *output);
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

void cveDNNGetAvailableBackends(std::vector<int>* backends, std::vector<int>* targets)
{
	backends->clear();
	targets->clear();
	std::vector< std::pair< cv::dnn::Backend, cv::dnn::Target > > result = cv::dnn::getAvailableBackends();
	for ( std::vector< std::pair< cv::dnn::Backend, cv::dnn::Target > >::iterator iter = result.begin(); iter != result.end(); iter++)
	{
		backends->push_back(iter->first);
		targets->push_back(iter->second);
	}
}