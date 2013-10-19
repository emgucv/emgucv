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
emgu::size cvMatGetSize(cv::Mat* mat)
{
   emgu::size s;
   s.width = mat->cols;
   s.height = mat->rows;
   return s;
}
void cvMatCopyToCvArr(cv::Mat* mat, CvArr* cvArray)
{
   cv::Mat dest = cv::cvarrToMat(cvArray);
   mat->copyTo(dest);
}
void cvMatFromCvArr(cv::Mat* mat, CvArr* cvArray)
{
   cv::Mat tmp = cv::cvarrToMat(cvArray);
   cv::swap(*mat, tmp);
}
int cvMatGetElementSize(cv::Mat* mat)
{
   return static_cast<int>( mat->elemSize());
}

bool cvMatIsEmpty(cv::Mat* mat)
{
   return mat->empty();
}

void CvMinMaxIdx(CvArr* src, double* minVal, double* maxVal, int* minIdx, int* maxIdx, CvArr* mask)
{
   cv::Mat srcMat = cv::cvarrToMat(src);
   cv::Mat maskMat = mask ? cv::cvarrToMat(mask) : cv::Mat();
   cv::minMaxIdx(srcMat, minVal, maxVal, minIdx, maxIdx, maskMat);
}