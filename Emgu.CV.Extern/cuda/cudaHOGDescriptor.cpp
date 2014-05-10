//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
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
   emgu::size* _winSize, 
   emgu::size* _blockSize, 
   emgu::size* _blockStride,
   emgu::size* _cellSize, 
   int _nbins, 
   double _winSigma,
   double _L2HysThreshold, 
   bool _gammaCorrection, 
   int _nlevels)
{
   cv::Size winSize(_winSize->width, _winSize->height);
   cv::Size blockSize(_blockSize->width, _blockSize->height);
   cv::Size blockStride(_blockStride->width, _blockStride->height);
   cv::Size cellSize(_cellSize->width, _cellSize->height);
   return new cv::cuda::HOGDescriptor(winSize, blockSize, blockStride, cellSize, _nbins, _winSigma, _L2HysThreshold, _gammaCorrection, _nlevels);
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
   std::vector<cv::Rect>* foundLocations,
   double hitThreshold, 
   emgu::size* winStride,
   emgu::size* padding, 
   double scale,
   int groupThreshold)
{
   //cvClearSeq(foundLocations);

   //std::vector<cv::Rect> rects;

   cv::Size ws(winStride->width, winStride->height);
   cv::Size ps(padding->width, padding->height);

   descriptor->detectMultiScale(*img, *foundLocations, hitThreshold, ws, ps, scale, groupThreshold);
   //if (!rects.empty())
   //   cvSeqPushMulti(foundLocations, &rects[0], static_cast<int>(rects.size()));
}