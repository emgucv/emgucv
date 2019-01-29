//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
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
	int nbins,
	cv::Ptr<cv::cuda::HOG>** sharedPtr)
{
	cv::Size _winSize(winSize->width, winSize->height);
	cv::Size _blockSize(blockSize->width, blockSize->height);
	cv::Size _blockStride(blockStride->width, blockStride->height);
	cv::Size _cellSize(cellSize->width, cellSize->height);
	cv::Ptr<cv::cuda::HOG> ptr = cv::cuda::HOG::create(_winSize, _blockSize, _blockStride, _cellSize, nbins);
	*sharedPtr = new cv::Ptr<cv::cuda::HOG>(ptr);
	return ptr.get();
}

void cudaHOGSetSVMDetector(cv::cuda::HOG* descriptor, cv::_InputArray* detector)
{
	descriptor->setSVMDetector(*detector);
}

void cudaHOGRelease(cv::Ptr<cv::cuda::HOG>** descriptor)
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
