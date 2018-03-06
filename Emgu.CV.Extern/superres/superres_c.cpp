//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "superres_c.h"

cv::superres::FrameSource* cvSuperresCreateFrameSourceVideo(cv::String* fileName, bool useGpu)
{
   cv::Ptr<cv::superres::FrameSource> ptr = useGpu ?
      cv::superres::createFrameSource_Video_CUDA(*fileName)
      : cv::superres::createFrameSource_Video(*fileName);
   ptr.addref();
   return ptr.get();
}
cv::superres::FrameSource* cvSuperresCreateFrameSourceCamera(int deviceId)
{
   cv::Ptr<cv::superres::FrameSource> ptr = cv::superres::createFrameSource_Camera(deviceId);
   ptr.addref();
   return ptr.get();
}
void cvSuperresFrameSourceRelease(cv::superres::FrameSource** frameSource)
{
   delete *frameSource;
   *frameSource = 0;
}
void cvSuperresFrameSourceNextFrame(cv::superres::FrameSource* frameSource, cv::_OutputArray* frame)
{
   frameSource->nextFrame(*frame);
}

cv::superres::SuperResolution* cvSuperResolutionCreate(int type, cv::superres::FrameSource* frameSource, cv::superres::FrameSource** frameSourceOut)
{
   cv::Ptr<cv::superres::SuperResolution> ptr = 
      (type == 1) ? cv::superres::createSuperResolution_BTVL1_CUDA() :
      //((type == 2) ? cv::superres::createSuperResolution_BTVL1_OCL() :
      cv::superres::createSuperResolution_BTVL1();
   
   cv::Ptr<cv::superres::FrameSource> fsPtr(frameSource);
   
   ptr->setInput(fsPtr);
   cv::Mat tmp;
   ptr->nextFrame(tmp);
   *frameSourceOut = dynamic_cast<cv::superres::FrameSource*>(ptr.get());

   fsPtr.addref();
   ptr.addref();
   return ptr.get();
}
void cvSuperResolutionRelease(cv::superres::SuperResolution** superres)
{
   delete *superres;
   *superres = 0;
}
