//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudaobjdetect_c.h"

cv::cuda::CascadeClassifier* cudaCascadeClassifierCreate(cv::String* filename)
{
   cv::Ptr<cv::cuda::CascadeClassifier> ptr = cv::cuda::CascadeClassifier::create(*filename);
   ptr.addref();
   return ptr.get();
}

cv::cuda::CascadeClassifier* cudaCascadeClassifierCreateFromFileStorage(cv::FileStorage* filestorage)
{
   cv::Ptr<cv::cuda::CascadeClassifier> ptr = cv::cuda::CascadeClassifier::create(*filestorage);
   ptr.addref();
   return ptr.get();
}

void cudaCascadeClassifierRelease(cv::cuda::CascadeClassifier** classifier)
{
   delete *classifier;
   *classifier = 0;
}

void cudaCascadeClassifierDetectMultiScale(cv::cuda::CascadeClassifier* classifier, cv::_InputArray* image, cv::_OutputArray* objects, cv::cuda::Stream* stream)
{
   classifier->detectMultiScale(*image, *objects, stream ? *stream : cv::cuda::Stream::Null());  
}

void cudaCascadeClassifierConvert(cv::cuda::CascadeClassifier* classifier, cv::_OutputArray* gpuObjects, std::vector<cv::Rect>* objects)
{
   classifier->convert(*gpuObjects, *objects);
}
/*

double cudaCascadeClassifierGetScaleFactor(cv::cuda::CascadeClassifier* classifier)
{
   return classifier->getScaleFactor();
}

void cudaCascadeClassifierSetScaleFactor(cv::cuda::CascadeClassifier* classifier, double scaleFactor)
{
   classifier->setScaleFactor(scaleFactor); 
}

int cudaCascadeClassifierGetMinNeighbors(cv::cuda::CascadeClassifier* classifier)
{
   return classifier->getMinNeighbors();
}

void cudaCascadeClassifierSetMinNeighbors(cv::cuda::CascadeClassifier* classifier, int minNeighbours)
{
   classifier->setMinNeighbors(minNeighbours); 
}*/

void cudaCascadeClassifierGetMinObjectSize(cv::cuda::CascadeClassifier* classifier, CvSize* minObjectSize)
{
   CvSize s = classifier->getMinObjectSize();
   minObjectSize->width = s.width;
   minObjectSize->height = s.height;
}

void cudaCascadeClassifierSetMinObjectSize(cv::cuda::CascadeClassifier* classifier, CvSize* minObjectSize)
{
   CvSize s = cvSize(minObjectSize->width, minObjectSize->height);
   classifier->setMinObjectSize(s);
}