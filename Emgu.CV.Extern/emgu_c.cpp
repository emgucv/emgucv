//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "emgu_c.h"

void cveGetCvStructSizes(emgu::cvStructSizes* sizes)
{
   sizes->CvPoint = sizeof(cv::Point);
   sizes->CvPoint2D32f = sizeof(cv::Point2f);
   sizes->CvPoint3D32f = sizeof(cv::Point3f);
   sizes->CvSize = sizeof(cv::Size);
   sizes->CvSize2D32f = sizeof(cv::Size2f);
   sizes->CvScalar = sizeof(cv::Scalar);
   sizes->CvRect = sizeof(cv::Rect);
   sizes->CvRotatedRect = sizeof(cv::RotatedRect);
   //sizes->CvMat = sizeof(CvMat);
   //sizes->CvMatND = sizeof(CvMatND);
   sizes->CvTermCriteria = sizeof(cv::TermCriteria);
   //sizes->IplImage = sizeof(IplImage);
   
}

void cveMemcpy(void* dst, void* src, int length)
{
   memcpy(dst, src, length);
}