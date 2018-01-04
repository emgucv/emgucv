//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"

void cveHOGDescriptorPeopleDetectorCreate(std::vector<float>* seq) 
{   
   std::vector<float> v = cv::HOGDescriptor::getDefaultPeopleDetector();  
   seq->resize(v.size());
   memcpy(&(*seq)[0], &v[0], sizeof(float)* seq->size());
}
cv::HOGDescriptor* cveHOGDescriptorCreateDefault() { return new cv::HOGDescriptor; }

cv::HOGDescriptor* cveHOGDescriptorCreate(
   CvSize* _winSize, 
   CvSize* _blockSize, 
   CvSize* _blockStride,
   CvSize* _cellSize, 
   int _nbins, 
   int _derivAperture, 
   double _winSigma,
   int _histogramNormType, 
   double _L2HysThreshold, 
   bool _gammaCorrection)
{
   return new cv::HOGDescriptor(*_winSize, *_blockSize, *_blockStride, *_cellSize, _nbins, _derivAperture, _winSigma, _histogramNormType, _L2HysThreshold, _gammaCorrection);
}

void cveHOGSetSVMDetector(cv::HOGDescriptor* descriptor, std::vector<float>* vector) 
{ 
   descriptor->setSVMDetector(*vector); 
}

void cveHOGDescriptorRelease(cv::HOGDescriptor** descriptor)
{
   delete *descriptor;
   *descriptor = 0;
}


void cveHOGDescriptorDetectMultiScale(
   cv::HOGDescriptor* descriptor, 
   cv::_InputArray* img, 
   std::vector<cv::Rect>* foundLocations,
   std::vector<double>* weights,
   double hitThreshold, 
   CvSize* winStride,
   CvSize* padding, 
   double scale,
   double finalThreshold, 
   bool useMeanshiftGrouping)
{
   descriptor->detectMultiScale(*img, *foundLocations, *weights, hitThreshold, *winStride, *padding, scale, finalThreshold, useMeanshiftGrouping );
}

void cveHOGDescriptorCompute(
    cv::HOGDescriptor *descriptor,
    cv::_InputArray* img, 
    std::vector<float> *descriptors,
    CvSize* winStride,
    CvSize* padding,
    std::vector< cv::Point >* locations) 
{
    std::vector<cv::Point> emptyVec;
    
    descriptor->compute(
       *img, 
       *descriptors,
       *winStride,
       *padding,
       locations ? *locations : emptyVec); 
}


/*
void CvHOGDescriptorDetect(
   cv::HOGDescriptor* descriptor, 
   CvArr* img, 
   CvSeq* foundLocations,
   double hitThreshold, 
   CvSize winStride,
   CvSize padding)
{
   cvClearSeq(foundLocations);

   std::vector<cv::Rect> rects;
   cv::Mat mat = cv::cvarrToMat(img);
   descriptor->detect(mat, rects, hitThreshold, winStride, padding);
   if (rects.size() > 0)
      cvSeqPushMulti(foundLocations, &rects[0], rects.size());
}*/

unsigned int cveHOGDescriptorGetDescriptorSize(cv::HOGDescriptor* descriptor)
{
   return static_cast<unsigned int>(descriptor->getDescriptorSize());
}