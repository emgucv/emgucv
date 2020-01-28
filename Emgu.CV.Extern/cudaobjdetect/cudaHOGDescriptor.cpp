//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudaobjdetect_c.h"

void cudaHOGGetDefaultPeopleDetector(cv::cuda::HOG* descriptor, cv::Mat* detector)
{
#ifdef HAVE_OPENCV_CUDAOBJDETECT
	cv::Mat d = descriptor->getDefaultPeopleDetector();
	cv::swap(d, *detector);
#else
	throw_no_cudaobjdetect();
#endif
}

cv::cuda::HOG* cudaHOGCreate(
	CvSize* winSize,
	CvSize* blockSize,
	CvSize* blockStride,
	CvSize* cellSize,
	int nbins,
	cv::Ptr<cv::cuda::HOG>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAOBJDETECT
	cv::Size _winSize(winSize->width, winSize->height);
	cv::Size _blockSize(blockSize->width, blockSize->height);
	cv::Size _blockStride(blockStride->width, blockStride->height);
	cv::Size _cellSize(cellSize->width, cellSize->height);
	cv::Ptr<cv::cuda::HOG> ptr = cv::cuda::HOG::create(_winSize, _blockSize, _blockStride, _cellSize, nbins);
	*sharedPtr = new cv::Ptr<cv::cuda::HOG>(ptr);
	return ptr.get();
#else
	throw_no_cudaobjdetect();
#endif
}

void cudaHOGSetSVMDetector(cv::cuda::HOG* descriptor, cv::_InputArray* detector)
{
#ifdef HAVE_OPENCV_CUDAOBJDETECT
	descriptor->setSVMDetector(*detector);
#else
	throw_no_cudaobjdetect();
#endif
}

void cudaHOGRelease(cv::Ptr<cv::cuda::HOG>** descriptor)
{
#ifdef HAVE_OPENCV_CUDAOBJDETECT
	delete *descriptor;
	*descriptor = 0;
#else
	throw_no_cudaobjdetect();
#endif
}

void cudaHOGDetectMultiScale(
	cv::cuda::HOG* descriptor,
	cv::_InputArray* img,
	std::vector<cv::Rect>* foundLocations,
	std::vector<double>* confidents)
{
#ifdef HAVE_OPENCV_CUDAOBJDETECT
	descriptor->detectMultiScale(*img, *foundLocations, confidents);
#else
	throw_no_cudaobjdetect();
#endif
}
