//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "tracking_c.h"

cv::Tracker* cveTrackerCreate(cv::String* trackerType)
{
   cv::Ptr<cv::Tracker> tracker = cv::Tracker::create(*trackerType);
   tracker.addref(); 
   return tracker.get();
}
bool cveTrackerInit(cv::Tracker* tracker, cv::Mat* image, CvRect* boundingBox)
{
   return tracker->init(*image, *boundingBox);
}
bool cveTrackerUpdate(cv::Tracker* tracker, cv::Mat* image, CvRect* boundingBox)
{
   cv::Rect2d box;
   bool result = tracker->update(*image, box);
   *boundingBox = box;
   return result;
}
void cveTrackerRelease(cv::Tracker** tracker)
{
   delete *tracker;
   *tracker = 0;
}

cv::MultiTracker* cveMultiTrackerInit(cv::String* trackerType)
{
   return new cv::MultiTracker(*trackerType);
}
bool cveMultiTrackerAdd(cv::MultiTracker* tracker, cv::Mat* image, CvRect* boundingBox)
{
   return tracker->add(*image, *boundingBox);
}
bool cveMultiTrackerAddType(cv::MultiTracker* tracker, cv::String* trackerType, cv::Mat* image, CvRect* boundingBox)
{
   return tracker->add(*trackerType, *image, *boundingBox);
}
bool cveMultiTrackerUpdate(cv::MultiTracker* tracker, cv::Mat* image, std::vector<CvRect>* boundingBox)
{
   std::vector<cv::Rect2d> bb;
   bool result = tracker->update(*image, bb);
   boundingBox->clear();
   for (std::vector<cv::Rect2d>::iterator it = bb.begin(); it != bb.end(); ++it)
   {
      boundingBox->push_back(*it);
   }
   return result;
}
void cveMultiTrackerRelease(cv::MultiTracker** tracker)
{
   delete* tracker;
   *tracker = 0;
}