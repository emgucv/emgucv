//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDA_C_H
#define EMGU_CUDA_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core.hpp"
#include "cvapi_compat.h"
#include "emgu_error.h"

#ifdef HAVE_OPENCV_CUDAOBJDETECT

#include "opencv2/cudaobjdetect.hpp"
#include "emgu_c.h"

#else

static inline CV_NORETURN void throw_no_cudaobjdetect() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without CUDA Objdetect support. To use this module, please switch to the Emgu CV runtime with CUDA support."); }

namespace cv
{
	namespace cuda
	{
		class CascadeClassifier
		{
			
		};

		class HOG
		{
			
		};
	}
}

#endif

//----------------------------------------------------------------------------
//
//  CudaCascadeClassifier
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::CascadeClassifier*) cudaCascadeClassifierCreate(cv::String* filename, cv::Ptr<cv::cuda::CascadeClassifier>** sharedPtr);

CVAPI(cv::cuda::CascadeClassifier*) cudaCascadeClassifierCreateFromFileStorage(cv::FileStorage* filestorage, cv::Ptr<cv::cuda::CascadeClassifier>** sharedPtr);

CVAPI(void) cudaCascadeClassifierRelease(cv::Ptr<cv::cuda::CascadeClassifier>** classifier);

CVAPI(void) cudaCascadeClassifierDetectMultiScale(cv::cuda::CascadeClassifier* classifier, cv::_InputArray* image, cv::_OutputArray* objects, cv::cuda::Stream* stream);

CVAPI(void) cudaCascadeClassifierConvert(cv::cuda::CascadeClassifier* classifier, cv::_OutputArray* gpuObjects, std::vector<cv::Rect>* objects);

CVAPI(void) cudaCascadeClassifierGetMinObjectSize(cv::cuda::CascadeClassifier* classifier, cv::Size* minObjectSize);

CVAPI(void) cudaCascadeClassifierSetMinObjectSize(cv::cuda::CascadeClassifier* classifier, cv::Size* minObjectSize);

//----------------------------------------------------------------------------
//
//  CudaHOG
//
//----------------------------------------------------------------------------
CVAPI(void) cudaHOGGetDefaultPeopleDetector(cv::cuda::HOG* descriptor, cv::Mat* detector);

CVAPI(cv::cuda::HOG*) cudaHOGCreate(
	cv::Size* winSize,
	cv::Size* blockSize,
	cv::Size* blockStride,
	cv::Size* cellSize,
	int nbins,
	cv::Ptr<cv::cuda::HOG>** sharedPtr);

CVAPI(void) cudaHOGSetSVMDetector(cv::cuda::HOG* descriptor, cv::_InputArray* detector);

CVAPI(void) cudaHOGRelease(cv::Ptr<cv::cuda::HOG>** descriptor);

CVAPI(void) cudaHOGDetectMultiScale(
	cv::cuda::HOG* descriptor,
	cv::_InputArray* img,
	std::vector<cv::Rect>* foundLocations,
	std::vector<double>* confidents);

#endif