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