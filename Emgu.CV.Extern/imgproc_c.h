//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_IMGPROC_C_H
#define EMGU_IMGPROC_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/calib3d/calib3d.hpp"

CVAPI(IplImage*) cvGetImageSubRect(IplImage* image, CvRect* rect);

//GrabCut
CVAPI(void) CvGrabCut(IplImage* img, IplImage* mask, cv::Rect* rect, IplImage* bgdModel, IplImage* fgdModel, int iterCount, int flag);

//StereoSGBM
CVAPI(cv::StereoSGBM*) CvStereoSGBMCreate(
  int minDisparity, int numDisparities, int SADWindowSize,
  int P1, int P2, int disp12MaxDiff,
  int preFilterCap, int uniquenessRatio,
  int speckleWindowSize, int speckleRange,
  bool fullDP);

CVAPI(void) CvStereoSGBMRelease(cv::StereoSGBM* obj);
CVAPI(void) CvStereoSGBMFindCorrespondence(cv::StereoSGBM* disparitySolver, IplImage* left, IplImage* right, IplImage* disparity);

CVAPI(bool) cvCheckRange(CvArr* arr, bool quiet, CvPoint* index, double minVal, double maxVal);

CVAPI(void) cvArrSqrt(CvArr* src, CvArr* dst);

#endif