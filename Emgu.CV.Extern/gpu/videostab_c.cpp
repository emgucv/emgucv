//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "videostab_c.h"

CaptureFrameSource* CaptureFrameSourceCreate(CvCapture* capture)
{
   return new CaptureFrameSource(capture);
}
void CaptureFrameSourceRelease(CaptureFrameSource** captureFrameSource)
{
   delete *captureFrameSource;
   *captureFrameSource = 0;
}
bool CaptureFrameSourceGetNextFrame(CaptureFrameSource* captureFrameSource, IplImage** nextFrame)
{
   cv::Mat mat = captureFrameSource->nextFrame();
   if (mat.empty())
      return false;

   if (!nextFrame)
   {
      *nextFrame = cvCreateImage(cvSize(mat.cols, mat.rows), mat.depth(), mat.channels());
   }
   CV_Assert(*nextFrame && mat.rows == (*nextFrame)->height && mat.cols == (*nextFrame)->width && mat.channels() == (*nextFrame)->nChannels);
   cv::Mat tmp = cv::cvarrToMat(*nextFrame);
   mat.copyTo(tmp);
   return true;
}

cv::videostab::OnePassStabilizer* OnePassStabilizerCreate(CaptureFrameSource* capture)
{
   cv::videostab::OnePassStabilizer* stabilizer = new cv::videostab::OnePassStabilizer();
   cv::Ptr<cv::videostab::IFrameSource> ptr(capture);
   ptr.addref(); // add reference such that it won't release the CaptureFrameSource
   stabilizer->setFrameSource(ptr);
   return stabilizer;
}

void OnePassStabilizerRelease(cv::videostab::OnePassStabilizer** stabilizer)
{
   delete *stabilizer;
   *stabilizer = 0;
}