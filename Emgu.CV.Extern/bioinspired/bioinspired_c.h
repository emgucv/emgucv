//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_BIOINSPIRED_C_H
#define EMGU_BIOINSPIRED_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_BIOINSPIRED

#include "opencv2/bioinspired/retina.hpp"
#include "opencv2/bioinspired/retinafasttonemapping.hpp"
#include "emgu_c.h"

#else

static inline CV_NORETURN void throw_no_bioinspired() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without Bioinspired support"); }

namespace cv
{
	namespace  bioinspired
	{
		class Retina
		{
			
		};
		class RetinaParameters
		{
			
		};
		class RetinaFastToneMapping
		{
			
		};
	}
}

#endif

//Retina
CVAPI(cv::bioinspired::Retina*) cveRetinaCreate(CvSize* inputSize, const bool colorMode, int colorSamplingMethod, const bool useRetinaLogSampling, const double reductionFactor, const double samplingStrength, cv::Ptr<cv::bioinspired::Retina>** sharedPtr);
CVAPI(void) cveRetinaRelease(cv::Ptr<cv::bioinspired::Retina>** sharedPtr);
CVAPI(void) cveRetinaRun(cv::bioinspired::Retina* retina, cv::_InputArray* image);
CVAPI(void) cveRetinaGetParvo(cv::bioinspired::Retina* retina, cv::_OutputArray* parvo);
CVAPI(void) cveRetinaGetMagno(cv::bioinspired::Retina* retina, cv::_OutputArray* magno);
CVAPI(void) cveRetinaClearBuffers(cv::bioinspired::Retina* retina);
CVAPI(void) cveRetinaGetParameters(cv::bioinspired::Retina* retina, cv::bioinspired::RetinaParameters* p);
CVAPI(void) cveRetinaSetParameters(cv::bioinspired::Retina* retina, cv::bioinspired::RetinaParameters* p);

//RetinaFastToneMapping
CVAPI(cv::bioinspired::RetinaFastToneMapping*) cveRetinaFastToneMappingCreate(CvSize* inputSize, cv::Ptr<cv::bioinspired::RetinaFastToneMapping>** sharedPtr);
CVAPI(void) cveRetinaFastToneMappingSetup(cv::bioinspired::RetinaFastToneMapping* toneMapping, float photoreceptorsNeighborhoodRadius, float ganglioncellsNeighborhoodRadius, float meanLuminanceModulatorK);
CVAPI(void) cveRetinaFastToneMappingApplyFastToneMapping(cv::bioinspired::RetinaFastToneMapping* toneMapping, cv::_InputArray* inputImage, cv::_OutputArray* outputToneMappedImage);
CVAPI(void) cveRetinaFastToneMappingRelease(cv::Ptr<cv::bioinspired::RetinaFastToneMapping>** sharedPtr);
#endif