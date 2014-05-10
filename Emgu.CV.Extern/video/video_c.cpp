//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "video_c.h"

//BackgroundSubtractorMOG2
cv::BackgroundSubtractorMOG2* CvBackgroundSubtractorMOG2Create(int history,  float varThreshold, bool bShadowDetection)
{
   cv::Ptr<cv::BackgroundSubtractorMOG2> ptr =  cv::createBackgroundSubtractorMOG2(history, varThreshold, bShadowDetection);
   ptr.addref();
   return ptr.get();
   //return new cv::BackgroundSubtractorMOG2(history, varThreshold, bShadowDetection);
}

void CvBackgroundSubtractorMOG2Release(cv::BackgroundSubtractorMOG2** bgSubstractor)
{
   delete *bgSubstractor;
   *bgSubstractor = 0;
}

//BackgroundSubtractor
void CvBackgroundSubtractorUpdate(cv::BackgroundSubtractor* bgSubstractor, IplImage* image, IplImage* fgmask, double learningRate)
{
   cv::Mat imgMat = cv::cvarrToMat(image);
   cv::Mat fgMat = cv::cvarrToMat(fgmask);
   bgSubstractor->apply(imgMat, fgMat, learningRate);
}

//BackgroundSubtractorMOG
cv::BackgroundSubtractorMOG* CvBackgroundSubtractorMOGCreate(int history, int nmixtures, double backgroundRatio, double noiseSigma)
{
   cv::Ptr<cv::BackgroundSubtractorMOG> ptr = cv::createBackgroundSubtractorMOG(history, nmixtures, backgroundRatio, noiseSigma);
  
   ptr.addref();
   return ptr.get();
}

void CvBackgroundSubtractorMOGRelease(cv::BackgroundSubtractorMOG** bgSubstractor)
{
   delete *bgSubstractor;
   *bgSubstractor = 0;
}

cv::DenseOpticalFlow* cveDenseOpticalFlowCreateDualTVL1()
{
   cv::Ptr<cv::DenseOpticalFlow> dof = cv::createOptFlow_DualTVL1();
   dof.addref();
   return dof.get();
}
void cveDenseOpticalFlowRelease(cv::DenseOpticalFlow** flow)
{
   delete *flow;
   *flow = 0;
}
void cveDenseOpticalFlowCalc(cv::DenseOpticalFlow* dof, cv::_InputArray* i0, cv::_InputArray* i1, cv::_InputOutputArray* flow)
{
   dof->calc(*i0, *i1, *flow);
}

void cveCalcOpticalFlowFarneback(cv::_InputArray* prev, cv::_InputArray* next, cv::_InputOutputArray* flow, double pyrScale, int levels, int winSize, int iterations, int polyN, double polySigma, int flags)
{
   cv::calcOpticalFlowFarneback(*prev, *next, *flow, pyrScale, levels, winSize, iterations, polyN, polySigma, flags);
}

void cveCalcOpticalFlowPyrLK(cv::_InputArray* prevImg, cv::_InputArray* nextImg, cv::_InputArray* prevPts, cv::_InputOutputArray* nextPts, cv::_OutputArray* status, cv::_OutputArray* err, CvSize* winSize, int maxLevel, CvTermCriteria* criteria, int flags, double minEigenThreshold)
{
   cv::calcOpticalFlowPyrLK(*prevImg, *nextImg, *prevPts, *nextPts, *status, *err, *winSize, maxLevel, *criteria, flags, minEigenThreshold);
}

void cveUpdateMotionHistory(cv::_InputArray* silhouette, cv::_InputOutputArray* mhi, double timestamp, double duration)
{
   cv::updateMotionHistory(*silhouette, *mhi, timestamp, duration);
}
void cveCalcMotionGradient(cv::_InputArray* mhi, cv::_OutputArray* mask, cv::_OutputArray* orientation, double delta1, double delta2, int apertureSize)
{
   cv::calcMotionGradient(*mhi, *mask, *orientation, delta1, delta2, apertureSize);
}
void cveCalcGlobalOrientation(cv::_InputArray* orientation, cv::_InputArray* mask, cv::_InputArray* mhi, double timestamp, double duration)
{
   cv::calcGlobalOrientation(*orientation, *mask, *mhi, timestamp, duration);
}

void cveCamShift( cv::_InputArray* probImage, CvRect* window, CvTermCriteria* criteria, CvBox2D* result)
{
   cv::Rect rect = *window;
   cv::RotatedRect rr = cv::CamShift(*probImage, rect, *criteria);
   *window = rect;
   *result = rr;
}

int cveMeanShift( cv::_InputArray* probImage, CvRect* window, CvTermCriteria* criteria )
{
   cv::Rect rect = *window;
   int result = cv::meanShift(*probImage, rect, *criteria);
   *window = rect;
   return result;
}