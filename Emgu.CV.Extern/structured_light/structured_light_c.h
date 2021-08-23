//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_STRUCTURED_LIGHT_C_H
#define EMGU_STRUCTURED_LIGHT_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_STRUCTURED_LIGHT
#include "opencv2/structured_light.hpp"
#else
static inline CV_NORETURN void throw_no_structured_light() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without structure light support"); }
namespace cv {
    namespace structured_light {
        class StructuredLightPattern {};
        class SinusoidalPattern {};
        class GrayCodePattern {};
	}
}

#endif

CVAPI(bool) cveStructuredLightPatternGenerate(
    cv::structured_light::StructuredLightPattern* structuredLight, 
    cv::_OutputArray* patternImages);
CVAPI(bool) cveStructuredLightPatternDecode(
    cv::structured_light::StructuredLightPattern* structuredLight, 
    std::vector< std::vector< cv::Mat > >* patternImages, 
    cv::_OutputArray* disparityMap,
    cv::_InputArray* blackImages,
    cv::_InputArray* whiteImages,
    int flags);

CVAPI(cv::structured_light::SinusoidalPattern*) cveSinusoidalPatternCreate(
    int width,
    int height,
    int nbrOfPeriods,
    float shiftValue,
    int methodId,
    int nbrOfPixelsBetweenMarkers,
    bool horizontal,
    bool setMarkers,
    std::vector< cv::Point2f >* markersLocation,
    cv::Ptr<cv::structured_light::SinusoidalPattern>** sharedPtr,
    cv::structured_light::StructuredLightPattern** structuredLightPattern,
    cv::Algorithm** algorithm);
CVAPI(void) cveSinusoidalPatternRelease(cv::Ptr<cv::structured_light::SinusoidalPattern>** pattern);
CVAPI(void) cveSinusoidalPatternComputePhaseMap(
    cv::structured_light::SinusoidalPattern* pattern,
    cv::_InputArray* patternImages,
    cv::_OutputArray* wrappedPhaseMap,
    cv::_OutputArray* shadowMask,
    cv::_InputArray* fundamental);
CVAPI(void) cveSinusoidalPatternUnwrapPhaseMap(
    cv::structured_light::SinusoidalPattern* pattern,
    cv::_InputArray* wrappedPhaseMap,
    cv::_OutputArray* unwrappedPhaseMap,
    CvSize* camSize,
    cv::_InputArray* shadowMask);


CVAPI(cv::structured_light::GrayCodePattern*) cveGrayCodePatternCreate(
    int width,
    int height,
    cv::Ptr<cv::structured_light::GrayCodePattern>** sharedPtr,
    cv::structured_light::StructuredLightPattern** structuredLightPattern,
    cv::Algorithm** algorithm);
CVAPI(void) cveGrayCodePatternRelease(cv::Ptr<cv::structured_light::GrayCodePattern>** pattern);



#endif