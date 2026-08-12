//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
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
	try
	{
		return new cv::Mat();
	}
	CVAPI_CATCH_CV_ERRORS(0)
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
	try
	{
		mat->create(row, cols, type);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::Mat* cveMatCreateWithData(int rows, int cols, int type, void* data, size_t step)
{
	try
	{
		return new cv::Mat(rows, cols, type, data, step);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::Mat* cveMatCreateMultiDimWithData(int ndims, const int* sizes, int type, void* data, size_t* steps)
{
	try
	{
		return new cv::Mat(ndims, sizes, type, data, steps);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::Mat* cveMatCreateFromRect(cv::Mat* mat, cv::Rect* roi)
{
	try
	{
		return new cv::Mat(*mat, *roi);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::Mat* cveMatCreateFromRange(cv::Mat* mat, cv::Range* rowRange, cv::Range* colRange)
{
	try
	{
		return new cv::Mat(*mat, *rowRange, *colRange);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveMatRelease(cv::Mat** mat)
{
	try
	{
		delete* mat;
		*mat = 0;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMatGetSize(cv::Mat* mat, cv::Size* size)
{
	try
	{
		size->width = mat->cols;
		size->height = mat->rows;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMatCopyTo(cv::Mat* mat, cv::_OutputArray* m, cv::_InputArray* mask)
{
	try
	{
		if (mask)
			mat->copyTo(*m, *mask);
		else
			mat->copyTo(*m);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

int cveMatGetElementSize(cv::Mat* mat)
{
	try
	{
		return static_cast<int>(mat->elemSize());
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

uchar* cveMatGetDataPointer(cv::Mat* mat)
{
	try
	{
		return mat->data;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
uchar* cveMatGetDataPointer2(cv::Mat* mat, int* indices)
{
	try
	{
		return mat->ptr(indices);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
size_t cveMatGetStep(cv::Mat* mat)
{
	try
	{
		return mat->step;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveMatSetTo(cv::Mat* mat, cv::_InputArray* value, cv::_InputArray* mask)
{
	try
	{
		mat->setTo(*value, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveMatSetToScalar(cv::Mat* mat, cv::Scalar* value, cv::_InputArray* mask)
{
	try
	{
		mat->setTo(*value, mask ? *mask : static_cast<cv::InputArray>(cv::noArray()));
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


cv::UMat* cveMatGetUMat(cv::Mat* mat, int access, cv::UMatUsageFlags usageFlags)
{
	try
	{
		cv::UMat* result = new cv::UMat();
		cv::UMat tmp = mat->getUMat(static_cast<cv::AccessFlag>(access), usageFlags);
		cv::swap(*result, tmp);
		return result;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveMatConvertTo(cv::Mat* mat, cv::_OutputArray* out, int rtype, double alpha, double beta)
{
	try
	{
		mat->convertTo(*out, rtype, alpha, beta);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::Mat* cveMatReshape(cv::Mat* mat, int cn, int rows)
{
	try
	{
		cv::Mat* result = new cv::Mat();
		cv::Mat m = mat->reshape(cn, rows);
		cv::swap(m, *result);
		return result;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::Mat* cveMatReshape2(cv::Mat* mat, int cn, int newndims, int* newsz)
{
	try
	{
		cv::Mat* result = new cv::Mat();
		cv::Mat m = mat->reshape(cn, newndims, newsz);
		cv::swap(m, *result);
		return result;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

double cveMatDot(cv::Mat* mat, cv::_InputArray* m)
{
	try
	{
		return mat->dot(*m);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveMatCross(cv::Mat* mat, cv::_InputArray* m, cv::Mat* result)
{
	try
	{
		cv::Mat r = mat->cross(*m);
		cv::swap(r, *result);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


void cveMatCopyDataTo(cv::Mat* mat, unsigned char* dest)
{
	try
	{
		cv::Mat destMat = cv::Mat(mat->dims, mat->size.data(), mat->type(), dest, 0);
		mat->copyTo(destMat);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveMatCopyDataFrom(cv::Mat* mat, unsigned char* source)
{
	try
	{
		cv::Mat fromMat = cv::Mat(mat->dims, mat->size.data(), mat->type(), source, 0);
		fromMat.copyTo(*mat);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveMatGetSizeOfDimension(cv::Mat* mat, int* sizes)
{
	try
	{
		memcpy(sizes, mat->size.data(), sizeof(int) * mat->dims);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveSwapMat(cv::Mat* mat1, cv::Mat* mat2)
{
	try
	{
		cv::swap(*mat1, *mat2);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveMatEye(int rows, int cols, int type, cv::Mat* m)
{
	try
	{
		cv::Mat e = cv::Mat::eye(rows, cols, type);
		cv::swap(e, *m);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMatDiag(cv::Mat* src, int d, cv::Mat* dst)
{
	try
	{
		cv::Mat diag = src->diag(d);
		cv::swap(diag, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMatT(cv::Mat* src, cv::Mat* dst)
{
	try
	{
		cv::Mat t = src->t();
		cv::swap(t, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMatZeros(int rows, int cols, int type, cv::Mat* dst)
{
	try
	{
		cv::Mat z = cv::Mat::zeros(rows, cols, type);
		cv::swap(z, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveMatOnes(int rows, int cols, int type, cv::Mat* dst)
{
	try
	{
		cv::Mat z = cv::Mat::ones(rows, cols, type);
		cv::swap(z, *dst);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
