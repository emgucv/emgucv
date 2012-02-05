//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gpu_c.h"

cv::gpu::CascadeClassifier_GPU* gpuCascadeClassifierCreate(const char* filename)
{
   return new cv::gpu::CascadeClassifier_GPU(filename);
}

void gpuCascadeClassifierRelease(cv::gpu::CascadeClassifier_GPU** classifier)
{
   delete *classifier;
   *classifier = 0;
}

int gpuCascadeClassifierDetectMultiScale(cv::gpu::CascadeClassifier_GPU* classifier, const cv::gpu::GpuMat* image, cv::gpu::GpuMat* objectsBuf, double scaleFactor, int minNeighbors, CvSize minSize, CvSeq* results)
{
   cvClearSeq(results);
   int count = classifier->detectMultiScale(*image, *objectsBuf, scaleFactor, minNeighbors, minSize);
   if (count == 0) return count;

   cv::gpu::GpuMat detectedRectangles = objectsBuf->colRange(0, count);
   cv::Mat mat;
   detectedRectangles.download(mat);
   cvSeqPushMulti(results, mat.data, count);
   return count;
}