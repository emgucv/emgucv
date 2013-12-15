//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "core_c_extra.h"

cv::UMat* cvUMatCreate()
{
   return new cv::UMat();
}
cv::UMat* cvUMatCreateWithType(int rows, int cols, int type)
{
   return new cv::UMat(rows, cols, type);
}

void cvUMatRelease(cv::UMat** mat)
{
   delete *mat;
   *mat = 0;
}
emgu::size cvUMatGetSize(cv::UMat* mat)
{
   emgu::size s;
   s.width = mat->cols;
   s.height = mat->rows;
   return s;
}
void cvUMatCopyTo(cv::UMat* mat, cv::_OutputArray* m)
{
   mat->copyTo(*m);
}

int cvUMatGetElementSize(cv::UMat* mat)
{
   return static_cast<int>( mat->elemSize());
}

int cvUMatGetChannels(cv::UMat* mat)
{
   return mat->channels();
}

bool cvUMatIsEmpty(cv::UMat* mat)
{
   return mat->empty();
}

void cvUMatSetTo(cv::UMat* mat, cv::_InputArray* value, cv::_InputArray* mask)
{
   mat->setTo(*value, mask ? *mask : cv::noArray());
}

cv::Mat* cvMatCreate()
{
   return new cv::Mat();
}
cv::Mat* cvMatCreateWithType(int rows, int cols, int type)
{
   return new cv::Mat(rows, cols, type);
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
   mat->setTo(*value, mask ? *mask : cv::noArray());
}

cv::_InputArray* cvInputArrayFromDouble(double* scalar)
{
   return new cv::_InputArray(*scalar);
}

cv::_InputArray* cvInputArrayFromScalar(cv::Scalar* scalar)
{
   return new cv::_InputArray(*scalar);
}
cv::_InputArray* cvInputArrayFromMat(cv::Mat* mat)
{
   return new cv::_InputArray(*mat);
}

cv::_InputArray* cvInputArrayFromGpuMat(cv::cuda::GpuMat* mat)
{
   return new cv::_InputArray(*mat);
}

void cvInputArrayRelease(cv::_InputArray** arr)
{
   delete *arr;
   *arr = 0;
}

cv::_OutputArray* cvOutputArrayFromMat(cv::Mat* mat)
{
   return new cv::_OutputArray(*mat);
}

cv::_OutputArray* cvOutputArrayFromGpuMat(cv::cuda::GpuMat* mat)
{
   return new cv::_OutputArray(*mat);
}

void cvOutputArrayRelease(cv::_OutputArray** arr)
{
   delete *arr;
   *arr = 0;
}

cv::_InputOutputArray* cvInputOutputArrayFromMat(cv::Mat* mat)
{
   return new cv::_InputOutputArray(*mat);
}
cv::_InputOutputArray* cvInputOutputArrayFromGpuMat(cv::Mat* mat)
{
   return new cv::_InputOutputArray(*mat);
}
void cvInputOutputArrayRelease(cv::_InputOutputArray** arr)
{
   delete *arr;
   *arr = 0;
}

cv::Scalar* cvScalarCreate(CvScalar* scalar)
{
   return new cv::Scalar(scalar->val[0], scalar->val[1], scalar->val[2], scalar->val[3]);
}
void cvScalarRelease(cv::Scalar** scalar)
{
   delete *scalar;
   *scalar = 0;
}

void cveMinMaxIdx(cv::_InputArray* src, double* minVal, double* maxVal, int* minIdx, int* maxIdx, cv::_InputArray* mask)
{
   cv::minMaxIdx(*src, minVal, maxVal, minIdx, maxIdx, mask ? *mask : cv::noArray());
}

void cveMinMaxLoc(cv::_InputArray* src, double* minVal, double* maxVal, CvPoint* minLoc, CvPoint* maxLoc, cv::_InputArray* mask)
{
   cv::Point minPt;
   cv::Point maxPt;
   cv::minMaxLoc(*src, minVal, maxVal, &minPt, &maxPt, mask ? *mask : cv::noArray());
   minLoc->x = minPt.x; minLoc->y = minPt.y;
   maxLoc->x = maxPt.x; maxLoc->y = maxPt.y;
}

