//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_BGSEGM_C_H
#define EMGU_BGSEGM_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/bgsegm.hpp"

//BackgroundSubtractorMOG
CVAPI(cv::bgsegm::BackgroundSubtractorMOG*) CvBackgroundSubtractorMOGCreate(int history, int nmixtures, double backgroundRatio, double noiseSigma);
CVAPI(void) CvBackgroundSubtractorMOGRelease(cv::bgsegm::BackgroundSubtractorMOG** bgSubtractor);

//BackgroundSubtractorGMG
CVAPI(cv::bgsegm::BackgroundSubtractorGMG*) CvBackgroundSubtractorGMGCreate(int initializationFrames, double decisionThreshold);
CVAPI(void) CvBackgroundSubtractorGMGRelease(cv::bgsegm::BackgroundSubtractorGMG** bgSubtractor);
#endif