//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
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

void cveDnnNMSBoxes2(
	std::vector<cv::RotatedRect>* bboxes,
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

void cveDnnSoftNMSBoxes(
	std::vector<cv::Rect>* bboxes,
	std::vector<float>* scores,
	std::vector<float>* updatedScores,
	float scoreThreshold,
	float nmsThreshold,
	std::vector<int>* indices,
	int topK,
	float sigma,
	int method)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::softNMSBoxes(*bboxes, *scores, *updatedScores, scoreThreshold, nmsThreshold, *indices, topK, sigma, static_cast<cv::dnn::SoftNMSMethod>(method));
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

cv::dnn::TextDetectionModel_DB* cveDnnTextDetectionModelDbCreate1(cv::String* model, cv::String* config, cv::dnn::TextDetectionModel** textDetectionModel, cv::dnn::Model** baseModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::TextDetectionModel_DB* ptr = new cv::dnn::TextDetectionModel_DB(*model, *config);
	*textDetectionModel = dynamic_cast<cv::dnn::TextDetectionModel*>(ptr);
	*baseModel = dynamic_cast<cv::dnn::Model*>(ptr);
	return ptr;
#else
	throw_no_dnn();
#endif
}

cv::dnn::TextDetectionModel_DB* cveDnnTextDetectionModelDbCreate2(cv::dnn::Net* network, cv::dnn::TextDetectionModel** textDetectionModel, cv::dnn::Model** baseModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::TextDetectionModel_DB* ptr = new cv::dnn::TextDetectionModel_DB(*network);
	*textDetectionModel = dynamic_cast<cv::dnn::TextDetectionModel*>(ptr);
	*baseModel = dynamic_cast<cv::dnn::Model*>(ptr);
	return ptr;
#else
	throw_no_dnn();
#endif
}

void cveDnnTextDetectionModelDbRelease(cv::dnn::TextDetectionModel_DB** textDetectionModel)
{
#ifdef HAVE_OPENCV_DNN
	delete* textDetectionModel;
	*textDetectionModel = 0;
#else
	throw_no_dnn();
#endif
}

cv::dnn::TextDetectionModel_EAST* cveDnnTextDetectionModelEastCreate1(cv::String* model, cv::String* config, cv::dnn::TextDetectionModel** textDetectionModel, cv::dnn::Model** baseModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::TextDetectionModel_EAST* ptr = new cv::dnn::TextDetectionModel_EAST(*model, *config);
	*textDetectionModel = dynamic_cast<cv::dnn::TextDetectionModel*>(ptr);
	*baseModel = dynamic_cast<cv::dnn::Model*>(ptr);
	return ptr;
#else
	throw_no_dnn();
#endif
}

cv::dnn::TextDetectionModel_EAST* cveDnnTextDetectionModelEastCreate2(cv::dnn::Net* network, cv::dnn::TextDetectionModel** textDetectionModel, cv::dnn::Model** baseModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::TextDetectionModel_EAST* ptr = new cv::dnn::TextDetectionModel_EAST(*network);
	*textDetectionModel = dynamic_cast<cv::dnn::TextDetectionModel*>(ptr);
	*baseModel = dynamic_cast<cv::dnn::Model*>(ptr);
	return ptr;
#else
	throw_no_dnn();
#endif
}

void cveDnnTextDetectionModelEastRelease(cv::dnn::TextDetectionModel_EAST** textDetectionModel)
{
#ifdef HAVE_OPENCV_DNN
	delete* textDetectionModel;
	*textDetectionModel = 0;
#else
	throw_no_dnn();
#endif
}

void cveDnnTextDetectionModelDetect(
	cv::dnn::TextDetectionModel* textDetectionModel,
	cv::_InputArray* frame,
	std::vector< std::vector< cv::Point > >* detections,
	std::vector<float>* confidences
)
{
#ifdef HAVE_OPENCV_DNN
	textDetectionModel->detect(*frame, *detections, *confidences);
#else
	throw_no_dnn();
#endif
}

void cveDnnTextDetectionModelDetectTextRectangles(
	cv::dnn::TextDetectionModel* textDetectionModel,
	cv::_InputArray* frame,
	std::vector< cv::RotatedRect >* detections,
	std::vector< float >* confidences
)
{
#ifdef HAVE_OPENCV_DNN
	textDetectionModel->detectTextRectangles(*frame, *detections, *confidences);
#else
	throw_no_dnn();
#endif
}

cv::dnn::TextRecognitionModel* cveDnnTextRecognitionModelCreate1(cv::String* model, cv::String* config, cv::dnn::Model** baseModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::TextRecognitionModel* ptr = new cv::dnn::TextRecognitionModel(*model, *config);
	*baseModel = dynamic_cast<cv::dnn::Model*>(ptr);
	return ptr;
#else
	throw_no_dnn();
#endif
}

cv::dnn::TextRecognitionModel* cveDnnTextRecognitionModelCreate2(cv::dnn::Net* network, cv::dnn::Model** baseModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::TextRecognitionModel* ptr = new cv::dnn::TextRecognitionModel(*network);
	*baseModel = dynamic_cast<cv::dnn::Model*>(ptr);
	return ptr;
#else
	throw_no_dnn();
#endif
}

void cveDnnTextRecognitionModelRelease(cv::dnn::TextRecognitionModel** textRecognitionModel)
{
#ifdef HAVE_OPENCV_DNN
	delete* textRecognitionModel;
	*textRecognitionModel = 0;
#else
	throw_no_dnn();
#endif
}

void cveDnnTextRecognitionModelSetDecodeOptsCTCPrefixBeamSearch(cv::dnn::TextRecognitionModel* textRecognitionModel, int beamSize, int vocPruneSize)
{
#ifdef HAVE_OPENCV_DNN
	textRecognitionModel->setDecodeOptsCTCPrefixBeamSearch(beamSize, vocPruneSize);
#else
	throw_no_dnn();
#endif
}

void cveDnnTextRecognitionModelSetVocabulary(cv::dnn::TextRecognitionModel* textRecognitionModel, std::vector< std::string >* vocabulary)
{
#ifdef HAVE_OPENCV_DNN
	textRecognitionModel->setVocabulary(*vocabulary);
#else
	throw_no_dnn();
#endif
}

void cveDnnTextRecognitionModelGetVocabulary(cv::dnn::TextRecognitionModel* textRecognitionModel, std::vector< std::string >* vocabulary)
{
#ifdef HAVE_OPENCV_DNN
	vocabulary->clear();
	std::vector< std::string > result = textRecognitionModel->getVocabulary();
	for (std::vector< std::string >::iterator iter = result.begin(); iter != result.end(); ++iter)
	{
		vocabulary->push_back(*iter);
	}
#else
	throw_no_dnn();
#endif
}

void cveDnnTextRecognitionModelRecognize1(cv::dnn::TextRecognitionModel* textRecognitionModel, cv::_InputArray* frame, cv::String* text)
{
#ifdef HAVE_OPENCV_DNN
	std::string s = textRecognitionModel->recognize(*frame);
	*text = s;
#else
	throw_no_dnn();
#endif
}

void cveDnnTextRecognitionModelRecognize2(
	cv::dnn::TextRecognitionModel* textRecognitionModel,
	cv::_InputArray* frame,
	cv::_InputArray* roiRects,
	std::vector< std::string >* results)
{
#ifdef HAVE_OPENCV_DNN
	textRecognitionModel->recognize(*frame, *roiRects, *results);
#else
	throw_no_dnn();
#endif
}

cv::dnn::Model* cveModelCreate(cv::String* model, cv::String* config)
{
#ifdef HAVE_OPENCV_DNN
	return new cv::dnn::Model(*model, *config);
#else
	throw_no_dnn();
#endif
}
cv::dnn::Model* cveModelCreateFromNet(cv::dnn::Net* network)
{
#ifdef HAVE_OPENCV_DNN
	return new cv::dnn::Model(*network);
#else
	throw_no_dnn();
#endif
}
void cveModelRelease(cv::dnn::Model** model)
{
#ifdef HAVE_OPENCV_DNN
	delete* model;
	*model = 0;
#else
	throw_no_dnn();
#endif
}
void cveModelPredict(cv::dnn::Model* model, cv::_InputArray* frame, cv::_OutputArray* outs)
{
#ifdef HAVE_OPENCV_DNN
	model->predict(*frame, *outs);
#else
	throw_no_dnn();
#endif
}

void cveModelSetInputMean(cv::dnn::Model* model, CvScalar* mean)
{
#ifdef HAVE_OPENCV_DNN
	model->setInputMean(*mean);
#else
	throw_no_dnn();
#endif
}
void cveModelSetInputScale(cv::dnn::Model* model, double value)
{
#ifdef HAVE_OPENCV_DNN
	model->setInputScale(value);
#else
	throw_no_dnn();
#endif
}
void cveModelSetInputSize(cv::dnn::Model* model, CvSize* size)
{
#ifdef HAVE_OPENCV_DNN
	model->setInputSize(*size);
#else
	throw_no_dnn();
#endif
}
void cveModelSetInputCrop(cv::dnn::Model* model, bool crop)
{
#ifdef HAVE_OPENCV_DNN
	model->setInputCrop(crop);
#else
	throw_no_dnn();
#endif
}
void cveModelSetInputSwapRB(cv::dnn::Model* model, bool swapRB)
{
#ifdef HAVE_OPENCV_DNN
	model->setInputSwapRB(swapRB);
#else
	throw_no_dnn();
#endif
}
void cveModelSetPreferableBackend(cv::dnn::Model* model, int backendId)
{
#ifdef HAVE_OPENCV_DNN
	model->setPreferableBackend((cv::dnn::Backend) backendId);
#else
	throw_no_dnn();
#endif
}
void cveModelSetPreferableTarget(cv::dnn::Model* model, int targetId)
{
#ifdef HAVE_OPENCV_DNN
	model->setPreferableTarget((cv::dnn::Target) targetId);
#else
	throw_no_dnn();
#endif
}

cv::dnn::DetectionModel* cveDnnDetectionModelCreate1(cv::String* model, cv::String* config, cv::dnn::Model** baseModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::DetectionModel* ptr = new cv::dnn::DetectionModel(*model, *config);
	*baseModel = dynamic_cast<cv::dnn::Model*>(ptr);
	return ptr;
#else
	throw_no_dnn();
#endif
}
cv::dnn::DetectionModel* cveDnnDetectionModelCreate2(cv::dnn::Net* network, cv::dnn::Model** baseModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::DetectionModel* ptr = new cv::dnn::DetectionModel(*network);
	*baseModel = dynamic_cast<cv::dnn::Model*>(ptr);
	return ptr;
#else
	throw_no_dnn();
#endif
}
void cveDnnDetectionModelRelease(cv::dnn::DetectionModel** detectionModel)
{
#ifdef HAVE_OPENCV_DNN
	delete* detectionModel;
	*detectionModel = 0;
#else
	throw_no_dnn();
#endif
}

void cveDnnDetectionModelDetect(
	cv::dnn::DetectionModel* detectionModel,
	cv::_InputArray* frame,
	std::vector< int >* classIds,
	std::vector< float >* confidences,
	std::vector< cv::Rect >* boxes,
	float confThreshold,
	float nmsThreshold)
{
#ifdef HAVE_OPENCV_DNN
	detectionModel->detect(*frame, *classIds, *confidences, *boxes, confThreshold, nmsThreshold);
#else
	throw_no_dnn();
#endif
}

cv::dnn::ClassificationModel* cveDnnClassificationModelCreate1(cv::String* model, cv::String* config, cv::dnn::Model** baseModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::ClassificationModel* ptr = new cv::dnn::ClassificationModel(*model, *config);
	*baseModel = dynamic_cast<cv::dnn::Model*>(ptr);
	return ptr;
#else
	throw_no_dnn();
#endif
}
cv::dnn::ClassificationModel* cveDnnClassificationModelCreate2(cv::dnn::Net* network, cv::dnn::Model** baseModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::ClassificationModel* ptr = new cv::dnn::ClassificationModel(*network);
	*baseModel = dynamic_cast<cv::dnn::Model*>(ptr);
	return ptr;
#else
	throw_no_dnn();
#endif
}
void cveDnnClassificationModelRelease(cv::dnn::ClassificationModel** classificationModel)
{
#ifdef HAVE_OPENCV_DNN
	delete* classificationModel;
	*classificationModel = 0;
#else
	throw_no_dnn();
#endif
}
void cveDnnClassificationModelClassify(
	cv::dnn::ClassificationModel* classificationModel,
	cv::_InputArray* frame,
	int* classId,
	float* conf)
{
#ifdef HAVE_OPENCV_DNN
	int outClassId;
	float outConf;
	classificationModel->classify(*frame, outClassId, outConf);
	*classId = outClassId;
	*conf = outConf;
#else
	throw_no_dnn();
#endif
}

cv::dnn::KeypointsModel* cveDnnKeypointsModelCreate1(cv::String* model, cv::String* config, cv::dnn::Model** baseModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::KeypointsModel* ptr = new cv::dnn::KeypointsModel(*model, *config);
	*baseModel = dynamic_cast<cv::dnn::Model*>(ptr);
	return ptr;
#else
	throw_no_dnn();
#endif
}
cv::dnn::KeypointsModel* cveDnnKeypointsModelCreate2(cv::dnn::Net* network, cv::dnn::Model** baseModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::KeypointsModel* ptr = new cv::dnn::KeypointsModel(*network);
	*baseModel = dynamic_cast<cv::dnn::Model*>(ptr);
	return ptr;
#else
	throw_no_dnn();
#endif
}
void cveDnnKeypointsModelRelease(cv::dnn::KeypointsModel** keypointsModel)
{
#ifdef HAVE_OPENCV_DNN
	delete* keypointsModel;
	*keypointsModel = 0;
#else
	throw_no_dnn();
#endif
}
void cveDnnKeypointsModelEstimate(
	cv::dnn::KeypointsModel* keypointsModel,
	cv::_InputArray* frame,
	std::vector< cv::Point2f >* keypoints,
	float thresh)
{
#ifdef HAVE_OPENCV_DNN
	std::vector< cv::Point2f > kp = keypointsModel->estimate(*frame, thresh);
	keypoints->clear();
	for (std::vector< cv::Point2f >::iterator iter = kp.begin(); iter != kp.end(); ++iter)
		keypoints->push_back(*iter);
#else
	throw_no_dnn();
#endif	
}


cv::dnn::SegmentationModel* cveDnnSegmentationModelCreate1(cv::String* model, cv::String* config, cv::dnn::Model** baseModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::SegmentationModel* ptr = new cv::dnn::SegmentationModel(*model, *config);
	*baseModel = dynamic_cast<cv::dnn::Model*>(ptr);
	return ptr;
#else
	throw_no_dnn();
#endif	
}
cv::dnn::SegmentationModel* cveDnnSegmentationModelCreate2(cv::dnn::Net* network, cv::dnn::Model** baseModel)
{
#ifdef HAVE_OPENCV_DNN
	cv::dnn::SegmentationModel* ptr = new cv::dnn::SegmentationModel(*network);
	*baseModel = dynamic_cast<cv::dnn::Model*>(ptr);
	return ptr;
#else
	throw_no_dnn();
#endif
}
void cveDnnSegmentationModelRelease(cv::dnn::SegmentationModel** segmentationModel)
{
#ifdef HAVE_OPENCV_DNN
	delete* segmentationModel;
	*segmentationModel = 0;
#else
	throw_no_dnn();
#endif
}
void cveDnnSegmentationModelSegment(
	cv::dnn::SegmentationModel* segmentationModel,
	cv::_InputArray* frame,
	cv::_OutputArray* mask)
{
#ifdef HAVE_OPENCV_DNN
	segmentationModel->segment(*frame, *mask);
#else
	throw_no_dnn();
#endif
}