void cveBitwiseAnd(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask)
{
   cv::bitwise_and(*src1, *src2, *dst, mask ? *mask : cv::noArray());
}
void cveBitwiseNot(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* mask)
{
   cv::bitwise_not(*src, *dst, mask ? *mask : cv::noArray());
}
void cveBitwiseOr(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask)
{
   cv::bitwise_or(*src1, *src2, *dst, mask ? *mask : cv::noArray());
}
void cveBitwiseXor(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask)
{
   cv::bitwise_xor(*src1, *src2, *dst, mask ? *mask : cv::noArray());
}

void cveAdd(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, int dtype)
{
   cv::add(*src1, *src2, *dst, mask ? *mask : cv::noArray(), dtype);
}
void cveSubtract(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, int dtype)
{
   cv::subtract(*src1, *src2, *dst, mask ? *mask : cv::noArray(), dtype);
}
void cveDivide(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, double scale, int dtype)
{
   cv::divide(*src1, *src2, *dst, scale, dtype);
}
void cveMultiply(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, double scale, int dtype)
{
   cv::multiply(*src1,*src2, *dst, scale, dtype);
}
void cveCountNonZero(cv::_InputArray* src)
{
   cv::countNonZero(*src);
}
void cveMin(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst)
{
   cv::min(*src1, *src2, *dst);
}
void cveMax(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst)
{
   cv::max(*src1, *src2, *dst);
}
void cveAbsDiff(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst)
{
   cv::absdiff(*src1, *src2, *dst);
}
void cveInRange(cv::_InputArray* src1, cv::_InputArray* lowerb, cv::_InputArray* upperb, cv::_OutputArray* dst)
{
   cv::inRange(*src1, *lowerb, *upperb, *dst);
}
void cveSqrt(cv::_InputArray* src, cv::_OutputArray* dst)
{
   cv::sqrt(*src, *dst);
}

void cveCompare(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int compop)
{
   cv::compare(*src1, *src2, *dst, compop);
}

void cveFlip(cv::_InputArray* src, cv::_OutputArray* dst, int flipCode)
{
   cv::flip(*src, *dst, flipCode);
}

void cveTranspose(cv::_InputArray* src, cv::_OutputArray* dst)
{
   cv::transpose(*src, *dst);
}

void cveLUT(cv::_InputArray* src, cv::_InputArray* lut, cv::_OutputArray* dst)
{
   cv::LUT(*src, *lut, *dst);
}

void cveSum(cv::_InputArray* src, CvScalar* result)
{
   cv::Scalar sum = cv::sum(*src);
   memcpy(&result->val[0], &sum.val[0], sizeof(double) * 4);
}
void cveMean(cv::_InputArray* src, cv::_InputArray* mask, CvScalar* result)
{
   cv::Scalar mean = cv::mean(*src, mask ? *mask : cv::noArray());
   memcpy(&result->val[0], &mean.val[0], sizeof(double) * 4);
}
void cveTrace(cv::_InputArray* mtx, CvScalar* result)
{
   cv::Scalar trace = cv::trace(*mtx);
   memcpy(&result->val[0], &trace.val[0], sizeof(double) * 4);
}
double cveDeterminant(cv::_InputArray* mtx)
{
   return cv::determinant(*mtx);
}
bool cveCheckRange(cv::_InputArray* arr, bool quiet, CvPoint* index, double minVal, double maxVal)
{
   cv::Point p;
   bool result = cv::checkRange(*arr, quiet, &p , minVal, maxVal);
   index->x = p.x;
   index->y = p.y;
   return result;
}

void cveGemm(cv::_InputArray* src1, cv::_InputArray* src2, double alpha, cv::_InputArray* src3, double beta, cv::_OutputArray* dst, int flags)
{
   cv::gemm(*src1, *src2, alpha, src3 ? *src3: cv::noArray(), beta, *dst, flags);
}

