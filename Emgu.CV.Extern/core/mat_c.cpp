//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "mat_c.h"

class EmguMatAllocator : public cv::MatAllocator
{
public:
   MatAllocateCallback dataAllocator;
   MatDeallocateCallback dataDeallocator;
   void* allocateDataAction;
   void* freeDataAction;
   EmguMatAllocator(MatAllocateCallback allocator, MatDeallocateCallback deallocator, void* allocateDataActionPtr, void* freeDataActionPtr)
      :MatAllocator()
   {
      dataAllocator = allocator;
      dataDeallocator = deallocator;
      allocateDataAction = allocateDataActionPtr;
      freeDataAction = freeDataActionPtr;
   }

    cv::UMatData* allocate(int dims, const int* sizes, int type,
                       void* data0, size_t* step, int /*flags*/, cv::UMatUsageFlags /*usageFlags*/) const
    {
        size_t total = CV_ELEM_SIZE(type);
        for( int i = dims-1; i >= 0; i-- )
        {
            if( step )
            {
                if( data0 && step[i] != CV_AUTOSTEP )
                {
                    CV_Assert(total <= step[i]);
                    total = step[i];
                }
                else
                    step[i] = total;
            }
            total *= sizes[i];
        }
        //uchar* data = data0 ? (uchar*)data0 : (uchar*)fastMalloc(total);
        uchar* data = data0 ? (uchar*)data0 : dataAllocator(CV_MAT_DEPTH(type), CV_MAT_CN(type), total, allocateDataAction);
        cv::UMatData* u = new cv::UMatData(this);
        u->data = u->origdata = data;
        u->size = total;
        if(data0)
            u->flags |= cv::UMatData::USER_ALLOCATED;

        return u;
    }

   bool allocate(cv::UMatData* u, int /*accessFlags*/, cv::UMatUsageFlags /*usageFlags*/) const
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
            dataDeallocator(freeDataAction);
            //cv::fastFree(u->origdata);
            u->origdata = 0;
         }
         delete u;
      }
   }
};


cv::MatAllocator* emguMatAllocatorCreate(MatAllocateCallback allocator, MatDeallocateCallback deallocator, void* allocateDataActionPtr, void* freeDataActionPtr)
{
   return new EmguMatAllocator(allocator, deallocator, allocateDataActionPtr, freeDataActionPtr);
}
void cvMatAllocatorRelease(cv::MatAllocator** allocator)
{
   if (*allocator != 0)
   {
      delete *allocator;
      *allocator = 0;
   }
}

cv::Mat* cvMatCreate()
{
   return new cv::Mat();
}
cv::MatAllocator* cvMatUseCustomAllocator(cv::Mat* mat, MatAllocateCallback allocator, MatDeallocateCallback deallocator, void* allocateDataActionPtr, void* freeDataActionPtr)
{
   cv::MatAllocator* a = new EmguMatAllocator(allocator, deallocator, allocateDataActionPtr, freeDataActionPtr);
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

cv::Mat* cvMatCreateFromRect(cv::Mat* mat, CvRect* roi)
{
   return new cv::Mat(*mat, *roi);
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
cv::Mat* cveArrToMat(CvArr* cvArray, bool copyData, bool allowND, int coiMode)
{
   cv::Mat* mat = new cv::Mat();
   cv::Mat tmp = cv::cvarrToMat(cvArray, copyData, allowND, coiMode);
   cv::swap(*mat, tmp);
   return mat;
}

IplImage* cveMatToIplImage(cv::Mat* mat)
{
   IplImage* result = new IplImage(*mat);
   return result;
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
   return mat->data;
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

void cvMatConvertTo(  cv::Mat* mat, cv::_OutputArray* out, int rtype, double alpha, double beta )
{
   mat->convertTo(*out, rtype, alpha, beta);
}

cv::Mat* cvMatReshape(cv::Mat* mat, int cn, int rows)
{
   cv::Mat* result = new cv::Mat();
   cv::Mat m = mat->reshape(cn, rows);
   cv::swap(m, *result);
   return result;
}

double cvMatDot(cv::Mat* mat, cv::_InputArray* m)
{
   return mat->dot(*m);
}
void cvMatCross(cv::Mat* mat, cv::_InputArray* m, cv::Mat* result)
{
   cv::Mat r = mat->cross(*m);
   cv::swap(r, *result);
}