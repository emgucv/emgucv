//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "emgu_c.h"

void cveGetCvStructSizes(emgu::cvStructSizes* sizes)
{
   sizes->CvPoint = sizeof(CvPoint);
   sizes->CvPoint2D32f = sizeof(CvPoint2D32f);
   sizes->CvPoint3D32f = sizeof(CvPoint3D32f);
   sizes->CvSize = sizeof(CvSize);
   sizes->CvSize2D32f = sizeof(CvSize2D32f);
   sizes->CvScalar = sizeof(CvScalar);
   sizes->CvRect = sizeof(CvRect);
   sizes->CvBox2D = sizeof(CvBox2D);
   sizes->CvMat = sizeof(CvMat);
   sizes->CvMatND = sizeof(CvMatND);
   sizes->CvTermCriteria = sizeof(CvTermCriteria);
   sizes->IplImage = sizeof(IplImage);
   
}

void testDrawLine(IplImage* img, int startX, int startY, int endX, int endY, CvScalar c)
{
   cv::Mat m = cv::cvarrToMat(img);
   cv::Point start(startX, startY);
   cv::Point end(endX, endY);
   cv::Scalar color(c.val[0], c.val[1], c.val[2], c.val[3]);
   cv::line(m, start, end, color);
}

void cveMemcpy(void* dst, void* src, int length)
{
   memcpy(dst, src, length);
}