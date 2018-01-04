//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_OBJDETECT_C_H
#define EMGU_OBJDETECT_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/objdetect/objdetect.hpp"
#include "opencv2/objdetect/objdetect_c.h"
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

#endif