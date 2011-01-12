#include <vector>
#include "opencv2/core/core_c.h"
#include "opencv2/features2d/features2d.hpp"

CVAPI(std::vector<cv::KeyPoint>*) VectorOfKeyPointCreate() 
{ 
   return new std::vector<cv::KeyPoint>(); 
}

CVAPI(std::vector<cv::KeyPoint>*) VectorOfKeyPointCreateSize(int size) 
{ 
   return new std::vector<cv::KeyPoint>(size); 
}

CVAPI(int) VectorOfKeyPointGetSize(std::vector<cv::KeyPoint>* v)
{
   return v->size();
}

CVAPI(void) VectorOfKeyPointPushMulti(std::vector<cv::KeyPoint>* v, cv::KeyPoint* values, int count)
{
   for(int i=0; i < count; i++) v->push_back(*values++);
}

CVAPI(void) VectorOfKeyPointClear(std::vector<cv::KeyPoint>* v)
{
   v->clear();
}

CVAPI(void) VectorOfKeyPointRelease(std::vector<cv::KeyPoint>* v)
{
   delete v;
}

CVAPI(void) VectorOfKeyPointCopyData(std::vector<cv::KeyPoint>* v, cv::KeyPoint* data)
{
   memcpy(data, &(*v)[0], v->size() * sizeof(cv::KeyPoint));
}

CVAPI(cv::KeyPoint*) VectorOfKeyPointGetStartAddress(std::vector<cv::KeyPoint>* v)
{
   return &(*v)[0];
}