//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_FACE_C_H
#define EMGU_FACE_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/face/facerec.hpp"

//FaceRecognizer
CVAPI(cv::face::FaceRecognizer*) CvEigenFaceRecognizerCreate(int numComponents, double threshold);   
CVAPI(cv::face::FaceRecognizer*) CvFisherFaceRecognizerCreate(int numComponents, double threshold);
CVAPI(cv::face::FaceRecognizer*) CvLBPHFaceRecognizerCreate(int radius, int neighbors, int gridX, int gridY, double threshold);
CVAPI(void) CvFaceRecognizerTrain(cv::face::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels);
CVAPI(void) CvFaceRecognizerUpdate(cv::face::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels);
CVAPI(void) CvFaceRecognizerPredict(cv::face::FaceRecognizer* recognizer, cv::_InputArray* image, int* label, double* distance);
CVAPI(void) CvFaceRecognizerSave(cv::face::FaceRecognizer* recognizer, cv::String* fileName);
CVAPI(void) CvFaceRecognizerLoad(cv::face::FaceRecognizer* recognizer, cv::String* fileName);
CVAPI(void) CvFaceRecognizerRelease(cv::face::FaceRecognizer** recognizer);

#endif