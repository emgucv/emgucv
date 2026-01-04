//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_OBJDETECT_C_H
#define EMGU_OBJDETECT_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_OBJDETECT
#include "opencv2/objdetect/objdetect.hpp"
//#include "opencv2/objdetect/objdetect_c.h"
#else
static inline CV_NORETURN void throw_no_objdetect() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without objdetect support. To use this module, please switch to the full Emgu CV runtime."); }
namespace cv
{
    class HOGDescriptor {};
    class CascadeClassifier {};
    class GraphicalCodeDetector {};
    class QRCodeEncoder {};
    class QRCodeDetector {};
    class QRCodeDetectorAruco {};
    class FaceDetectorYN {};
    class FaceRecognizerSF {};

    namespace barcode
    {
        class BarcodeDetector {};
    }
}
#endif
#include "vectors_c.h"

CVAPI(void) cveHOGDescriptorPeopleDetectorCreate(std::vector<float>* seq);

CVAPI(cv::HOGDescriptor*) cveHOGDescriptorCreateDefault();

CVAPI(cv::HOGDescriptor*) cveHOGDescriptorCreate(
   CvSize* _winSize, 
   CvSize* _blockSize, 
   CvSize* _blockStride,
   CvSize* _cellSize, 
   int _nbins, 
   int _derivAperture, 
   double _winSigma,
   int _histogramNormType, 
   double _L2HysThreshold, 
   bool _gammaCorrection);

CVAPI(void) cveHOGSetSVMDetector(cv::HOGDescriptor* descriptor, std::vector<float>* vector);

CVAPI(void) cveHOGDescriptorRelease(cv::HOGDescriptor** descriptor);

CVAPI(void) cveHOGDescriptorDetectMultiScale(
   cv::HOGDescriptor* descriptor, 
   cv::_InputArray* img, 
   std::vector<cv::Rect>* foundLocations,
   std::vector<double>* weights,
   double hitThreshold, 
   CvSize* winStride,
   CvSize* padding, 
   double scale,
   double finalThreshold, 
   bool useMeanshiftGrouping);

CVAPI(void) cveHOGDescriptorCompute(
    cv::HOGDescriptor *descriptor,
    cv::_InputArray* img, 
    std::vector<float> *descriptors,
    CvSize* winStride,
    CvSize* padding,
    std::vector< cv::Point >* locations);

/*
CVAPI(void) cvHOGDescriptorDetect(
   cv::HOGDescriptor* descriptor, 
   CvArr* img, 
   CvSeq* foundLocations,
   double hitThreshold, 
   CvSize winStride,
   CvSize padding)
{
   cvClearSeq(foundLocations);

   std::vector<cv::Point> hits;
   cv::Mat mat = cv::cvarrToMat(img);
   descriptor->detect(mat, hits, hitThreshold, winStride, padding);
   cvSeqPushMulti(foundLocations, &hits.front(), hits.size());
}*/

CVAPI(unsigned int) cveHOGDescriptorGetDescriptorSize(cv::HOGDescriptor* descriptor);

CVAPI(cv::CascadeClassifier*) cveCascadeClassifierCreate();
CVAPI(cv::CascadeClassifier*) cveCascadeClassifierCreateFromFile(cv::String* fileName);
CVAPI(bool) cveCascadeClassifierRead(cv::CascadeClassifier* classifier, cv::FileNode* node);
CVAPI(void) cveCascadeClassifierRelease(cv::CascadeClassifier** classifier);
CVAPI(void) cveCascadeClassifierDetectMultiScale( 
   cv::CascadeClassifier* classifier,
   cv::_InputArray* image,
   std::vector<cv::Rect>* objects,
   double scaleFactor,
   int minNeighbors, int flags,
   CvSize* minSize,
   CvSize* maxSize); 
CVAPI(bool) cveCascadeClassifierIsOldFormatCascade(cv::CascadeClassifier* classifier);
CVAPI(void) cveCascadeClassifierGetOriginalWindowSize(cv::CascadeClassifier* classifier, CvSize* size);

CVAPI(void) cveGroupRectangles1(std::vector< cv::Rect >* rectList, int groupThreshold, double eps);
CVAPI(void) cveGroupRectangles2(std::vector<cv::Rect>* rectList, std::vector<int>* weights,	int groupThreshold, double eps);
CVAPI(void) cveGroupRectangles3(std::vector<cv::Rect>* rectList, int groupThreshold, double eps, std::vector<int>* weights, std::vector<double>* levelWeights);
CVAPI(void) cveGroupRectangles4(std::vector<cv::Rect>* rectList, std::vector<int>* rejectLevels, std::vector<double>* levelWeights, int groupThreshold, double eps);
CVAPI(void) cveGroupRectanglesMeanshift(std::vector<cv::Rect>* rectList, std::vector<double>* foundWeights,	std::vector<double>* foundScales, double detectThreshold, CvSize* winDetSize);

