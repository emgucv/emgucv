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
CVAPI(cv::face::FaceRecognizer*) cveEigenFaceRecognizerCreate(int numComponents, double threshold);   
CVAPI(cv::face::FaceRecognizer*) cveFisherFaceRecognizerCreate(int numComponents, double threshold);
CVAPI(cv::face::FaceRecognizer*) cveLBPHFaceRecognizerCreate(int radius, int neighbors, int gridX, int gridY, double threshold);
CVAPI(void) cveFaceRecognizerTrain(cv::face::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels);
CVAPI(void) cveFaceRecognizerUpdate(cv::face::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels);
CVAPI(void) cveFaceRecognizerPredict(cv::face::FaceRecognizer* recognizer, cv::_InputArray* image, int* label, double* distance);
CVAPI(void) cveFaceRecognizerWrite(cv::face::FaceRecognizer* recognizer, cv::String* fileName);
CVAPI(void) cveFaceRecognizerRead(cv::face::FaceRecognizer* recognizer, cv::String* fileName);
CVAPI(void) cveFaceRecognizerRelease(cv::face::FaceRecognizer** recognizer);

CVAPI(cv::face::BIF*) cveBIFCreate(int numBands, int numRotations);
CVAPI(void) cveBIFCompute(cv::face::BIF* bif, cv::_InputArray* image, cv::_OutputArray* features);
CVAPI(void) cveBIFRelease(cv::face::BIF** bif);


CVAPI(cv::face::FacemarkAAM::Params*) cveFacemarkAAMParamsCreate();
CVAPI(void) cveFacemarkAAMParamsRelease(cv::face::FacemarkAAM::Params** params);

CVAPI(cv::face::FacemarkAAM*) cveFacemarkAAMCreate(cv::face::FacemarkAAM::Params* parameters, cv::face::Facemark** facemark, cv::Algorithm** algorithm);
CVAPI(void) cveFacemarkAAMRelease(cv::face::FacemarkAAM** facemark);

CVAPI(cv::face::FacemarkLBF::Params*) cveFacemarkLBFParamsCreate();
CVAPI(void) cveFacemarkLBFParamsRelease(cv::face::FacemarkLBF::Params** params);

CVAPI(cv::face::FacemarkLBF*) cveFacemarkLBFCreate(cv::face::FacemarkLBF::Params* parameters, cv::face::Facemark** facemark, cv::Algorithm** algorithm);
CVAPI(void) cveFacemarkLBFRelease(cv::face::FacemarkLBF** facemark);

/*
CVAPI(cv::face::FacemarkKazemi::Params*) cveFacemarkKazemiParamsCreate();
CVAPI(void) cveFacemarkKazemiParamsRelease(cv::face::FacemarkKazemi::Params** params);

CVAPI(cv::face::FacemarkKazemi*) cveFacemarkKazemiCreate(cv::face::FacemarkKazemi::Params* parameters, cv::face::Facemark** facemark, cv::Algorithm** algorithm);
CVAPI(void) cveFacemarkKazemiRelease(cv::face::FacemarkKazemi** facemark);
*/

typedef bool(*CSharp_FaceDetector)(const cv::_InputArray*, const cv::_OutputArray*);
CVAPI(bool) cveFacemarkSetFaceDetector(cv::face::Facemark* facemark, CSharp_FaceDetector detector);

CVAPI(void) cveFacemarkLoadModel(cv::face::Facemark* facemark, cv::String* model);
CVAPI(bool) cveFacemarkGetFaces(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_OutputArray* faces);
CVAPI(bool) cveFacemarkFit(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_InputArray* faces, cv::_InputOutputArray* landmarks);
CVAPI(bool) cveFacemarkAddTrainingSample(cv::face::Facemark* facemark, cv::_InputArray* image, cv::_InputArray* landmarks);
CVAPI(void) cveFacemarkTraining(cv::face::Facemark* facemark);

CVAPI(void) cveDrawFacemarks(cv::_InputOutputArray* image, cv::_InputArray* points,	CvScalar* color);
#endif