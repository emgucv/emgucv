//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDAIMGPROC_C_H
#define EMGU_CUDAIMGPROC_C_H

#include "opencv2/cuda.hpp"
#include "opencv2/cudaimgproc.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "emgu_c.h"

CVAPI(void) cudaBlendLinear(
   const cv::cuda::GpuMat* img1, const cv::cuda::GpuMat* img2, 
   const cv::cuda::GpuMat* weights1, const cv::cuda::GpuMat* weights2, 
   cv::cuda::GpuMat* result, cv::cuda::Stream* stream);
   
CVAPI(void) cudaCvtColor(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int code, cv::cuda::Stream* stream);

CVAPI(void) cudaSwapChannels(cv::cuda::GpuMat* image, const int* dstOrder, cv::cuda::Stream* stream);


CVAPI(void) cudaMeanShiftFiltering(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int sp, int sr,
   CvTermCriteria* criteria, cv::cuda::Stream* stream);

CVAPI(void) cudaMeanShiftProc(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dstr, cv::cuda::GpuMat* dstsp, int sp, int sr,
   CvTermCriteria* criteria, cv::cuda::Stream* stream);

CVAPI(void) cudaMeanShiftSegmentation(const cv::cuda::GpuMat* src, cv::Mat* dst, int sp, int sr, int minsize,
   CvTermCriteria* criteria);

CVAPI(void) cudaHistEven(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* hist, cv::cuda::GpuMat* buffer, int histSize, int lowerLevel, int upperLevel, cv::cuda::Stream* stream);

CVAPI(cv::cuda::CornernessCriteria*) cudaCreateHarrisCorner(int srcType, int blockSize, int ksize, double k, int borderType);

CVAPI(void) cudaCornernessCriteriaCompute(cv::cuda::CornernessCriteria* detector, const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream);

CVAPI(void) cudaCornernessCriteriaRelease(cv::cuda::CornernessCriteria** detector);

CVAPI(void) cudaBilateralFilter(cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int kernelSize, float sigmaColor, float sigmaSpatial, int borderMode, cv::cuda::Stream* stream);

//----------------------------------------------------------------------------
//
//  CudaCLAHE
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::CLAHE*) cudaCLAHECreate(double clipLimit, emgu::size* tileGridSize);
CVAPI(void) cudaCLAHEApply(cv::cuda::CLAHE* clahe, cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst,  cv::cuda::Stream* stream);
CVAPI(void) cudaCLAHERelease(cv::cuda::CLAHE** clahe);

//----------------------------------------------------------------------------
//
//  CannyEdgeDetector
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::CannyEdgeDetector*) cudaCreateCannyEdgeDetector(double lowThreshold, double highThreshold, int apertureSize, bool L2gradient);

CVAPI(void) cudaCannyEdgeDetectorDetect(cv::cuda::CannyEdgeDetector* detector, cv::cuda::GpuMat* src, cv::cuda::GpuMat* edges);

CVAPI(void) cudaCannyEdgeDetectorRelease(cv::cuda::CannyEdgeDetector** detector);

//----------------------------------------------------------------------------
//
//  CudaGoodFeaturesToTrackDetector
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::CornersDetector*) cudaGoodFeaturesToTrackDetectorCreate(int srcType, int maxCorners, double qualityLevel, double minDistance, int blockSize, bool useHarrisDetector, double harrisK);
CVAPI(void) cudaCornersDetectorDetect(cv::cuda::CornersDetector* detector, const cv::cuda::GpuMat* image, cv::cuda::GpuMat* corners, const cv::cuda::GpuMat* mask);
CVAPI(void) cudaCornersDetectorRelease(cv::cuda::CornersDetector** detector);


//----------------------------------------------------------------------------
//
//  CudaTemplateMatching
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::TemplateMatching*) cudaTemplateMatchingCreate(int srcType, int method, emgu::size* blockSize);
CVAPI(void) cudaTemplateMatchingRelease(cv::cuda::TemplateMatching** tm);
CVAPI(void) cudaTemplateMatchingMatch(cv::cuda::TemplateMatching* tm, const cv::cuda::GpuMat* image, const cv::cuda::GpuMat* templ, cv::cuda::GpuMat* result, cv::cuda::Stream* stream);

//----------------------------------------------------------------------------
//
//  CudaHoughLinesDetector
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::HoughLinesDetector*) cudaHoughLinesDetectorCreate(float rho, float theta, int threshold, bool doSort, int maxLines);
CVAPI(void) cudaHoughLinesDetectorDetect(cv::cuda::HoughLinesDetector* detector, cv::cuda::GpuMat* src, cv::cuda::GpuMat* lines);
CVAPI(void) cudaHoughLinesDetectorRelease(cv::cuda::HoughLinesDetector** detector);

//----------------------------------------------------------------------------
//
//  CudaHoughSegmentDetector
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::HoughSegmentDetector*) cudaHoughSegmentDetectorCreate(float rho, float theta, int minLineLength, int maxLineGap, int maxLines);
CVAPI(void) cudaHoughSegmentDetectorDetect(cv::cuda::HoughSegmentDetector* detector, cv::cuda::GpuMat* src, cv::cuda::GpuMat* lines);
CVAPI(void) cudaHoughSegmentDetectorRelease(cv::cuda::HoughSegmentDetector** detector);

//----------------------------------------------------------------------------
//
//  CudaHoughCircleDetector
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::HoughCirclesDetector*) cudaHoughCirclesDetectorCreate(float dp, float minDist, int cannyThreshold, int votesThreshold, int minRadius, int maxRadius, int maxCircles);
CVAPI(void) cudaHoughCirclesDetectorDetect(cv::cuda::HoughCirclesDetector* detector, cv::cuda::GpuMat* src, cv::cuda::GpuMat* circles);
CVAPI(void) cudaHoughCirclesDetectorRelease(cv::cuda::HoughCirclesDetector** detector);

CVAPI(void) cudaGammaCorrection(cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, bool forward, cv::cuda::Stream* stream);
#endif