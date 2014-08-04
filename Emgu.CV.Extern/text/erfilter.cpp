//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "text_c.h"

//ERFilter
cv::text::ERFilter* CvERFilterNM1Create(
   cv::String* classifier, 
   int thresholdDelta, 
   float minArea,
   float maxArea, 
   float minProbability,
   bool nonMaxSuppression,
   float minProbabilityDiff)
{
   cv::Ptr<cv::text::ERFilter> filter = cv::text::createERFilterNM1(cv::text::loadClassifierNM1(*classifier), thresholdDelta, minArea, maxArea, minProbability, nonMaxSuppression, minProbabilityDiff);
   filter.addref();
   return filter.get();
}
cv::text::ERFilter* CvERFilterNM2Create(cv::String* classifier, float minProbability)
{
   cv::Ptr<cv::text::ERFilter> filter = cv::text::createERFilterNM2(cv::text::loadClassifierNM2(*classifier), minProbability);
   filter.addref();
   return filter.get();

}
void CvERFilterRelease(cv::text::ERFilter** filter)
{
   delete *filter;
   *filter = 0;
}
void CvERFilterRun(cv::text::ERFilter* filter, cv::_InputArray* image, std::vector<cv::text::ERStat>* regions)
{
   filter->run(*image, *regions);
}

void CvERGrouping(cv::_InputArray* channels, std::vector<cv::text::ERStat>** regions, int count, cv::String* fileName, float minProbability, std::vector<cv::Rect>* groups)
{
   std::vector< std::vector< cv::text::ERStat > > statVecs;
   for (int i = 0; i < count; i++)
   {
      statVecs.push_back(*regions[i]);
   }
   
   cv::text::erGrouping(*channels, statVecs, *fileName, minProbability, *groups);

}