CVAPI(cv::QRCodeDetector*) cveQRCodeDetectorCreate(cv::GraphicalCodeDetector** graphicalCodeDetector);
CVAPI(void) cveQRCodeDetectorRelease(cv::QRCodeDetector** detector);
CVAPI(void) cveQRCodeDetectorDecodeCurved(cv::QRCodeDetector* detector, cv::_InputArray* img, cv::_InputArray* points, cv::String* decodedInfo, cv::_OutputArray* straightCode);
CVAPI(int) cveQRCodeDetectorGetEncoding(cv::QRCodeDetector* detector, int codeIdx);

CVAPI(cv::QRCodeEncoder*) cveQRCodeEncoderCreate(
    cv::Ptr<cv::QRCodeEncoder>** sharedPtr,
    int version,
    int correctionLevel,
    int mode,
    int structureNumber);
CVAPI(void) cveQRCodeEncoderRelease(cv::QRCodeEncoder** encoder, cv::Ptr<cv::QRCodeEncoder>** sharedPtr);
CVAPI(void) cveQRCodeEncoderEncode(cv::QRCodeEncoder* encoder, cv::String* encodedInfo, cv::_OutputArray* qrcode);

CVAPI(cv::QRCodeDetectorAruco*) cveQRCodeDetectorArucoCreate(cv::GraphicalCodeDetector** graphicalCodeDetector);
CVAPI(void) cveQRCodeDetectorArucoRelease(cv::QRCodeDetectorAruco** detector);

CVAPI(cv::barcode::BarcodeDetector*) cveBarcodeDetectorCreate(
    cv::String* prototxtPath,
    cv::String* modelPath,
    cv::GraphicalCodeDetector** graphicalCodeDetector);
CVAPI(void) cveBarcodeDetectorRelease(cv::barcode::BarcodeDetector** detector);

CVAPI(bool) cveGraphicalCodeDetectorDetect(cv::GraphicalCodeDetector* detector, cv::_InputArray* img, cv::_OutputArray* points);
CVAPI(bool) cveGraphicalCodeDetectorDetectMulti(cv::GraphicalCodeDetector* detector, cv::_InputArray* img, cv::_OutputArray* points);
CVAPI(void) cveGraphicalCodeDetectorDecode(cv::GraphicalCodeDetector* detector, cv::_InputArray* img, cv::_InputArray* points, cv::_OutputArray* straightCode, cv::String* output);
CVAPI(bool) cveGraphicalCodeDetectorDecodeMulti(
    cv::GraphicalCodeDetector* detector,
    cv::_InputArray* img,
    cv::_InputArray* points,
    std::vector< std::string >* decodedInfo,
    cv::_OutputArray* straightCode);
CVAPI(bool) cveGraphicalCodeDetectorDetectAndDecodeMulti(
    cv::GraphicalCodeDetector* detector,
    cv::_InputArray* img,
    std::vector< std::string >* decodedInfo,
    cv::_OutputArray* points,
    cv::_OutputArray* straightCode);


CVAPI(cv::FaceDetectorYN*) cveFaceDetectorYNCreate(
    cv::String* model,
    cv::String* config,
    CvSize* inputSize,
    float scoreThreshold,
    float nmsThreshold,
    int topK,
    int backendId,
    int targetId,
    cv::Ptr<cv::FaceDetectorYN>** sharedPtr);
CVAPI(void) cveFaceDetectorYNRelease(cv::Ptr<cv::FaceDetectorYN>** faceDetector);
CVAPI(int) cveFaceDetectorYNDetect(cv::FaceDetectorYN* faceDetector, cv::_InputArray* image, cv::_OutputArray* faces);


CVAPI(cv::FaceRecognizerSF*) cveFaceRecognizerSFCreate(
    cv::String* model, 
    cv::String* config, 
    int backendId, 
    int targetId, 
    cv::Ptr<cv::FaceRecognizerSF>** sharedPtr);
CVAPI(void) cveFaceRecognizerSFRelease(cv::Ptr<cv::FaceRecognizerSF>** faceRecognizer);
CVAPI(void) cveFaceRecognizerSFAlignCrop(cv::FaceRecognizerSF* faceRecognizer, cv::_InputArray* srcImg, cv::_InputArray* faceBox, cv::_OutputArray* alignedImg);
CVAPI(void) cveFaceRecognizerSFFeature(cv::FaceRecognizerSF* faceRecognizer, cv::_InputArray* alignedImg, cv::_OutputArray* faceFeature);
CVAPI(double) cveFaceRecognizerSFMatch(cv::FaceRecognizerSF* faceRecognizer, cv::_InputArray* faceFeature1, cv::_InputArray* faceFeature2, int disType);
#endif