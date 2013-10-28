//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"

//ERFilter
cv::ERFilter* CvERFilterNM1Create(
   const char* classifier, 
   int thresholdDelta, 
   float minArea,
   float maxArea, 
   float minProbability,
   bool nonMaxSuppression,
   float minProbabilityDiff)
{
   cv::Ptr<cv::ERFilter> filter = cv::createERFilterNM1(cv::loadClassifierNM1(classifier), thresholdDelta, minArea, maxArea, minProbability, nonMaxSuppression, minProbabilityDiff);
   filter.addref();
   return filter.get();
}
cv::ERFilter* CvERFilterNM2Create(const char* classifier, float minProbability)
{
   cv::Ptr<cv::ERFilter> filter = cv::createERFilterNM2(cv::loadClassifierNM2(classifier), minProbability);
   filter.addref();
   return filter.get();

}
void CvERFilterRelease(cv::ERFilter** filter)
{
   delete *filter;
   *filter = 0;
}
void CvERFilterRun(cv::ERFilter* filter, CvArr* image, std::vector<cv::ERStat>* regions)
{
   cv::Mat mat = cv::cvarrToMat(image);
   filter->run(mat, *regions);
}

void CvERGrouping(IplImage** channels, std::vector<cv::ERStat>** regions, int count, std::vector<cv::Rect>* groups)
{
   std::vector< cv::Mat > channelMat; 
   std::vector< std::vector< cv::ERStat > > statVecs;
   for (int i = 0; i < count; i++)
   {
      channelMat.push_back(cv::cvarrToMat(channels[i]));
      statVecs.push_back(*regions[i]);
   }
   cv::erGrouping(channelMat, statVecs, *groups);

}

//----------------------------------------------------------------------------
//
//  Vector of ERStat
//
//----------------------------------------------------------------------------
std::vector<cv::ERStat>* VectorOfERStatCreate()
{
   return new std::vector<cv::ERStat>();
}

std::vector<cv::ERStat>* VectorOfERStatCreateSize(int size)
{
   return new std::vector<cv::ERStat>(size);
}

int VectorOfERStatGetSize(std::vector<cv::ERStat>* v)
{
   return v->size();
}

void VectorOfERStatClear(std::vector<cv::ERStat>* v)
{
   v->clear();
}

void VectorOfERStatRelease(std::vector<cv::ERStat>* v)
{
   delete v;
}

void VectorOfERStatCopyData(std::vector<cv::ERStat>* v, cv::ERStat* data)
{
   VectorCopyData<cv::ERStat>(v, data);
}

cv::ERStat* VectorOfERStatGetStartAddress(std::vector<cv::ERStat>* v)
{
   return v->empty() ? NULL : &(*v)[0];
}