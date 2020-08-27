//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "nonfree_c.h"

//SURFDetector
cv::xfeatures2d::SURF* cveSURFCreate(double hessianThresh, int nOctaves, int nOctaveLayers, bool extended, bool upright, cv::Feature2D** feature2D, cv::Ptr<cv::xfeatures2d::SURF>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	cv::Ptr<cv::xfeatures2d::SURF> surfPtr = cv::xfeatures2d::SURF::create(hessianThresh, nOctaves, nOctaveLayers, extended, upright);
	*sharedPtr = new cv::Ptr<cv::xfeatures2d::SURF>(surfPtr);
	*feature2D = dynamic_cast<cv::Feature2D*>(surfPtr.get());

	return surfPtr.get();
#else
	throw_no_xfeatures2d();
#endif
}

void cveSURFRelease(cv::Ptr<cv::xfeatures2d::SURF>** sharedPtr)
{
#ifdef HAVE_OPENCV_XFEATURES2D
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_xfeatures2d();
#endif
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
