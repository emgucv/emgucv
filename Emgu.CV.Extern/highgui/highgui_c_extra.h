//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_HIGHGUI_C_H
#define EMGU_HIGHGUI_C_H

#include "opencv2/highgui/highgui_c.h"
#include "opencv2/highgui/highgui.hpp"

struct ColorPoint
{
   CvPoint3D32f position;
   unsigned char blue;
   unsigned char green;
   unsigned char red;
};

CVAPI(void) OpenniGetColorPoints(
                                 CvCapture* capture, // must be an openni capture
                                 CvSeq* points, // sequence of ColorPoint
                                 IplImage* mask // CV_8UC1
                                 );

CVAPI(cv::Mat*) cvMatCreateFromFile(char* fileName, int flags);
#endif