//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "video_c.h"

//BackgroundSubtractorMOG2
cv::BackgroundSubtractorMOG2* CvBackgroundSubtractorMOG2Create(int history,  float varThreshold, bool bShadowDetection)
{
   cv::Ptr<cv::BackgroundSubtractorMOG2> ptr =  cv::createBackgroundSubtractorMOG2(history, varThreshold, bShadowDetection);
   ptr.addref();
   return ptr.obj;
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
   return ptr.obj;
}

void CvBackgroundSubtractorMOGRelease(cv::BackgroundSubtractorMOG** bgSubstractor)
{
   delete *bgSubstractor;
   *bgSubstractor = 0;
}

CVAPI(void)  cvCalcOpticalFlowDualTVL1(CvArr* i0, CvArr* i1, CvArr* flow)
{
   cv::Mat i0Mat = cv::cvarrToMat(i0);
   cv::Mat i1Mat = cv::cvarrToMat(i1);
   cv::Mat flowMat = cv::cvarrToMat(flow);

   cv::Ptr<cv::DenseOpticalFlow> dof = cv::createOptFlow_DualTVL1();
   dof->calc(i0Mat, i1Mat, flowMat);
}