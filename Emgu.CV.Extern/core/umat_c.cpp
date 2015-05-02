//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "umat_c.h"

cv::UMat* cveUMatCreate()
{
   return new cv::UMat();
}

void cveUMatUseCustomAllocator(cv::UMat* mat, MatAllocateCallback allocator, MatDeallocateCallback deallocator, void* allocateDataActionPtr, void* freeDataActionPtr, cv::MatAllocator** matAllocator, cv::MatAllocator** oclAllocator)
{
   *matAllocator = emguMatAllocatorCreate(allocator, deallocator, allocateDataActionPtr, freeDataActionPtr);
   *oclAllocator = mat->getStdAllocator(*matAllocator);
   
   if (*oclAllocator == *matAllocator)
   {
      *oclAllocator = 0;
      mat->allocator = *matAllocator;
   } else
   {
      mat->allocator = *oclAllocator;
   }
}
void cveUMatCreateData(cv::UMat* mat, int row, int cols, int type)
{
   mat->create(row, cols, type);
}
cv::UMat* cveUMatCreateFromROI(cv::UMat* mat, CvRect* roi)
{
   return new cv::UMat(*mat, *roi);
}
void cveUMatRelease(cv::UMat** mat)
{
   delete *mat;
   *mat = 0;
}
emgu::size cveUMatGetSize(cv::UMat* mat)
{
   emgu::size s;
   s.width = mat->cols;
   s.height = mat->rows;
   return s;
}
void cveUMatCopyTo(cv::UMat* mat, cv::_OutputArray* m, cv::_InputArray* mask)
{
   if (mask)
   {
      CV_Error(0, "This is not implemented");
      //mat->copyTo(*m, *mask);
   }
   else
      mat->copyTo(*m);
}

int cveUMatGetElementSize(cv::UMat* mat)
{
   return static_cast<int>( mat->elemSize());
}

void cveUMatSetTo(cv::UMat* mat, cv::_InputArray* value, cv::_InputArray* mask)
{
   mat->setTo(*value, mask ? *mask : (cv::InputArray) cv::noArray());
}

cv::Mat* cveUMatGetMat(cv::UMat* mat, int access)
{
   cv::Mat* result = new cv::Mat();
   cv::Mat tmp = mat->getMat(access);
   cv::swap(*result, tmp);
   return result;
}

void cveUMatConvertTo(  cv::UMat* mat, cv::_OutputArray* out, int rtype, double alpha, double beta )
{
   mat->convertTo(*out, rtype, alpha, beta);
}

cv::UMat* cveUMatReshape(cv::UMat* mat, int cn, int rows)
{
   cv::UMat* result = new cv::UMat();
   cv::UMat m = mat->reshape(cn, rows);
   cv::swap(m, *result);
   return result;
}
