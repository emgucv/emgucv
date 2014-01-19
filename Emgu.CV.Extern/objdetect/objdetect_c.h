//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_OBJDETECT_C_H
#define EMGU_OBJDETECT_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/objdetect/objdetect.hpp"
#include "opencv2/objdetect/objdetect_c.h"
#include "vectors_c.h"

CVAPI(void) CvHOGDescriptorPeopleDetectorCreate(std::vector<float>* seq);

CVAPI(cv::HOGDescriptor*) CvHOGDescriptorCreateDefault();

CVAPI(cv::HOGDescriptor*) CvHOGDescriptorCreate(
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

CVAPI(void) CvHOGSetSVMDetector(cv::HOGDescriptor* descriptor, std::vector<float>* vector);

CVAPI(void) CvHOGDescriptorRelease(cv::HOGDescriptor* descriptor);

CVAPI(void) CvHOGDescriptorDetectMultiScale(
   cv::HOGDescriptor* descriptor, 
   CvArr* img, 
   CvSeq* foundLocations,
   double hitThreshold, 
   CvSize* winStride,
   CvSize* padding, 
   double scale,
   double finalThreshold, 
   bool useMeanshiftGrouping);

CVAPI(void) CvHOGDescriptorCompute(
    cv::HOGDescriptor *descriptor,
    CvArr *img, 
    std::vector<float> *descriptors,
    CvSize* winStride,
    CvSize* padding,
    CvSeq* locationSeq);


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

CVAPI(unsigned int) CvHOGDescriptorGetDescriptorSize(cv::HOGDescriptor* descriptor);


CVAPI(cv::CascadeClassifier*) CvCascadeClassifierCreate(char* fileName);
CVAPI(void) CvCascadeClassifierRelease(cv::CascadeClassifier** classifier);
CVAPI(void) CvCascadeClassifierDetectMultiScale( 
   cv::CascadeClassifier* classifier,
   cv::_InputArray* image,
   std::vector<cv::Rect>* objects,
   double scaleFactor,
   int minNeighbors, int flags,
   CvSize* minSize,
   CvSize* maxSize); 

//ERFilter
CVAPI(cv::ERFilter*) CvERFilterNM1Create(
   const char* classifier,
   int thresholdDelta,
   float minArea,
   float maxArea,
   float minProbability,
   bool nonMaxSuppression,
   float minProbabilityDiff);
CVAPI(cv::ERFilter*) CvERFilterNM2Create(const char* classifier, float minProbability);
CVAPI(void) CvERFilterRelease(cv::ERFilter** filter);
CVAPI(void) CvERFilterRun(cv::ERFilter* filter, cv::_InputArray* image, std::vector<cv::ERStat>* regions);

CVAPI(void) CvERGrouping(IplImage** channels, std::vector<cv::ERStat>** regions, int count, const char* fileName, float minProbability, std::vector<cv::Rect>* groups);

#endif