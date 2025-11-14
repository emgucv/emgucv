//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_OBJDETECT_C_H
#define EMGU_OBJDETECT_C_H

#include "opencv2/core.hpp"
#include "cvapi_compat.h"

#ifdef HAVE_OPENCV_OBJDETECT
#include "opencv2/objdetect/objdetect.hpp"
//#include "opencv2/objdetect/objdetect_c.h"
#else
static inline CV_NORETURN void throw_no_objdetect() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without objdetect support. To use this module, please switch to the full Emgu CV runtime."); }
namespace cv
{

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




CVAPI(void) cveGroupRectangles1(std::vector< cv::Rect >* rectList, int groupThreshold, double eps);
CVAPI(void) cveGroupRectangles2(std::vector<cv::Rect>* rectList, std::vector<int>* weights,	int groupThreshold, double eps);
CVAPI(void) cveGroupRectangles3(std::vector<cv::Rect>* rectList, int groupThreshold, double eps, std::vector<int>* weights, std::vector<double>* levelWeights);
CVAPI(void) cveGroupRectangles4(std::vector<cv::Rect>* rectList, std::vector<int>* rejectLevels, std::vector<double>* levelWeights, int groupThreshold, double eps);
CVAPI(void) cveGroupRectanglesMeanshift(std::vector<cv::Rect>* rectList, std::vector<double>* foundWeights,	std::vector<double>* foundScales, double detectThreshold, cv::Size* winDetSize);

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
    cv::Size* inputSize,
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