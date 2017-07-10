//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "umat_c.h"

cv::UMat* cveUMatCreate(cv::UMatUsageFlags usage)
{
   return new cv::UMat(usage);
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
void cveUMatCreateData(cv::UMat* mat, int row, int cols, int type, cv::UMatUsageFlags flags)
{
   mat->create(row, cols, type, flags);
}
cv::UMat* cveUMatCreateFromRect(cv::UMat* mat, CvRect* roi)
{
   return new cv::UMat(*mat, *roi);
}
cv::UMat* cveUMatCreateFromRange(cv::UMat* umat, cv::Range* rowRange, cv::Range* colRange)
{
   return new cv::UMat(*umat, *rowRange, *colRange);
}
void cveUMatRelease(cv::UMat** mat)
{
   delete *mat;
   *mat = 0;
}
void cveUMatGetSize(cv::UMat* mat, CvSize* s)
{
   s->width = mat->cols;
   s->height = mat->rows;
}
void cveUMatCopyTo(cv::UMat* mat, cv::_OutputArray* m, cv::_InputArray* mask)
{
   if (mask)
   {
      mat->copyTo(*m, *mask);
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

void cveUMatConvertTo(cv::UMat* mat, cv::_OutputArray* out, int rtype, double alpha, double beta )
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

void cveUMatCopyDataTo(cv::UMat* mat, unsigned char* dest)
{
   //const int* sizes = mat->size;
   cv::Mat destMat = cv::Mat(mat->dims, mat->size, mat->type(), dest);
   mat->copyTo(destMat);
}

void cveUMatCopyDataFrom(cv::UMat* mat, unsigned char* source)
{
   //const int* sizes = mat->size;
   cv::Mat fromMat = cv::Mat(mat->dims, mat->size, mat->type(), source);
   fromMat.copyTo(*mat);
}

double cveUMatDot(cv::UMat* mat, cv::_InputArray* m)
{
   return mat->dot(*m);
}

void cveSwapUMat(cv::UMat* mat1, cv::UMat* mat2)
{
   cv::swap(*mat1, *mat2);
}