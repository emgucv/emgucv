//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "dnn_c.h"

cv::dnn::Net* cveReadNetFromDarknet(cv::String* cfgFile, cv::String* darknetModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::Net net = cv::dnn::readNetFromDarknet(*cfgFile, *darknetModel);
	return new cv::dnn::Net(net);
#else
	throw_no_dnn();
#endif
}
cv::dnn::Net* cveReadNetFromDarknet2(const char *bufferCfg, int lenCfg, const char *bufferModel, int lenModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::Net net = cv::dnn::readNetFromDarknet(bufferCfg, lenCfg, bufferModel, lenModel);
	return new cv::dnn::Net(net);
#else
	throw_no_dnn();
#endif
}

cv::dnn::Net* cveReadNetFromCaffe(cv::String* prototxt, cv::String* caffeModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::Net net = cv::dnn::readNetFromCaffe(*prototxt, *caffeModel);
	return new cv::dnn::Net(net);
#else
	throw_no_dnn();
#endif
}
cv::dnn::Net* cveReadNetFromCaffe2(const char *bufferProto, int lenProto, const char *bufferModel, int lenModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::Net net = cv::dnn::readNetFromCaffe(bufferProto, lenProto, bufferModel, lenModel);
	return new cv::dnn::Net(net);
#else
	throw_no_dnn();
#endif
}
cv::dnn::Net* cveReadNetFromTensorflow(cv::String* model, cv::String* config)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::Net net = cv::dnn::readNetFromTensorflow(*model, *config);
	return new cv::dnn::Net(net);
#else
	throw_no_dnn();
#endif
}
cv::dnn::Net* cveReadNetFromTensorflow2(const char *bufferModel, int lenModel, const char *bufferConfig, int lenConfig)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::Net net = cv::dnn::readNetFromTensorflow(bufferModel, lenModel, bufferConfig, lenConfig);
	return new cv::dnn::Net(net);
#else
	throw_no_dnn();
#endif
}

cv::dnn::Net* cveReadNetFromONNX(cv::String* onnxFile)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::Net net = cv::dnn::readNetFromONNX(*onnxFile);
	return new cv::dnn::Net(net);
#else
	throw_no_dnn();
#endif
}
void cveReadTensorFromONNX(cv::String* path, cv::Mat* tensor)
{
#ifdef HAVE_OPENCV_DNN
	cv::Mat t = cv::dnn::readTensorFromONNX(*path);
	cv::swap(t, *tensor);
#else
	throw_no_dnn();
#endif
}


cv::dnn::Net* cveReadNet(cv::String* model, cv::String* config, cv::String* framework)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::Net net = cv::dnn::readNet(*model, *config, *framework);
	return new cv::dnn::Net(net);
#else
	throw_no_dnn();
#endif
}
cv::dnn::Net* cveReadNetFromModelOptimizer(cv::String* xml, cv::String* bin)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::Net net = cv::dnn::readNetFromModelOptimizer(*xml, *bin);
	return new cv::dnn::Net(net);
#else
	throw_no_dnn();
#endif
}


cv::dnn::Net* cveDnnNetCreate()
{
#ifdef HAVE_OPENCV_DNN
   return new cv::dnn::Net();
#else
	throw_no_dnn();
#endif
}

void cveDnnNetSetInput(cv::dnn::Net* net, cv::_InputArray* blob, cv::String* name, double scalefactor, CvScalar* mean)
{
#ifdef HAVE_OPENCV_DNN
	net->setInput(*blob, name ? *name : "", scalefactor, *mean);
#else
	throw_no_dnn();
#endif
}

void cveDnnNetForward(cv::dnn::Net* net, cv::String* outputName, cv::Mat* output)
{
#ifdef HAVE_OPENCV_DNN
   cv::Mat m = net->forward(*outputName);
   cv::swap(m, *output);
#else
	throw_no_dnn();
#endif
}
void cveDnnNetForward2(cv::dnn::Net* net, cv::_OutputArray* outputBlobs, cv::String* outputName)
{
#ifdef HAVE_OPENCV_DNN
	net->forward(*outputBlobs, *outputName);
#else
	throw_no_dnn();
#endif
}
void cveDnnNetForward3(cv::dnn::Net* net, cv::_OutputArray* outputBlobs, std::vector<cv::String>* outBlobNames)
{
#ifdef HAVE_OPENCV_DNN
	net->forward(*outputBlobs, *outBlobNames);
#else
	throw_no_dnn();
#endif
}
void cveDnnNetRelease(cv::dnn::Net** net)
{
#ifdef HAVE_OPENCV_DNN
   delete *net;
   *net = 0;
#else
	throw_no_dnn();
#endif
}

std::vector<cv::String>* cveDnnNetGetLayerNames(cv::dnn::Net* net)
{
#ifdef HAVE_OPENCV_DNN
	return new std::vector<cv::String>(net->getLayerNames());
#else
	throw_no_dnn();
#endif
}

