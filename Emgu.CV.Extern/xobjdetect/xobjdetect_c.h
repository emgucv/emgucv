//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_XOBJDETECT_C_H
#define EMGU_XOBJDETECT_C_H

#include "opencv2/core.hpp"
#include "cvapi_compat.h"

#ifdef HAVE_OPENCV_XOBJDETECT
#include "opencv2/xobjdetect.hpp"
#else
static inline CV_NORETURN void throw_no_xobjdetect() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without xobjdetect support. To use this module, please switch to the full Emgu CV runtime."); }
namespace cv {
	namespace xobjdetect {
		class WBDetector {};
	}
    class HOGDescriptor {};
    class CascadeClassifier {};
}
#endif
CVAPI(cv::xobjdetect::WBDetector*) cveWBDetectorCreate(cv::Ptr<cv::xobjdetect::WBDetector>** sharedPtr);
CVAPI(void) cveWBDetectorRead(cv::xobjdetect::WBDetector* detector, cv::FileNode* node);
CVAPI(void) cveWBDetectorWrite(cv::xobjdetect::WBDetector* detector, cv::FileStorage* fs);
CVAPI(void) cveWBDetectorTrain(cv::xobjdetect::WBDetector* detector, cv::String* posSamples, cv::String* negImgs);
CVAPI(void) cveWBDetectorDetect(cv::xobjdetect::WBDetector* detector, cv::Mat* img, std::vector<cv::Rect>* bboxes, std::vector<double>* confidences);
CVAPI(void) cveWBDetectorRelease(cv::xobjdetect::WBDetector** detector, cv::Ptr<cv::xobjdetect::WBDetector>** sharedPtr);

CVAPI(void) cveHOGDescriptorPeopleDetectorCreate(std::vector<float>* seq);

CVAPI(cv::HOGDescriptor*) cveHOGDescriptorCreateDefault();

CVAPI(cv::HOGDescriptor*) cveHOGDescriptorCreate(
    cv::Size* _winSize,
    cv::Size* _blockSize,
    cv::Size* _blockStride,
    cv::Size* _cellSize,
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
    cv::Size* winStride,
    cv::Size* padding,
    double scale,
    double finalThreshold,
    bool useMeanshiftGrouping);

CVAPI(void) cveHOGDescriptorCompute(
    cv::HOGDescriptor* descriptor,
    cv::_InputArray* img,
    std::vector<float>* descriptors,
    cv::Size* winStride,
    cv::Size* padding,
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
    cv::Size* minSize,
    cv::Size* maxSize);
CVAPI(bool) cveCascadeClassifierIsOldFormatCascade(cv::CascadeClassifier* classifier);
CVAPI(void) cveCascadeClassifierGetOriginalWindowSize(cv::CascadeClassifier* classifier, cv::Size* size);

CVAPI(void) cveGroupRectangles1(std::vector< cv::Rect >* rectList, int groupThreshold, double eps);
CVAPI(void) cveGroupRectangles2(std::vector<cv::Rect>* rectList, std::vector<int>* weights, int groupThreshold, double eps);
CVAPI(void) cveGroupRectangles3(std::vector<cv::Rect>* rectList, int groupThreshold, double eps, std::vector<int>* weights, std::vector<double>* levelWeights);
CVAPI(void) cveGroupRectangles4(std::vector<cv::Rect>* rectList, std::vector<int>* rejectLevels, std::vector<double>* levelWeights, int groupThreshold, double eps);
CVAPI(void) cveGroupRectanglesMeanshift(std::vector<cv::Rect>* rectList, std::vector<double>* foundWeights, std::vector<double>* foundScales, double detectThreshold, cv::Size* winDetSize);


#endif