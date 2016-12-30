//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"

cv::CascadeClassifier* cveCascadeClassifierCreate()
{
   return new cv::CascadeClassifier();
}
cv::CascadeClassifier* cveCascadeClassifierCreateFromFile(cv::String* fileName)
{
   return new cv::CascadeClassifier(*fileName);
}
bool cveCascadeClassifierRead(cv::CascadeClassifier* classifier, cv::FileNode* node)
{
   return classifier->read(*node);
}
void cveCascadeClassifierRelease(cv::CascadeClassifier** classifier)
{
   delete *classifier;
   *classifier = 0;
}
void cveCascadeClassifierDetectMultiScale( 
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
bool cveCascadeClassifierIsOldFormatCascade(cv::CascadeClassifier* classifier)
{
   return classifier->isOldFormatCascade();
}

void cveCascadeClassifierGetOriginalWindowSize(cv::CascadeClassifier* classifier, CvSize* size)
{
   cv::Size s = classifier->getOriginalWindowSize();
   size->width = s.width;
   size->height = s.height;
}