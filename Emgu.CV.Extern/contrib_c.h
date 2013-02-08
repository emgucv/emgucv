//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CONTRIB_C_H
#define EMGU_CONTRIB_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/contrib/contrib.hpp"
#include "opencv2/contrib/retina.hpp"

///Octree
CVAPI(cv::Octree*) CvOctreeCreate();

CVAPI(void) CvOctreeBuildTree(cv::Octree* tree, cv::Point3f* points, int numberOfPoints, int maxLevels, int minPoints );

CVAPI(void) CvOctreeGetPointsWithinSphere(cv::Octree* tree, cv::Point3f* center, float radius, CvSeq* pointSeq );

CVAPI(void) CvOctreeRelease(cv::Octree* tree);

//CvAdaptiveSkinDetector
CVAPI(CvAdaptiveSkinDetector*) CvAdaptiveSkinDetectorCreate(int samplingDivider, int morphingMethod);
CVAPI(void) CvAdaptiveSkinDetectorRelease(CvAdaptiveSkinDetector* detector);
CVAPI(void) CvAdaptiveSkinDetectorProcess(CvAdaptiveSkinDetector* detector, IplImage *inputBGRImage, IplImage *outputHueMask);

//Retina
CVAPI(cv::Retina*) CvRetinaCreate(CvSize inputSize, const bool colorMode, int colorSamplingMethod, const bool useRetinaLogSampling, const double reductionFactor, const double samplingStrength);
CVAPI(void) CvRetinaRelease(cv::Retina** retina);
CVAPI(void) CvRetinaRun(cv::Retina* retina, IplImage* image);
CVAPI(void) CvRetinaGetParvo(cv::Retina* retina, IplImage* parvo);
CVAPI(void) CvRetinaGetMagno(cv::Retina* retina, IplImage* magno);
CVAPI(void) CvRetinaClearBuffers(cv::Retina* retina);
CVAPI(void) CvRetinaGetParameters(cv::Retina* retina, cv::Retina::RetinaParameters* p);
CVAPI(void) CvRetinaSetParameters(cv::Retina* retina, cv::Retina::RetinaParameters* p);

//FaceRecognizer
CVAPI(cv::FaceRecognizer*) CvEigenFaceRecognizerCreate(int numComponents, double threshold);   
CVAPI(cv::FaceRecognizer*) CvFisherFaceRecognizerCreate(int numComponents, double threshold);
CVAPI(cv::FaceRecognizer*) CvLBPHFaceRecognizerCreate(int radius, int neighbors, int gridX, int gridY, double threshold);
CVAPI(void) CvFaceRecognizerTrain(cv::FaceRecognizer* recognizer, IplImage** images, int* labels, int count);
CVAPI(void) CvFaceRecognizerPredict(cv::FaceRecognizer* recognizer, IplImage* image, int* label, double* distance);
CVAPI(void) CvFaceRecognizerSave(cv::FaceRecognizer* recognizer, const char* fileName);
CVAPI(void) CvFaceRecognizerLoad(cv::FaceRecognizer* recognizer, const char* fileName);
CVAPI(void) CvFaceRecognizerRelease(cv::FaceRecognizer** recognizer);

//color map
CVAPI(void) CvApplyColorMap(IplImage* src, IplImage* dst, int colorMap);
#endif