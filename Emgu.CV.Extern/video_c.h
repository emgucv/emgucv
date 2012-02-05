//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_VIDEO_C_H
#define EMGU_VIDEO_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/video/video.hpp"

//BackgroundSubtractorMOG2
CVAPI(cv::BackgroundSubtractorMOG2*) CvBackgroundSubtractorMOG2Create(int history,  float varThreshold, bool bShadowDetection);
CVAPI(void) CvBackgroundSubtractorMOG2Release(cv::BackgroundSubtractorMOG2** bgSubstractor);

//BackgroundSubtractor
CVAPI(void) CvBackgroundSubtractorUpdate(cv::BackgroundSubtractor* bgSubstractor, IplImage* image, IplImage* fgmask, double learningRate);

//BackgroundSubtractorMOG
CVAPI(cv::BackgroundSubtractorMOG*) CvBackgroundSubtractorMOGCreate(int history, int nmixtures, double backgroundRatio, double noiseSigma);
CVAPI(void) CvBackgroundSubtractorMOGRelease(cv::BackgroundSubtractorMOG** bgSubstractor);
#endif