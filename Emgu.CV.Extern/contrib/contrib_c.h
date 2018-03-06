//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CONTRIB_C_H
#define EMGU_CONTRIB_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/contrib/contrib.hpp"
#include "opencv2/contrib/compat.hpp"

///Octree
CVAPI(cv::Octree*) CvOctreeCreate();

CVAPI(void) CvOctreeBuildTree(cv::Octree* tree, cv::Point3f* points, int numberOfPoints, int maxLevels, int minPoints );

CVAPI(void) CvOctreeGetPointsWithinSphere(cv::Octree* tree, cv::Point3f* center, float radius, std::vector<cv::Point3f>* points );

CVAPI(void) CvOctreeRelease(cv::Octree* tree);

//CvAdaptiveSkinDetector
CVAPI(CvAdaptiveSkinDetector*) CvAdaptiveSkinDetectorCreate(int samplingDivider, int morphingMethod);
CVAPI(void) CvAdaptiveSkinDetectorRelease(CvAdaptiveSkinDetector* detector);
CVAPI(void) CvAdaptiveSkinDetectorProcess(CvAdaptiveSkinDetector* detector, IplImage *inputBGRImage, IplImage *outputHueMask);

//FaceRecognizer
CVAPI(cv::FaceRecognizer*) CvEigenFaceRecognizerCreate(int numComponents, double threshold);   
CVAPI(cv::FaceRecognizer*) CvFisherFaceRecognizerCreate(int numComponents, double threshold);
CVAPI(cv::FaceRecognizer*) CvLBPHFaceRecognizerCreate(int radius, int neighbors, int gridX, int gridY, double threshold);
CVAPI(void) CvFaceRecognizerTrain(cv::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels);
CVAPI(void) CvFaceRecognizerUpdate(cv::FaceRecognizer* recognizer, cv::_InputArray* images, cv::_InputArray* labels);
CVAPI(void) CvFaceRecognizerPredict(cv::FaceRecognizer* recognizer, cv::_InputArray* image, int* label, double* distance);
CVAPI(void) CvFaceRecognizerSave(cv::FaceRecognizer* recognizer, cv::String* fileName);
CVAPI(void) CvFaceRecognizerLoad(cv::FaceRecognizer* recognizer, cv::String* fileName);
CVAPI(void) CvFaceRecognizerRelease(cv::FaceRecognizer** recognizer);

//color map
CVAPI(void) cveApplyColorMap(cv::_InputArray* src, cv::_OutputArray* dst, int colorMap);

//LevMarqSparse
CVAPI(cv::LevMarqSparse*) CvCreateLevMarqSparse();
CVAPI(void) CvLevMarqSparseAdjustBundle(int numberOfFrames, int pointCount, CvPoint3D64f* points, CvMat* imagePoints, CvMat*  visibility, std::vector<cv::Mat>* cameraMatrix, std::vector<cv::Mat>* R, std::vector<cv::Mat>* T, std::vector<cv::Mat>* distCoeffs, CvTermCriteria* termCrit);
CVAPI(void) CvReleaseLevMarqSparse(cv::LevMarqSparse** levMarq);

//ChamferMatching
CVAPI(int) cveChamferMatching( 
   cv::Mat* img, cv::Mat* templ,
   std::vector< std::vector<cv::Point> >* results, std::vector<float>* cost,
   double templScale, int maxMatches,
   double minMatchDistance, int padX,
   int padY, int scales, double minScale, double maxScale,
   double orientationWeight, double truncate);

//SelfSimDescriptor
CVAPI(cv::SelfSimDescriptor*) CvSelfSimDescriptorCreate(int smallSize,int largeSize, int startDistanceBucket, int numberOfDistanceBuckets, int numberOfAngles);
CVAPI(void) CvSelfSimDescriptorRelease(cv::SelfSimDescriptor* descriptor);
CVAPI(void) CvSelfSimDescriptorCompute(cv::SelfSimDescriptor* descriptor, cv::Mat* image, std::vector<float>* descriptors, cv::Size* winStride, std::vector<  cv::Point >* locations);
CVAPI(int) CvSelfSimDescriptorGetDescriptorSize(cv::SelfSimDescriptor* descriptor);
#endif