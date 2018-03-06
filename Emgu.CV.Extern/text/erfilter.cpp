//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "text_c.h"

//ERFilter
cv::text::ERFilter* cveERFilterNM1Create(
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
cv::text::ERFilter* cveERFilterNM2Create(cv::String* classifier, float minProbability)
{
   cv::Ptr<cv::text::ERFilter> filter = cv::text::createERFilterNM2(cv::text::loadClassifierNM2(*classifier), minProbability);
   filter.addref();
   return filter.get();

}
void cveERFilterRelease(cv::text::ERFilter** filter)
{
   delete *filter;
   *filter = 0;
}
void cveERFilterRun(cv::text::ERFilter* filter, cv::_InputArray* image, std::vector<cv::text::ERStat>* regions)
{
   filter->run(*image, *regions);
}

void cveERGrouping(
   cv::_InputArray* image, cv::_InputArray* channels, 
   std::vector<cv::text::ERStat>** regions, int count, 
   std::vector< std::vector<cv::Vec2i> >* groups, std::vector<cv::Rect>* group_rects, 
   int method, cv::String* fileName, float minProbability )
{
   std::vector< std::vector< cv::text::ERStat > > statVecs;
   for (int i = 0; i < count; i++)
   {
      statVecs.push_back(*regions[i]);
   }
   
   cv::text::erGrouping(*image, *channels, statVecs, *groups, *group_rects, method, *fileName, minProbability);

}

void cveMSERsToERStats(
	cv::_InputArray* image,
	std::vector< std::vector< cv::Point > >* contours,
	std::vector< std::vector< cv::text::ERStat> >* regions)
{
	cv::text::MSERsToERStats(*image, *contours, *regions);
}

void cveComputeNMChannels(cv::_InputArray* src, cv::_OutputArray* channels, int mode)
{
	cv::text::computeNMChannels(*src, *channels, mode);
}