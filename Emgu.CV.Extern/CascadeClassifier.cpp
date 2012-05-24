//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"

cv::CascadeClassifier* CvCascadeClassifierCreate(char* fileName)
{
   return new cv::CascadeClassifier(fileName);
}
void CvCascadeClassifierRelease(cv::CascadeClassifier** classifier)
{
   delete *classifier;
   *classifier = 0;
}
void CvCascadeClassifierDetectMultiScale( 
   cv::CascadeClassifier* classifier,
   const IplImage* image,
   CvSeq* objects,
   double scaleFactor,
   int minNeighbors, int flags,
   CvSize minSize,
   CvSize maxSize)
{
   std::vector<cv::Rect> rectangles;
   cv::Mat mat = cv::cvarrToMat(image);
   classifier->detectMultiScale(mat, rectangles, scaleFactor, minNeighbors, flags, minSize, maxSize);
   cvClearSeq(objects);
   if (rectangles.size() > 0)
      cvSeqPushMulti(objects, &rectangles[0], static_cast<int>(rectangles.size()), 1);
}