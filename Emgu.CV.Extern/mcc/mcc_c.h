//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_MCC_C_H
#define EMGU_MCC_C_H

#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_MCC
#include "opencv2/mcc.hpp"
#else
static inline CV_NORETURN void throw_no_mcc() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without mcc support"); }

#endif

CVAPI(cv::mcc::CChecker*) cveCCheckerCreate(cv::Ptr<cv::mcc::CChecker>** sharedPtr);
CVAPI(void) cveCCheckerRelease(cv::Ptr<cv::mcc::CChecker>** sharedPtr);


CVAPI(cv::mcc::CCheckerDraw*) cveCCheckerDrawCreate(
    cv::mcc::CChecker* pChecker,
    CvScalar* color,
    int thickness,
    cv::Ptr<cv::mcc::CCheckerDraw>** sharedPtr);
CVAPI(void) cveCCheckerDrawDraw(cv::mcc::CCheckerDraw* ccheckerDraw, cv::_InputOutputArray* img);
CVAPI(void) cveCCheckerDrawRelease(cv::Ptr<cv::mcc::CCheckerDraw>** sharedPtr);

CVAPI(cv::mcc::CCheckerDetector*) cveCCheckerDetectorCreate(cv::Algorithm** algorithm, cv::Ptr<cv::mcc::CCheckerDetector>** sharedPtr);
CVAPI(bool) cveCCheckerDetectorProcess(
    cv::mcc::CCheckerDetector* detector,
    cv::_InputArray* image, 
    const cv::mcc::TYPECHART chartType,
    const int nc, 
    bool useNet,
    cv::mcc::DetectorParameters* param);
CVAPI(cv::mcc::CChecker*) cveCCheckerDetectorGetBestColorChecker(cv::mcc::CCheckerDetector* detector);
CVAPI(void) cveCCheckerDetectorRelease(cv::Ptr<cv::mcc::CCheckerDetector>** sharedPtr);

#endif