//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_C_H
#define EMGU_C_H

#include "opencv2/core/core_c.h"

namespace emgu
{
   struct size
   {
      int width;
      int height;
   };

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
      int CvHistogram;
      int CvTermCriteria;
      int CvSeq;
      int CvContour;
      int IplImage;
   };
}

CVAPI(void) getCvStructSizes(emgu::cvStructSizes* sizes);

CVAPI(void) testDrawLine(IplImage* img, int startX, int startY, int endX, int endY, CvScalar c);

#endif