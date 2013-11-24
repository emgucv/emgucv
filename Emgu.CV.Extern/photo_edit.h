//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef PHOTO_EDIT_H
#define PHOTO_EDIT_H

#include "opencv2/core/core_c.h"
#include "opencv2/imgproc/imgproc.hpp"
//#include "opencv2/calib3d/calib3d.hpp"

CVAPI(void) cvBlendBgraOverBgr(IplImage* bgra, IplImage* bgr, IplImage* dstBgr);
CVAPI(void) cvBlendBgrOverBgrWithAlpha(IplImage* bgrTop, IplImage* alpha, IplImage* bgr, IplImage* dstBgr);
CVAPI(void) cvVignetteMaskCreate(IplImage* mask, float centerX, float centerY, float fullColorRadius, float halfPowerRadius);
CVAPI(void) cvCovertBgr2Bgr_Gray(IplImage* bgr, IplImage* gray_3channel);

//start is [0, 360)
//end is [0.360)
//if start > end, that means the range covered is [start, 360) & [0, end]
CVAPI(void) cvSelectiveColor(IplImage* bgr, IplImage* result, float start, float end);

#endif