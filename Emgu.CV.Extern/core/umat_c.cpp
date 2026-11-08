//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "umat_c.h"

cv::UMat* cveUMatCreate(int usage)
{
   try
   {
      return new cv::UMat(static_cast<cv::UMatUsageFlags>(usage));
   }
   CVAPI_CATCH_CV_ERRORS(0)
}
/*
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
}*/
void cveUMatCreateData(cv::UMat* mat, int row, int cols, int type, int flags)
{
   try
   {
      mat->create(row, cols, type, static_cast<cv::UMatUsageFlags>(flags));
   }
   CVAPI_CATCH_CV_ERRORS_VOID
}
cv::UMat* cveUMatCreateFromRect(cv::UMat* mat, cv::Rect* roi)
{
   try
   {
      return new cv::UMat(*mat, *roi);
   }
   CVAPI_CATCH_CV_ERRORS(0)
}
cv::UMat* cveUMatCreateFromRange(cv::UMat* umat, cv::Range* rowRange, cv::Range* colRange)
{
   try
   {
      return new cv::UMat(*umat, *rowRange, *colRange);
   }
   CVAPI_CATCH_CV_ERRORS(0)
}
void cveUMatRelease(cv::UMat** mat)
{
   try
   {
      delete *mat;
      *mat = 0;
   }
   CVAPI_CATCH_CV_ERRORS_VOID
}
void cveUMatGetSize(cv::UMat* mat, cv::Size* s)
{
   s->width = mat->cols;
   s->height = mat->rows;
}
void cveUMatCopyTo(cv::UMat* mat, cv::_OutputArray* m, cv::_InputArray* mask)
{
   try
   {
      if (mask)
      {
         mat->copyTo(*m, *mask);
      }
      else
         mat->copyTo(*m);
   }
   CVAPI_CATCH_CV_ERRORS_VOID
}

int cveUMatGetElementSize(cv::UMat* mat)
{
   return static_cast<int>( mat->elemSize());
}

void cveUMatSetTo(cv::UMat* mat, cv::_InputArray* value, cv::_InputArray* mask)
{
   try
   {
      mat->setTo(*value, mask ? *mask : (cv::InputArray) cv::noArray());
   }
   CVAPI_CATCH_CV_ERRORS_VOID
}

cv::Mat* cveUMatGetMat(cv::UMat* mat, int access)
{
   try
   {
      cv::Mat* result = new cv::Mat();
      cv::Mat tmp = mat->getMat( static_cast<cv::AccessFlag>( access ));
      cv::swap(*result, tmp);
      return result;
   }
   CVAPI_CATCH_CV_ERRORS(0)
}

void cveUMatConvertTo(cv::UMat* mat, cv::_OutputArray* out, int rtype, double alpha, double beta )
{
   try
   {
      mat->convertTo(*out, rtype, alpha, beta);
   }
   CVAPI_CATCH_CV_ERRORS_VOID
}

cv::UMat* cveUMatReshape(cv::UMat* mat, int cn, int rows)
{
   try
   {
      cv::UMat* result = new cv::UMat();
      cv::UMat m = mat->reshape(cn, rows);
      cv::swap(m, *result);
      return result;
   }
   CVAPI_CATCH_CV_ERRORS(0)
}

void cveUMatCopyDataTo(cv::UMat* mat, unsigned char* dest)
{
   try
   {
      //const int* sizes = mat->size;
      cv::Mat destMat = cv::Mat(mat->dims, mat->size.data(), mat->type(), dest);
      mat->copyTo(destMat);
   }
   CVAPI_CATCH_CV_ERRORS_VOID
}

void cveUMatCopyDataFrom(cv::UMat* mat, unsigned char* source)
{
   try
   {
      //const int* sizes = mat->size;
      cv::Mat fromMat = cv::Mat(mat->dims, mat->size.data(), mat->type(), source);
      fromMat.copyTo(*mat);
   }
   CVAPI_CATCH_CV_ERRORS_VOID
}

double cveUMatDot(cv::UMat* mat, cv::_InputArray* m)
{
   try
   {
      return mat->dot(*m);
   }
   CVAPI_CATCH_CV_ERRORS(0.0)
}

void cveSwapUMat(cv::UMat* mat1, cv::UMat* mat2)
{
   try
   {
      cv::swap(*mat1, *mat2);
   }
   CVAPI_CATCH_CV_ERRORS_VOID
}