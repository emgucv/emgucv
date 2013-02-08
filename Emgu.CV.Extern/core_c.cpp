//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "core_c_extra.h"

cv::Mat* cvMatCreate()
{
   return new cv::Mat();
}
void cvMatRelease(cv::Mat** mat)
{
   delete *mat;
   *mat = 0;
}
CvSize cvMatGetSize(cv::Mat* mat)
{
   return mat->size();
}
void cvMatCopyToCvArr(cv::Mat* mat, CvArr* cvArray)
{
   IplImage iplImage = (*mat);
   cvCopy(&iplImage, cvArray);
}
int cvMatGetElementSize(cv::Mat* mat)
{
   return static_cast<int>( mat->elemSize());
}