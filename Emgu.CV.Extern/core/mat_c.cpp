//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "mat_c.h"

class EmguMatAllocator : public cv::MatAllocator
{
public:
   MatAllocateCallback dataAllocator;
   MatDeallocateCallback dataDeallocator;
   EmguMatAllocator(MatAllocateCallback allocator, MatDeallocateCallback deallocator)
      :MatAllocator()
   {
      dataAllocator = allocator;
      dataDeallocator = deallocator;
   }

   cv::UMatData* allocate(int dims, const int* sizes, int type,
      void* data0, size_t* step, int /*flags*/) const
   {
      size_t total = CV_ELEM_SIZE(type);
      for (int i = dims - 1; i >= 0; i--)
      {
         if (step)
         {
            if (data0 && step[i] != CV_AUTOSTEP)
            {
               CV_Assert(total <= step[i]);
               total = step[i];
            }
            else
               step[i] = total;
         }
         total *= sizes[i];
      }

      uchar* data = data0 ? (uchar*)data0 : dataAllocator(CV_MAT_DEPTH(type), CV_MAT_CN(type), total);
      cv::UMatData* u = new cv::UMatData(this);
      u->data = u->origdata = data;
      u->size = total;
      u->refcount = data0 == 0;
      if (data0)
         u->flags |= cv::UMatData::USER_ALLOCATED;

      return u;
   }

   bool allocate(cv::UMatData* u, int /*accessFlags*/) const
   {
      if (!u) return false;
      CV_XADD(&u->urefcount, 1);
      return true;
   }

   void deallocate(cv::UMatData* u) const
   {
      if (u && u->refcount == 0)
      {
         if (!(u->flags & cv::UMatData::USER_ALLOCATED))
         {
            dataDeallocator(u->origdata);
            //cv::fastFree(u->origdata);
            u->origdata = 0;
         }
         delete u;
      }
   }
};


cv::MatAllocator* emguMatAllocatorCreate(MatAllocateCallback allocator, MatDeallocateCallback deallocator)
{
   return new EmguMatAllocator(allocator, deallocator);
}
void cvMatAllocatorRelease(cv::MatAllocator** allocator)
{
   delete *allocator;
   *allocator = 0;
}

cv::Mat* cvMatCreate()
{
   return new cv::Mat();
}
cv::MatAllocator* cvMatUseCustomAllocator(cv::Mat* mat, MatAllocateCallback allocator, MatDeallocateCallback deallocator)
{
   cv::MatAllocator* a = new EmguMatAllocator(allocator, deallocator);
   mat->allocator = a;
   return a;
}

void cvMatCreateData(cv::Mat* mat, int row, int cols, int type)
{
   mat->create(row, cols, type);
}

cv::Mat* cvMatCreateWithData(int rows, int cols, int type, void* data, size_t step)
{
   return new cv::Mat(rows, cols, type, data, step);
}

void cvMatRelease(cv::Mat** mat)
{
   delete *mat;
   *mat = 0;
}
emgu::size cvMatGetSize(cv::Mat* mat)
{
   emgu::size s;
   s.width = mat->cols;
   s.height = mat->rows;
   return s;
}
void cvMatCopyTo(cv::Mat* mat, cv::_OutputArray* m, cv::_InputArray* mask)
{
   if (mask)
      mat->copyTo(*m, *mask);
   else
      mat->copyTo(*m);
}
void cveArrToMat(CvArr* cvArray, cv::Mat* mat)
{
   cv::Mat tmp = cv::cvarrToMat(cvArray);
   cv::swap(*mat, tmp);
}
int cvMatGetElementSize(cv::Mat* mat)
{
   return static_cast<int>( mat->elemSize());
}

int cvMatGetChannels(cv::Mat* mat)
{
   return mat->channels();
}
int cvMatGetDepth(cv::Mat* mat)
{
   return mat->depth();
}
uchar* cvMatGetDataPointer(cv::Mat* mat)
{
   return mat->ptr(0);
}
size_t cvMatGetStep(cv::Mat* mat)
{
   return mat->step;
}

bool cvMatIsEmpty(cv::Mat* mat)
{
   return mat->empty();
}

void cvMatSetTo(cv::Mat* mat, cv::_InputArray* value, cv::_InputArray* mask)
{
   mat->setTo(*value, mask ? *mask : (cv::InputArray) cv::noArray());
}

cv::UMat* cvMatGetUMat(cv::Mat* mat, int access)
{
   cv::UMat* result = new cv::UMat();
   cv::UMat tmp = mat->getUMat(access);
   cv::swap(*result, tmp);
   return result;
}


