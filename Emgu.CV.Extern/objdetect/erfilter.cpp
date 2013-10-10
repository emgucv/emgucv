//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"

//ERFilter
cv::ERFilter* CvERFilterNM1Create()
{
   cv::Ptr<cv::ERFilter> filter = cv::createERFilterNM1();
   filter.addref();
   return filter.obj;
}
cv::ERFilter* CvERFilterNM2Create()
{
   cv::Ptr<cv::ERFilter> filter = cv::createERFilterNM2();
   filter.addref();
   return filter.obj;

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