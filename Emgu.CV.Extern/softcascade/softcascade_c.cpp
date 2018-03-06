//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "softcascade_c.h"

cv::softcascade::Detector* cveSoftCascadeDetectorCreate(cv::String* fileName, double minScale, double maxScale, int scales, int rejCriteria)
{
   cv::FileStorage fs(*fileName, cv::FileStorage::READ);
   char errMsg[512];

   if (!fs.isOpened())
   {
      sprintf(errMsg, "Unable to open soft cascade file: %s", fileName );
      CV_Error(0, errMsg);
   }
   
   cv::softcascade::Detector* cascade = new cv::softcascade::Detector(minScale, maxScale, scales, rejCriteria);
   
   if (!cascade->load(fs.getFirstTopLevelNode()))
   {
      sprintf(errMsg, "Invalid soft cascade file (cannot parse): %s", fileName );
      CV_Error(0, errMsg);
   }
   return cascade;
}

void cveSoftCascadeDetectorDetect(cv::softcascade::Detector* detector, cv::_InputArray* image, std::vector<cv::Rect>* rois, std::vector<cv::Rect>* rects, std::vector<float>* confidents)
{
   std::vector<cv::softcascade::Detection> detections;
   detector->detect(*image, rois? *rois : cv::noArray(), detections);

   rects->resize(detections.size());
   confidents->resize(detections.size());
   int i = 0;
   for (std::vector<cv::softcascade::Detection>::const_iterator it = detections.begin(); it != detections.end(); ++it, ++i)
   {
      (*rects)[i] = (*it).bb() ;
      (*confidents)[i] = (*it).confidence;
   }
}

void cveSoftCascadeDetectorRelease(cv::softcascade::Detector** detector)
{
   delete *detector;
   *detector = 0;
}

cv::softcascade::SCascade* cudaSoftCascadeDetectorCreate(cv::String* fileName, const double minScale, const double maxScale, const int scales, const int flags)
{
   cv::FileStorage fs(*fileName, cv::FileStorage::READ);
   char errMsg[512];

   if (!fs.isOpened())
   {
      sprintf(errMsg, "Unable to open soft cascade file: %s", fileName );
      CV_Error(0, errMsg);
   }

   cv::softcascade::SCascade* cascade = new cv::softcascade::SCascade(minScale, maxScale, scales, flags| cv::softcascade::ChannelsProcessor::SEPARABLE);
   
   if (!cascade->load(fs.getFirstTopLevelNode()))
   {
      sprintf(errMsg, "Invalid soft cascade file (cannot parse): %s", fileName );
      CV_Error(0, errMsg);
   }
   return cascade;
}

void cudaSoftCascadeDetectorDetect(cv::softcascade::SCascade* detector, cv::cuda::GpuMat* image, cv::cuda::GpuMat* rois, cv::cuda::GpuMat* detections, cv::cuda::Stream* stream)
{
   detector->detect(*image, *rois, *detections, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaSoftCascadeDetectorRelease(cv::softcascade::SCascade** detector)
{
   delete *detector;
   *detector = 0;
}