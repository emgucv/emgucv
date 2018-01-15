//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_FACE_C_H
#define EMGU_FACE_C_H

#include "opencv2/core/core_c.h"
//#include "opencv2/face/facerec.hpp"
#include "opencv2/face/bif.hpp"
#include "opencv2/face.hpp"

//FaceRecognizer
CVAPI(cv::face::FaceRecognizer*) CvEigenFaceRecognizerCreate(int numComponents, double threshold);   
CVAPI(cv::face::FaceRecognizer*) CvFisherFaceRecognizerCreate(int numComponents, double threshold);
CVAPI(cv::face::FaceRecognizer*) CvLBPHFaceRecognizerCreate(int radius, int neighbors, int gridX, int gridY, double threshold);
CVAPI(void) CvFaceRecognizerTrain(cv::face::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels);
CVAPI(void) CvFaceRecognizerUpdate(cv::face::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels);
CVAPI(void) CvFaceRecognizerPredict(cv::face::FaceRecognizer* recognizer, cv::_InputArray* image, int* label, double* distance);
CVAPI(void) CvFaceRecognizerWrite(cv::face::FaceRecognizer* recognizer, cv::String* fileName);
CVAPI(void) CvFaceRecognizerRead(cv::face::FaceRecognizer* recognizer, cv::String* fileName);
CVAPI(void) CvFaceRecognizerRelease(cv::face::FaceRecognizer** recognizer);

CVAPI(cv::face::BIF*) cveBIFCreate(int numBands, int numRotations);
CVAPI(void) cveBIFCompute(cv::face::BIF* bif, cv::_InputArray* image, cv::_OutputArray* features);
CVAPI(void) cveBIFRelease(cv::face::BIF** bif);

CVAPI(cv::face::FacemarkLBF::Params*) cveFacemarkLBFParamsCreate();
CVAPI(void) cveFacemarkLBFParamsRelease(cv::face::FacemarkLBF::Params** params);

CVAPI(cv::face::FacemarkLBF*) cveFacemarkLBFCreate(cv::face::FacemarkLBF::Params* parameters, cv::face::Facemark** facemark);
CVAPI(void) cveFacemarkLBFRelease(cv::face::FacemarkLBF** facemark);

typedef bool(*CSharp_FaceDetector)(const cv::_InputArray*, const cv::_OutputArray*);
CVAPI(bool) cveFacemarkSetFaceDetector(cv::face::Facemark* facemark, CSharp_FaceDetector detector);
#endif