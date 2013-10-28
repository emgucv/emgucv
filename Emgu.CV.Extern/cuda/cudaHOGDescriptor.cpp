//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cuda_c.h"

void cudaHOGDescriptorGetPeopleDetector64x128(std::vector<float>* vector)
{
   std::vector<float> v = cv::cuda::HOGDescriptor::getPeopleDetector64x128();
   v.swap(*vector);
}

void cudaHOGDescriptorGetPeopleDetector48x96(std::vector<float>* vector)
{
   std::vector<float> v = cv::cuda::HOGDescriptor::getPeopleDetector48x96();
   v.swap(*vector);
}

cv::cuda::HOGDescriptor* cudaHOGDescriptorCreateDefault() { return new cv::cuda::HOGDescriptor; }

cv::cuda::HOGDescriptor* cudaHOGDescriptorCreate(
   cv::Size* _winSize, 
   cv::Size* _blockSize, 
   cv::Size* _blockStride,
   cv::Size* _cellSize, 
   int _nbins, 
   double _winSigma,
   double _L2HysThreshold, 
   bool _gammaCorrection, 
   int _nlevels)
{
   return new cv::cuda::HOGDescriptor(*_winSize, *_blockSize, *_blockStride, *_cellSize, _nbins, _winSigma, _L2HysThreshold, _gammaCorrection, _nlevels);
}

void cudaHOGSetSVMDetector(cv::cuda::HOGDescriptor* descriptor, std::vector<float>* vector) 
{ 
   descriptor->setSVMDetector(*vector); 
}

void cudaHOGDescriptorRelease(cv::cuda::HOGDescriptor** descriptor) 
{ 
   delete *descriptor;
   *descriptor = 0;
}

void cudaHOGDescriptorDetectMultiScale(
   cv::cuda::HOGDescriptor* descriptor, 
   cv::cuda::GpuMat* img, 
   CvSeq* foundLocations,
   double hitThreshold, 
   CvSize winStride,
   CvSize padding, 
   double scale,
   int groupThreshold)
{
   cvClearSeq(foundLocations);

   std::vector<cv::Rect> rects;

   descriptor->detectMultiScale(*img, rects, hitThreshold, winStride, padding, scale, groupThreshold);
   if (!rects.empty())
      cvSeqPushMulti(foundLocations, &rects[0], static_cast<int>(rects.size()));
}