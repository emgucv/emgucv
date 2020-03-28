//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_HIGHGUI_C_H
#define EMGU_HIGHGUI_C_H
#include "opencv2/core/cvdef.h"
#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_HIGHGUI
#include "opencv2/highgui/highgui_c.h"
#include "opencv2/highgui/highgui.hpp"
#else
static inline CV_NORETURN void throw_no_highgui() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without highgui support"); }
#endif


CVAPI(void) cveImshow(cv::String* winname, cv::_InputArray* mat);
CVAPI(void) cveNamedWindow(cv::String* winname, int flags);
CVAPI(void) cveSetWindowProperty(cv::String* winname, int propId, double propValue);
CVAPI(void) cveSetWindowTitle(cv::String* winname, cv::String* title);
CVAPI(double) cveGetWindowProperty(cv::String* winname, int propId);
CVAPI(void) cveDestroyWindow(cv::String* winname);
CVAPI(void) cveDestroyAllWindows();
CVAPI(int) cveWaitKey(int delay);

CVAPI(void) cveSelectROI(
	cv::String* windowName,
	cv::_InputArray* img,
	bool showCrosshair,
	bool fromCenter,
	CvRect* roi);

CVAPI(void) cveSelectROIs(
	cv::String* windowName,
	cv::_InputArray* img,
	std::vector< cv::Rect >* boundingBoxs,
	bool showCrosshair,
	bool fromCenter);
#endif