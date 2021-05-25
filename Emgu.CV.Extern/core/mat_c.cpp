//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
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
		//uchar* data = data0 ? (uchar*)data0 : (uchar*)fastMalloc(total);
		uchar* data = data0 ? static_cast<uchar*>(data0) : dataAllocator(CV_MAT_DEPTH(type), CV_MAT_CN(type), total, allocateDataAction);
		cv::UMatData* u = new cv::UMatData(this);
		u->data = u->origdata = data;
		u->size = total;
		if (data0)
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

/*
cv::MatAllocator* emguMatAllocatorCreate(MatAllocateCallback allocator, MatDeallocateCallback deallocator, void* allocateDataActionPtr, void* freeDataActionPtr)
{
   return new EmguMatAllocator(allocator, deallocator, allocateDataActionPtr, freeDataActionPtr);
}
void cveMatAllocatorRelease(cv::MatAllocator** allocator)
{
   if (*allocator != 0)
   {
	  delete *allocator;
	  *allocator = 0;
   }
}*/

cv::Mat* cveMatCreate()
{
	return new cv::Mat();
}
/*
cv::MatAllocator* cveMatUseCustomAllocator(cv::Mat* mat, MatAllocateCallback allocator, MatDeallocateCallback deallocator, void* allocateDataActionPtr, void* freeDataActionPtr)
{
   cv::MatAllocator* a = new EmguMatAllocator(allocator, deallocator, allocateDataActionPtr, freeDataActionPtr);
   mat->allocator = a;
   return a;
}
*/

void cveMatCreateData(cv::Mat* mat, int row, int cols, int type)
{
	mat->create(row, cols, type);
}

cv::Mat* cveMatCreateWithData(int rows, int cols, int type, void* data, size_t step)
{
	return new cv::Mat(rows, cols, type, data, step);
}

cv::Mat* cveMatCreateMultiDimWithData(int ndims, const int* sizes, int type, void* data, size_t* steps)
{
	return new cv::Mat(ndims, sizes, type, data, steps);
}

cv::Mat* cveMatCreateFromRect(cv::Mat* mat, CvRect* roi)
{
	return new cv::Mat(*mat, *roi);
}

cv::Mat* cveMatCreateFromRange(cv::Mat* mat, cv::Range* rowRange, cv::Range* colRange)
{
	return new cv::Mat(*mat, *rowRange, *colRange);
}

void cveMatRelease(cv::Mat** mat)
{
	delete* mat;
	*mat = 0;
}
void cveMatGetSize(cv::Mat* mat, CvSize* size)
{
	size->width = mat->cols;
	size->height = mat->rows;
}
void cveMatCopyTo(cv::Mat* mat, cv::_OutputArray* m, cv::_InputArray* mask)
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

IplImage* cveMatToIplImage(cv::Mat* m)
{
	IplImage* result = new IplImage();
	CV_Assert(m->dims <= 2);
	cvInitImageHeader(result, cvSize(m->size()), cvIplDepth(m->flags), m->channels());
	cvSetData(result, m->data, static_cast<int>(m->step[0]));
	return result;
}
int cveMatGetElementSize(cv::Mat* mat)
{
	return static_cast<int>(mat->elemSize());
}

//int cveMatGetChannels(cv::Mat* mat)
//{
//   return mat->channels();
//}

uchar* cveMatGetDataPointer(cv::Mat* mat)
{
	return mat->data;
}
uchar* cveMatGetDataPointer2(cv::Mat* mat, int* indices)
{
	return mat->ptr(indices);
}
size_t cveMatGetStep(cv::Mat* mat)
{
	return mat->step;
}

void cveMatSetTo(cv::Mat* mat, cv::_InputArray* value, cv::_InputArray* mask)
{
	mat->setTo(*value, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
}

cv::UMat* cveMatGetUMat(cv::Mat* mat, int access, cv::UMatUsageFlags usageFlags)
{
	cv::UMat* result = new cv::UMat();
	cv::UMat tmp = mat->getUMat(static_cast<cv::AccessFlag>(access), usageFlags);
	cv::swap(*result, tmp);
	return result;
}

void cveMatConvertTo(cv::Mat* mat, cv::_OutputArray* out, int rtype, double alpha, double beta)
{
	mat->convertTo(*out, rtype, alpha, beta);
}

cv::Mat* cveMatReshape(cv::Mat* mat, int cn, int rows)
{
	cv::Mat* result = new cv::Mat();
	cv::Mat m = mat->reshape(cn, rows);
	cv::swap(m, *result);
	return result;
}

double cveMatDot(cv::Mat* mat, cv::_InputArray* m)
{
	return mat->dot(*m);
}
void cveMatCross(cv::Mat* mat, cv::_InputArray* m, cv::Mat* result)
{
	cv::Mat r = mat->cross(*m);
	cv::swap(r, *result);
}


void cveMatCopyDataTo(cv::Mat* mat, unsigned char* dest)
{
	const int* sizes = mat->size;
	cv::Mat destMat = cv::Mat(mat->dims, mat->size, mat->type(), dest);
	mat->copyTo(destMat);
}

void cveMatCopyDataFrom(cv::Mat* mat, unsigned char* source)
{
	const int* sizes = mat->size;
	cv::Mat fromMat = cv::Mat(mat->dims, mat->size, mat->type(), source);
	fromMat.copyTo(*mat);
}

void cveMatGetSizeOfDimension(cv::Mat* mat, int* sizes)
{
	const int* s = mat->size;
	memcpy(sizes, s, sizeof(int) * mat->dims);
}

void cveSwapMat(cv::Mat* mat1, cv::Mat* mat2)
{
	cv::swap(*mat1, *mat2);
}

void cveMatEye(int rows, int cols, int type, cv::Mat* m)
{
	cv::Mat e = cv::Mat::eye(rows, cols, type);
	cv::swap(e, *m);
}
void cveMatDiag(cv::Mat* src, int d, cv::Mat* dst)
{
	cv::Mat diag = src->diag(d);
	cv::swap(diag, *dst);
}
void cveMatT(cv::Mat* src, cv::Mat* dst)
{
	cv::Mat t = src->t();
	cv::swap(t, *dst);
}
void cveMatZeros(int rows, int cols, int type, cv::Mat* dst)
{
	cv::Mat z = cv::Mat::zeros(rows, cols, type);
	cv::swap(z, *dst);
}
void cveMatOnes(int rows, int cols, int type, cv::Mat* dst)
{
	cv::Mat z = cv::Mat::ones(rows, cols, type);
	cv::swap(z, *dst);
}
