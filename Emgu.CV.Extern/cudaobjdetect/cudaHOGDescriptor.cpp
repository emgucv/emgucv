//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudaobjdetect_c.h"

void cudaHOGGetDefaultPeopleDetector(cv::cuda::HOG* descriptor, cv::Mat* detector)
{
   cv::Mat d = descriptor->getDefaultPeopleDetector();
   cv::swap(d, *detector);
}

cv::cuda::HOG* cudaHOGCreate(
   CvSize* winSize, 
   CvSize* blockSize, 
   CvSize* blockStride,
   CvSize* cellSize, 
   int nbins)
{
   cv::Size _winSize(winSize->width, winSize->height);
   cv::Size _blockSize(blockSize->width, blockSize->height);
   cv::Size _blockStride(blockStride->width, blockStride->height);
   cv::Size _cellSize(cellSize->width, cellSize->height);
   cv::Ptr<cv::cuda::HOG> ptr = cv::cuda::HOG::create(_winSize, _blockSize, _blockStride, _cellSize, nbins);
   ptr.addref();
   return ptr.get();
}

void cudaHOGSetSVMDetector(cv::cuda::HOG* descriptor, cv::_InputArray* detector)
{ 
   descriptor->setSVMDetector(*detector); 
}

void cudaHOGRelease(cv::cuda::HOG** descriptor) 
{ 
   delete *descriptor;
   *descriptor = 0;
}

void cudaHOGDetectMultiScale(
   cv::cuda::HOG* descriptor, 
   cv::_InputArray* img, 
   std::vector<cv::Rect>* foundLocations,
   std::vector<double>* confidents)
{
   descriptor->detectMultiScale(*img, *foundLocations, confidents);
}

/*
double cudaHOGGetWinSigma(cv::cuda::HOG* descriptor)
{
   return descriptor->getWinSigma();
}

void cudaHOGSetWinSigma(cv::cuda::HOG* descriptor, double winSigma)
{
   descriptor->setWinSigma(winSigma);
}

int cudaHOGGetNumLevels(cv::cuda::HOG* descriptor)
{
   return descriptor->getNumLevels();
}

void cudaHOGSetNumLevels(cv::cuda::HOG* descriptor, int numLevels)
{
   descriptor->setNumLevels(numLevels);
}

int cudaHOGGetGroupThreshold(cv::cuda::HOG* descriptor)
{
   return descriptor->getGroupThreshold();
}

void cudaHOGSetGroupThreshold(cv::cuda::HOG* descriptor, int groupThreshold)
{
   descriptor->setGroupThreshold(groupThreshold);
}

double cudaHOGGetHitThreshold(cv::cuda::HOG* descriptor)
{
   return descriptor->getHitThreshold();
}

void cudaHOGSetHitThreshold(cv::cuda::HOG* descriptor, double hitThreshold)
{
   descriptor->setHitThreshold(hitThreshold);
}

double cudaHOGGetScaleFactor(cv::cuda::HOG* descriptor)
{
   return descriptor->getScaleFactor();
}

void cudaHOGSetScaleFactor(cv::cuda::HOG* descriptor, double scaleFactor)
{
   descriptor->setScaleFactor(scaleFactor);
}

void cudaHOGGetWinStride(cv::cuda::HOG* descriptor, CvSize* winStride)
{
   CvSize s = descriptor->getWinStride();
   winStride->width = s.width;
   winStride->height = s.height;
}

void cudaHOGSetWinStride(cv::cuda::HOG* descriptor, CvSize* winStride)
{
   CvSize s = cvSize(winStride->width, winStride->height);
   descriptor->setWinStride(s); 
}

bool cudaHOGGetGammaCorrection(cv::cuda::HOG* descriptor)
{
   return descriptor->getGammaCorrection();
}

void cudaHOGSetGammaCorrection(cv::cuda::HOG* descriptor, bool gammaCorrection)
{
   descriptor->setGammaCorrection(gammaCorrection);
}

double cudaHOGGetL2HysThreshold(cv::cuda::HOG* descriptor)
{
   return descriptor->getL2HysThreshold();
}

void cudaHOGSetL2HysThreshold(cv::cuda::HOG* descriptor, double l2HysThreshold)
{
   descriptor->setL2HysThreshold(l2HysThreshold);
} */