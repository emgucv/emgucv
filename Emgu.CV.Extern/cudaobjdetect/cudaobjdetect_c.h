//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDA_C_H
#define EMGU_CUDA_C_H

//#include "opencv2/cuda.hpp"
//#include "opencv2/core/cuda.hpp"
//#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "opencv2/cudaobjdetect.hpp"
#include "emgu_c.h"

//----------------------------------------------------------------------------
//
//  CudaCascadeClassifier
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::CascadeClassifier*) cudaCascadeClassifierCreate(cv::String* filename);

CVAPI(cv::cuda::CascadeClassifier*) cudaCascadeClassifierCreateFromFileStorage(cv::FileStorage* filestorage);

CVAPI(void) cudaCascadeClassifierRelease(cv::cuda::CascadeClassifier** classifier);

CVAPI(void) cudaCascadeClassifierDetectMultiScale(cv::cuda::CascadeClassifier* classifier, cv::_InputArray* image, cv::_OutputArray* objects, cv::cuda::Stream* stream);

CVAPI(void) cudaCascadeClassifierConvert(cv::cuda::CascadeClassifier* classifier, cv::_OutputArray* gpuObjects, std::vector<cv::Rect>* objects);

/*
CVAPI(double) cudaCascadeClassifierGetScaleFactor(cv::cuda::CascadeClassifier* classifier);

CVAPI(void) cudaCascadeClassifierSetScaleFactor(cv::cuda::CascadeClassifier* classifier, double scaleFactor);

CVAPI(int) cudaCascadeClassifierGetMinNeighbors(cv::cuda::CascadeClassifier* classifier);

CVAPI(void) cudaCascadeClassifierSetMinNeighbors(cv::cuda::CascadeClassifier* classifier, int minNeighbours);
*/
CVAPI(void) cudaCascadeClassifierGetMinObjectSize(cv::cuda::CascadeClassifier* classifier, CvSize* minObjectSize);

CVAPI(void) cudaCascadeClassifierSetMinObjectSize(cv::cuda::CascadeClassifier* classifier, CvSize* minObjectSize);

//----------------------------------------------------------------------------
//
//  CudaHOG
//
//----------------------------------------------------------------------------
CVAPI(void) cudaHOGGetDefaultPeopleDetector(cv::cuda::HOG* descriptor, cv::Mat* detector);

CVAPI(cv::cuda::HOG*) cudaHOGCreate(
   CvSize* winSize,
   CvSize* blockSize,
   CvSize* blockStride,
   CvSize* cellSize,
   int nbins);

CVAPI(void) cudaHOGSetSVMDetector(cv::cuda::HOG* descriptor, cv::_InputArray* detector);

CVAPI(void) cudaHOGRelease(cv::cuda::HOG** descriptor);

CVAPI(void) cudaHOGDetectMultiScale(
   cv::cuda::HOG* descriptor, 
   cv::_InputArray* img, 
   std::vector<cv::Rect>* foundLocations,
   std::vector<double>* confidents);

/*
CVAPI(double) cudaHOGGetWinSigma(cv::cuda::HOG* descriptor);
CVAPI(void) cudaHOGSetWinSigma(cv::cuda::HOG* descriptor, double winSigma);

CVAPI(int) cudaHOGGetNumLevels(cv::cuda::HOG* descriptor);
CVAPI(void) cudaHOGSetNumLevels(cv::cuda::HOG* descriptor, int numLevels);

CVAPI(int) cudaHOGGetGroupThreshold(cv::cuda::HOG* descriptor);
CVAPI(void) cudaHOGSetGroupThreshold(cv::cuda::HOG* descriptor, int groupThreshold);

CVAPI(double) cudaHOGGetHitThreshold(cv::cuda::HOG* descriptor);
CVAPI(void) cudaHOGSetHitThreshold(cv::cuda::HOG* descriptor, double hitThreshold);

CVAPI(double) cudaHOGGetScaleFactor(cv::cuda::HOG* descriptor);
CVAPI(void) cudaHOGSetScaleFactor(cv::cuda::HOG* descriptor, double scaleFactor); 

CVAPI(void) cudaHOGGetWinStride(cv::cuda::HOG* descriptor, CvSize* winStride);
CVAPI(void) cudaHOGSetWinStride(cv::cuda::HOG* descriptor, CvSize* winStride);

CVAPI(bool) cudaHOGGetGammaCorrection(cv::cuda::HOG* descriptor);
CVAPI(void) cudaHOGSetGammaCorrection(cv::cuda::HOG* descriptor, bool gammaCorrection);

CVAPI(double) cudaHOGGetL2HysThreshold(cv::cuda::HOG* descriptor);
CVAPI(void) cudaHOGSetL2HysThreshold(cv::cuda::HOG* descriptor, double l2HysThreshold);
*/
#endif