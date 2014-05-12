//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cuda_c.h"

cv::cuda::CascadeClassifier_CUDA* cudaCascadeClassifierCreate(cv::String* filename)
{
   return new cv::cuda::CascadeClassifier_CUDA(*filename);
}

void cudaCascadeClassifierRelease(cv::cuda::CascadeClassifier_CUDA** classifier)
{
   delete *classifier;
   *classifier = 0;
}

int cudaCascadeClassifierDetectMultiScale(cv::cuda::CascadeClassifier_CUDA* classifier, const cv::cuda::GpuMat* image, cv::cuda::GpuMat* objectsBuf, double scaleFactor, int minNeighbors, CvSize minSize, CvSeq* results)
{
   cvClearSeq(results);
   int count = classifier->detectMultiScale(*image, *objectsBuf, scaleFactor, minNeighbors, minSize);
   if (count == 0) return count;

   cv::cuda::GpuMat detectedRectangles = objectsBuf->colRange(0, count);
   cv::Mat mat;
   detectedRectangles.download(mat);
   cvSeqPushMulti(results, mat.data, count);
   return count;
}