int cveDnnGetLayerId(cv::dnn::Net* net, cv::String* layer)
{
#ifdef HAVE_OPENCV_DNN
	return net->getLayerId(*layer);
#else
	throw_no_dnn();
#endif
}
cv::dnn::Layer* cveDnnGetLayerByName(cv::dnn::Net* net, cv::String* layerName, cv::Ptr<cv::dnn::Layer>** sharedPtr)
{
#ifdef HAVE_OPENCV_DNN
	cv::Ptr<cv::dnn::Layer> layerPtr = net->getLayer(*layerName);
	*sharedPtr = new cv::Ptr<cv::dnn::Layer>(layerPtr);
	return (*sharedPtr)->get();
#else
	throw_no_dnn();
#endif
}
cv::dnn::Layer* cveDnnGetLayerById(cv::dnn::Net* net, int layerId, cv::Ptr<cv::dnn::Layer>** sharedPtr)
{
#ifdef HAVE_OPENCV_DNN
	cv::Ptr<cv::dnn::Layer> layerPtr = net->getLayer(layerId);
	*sharedPtr = new cv::Ptr<cv::dnn::Layer>(layerPtr);
	return (*sharedPtr)->get();
#else
	throw_no_dnn();
#endif
}
void cveDnnLayerRelease(cv::Ptr<cv::dnn::Layer>** layer)
{
#ifdef HAVE_OPENCV_DNN
	delete *layer;
	layer = 0;
#else
	throw_no_dnn();
#endif
}
std::vector<cv::Mat>* cveDnnLayerGetBlobs(cv::dnn::Layer* layer)
{
#ifdef HAVE_OPENCV_DNN
	return &(layer->blobs);
#else
	throw_no_dnn();
#endif
}

void cveDnnNetGetUnconnectedOutLayers(cv::dnn::Net* net, std::vector<int>* layerIds)
{
#ifdef HAVE_OPENCV_DNN
	std::vector<int> v = net->getUnconnectedOutLayers();
	*layerIds = v;
	//layerIds->clear();

	//layerIds->resize(v.size());
	//memccpy(&layerIds[0], &v[0], 0, sizeof(int) * v.size());
#else
	throw_no_dnn();
#endif
}
void cveDnnNetGetUnconnectedOutLayersNames(cv::dnn::Net* net, std::vector<cv::String>* layerNames)
{
#ifdef HAVE_OPENCV_DNN
	std::vector<cv::String> v = net->getUnconnectedOutLayersNames();
	*layerNames = v;
	//layerNames->clear();
	//for (std::vector<cv::String>::iterator it = v.begin(); it != v.end(); ++it)
	//{
	//	layerNames->push_back(*it);
	//}
#else
	throw_no_dnn();
#endif
}

int64 cveDnnNetGetPerfProfile(cv::dnn::Net* net, std::vector<double>* timings)
{
#ifdef HAVE_OPENCV_DNN
	return net->getPerfProfile(*timings);
#else
	throw_no_dnn();
#endif
}

CVAPI(void) cveDnnNetDump(cv::dnn::Net* net, cv::String* string)
{
#ifdef HAVE_OPENCV_DNN
	*string = net->dump();
#else
	throw_no_dnn();
#endif
}

void cveDnnNetDumpToFile(cv::dnn::Net* net, cv::String* path)
{
#ifdef HAVE_OPENCV_DNN
	net->dumpToFile(*path);
#else
	throw_no_dnn();
#endif
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
#ifdef HAVE_OPENCV_DNN
	cv::dnn::blobFromImage(*image, *blob, scalefactor, *size, *mean, swapRB, crop, ddepth);
#else
	throw_no_dnn();
#endif
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
#ifdef HAVE_OPENCV_DNN
	cv::dnn::blobFromImages(*images, *blob, scalefactor, *size, *mean, swapRB, crop, ddepth);
#else
	throw_no_dnn();
#endif
}

void cveDnnImagesFromBlob(cv::Mat* blob, cv::_OutputArray* images)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::imagesFromBlob(*blob, *images);
#else
	throw_no_dnn();
#endif
}

void cveDnnShrinkCaffeModel(cv::String* src, cv::String* dst)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::shrinkCaffeModel(*src, *dst);
#else
	throw_no_dnn();
#endif
}

void cveDnnWriteTextGraph(cv::String* model, cv::String* output)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::writeTextGraph(*model, *output);
#else
	throw_no_dnn();
#endif
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
#ifdef HAVE_OPENCV_DNN
	cv::dnn::NMSBoxes(*bboxes, *scores, scoreThreshold, nmsThreshold, *indices, eta, topK);
#else
	throw_no_dnn();
#endif
}

void cveDNNGetAvailableBackends(std::vector<int>* backends, std::vector<int>* targets)
{
#ifdef HAVE_OPENCV_DNN
	backends->clear();
	targets->clear();
	std::vector< std::pair< cv::dnn::Backend, cv::dnn::Target > > result = cv::dnn::getAvailableBackends();
	for ( std::vector< std::pair< cv::dnn::Backend, cv::dnn::Target > >::iterator iter = result.begin(); iter != result.end(); iter++)
	{
		backends->push_back(iter->first);
		targets->push_back(iter->second);
	}
#else
	throw_no_dnn();
#endif
}