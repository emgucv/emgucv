//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_C_H
#define EMGU_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/objdetect/objdetect.hpp"
//#include "opencv2/text/erfilter.hpp"

namespace emgu
{
/*
   struct size
   {
      int width;
      int height;
   };
*/
   struct cvStructSizes
   {
      int CvPoint;
      int CvPoint2D32f;
      int CvPoint3D32f;
      int CvSize;
      int CvSize2D32f;
      int CvScalar;
      int CvRect;
      int CvBox2D;
      int CvMat;
      int CvMatND;
      //int CvHistogram;
      int CvTermCriteria;
      //int CvSeq;
      //int CvContour;
      int IplImage;
      //int ERStat;
   };
}

CVAPI(void) cveGetCvStructSizes(emgu::cvStructSizes* sizes);

CVAPI(void) testDrawLine(IplImage* img, int startX, int startY, int endX, int endY, CvScalar c);

CVAPI(void) cveMemcpy(void* dst, void* src, int length);
#endif