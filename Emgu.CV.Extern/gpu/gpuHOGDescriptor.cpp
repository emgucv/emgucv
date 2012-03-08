//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gpu_c.h"

void gpuHOGDescriptorGetPeopleDetector64x128(std::vector<float>* vector)
{
   std::vector<float> v = cv::gpu::HOGDescriptor::getPeopleDetector64x128();
   v.swap(*vector);
}

void gpuHOGDescriptorGetPeopleDetector48x96(std::vector<float>* vector)
{
   std::vector<float> v = cv::gpu::HOGDescriptor::getPeopleDetector48x96();
   v.swap(*vector);
}

cv::gpu::HOGDescriptor* gpuHOGDescriptorCreateDefault() { return new cv::gpu::HOGDescriptor; }

cv::gpu::HOGDescriptor* gpuHOGDescriptorCreate(
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
   return new cv::gpu::HOGDescriptor(*_winSize, *_blockSize, *_blockStride, *_cellSize, _nbins, _winSigma, _L2HysThreshold, _gammaCorrection, _nlevels);
}

void gpuHOGSetSVMDetector(cv::gpu::HOGDescriptor* descriptor, std::vector<float>* vector) 
{ 
   descriptor->setSVMDetector(*vector); 
}

void gpuHOGDescriptorRelease(cv::gpu::HOGDescriptor** descriptor) 
{ 
   delete *descriptor;
   *descriptor = 0;
}

void gpuHOGDescriptorDetectMultiScale(
   cv::gpu::HOGDescriptor* descriptor, 
   cv::gpu::GpuMat* img, 
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
      cvSeqPushMulti(foundLocations, &rects[0], rects.size());
}