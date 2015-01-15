//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"

cv::CascadeClassifier* CvCascadeClassifierCreate(cv::String* fileName)
{
   return new cv::CascadeClassifier(*fileName);
}
void CvCascadeClassifierRelease(cv::CascadeClassifier** classifier)
{
   delete *classifier;
   *classifier = 0;
}
void CvCascadeClassifierDetectMultiScale( 
   cv::CascadeClassifier* classifier,
   cv::_InputArray* image,
   std::vector<cv::Rect>* objects,
   double scaleFactor,
   int minNeighbors, int flags,
   CvSize* minSize,
   CvSize* maxSize)
{
   classifier->detectMultiScale(*image, *objects, scaleFactor, minNeighbors, flags, *minSize, *maxSize);
}
bool CvCascadeClassifierIsOldFormatCascade(cv::CascadeClassifier* classifier)
{
   return classifier->isOldFormatCascade();
}

void CvCascadeClassifierGetOriginalWindowSize(cv::CascadeClassifier* classifier, CvSize* size)
{
   cv::Size s = classifier->getOriginalWindowSize();
   size->width = s.width;
   size->height = s.height;
}