//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_BIOINSPIRED_C_H
#define EMGU_BIOINSPIRED_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/bioinspired/retina.hpp"
#include "emgu_c.h"

//Retina
CVAPI(cv::bioinspired::Retina*) cveRetinaCreate(CvSize* inputSize, const bool colorMode, int colorSamplingMethod, const bool useRetinaLogSampling, const double reductionFactor, const double samplingStrength);
CVAPI(void) cveRetinaRelease(cv::bioinspired::Retina** retina);
CVAPI(void) cveRetinaRun(cv::bioinspired::Retina* retina, cv::_InputArray* image);
CVAPI(void) cveRetinaGetParvo(cv::bioinspired::Retina* retina, cv::_OutputArray* parvo);
CVAPI(void) cveRetinaGetMagno(cv::bioinspired::Retina* retina, cv::_OutputArray* magno);
CVAPI(void) cveRetinaClearBuffers(cv::bioinspired::Retina* retina);
CVAPI(void) cveRetinaGetParameters(cv::bioinspired::Retina* retina, cv::bioinspired::RetinaParameters* p);
CVAPI(void) cveRetinaSetParameters(cv::bioinspired::Retina* retina, cv::bioinspired::RetinaParameters* p);

#endif