void cveAddWeighted(cv::_InputArray* src1, double alpha, cv::_InputArray* src2, double beta, double gamma, cv::_OutputArray* dst, int dtype)
{
   cv::addWeighted(*src1, alpha, *src2, beta, gamma, *dst, dtype);
}
void cveConvertScaleAbs(cv::_InputArray* src, cv::_OutputArray* dst, double alpha, double beta)
{
   cv::convertScaleAbs(*src, *dst, alpha, beta);
}
void cveReduce(cv::_InputArray* src, cv::_OutputArray* dst, int dim, int rtype, int dtype)
{
   cv::reduce(*src, *dst, dim, rtype, dtype);
}
void cveRandShuffle(cv::_InputOutputArray* dst, double iterFactor, uint64 rng)
{
   if (rng == 0)
   {
      cv::randShuffle(*dst, iterFactor);
   } else
   {
      cv::RNG r(rng);
      cv::randShuffle(*dst, iterFactor, &r);
   }
}
void cvePow(cv::_InputArray* src, double power, cv::_OutputArray* dst)
{
   cv::pow(*src, power, *dst);
}
void cveExp(cv::_InputArray* src, cv::_OutputArray* dst)
{
   cv::exp(*src, *dst);
}
void cveLog(cv::_InputArray* src, cv::_OutputArray* dst)
{
   cv::log(*src, *dst);
}
void cveCartToPolar(cv::_InputArray* x, cv::_InputArray* y, cv::_OutputArray* magnitude, cv::_OutputArray* angle, bool angleInDegrees)
{
   cv::cartToPolar(*x, *y, *magnitude, angle ?  *angle : cv::noArray(), angleInDegrees);
}
void cvePolarToCart(cv::_InputArray* magnitude, cv::_InputArray* angle, cv::_OutputArray* x, cv::_OutputArray* y, bool angleInDegrees)
{
   cv::polarToCart(*magnitude, *angle, *x, *y, angleInDegrees);
}
void cveSetIdentity(cv::_InputOutputArray* mtx, CvScalar* scalar)
{
   cv::setIdentity(*mtx, *scalar);
}
void cveSolve(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int flags)
{
   cv::solve(*src1, *src2, *dst, flags);
}
void cveInvert(cv::_InputArray* src, cv::_OutputArray* dst, int flags)
{
   cv::invert(*src, *dst, flags);
}

void cveDft(cv::_InputArray* src, cv::_OutputArray* dst, int flags, int nonzeroRows)
{
   cv::dft(*src, *dst, flags, nonzeroRows);
}
void cveDct(cv::_InputArray* src, cv::_OutputArray* dst, int flags)
{
   cv::dct(*src, *dst, flags);
}
void cveMulSpectrums(cv::_InputArray *a, cv::_InputArray* b, cv::_OutputArray* c, int flags, bool conjB)
{
   cv::mulSpectrums(*a, *b, *c, flags, conjB);
}
void cveTransform(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* m)
{
   cv::transform(*src, *dst, *m);
}

void cveMahalanobis(cv::_InputArray* v1, cv::_InputArray* v2, cv::_InputArray* icovar)
{
   cv::Mahalanobis(*v1, *v2, *icovar);
}

void cveNormalize(cv::_InputArray* src, cv::_OutputArray* dst, double alpha, double beta, int normType, int dType, cv::_InputArray* mask)
{
   cv::normalize(*src, *dst, alpha, beta, normType, dType, mask ? *mask : cv::noArray());
}

void cvePerspectiveTransform(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* m)
{
   cv::perspectiveTransform(*src, *dst, *m);
}

void cveMulTransposed(cv::_InputArray* src, cv::_OutputArray* dst, bool aTa, cv::_InputArray* delta, double scale, int dtype)
{
   cv::mulTransposed(*src, *dst, aTa, delta ? *delta : cv::noArray(), dtype);
}

void cveSplit(cv::_InputArray* src, cv::_OutputArray* mv)
{
   cv::split(*src, *mv);
}
void cveMerge(cv::_InputArray* mv, cv::_OutputArray* dst)
{
   cv::merge(*mv, *dst);
}
void cveMixChannels(cv::_InputArray* src, cv::_InputOutputArray* dst, const int* fromTo, int npairs)
{
   cv::mixChannels(*src, *dst, fromTo, npairs);
}