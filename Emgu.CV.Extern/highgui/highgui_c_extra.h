//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_HIGHGUI_C_H
#define EMGU_HIGHGUI_C_H

#include "opencv2/highgui/highgui_c.h"
#include "opencv2/highgui/highgui.hpp"

CVAPI(void) cveImshow(cv::String* winname, cv::_InputArray* mat);
CVAPI(void) cveNamedWindow(cv::String* winname, int flags);
CVAPI(void) cveDestroyWindow(cv::String* winname);
CVAPI(void) cveDestroyAllWindows();
CVAPI(int) cveWaitKey(int delay);

#endif