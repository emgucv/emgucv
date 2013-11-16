//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_PHOTO_C_H
#define EMGU_PHOTO_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/photo/photo.hpp"

#include "opencv2/photo/cuda.hpp"

CVAPI(void) cvFastNlMeansDenoising(CvArr* src, CvArr* dst, float h, int templateWindowSize, int searchWindowSize);

CVAPI(void) cvFastNlMeansDenoisingColored(CvArr* src, CvArr* dst, float h, float hColor, int templateWindowSize, int searchWindowSize);

CVAPI(void) cudaNonLocalMeans(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, float h, int searchWindow, int blockSize, int borderMode, cv::cuda::Stream* stream);
#endif