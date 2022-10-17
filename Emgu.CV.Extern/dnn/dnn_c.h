//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
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
static inline CV_NORETURN void throw_no_dnn() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without DNN support. To use this module, please switch to the full Emgu CV runtime."); }

namespace cv
{
	namespace dnn
	{
		class Net {};

		class Layer	{};

		class Model {};

		class DetectionModel {};
		
		class ClassificationModel {};

		class SegmentationModel {};
		
		class TextDetectionModel {};

		class TextDetectionModel_EAST {};

		class TextDetectionModel_DB {};

		class TextRecognitionModel {};
		
		class KeypointsModel {};
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

CVAPI(void) cveDnnNMSBoxes2(
	std::vector<cv::RotatedRect>* bboxes,
	std::vector<float>* scores,
	float scoreThreshold,
	float nmsThreshold,
	std::vector<int>* indices,
	float eta,
	int topK);

CVAPI(void) cveDnnSoftNMSBoxes(
	std::vector<cv::Rect>* bboxes,
	std::vector<float>* scores,
	std::vector<float>* updatedScores,
	float scoreThreshold,
	float nmsThreshold,
	std::vector<int>* indices,
	int topK,
	float sigma,
	int method);

CVAPI(void) cveDNNGetAvailableBackends(std::vector<int>* backends, std::vector<int>* targets);

CVAPI(cv::dnn::TextDetectionModel_DB*) cveDnnTextDetectionModelDbCreate1(cv::String* model, cv::String* config, cv::dnn::TextDetectionModel** textDetectionModel, cv::dnn::Model** baseModel);
CVAPI(cv::dnn::TextDetectionModel_DB*) cveDnnTextDetectionModelDbCreate2(cv::dnn::Net* network, cv::dnn::TextDetectionModel** textDetectionModel, cv::dnn::Model** baseModel);
CVAPI(void) cveDnnTextDetectionModelDbRelease(cv::dnn::TextDetectionModel_DB** textDetectionModel);

CVAPI(cv::dnn::TextDetectionModel_EAST*) cveDnnTextDetectionModelEastCreate1(cv::String* model, cv::String* config, cv::dnn::TextDetectionModel** textDetectionModel, cv::dnn::Model** baseModel);
CVAPI(cv::dnn::TextDetectionModel_EAST*) cveDnnTextDetectionModelEastCreate2(cv::dnn::Net* network, cv::dnn::TextDetectionModel** textDetectionModel, cv::dnn::Model** baseModel);
CVAPI(void) cveDnnTextDetectionModelEastRelease(cv::dnn::TextDetectionModel_EAST** textDetectionModel);

CVAPI(void) cveDnnTextDetectionModelDetect(
	cv::dnn::TextDetectionModel* textDetectionModel,
	cv::_InputArray* frame,
	std::vector< std::vector< cv::Point > >* detections,
	std::vector<float>* confidences
);
CVAPI(void) cveDnnTextDetectionModelDetectTextRectangles(
	cv::dnn::TextDetectionModel* textDetectionModel,
	cv::_InputArray* frame,
	std::vector< cv::RotatedRect >* detections,
	std::vector< float >* confidences
);


CVAPI(cv::dnn::TextRecognitionModel*) cveDnnTextRecognitionModelCreate1(cv::String* model, cv::String* config, cv::dnn::Model** baseModel);
CVAPI(cv::dnn::TextRecognitionModel*) cveDnnTextRecognitionModelCreate2(cv::dnn::Net* network, cv::dnn::Model** baseModel);
CVAPI(void) cveDnnTextRecognitionModelRelease(cv::dnn::TextRecognitionModel** textRecognitionModel);
CVAPI(void) cveDnnTextRecognitionModelSetDecodeOptsCTCPrefixBeamSearch(cv::dnn::TextRecognitionModel* textRecognitionModel, int beamSize, int vocPruneSize);
CVAPI(void) cveDnnTextRecognitionModelSetVocabulary(cv::dnn::TextRecognitionModel* textRecognitionModel, std::vector< std::string >* vocabulary);
CVAPI(void) cveDnnTextRecognitionModelGetVocabulary(cv::dnn::TextRecognitionModel* textRecognitionModel, std::vector< std::string >* vocabulary);
CVAPI(void) cveDnnTextRecognitionModelRecognize1(cv::dnn::TextRecognitionModel* textRecognitionModel, cv::_InputArray* frame, cv::String* text);
CVAPI(void) cveDnnTextRecognitionModelRecognize2(
	cv::dnn::TextRecognitionModel* textRecognitionModel,
	cv::_InputArray* frame, 
	cv::_InputArray* roiRects, 
	std::vector< std::string >* results);


CVAPI(cv::dnn::Model*) cveModelCreate(cv::String* model, cv::String* config);
CVAPI(cv::dnn::Model*) cveModelCreateFromNet(cv::dnn::Net* network);
CVAPI(void) cveModelRelease(cv::dnn::Model** model);
CVAPI(void) cveModelPredict(cv::dnn::Model* model, cv::_InputArray* frame, cv::_OutputArray* outs);

CVAPI(void) cveModelSetInputMean(cv::dnn::Model* model, CvScalar* mean);
CVAPI(void) cveModelSetInputScale(cv::dnn::Model* model, double value);
CVAPI(void) cveModelSetInputSize(cv::dnn::Model* model, CvSize* size);
CVAPI(void) cveModelSetInputCrop(cv::dnn::Model* model, bool crop);
CVAPI(void) cveModelSetInputSwapRB(cv::dnn::Model* model, bool swapRB);
CVAPI(void) cveModelSetPreferableBackend(cv::dnn::Model* model, int backendId);
CVAPI(void) cveModelSetPreferableTarget(cv::dnn::Model* model, int targetId);

CVAPI(cv::dnn::DetectionModel*) cveDnnDetectionModelCreate1(cv::String* model, cv::String* config, cv::dnn::Model** baseModel);
CVAPI(cv::dnn::DetectionModel*) cveDnnDetectionModelCreate2(cv::dnn::Net* network, cv::dnn::Model** baseModel);
CVAPI(void) cveDnnDetectionModelRelease(cv::dnn::DetectionModel** detectionModel);
CVAPI(void) cveDnnDetectionModelDetect(
	cv::dnn::DetectionModel* detectionModel,
	cv::_InputArray* frame, 
	std::vector< int >* classIds,
	std::vector< float >* confidences,
	std::vector< cv::Rect >* boxes,
	float confThreshold, 
	float nmsThreshold);

CVAPI(cv::dnn::ClassificationModel*) cveDnnClassificationModelCreate1(cv::String* model, cv::String* config, cv::dnn::Model** baseModel);
CVAPI(cv::dnn::ClassificationModel*) cveDnnClassificationModelCreate2(cv::dnn::Net* network, cv::dnn::Model** baseModel);
CVAPI(void) cveDnnClassificationModelRelease(cv::dnn::ClassificationModel** classificationModel);
CVAPI(void) cveDnnClassificationModelClassify(
	cv::dnn::ClassificationModel* classificationModel,
	cv::_InputArray* frame,
	int* classId, 
	float* conf);


CVAPI(cv::dnn::KeypointsModel*) cveDnnKeypointsModelCreate1(cv::String* model, cv::String* config, cv::dnn::Model** baseModel);
CVAPI(cv::dnn::KeypointsModel*) cveDnnKeypointsModelCreate2(cv::dnn::Net* network, cv::dnn::Model** baseModel);
CVAPI(void) cveDnnKeypointsModelRelease(cv::dnn::KeypointsModel** keypointsModel);
CVAPI(void) cveDnnKeypointsModelEstimate(
	cv::dnn::KeypointsModel* keypointsModel,
	cv::_InputArray* frame,
	std::vector< cv::Point2f >* keypoints,
	float thresh);


CVAPI(cv::dnn::SegmentationModel*) cveDnnSegmentationModelCreate1(cv::String* model, cv::String* config, cv::dnn::Model** baseModel);
CVAPI(cv::dnn::SegmentationModel*) cveDnnSegmentationModelCreate2(cv::dnn::Net* network, cv::dnn::Model** baseModel);
CVAPI(void) cveDnnSegmentationModelRelease(cv::dnn::SegmentationModel** segmentationModel);
CVAPI(void) cveDnnSegmentationModelSegment(
	cv::dnn::SegmentationModel* segmentationModel,
	cv::_InputArray* frame,
	cv::_OutputArray* mask);

#endif