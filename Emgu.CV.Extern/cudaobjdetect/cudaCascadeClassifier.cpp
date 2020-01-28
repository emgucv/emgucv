//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudaobjdetect_c.h"

cv::cuda::CascadeClassifier* cudaCascadeClassifierCreate(cv::String* filename, cv::Ptr<cv::cuda::CascadeClassifier>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAOBJDETECT
   cv::Ptr<cv::cuda::CascadeClassifier> ptr = cv::cuda::CascadeClassifier::create(*filename);
   *sharedPtr = new cv::Ptr < cv::cuda::CascadeClassifier >(ptr);
   return ptr.get();
#else
	throw_no_cudaobjdetect();
#endif
}

cv::cuda::CascadeClassifier* cudaCascadeClassifierCreateFromFileStorage(cv::FileStorage* filestorage, cv::Ptr<cv::cuda::CascadeClassifier>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAOBJDETECT
   cv::Ptr<cv::cuda::CascadeClassifier> ptr = cv::cuda::CascadeClassifier::create(*filestorage);
   *sharedPtr = new cv::Ptr < cv::cuda::CascadeClassifier >(ptr);
   return ptr.get();
#else
	throw_no_cudaobjdetect();
#endif
}

void cudaCascadeClassifierRelease(cv::Ptr<cv::cuda::CascadeClassifier>** classifier)
{
#ifdef HAVE_OPENCV_CUDAOBJDETECT
   delete *classifier;
   *classifier = 0;
#else
	throw_no_cudaobjdetect();
#endif
}

void cudaCascadeClassifierDetectMultiScale(cv::cuda::CascadeClassifier* classifier, cv::_InputArray* image, cv::_OutputArray* objects, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAOBJDETECT
   classifier->detectMultiScale(*image, *objects, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaobjdetect();
#endif
}

void cudaCascadeClassifierConvert(cv::cuda::CascadeClassifier* classifier, cv::_OutputArray* gpuObjects, std::vector<cv::Rect>* objects)
{
#ifdef HAVE_OPENCV_CUDAOBJDETECT
   classifier->convert(*gpuObjects, *objects);
#else
	throw_no_cudaobjdetect();
#endif
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
#ifdef HAVE_OPENCV_CUDAOBJDETECT
   cv::Size s = classifier->getMinObjectSize();
   minObjectSize->width = s.width;
   minObjectSize->height = s.height;
#else
	throw_no_cudaobjdetect();
#endif
}

void cudaCascadeClassifierSetMinObjectSize(cv::cuda::CascadeClassifier* classifier, CvSize* minObjectSize)
{
#ifdef HAVE_OPENCV_CUDAOBJDETECT
   CvSize s = cvSize(minObjectSize->width, minObjectSize->height);
   classifier->setMinObjectSize(s);
#else
	throw_no_cudaobjdetect();
#endif
}