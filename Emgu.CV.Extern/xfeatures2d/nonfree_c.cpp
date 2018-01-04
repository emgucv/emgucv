//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "nonfree_c.h"

//SIFTDetector
cv::xfeatures2d::SIFT* cveSIFTCreate(
   int nFeatures, int nOctaveLayers, 
   double contrastThreshold, double edgeThreshold, 
   double sigma, cv::Feature2D** feature2D)
{
   cv::Ptr<cv::xfeatures2d::SIFT> siftPtr = cv::xfeatures2d::SIFT::create(nFeatures, nOctaveLayers, contrastThreshold, edgeThreshold, sigma);
   siftPtr.addref();
   *feature2D = dynamic_cast<cv::Feature2D*>(siftPtr.get());
   
   return siftPtr.get();
}

void cveSIFTRelease(cv::xfeatures2d::SIFT** detector)
{
   delete *detector;
   *detector = 0;
}

//SURFDetector
cv::xfeatures2d::SURF* cveSURFCreate(double hessianThresh, int nOctaves, int nOctaveLayers, bool extended, bool upright, cv::Feature2D** feature2D)
{
   cv::Ptr<cv::xfeatures2d::SURF> surfPtr = cv::xfeatures2d::SURF::create(hessianThresh, nOctaves, nOctaveLayers, extended, upright);
   surfPtr.addref();
   *feature2D = dynamic_cast<cv::Feature2D*>(surfPtr.get());
   
   return surfPtr.get();
}

void cveSURFRelease(cv::xfeatures2d::SURF** detector)
{
   delete *detector;
   *detector = 0;
}

/*

//----------------------------------------------------------------------------
//
//  VIBE GPU
//
//----------------------------------------------------------------------------
cv::cuda::VIBE_GPU* gpuVibeCreate(unsigned long rngSeed, cv::cuda::GpuMat* firstFrame, cv::cuda::Stream* stream)
{
   cv::cuda::VIBE_GPU* vibe = new cv::cuda::VIBE_GPU(rngSeed);
   vibe->initialize(*firstFrame, stream ? *stream : cv::cuda::Stream::Null());
   return vibe;
}
void gpuVibeCompute(cv::cuda::VIBE_GPU* vibe, cv::cuda::GpuMat* frame, cv::cuda::GpuMat* fgMask, cv::cuda::Stream* stream)
{
   (*vibe)(*frame, *fgMask, stream ? *stream : cv::cuda::Stream::Null());
}
void gpuVibeRelease(cv::cuda::VIBE_GPU** vibe)
{
   (*vibe)->release();
   delete *vibe;
   *vibe = 0;
}*/
