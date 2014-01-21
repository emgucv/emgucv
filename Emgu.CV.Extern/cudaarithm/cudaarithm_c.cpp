//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudaarithm_c.h"

void cudaExp(cv::_InputArray* a, cv::_OutputArray* b, cv::cuda::Stream* stream)
{
   cv::cuda::exp(*a, *b, stream? *stream : cv::cuda::Stream::Null());
}

void cudaPow(cv::_InputArray* src, double power, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   cv::cuda::pow(*src, power, *dst, stream? *stream : cv::cuda::Stream::Null()); 
}

void cudaLog(cv::_InputArray* a, cv::_OutputArray* b, cv::cuda::Stream* stream)
{
   cv::cuda::log(*a, *b, stream? *stream : cv::cuda::Stream::Null());
}

void cudaMagnitude(cv::_InputArray* x, cv::_InputArray* y, cv::_OutputArray* magnitude, cv::cuda::Stream* stream)
{
   cv::cuda::magnitude(*x, *y, *magnitude, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMagnitudeSqr(cv::_InputArray* x, cv::_InputArray* y, cv::_OutputArray* magnitude, cv::cuda::Stream* stream)
{
   cv::cuda::magnitudeSqr(*x, *y, *magnitude, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaPhase(cv::_InputArray* x, cv::_InputArray* y, cv::_OutputArray* angle, bool angleInDegrees, cv::cuda::Stream* stream)
{
   cv::cuda::phase(*x, *y, *angle, angleInDegrees, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaCartToPolar(cv::_InputArray* x, cv::_InputArray* y, cv::_OutputArray* magnitude, cv::_OutputArray* angle, bool angleInDegrees, cv::cuda::Stream* stream)
{
   cv::cuda::cartToPolar(*x, *y, *magnitude, *angle, angleInDegrees, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaPolarToCart(cv::_InputArray* magnitude, cv::_InputArray* angle, cv::_OutputArray* x, cv::_OutputArray* y, bool angleInDegrees, cv::cuda::Stream* stream)
{
   cv::cuda::polarToCart(*magnitude, *angle, *x, *y, angleInDegrees, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMerge(const cv::cuda::GpuMat** src, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream)
{
   int channels = dst->channels();
   cv::cuda::GpuMat* srcArr = new cv::cuda::GpuMat[channels];
   for (int i = 0; i < channels; ++i)
      srcArr[i] = *(src[i]);
   cv::cuda::merge(srcArr, dst->channels(), *dst, stream ? *stream : cv::cuda::Stream::Null());
   delete[] srcArr;
}

//only support single channel gpuMat
void cudaMinMaxLoc(cv::_InputArray* src, double* minVal, double* maxVal, CvPoint* minLoc, CvPoint* maxLoc, cv::_InputArray* mask)
{
   cv::Point minimunLoc, maximunLoc;
   cv::_InputArray maskMat = mask ? *mask : (cv::_InputArray) cv::noArray();
   cv::cuda::minMaxLoc(*src, minVal, maxVal, &minimunLoc, &maximunLoc, maskMat);
   maxLoc->x = maximunLoc.x; maxLoc->y = maximunLoc.y;
   minLoc->x = minimunLoc.x; minLoc->y = minimunLoc.y;
}

void cudaMeanStdDev(cv::_InputArray* mtx, CvScalar* mean, CvScalar* stddev, cv::cuda::GpuMat* buffer)
{
   cv::Scalar meanVal, stdDevVal;
   if (buffer)
      cv::cuda::meanStdDev(*mtx, meanVal, stdDevVal, *buffer);
   else
      cv::cuda::meanStdDev(*mtx, meanVal, stdDevVal);

   memcpy(mean->val, meanVal.val, sizeof(double)*4);
   memcpy(stddev->val, stdDevVal.val, sizeof(double)*4);
}

double cudaNorm(cv::_InputArray* src1, cv::_InputArray* src2, int normType)
{
   if (src2)
      return cv::cuda::norm(*src1, *src2, normType);
   else
      return cv::cuda::norm(*src1, normType);
}

int cudaCountNonZero(cv::_InputArray* src)
{
   cv::cuda::GpuMat buf;
   return cv::cuda::countNonZero(*src, buf);
}

void cudaReduce(cv::_InputArray* mtx, cv::_OutputArray* vec, int dim, int reduceOp, cv::cuda::Stream* stream)
{
   cv::cuda::reduce(*mtx, *vec, dim, reduceOp, vec->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseNot(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::cuda::bitwise_not(*src, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseAnd(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::cuda::bitwise_and(*src1, *src2, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseAndS(cv::_InputArray* src1, const CvScalar* sc, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::Scalar s = *sc;
   cv::cuda::bitwise_and(*src1, s, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseOr(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::cuda::bitwise_or(*src1, *src2, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseOrS(cv::_InputArray* src1, const CvScalar* sc, cv::_OutputArray* dst,  cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::Scalar s = *sc;
   cv::cuda::bitwise_or(*src1, s, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseXor(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::cuda::bitwise_xor(*src1, *src2, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseXorS(cv::_InputArray* src1, const CvScalar* sc, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::Scalar s = *sc;
   cv::cuda::bitwise_xor(*src1, s, *dst,  mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMin(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   cv::cuda::min(*src1, *src2, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMinS(cv::_InputArray* src1, double src2, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   cv::cuda::min(*src1, src2, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMax(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   cv::cuda::max(*src1, *src2, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMaxS(cv::_InputArray* src1, double src2, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   cv::cuda::max(*src1, src2, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaGemm(const cv::cuda::GpuMat* src1, const cv::cuda::GpuMat* src2, double alpha, 
                const cv::cuda::GpuMat* src3, double beta, cv::cuda::GpuMat* dst, int flags, cv::cuda::Stream* stream)
{
   cv::cuda::GpuMat src3Mat = src3 ? *src3 : cv::cuda::GpuMat();
   cv::cuda::gemm(*src1, *src2, alpha, src3Mat, beta, *dst, flags, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaLShift(const cv::cuda::GpuMat* a, CvScalar* scale, cv::cuda::GpuMat* c, cv::cuda::Stream* stream)
{
	cv::cuda::lshift(*a, *scale, *c, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaRShift(const cv::cuda::GpuMat* a, CvScalar* scale, cv::cuda::GpuMat* c, cv::cuda::Stream* stream)
{
	cv::cuda::rshift(*a, *scale, *c, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaAdd(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::cuda::add(*a, *b, *c, mask ? *mask : (cv::_InputArray) cv::noArray(), c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaAddS(cv::_InputArray* a, const CvScalar* scale, cv::_OutputArray* c, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::Scalar s = *scale;
   cv::cuda::add(*a, s, *c, mask ? *mask : (cv::_InputArray) cv::noArray(), c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaSubtract(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::cuda::subtract(*a, *b, *c, mask ? *mask : (cv::_InputArray) cv::noArray(), c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaSubtractS(cv::_InputArray* a, const CvScalar* scale, cv::_OutputArray* c, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::Scalar s = *scale;
   cv::cuda::subtract(*a, s, *c, mask ? *mask : (cv::_InputArray) cv::noArray(), c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMultiply(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, double scale, cv::cuda::Stream* stream)
{
   cv::cuda::multiply(*a, *b, *c, scale, c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMultiplyS(cv::_InputArray* a, const CvScalar* s, cv::_OutputArray* c, cv::cuda::Stream* stream)
{
   cv::Scalar scalar = *s;
   cv::cuda::multiply(*a, scalar, *c, 1, c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaDivide(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, double scale, cv::cuda::Stream* stream)
{
   cv::cuda::divide(*a, *b, *c, scale, c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaDivideSR(cv::_InputArray* a, const CvScalar* s, cv::_OutputArray* c, cv::cuda::Stream* stream)
{
   cv::Scalar scalar = *s;
   cv::cuda::divide(*a, scalar, *c, 1, c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaDivideSL(const double s, cv::_InputArray* b, cv::_OutputArray* c, cv::cuda::Stream* stream)
{
   cv::cuda::divide(s, *b, *c, c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaAddWeighted(cv::_InputArray* src1, double alpha, cv::_InputArray* src2, double beta, double gamma, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   cv::cuda::addWeighted(*src1, alpha, *src2, beta, gamma, *dst, dst->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaAbsdiff(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, cv::cuda::Stream* stream)
{
   cv::cuda::absdiff(*a, *b, *c, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaAbs(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   cv::cuda::abs(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaSqr(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   cv::cuda::sqr(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaSqrt(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   cv::cuda::sqrt(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaAbsdiffS(cv::_InputArray* a, const CvScalar* s, cv::_OutputArray* c, cv::cuda::Stream* stream)
{
   cv::Scalar scalar = *s;
   cv::cuda::absdiff(*a, scalar, *c, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaCompare(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, int cmpop, cv::cuda::Stream* stream)
{
   cv::cuda::compare(*a, *b, *c, cmpop, stream ? *stream : cv::cuda::Stream::Null());
}

double cudaThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double thresh, double maxval, int type, cv::cuda::Stream* stream)
{
   return cv::cuda::threshold(*src, *dst, thresh, maxval, type, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaCopyMakeBorder(cv::_InputArray* src, cv::_OutputArray* dst, int top, int bottom, int left, int right, int gpuBorderType, const CvScalar* value, cv::cuda::Stream* stream)
{
   cv::cuda::copyMakeBorder(*src, *dst, top, bottom, left, right, gpuBorderType, *value, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaIntegral(cv::_InputArray* src, cv::_OutputArray* sum, cv::cuda::GpuMat* buffer, cv::cuda::Stream* stream)
{
   CV_Assert(!stream || buffer);
   cv::cuda::integralBuffered(*src, *sum, *buffer, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaSqrIntegral(cv::_InputArray* src, cv::_OutputArray* sqrSum, cv::cuda::GpuMat* buffer,  cv::cuda::Stream* stream)
{
   CV_Assert(!stream || buffer);
   cv::cuda::sqrIntegral(*src, *sqrSum, *buffer, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaDft(cv::_InputArray* src, cv::_OutputArray* dst, int flags, cv::cuda::Stream* stream)
{
   cv::cuda::dft(*src, *dst, dst->size(), flags | (dst->channels() == 1 ? cv::DFT_REAL_OUTPUT : 0), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaFlip(cv::_InputArray* src, cv::_OutputArray* dst, int flipcode, cv::cuda::Stream* stream)
{
   cv::cuda::flip(*src, *dst, flipcode, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaSplit(const cv::cuda::GpuMat* src, cv::cuda::GpuMat** dst, cv::cuda::Stream* stream)
{
   int channels = src->channels();
   cv::cuda::GpuMat* dstArr = new cv::cuda::GpuMat[channels];
   for (int i = 0; i < channels; i++)
      dstArr[i] = *(dst[i]);
   cv::cuda::split(*src, dstArr, stream? *stream : cv::cuda::Stream::Null());
   delete[] dstArr;
}

cv::cuda::LookUpTable* cudaLookUpTableCreate( const CvArr* lut )
{
   cv::Mat lutMat = cv::cvarrToMat(lut);
   cv::Ptr<cv::cuda::LookUpTable> ptr = cv::cuda::createLookUpTable(lutMat);
   ptr.addref();
   return ptr.get();
}
void cudaLookUpTableTransform(cv::cuda::LookUpTable* lut, cv::_InputArray* image, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   lut->transform(*image, *dst, stream ? *stream : cv::cuda::Stream::Null());
}
void cudaLookUpTableRelease(cv::cuda::LookUpTable** lut)
{
   delete *lut;
   *lut=0;
}