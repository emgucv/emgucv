//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_MCC_C_H
#define EMGU_MCC_C_H

//#include "opencv2/core/core.hpp"
//#include "cvapi_compat.h"

#include "objdetect_c.h"

#ifdef HAVE_OPENCV_OBJDETECT
#include "opencv2/objdetect/mcc_checker_detector.hpp"
#else
//static inline CV_NORETURN void throw_no_mcc() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without mcc support. To use this module, please switch to the full Emgu CV runtime."); }

namespace cv {
	namespace mcc {
		class CChecker {};
		//class CCheckerDraw {};
		class CCheckerDetector {};
		class DetectorParametersMCC {};
		enum ColorChart {};
	}

}
#endif

CVAPI(cv::mcc::CChecker*) cveCCheckerCreate(cv::Ptr<cv::mcc::CChecker>** sharedPtr);
CVAPI(void) cveCCheckerGetBox(cv::mcc::CChecker* checker, std::vector< cv::Point2f >* box);
CVAPI(void) cveCCheckerSetBox(cv::mcc::CChecker* checker, std::vector< cv::Point2f >* box);
CVAPI(void) cveCCheckerGetCenter(cv::mcc::CChecker* checker, cv::Point2f* center);
CVAPI(void) cveCCheckerSetCenter(cv::mcc::CChecker* checker, cv::Point2f* center);
CVAPI(void) cveCCheckerRelease(cv::Ptr<cv::mcc::CChecker>** sharedPtr);
CVAPI(void) cveCCheckerGetChartsRGB(cv::mcc::CChecker* checker, cv::_OutputArray* chartsRgb);
CVAPI(void) cveCCheckerSetChartsRGB(cv::mcc::CChecker* checker, cv::Mat* chartsRgb);

/*
CVAPI(cv::mcc::CCheckerDraw*) cveCCheckerDrawCreate(
	cv::mcc::CChecker* pChecker,
	cv::Scalar* color,
	int thickness,
	cv::Ptr<cv::mcc::CCheckerDraw>** sharedPtr);
CVAPI(void) cveCCheckerDrawDraw(cv::mcc::CCheckerDraw* ccheckerDraw, cv::_InputOutputArray* img);
CVAPI(void) cveCCheckerDrawRelease(cv::Ptr<cv::mcc::CCheckerDraw>** sharedPtr);
*/

CVAPI(cv::mcc::CCheckerDetector*) cveCCheckerDetectorCreate(
	cv::Algorithm** algorithm, 
	cv::Ptr<cv::mcc::CCheckerDetector>** sharedPtr);

CVAPI(bool) cveCCheckerDetectorProcess(
	cv::mcc::CCheckerDetector* detector,
	cv::_InputArray* image,
	std::vector< cv::Rect >* regionOfInterest,
	int nc);

CVAPI(void) cveCCheckerDetectorDraw(
	cv::mcc::CCheckerDetector* detector,
	cv::mcc::CChecker* pChecker, 
	cv::_InputOutputArray* img, 
	cv::Scalar* color, 
	int thickness);

CVAPI(cv::mcc::CChecker*) cveCCheckerDetectorGetBestColorChecker(cv::mcc::CCheckerDetector* detector);

CVAPI(void) cveCCheckerDetectorRelease(cv::Ptr<cv::mcc::CCheckerDetector>** sharedPtr);

CVAPI(cv::mcc::DetectorParametersMCC*) cveDetectorParametersMCCCreate();
CVAPI(void) cveDetectorParametersMCCRelease(cv::mcc::DetectorParametersMCC** parameters);


#endif