//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_BIOINSPIRED_C_H
#define EMGU_BIOINSPIRED_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/bioinspired/retina.hpp"
#include "emgu_c.h"

//Retina
CVAPI(cv::bioinspired::Retina*) CvRetinaCreate(emgu::size* inputSize, const bool colorMode, int colorSamplingMethod, const bool useRetinaLogSampling, const double reductionFactor, const double samplingStrength);
CVAPI(void) CvRetinaRelease(cv::bioinspired::Retina** retina);
CVAPI(void) CvRetinaRun(cv::bioinspired::Retina* retina, cv::_InputArray* image);
CVAPI(void) CvRetinaGetParvo(cv::bioinspired::Retina* retina, cv::_OutputArray* parvo);
CVAPI(void) CvRetinaGetMagno(cv::bioinspired::Retina* retina, cv::_OutputArray* magno);
CVAPI(void) CvRetinaClearBuffers(cv::bioinspired::Retina* retina);
CVAPI(void) CvRetinaGetParameters(cv::bioinspired::Retina* retina, cv::bioinspired::Retina::RetinaParameters* p);
CVAPI(void) CvRetinaSetParameters(cv::bioinspired::Retina* retina, cv::bioinspired::Retina::RetinaParameters* p);

#endif