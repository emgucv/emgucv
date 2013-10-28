//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudaarithm_c.h"

void cudaExp(const cv::cuda::GpuMat* a, cv::cuda::GpuMat* b, cv::cuda::Stream* stream)
{
   cv::cuda::exp(*a, *b, stream? *stream : cv::cuda::Stream::Null());
}

void cudaPow(const cv::cuda::GpuMat* src, double power, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream)
{
   cv::cuda::pow(*src, power, *dst, stream? *stream : cv::cuda::Stream::Null()); 
}

void cudaLog(const cv::cuda::GpuMat* a, cv::cuda::GpuMat* b, cv::cuda::Stream* stream)
{
   cv::cuda::log(*a, *b, stream? *stream : cv::cuda::Stream::Null());
}

void cudaMagnitude(const cv::cuda::GpuMat* x, const cv::cuda::GpuMat* y, cv::cuda::GpuMat* magnitude, cv::cuda::Stream* stream)
{
   cv::cuda::magnitude(*x, *y, *magnitude, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMagnitudeSqr(const cv::cuda::GpuMat* x, const cv::cuda::GpuMat* y, cv::cuda::GpuMat* magnitude, cv::cuda::Stream* stream)
{
   cv::cuda::magnitudeSqr(*x, *y, *magnitude, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaPhase(const cv::cuda::GpuMat* x, const cv::cuda::GpuMat* y, cv::cuda::GpuMat* angle, bool angleInDegrees, cv::cuda::Stream* stream)
{
   cv::cuda::phase(*x, *y, *angle, angleInDegrees, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaCartToPolar(const cv::cuda::GpuMat* x, const cv::cuda::GpuMat* y, cv::cuda::GpuMat* magnitude, cv::cuda::GpuMat* angle, bool angleInDegrees, cv::cuda::Stream* stream)
{
   cv::cuda::cartToPolar(*x, *y, *magnitude, *angle, angleInDegrees, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaPolarToCart(const cv::cuda::GpuMat* magnitude, const cv::cuda::GpuMat* angle, cv::cuda::GpuMat* x, cv::cuda::GpuMat* y, bool angleInDegrees, cv::cuda::Stream* stream)
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
void cudaMinMaxLoc(const cv::cuda::GpuMat* src, 
                     double* minVal, double* maxVal, 
                     CvPoint* minLoc, CvPoint* maxLoc, 
                     const cv::cuda::GpuMat* mask)
{
   cv::Point minimunLoc, maximunLoc;
   cv::cuda::GpuMat maskMat = mask ? *mask : cv::cuda::GpuMat();
   cv::cuda::minMaxLoc(*src, minVal, maxVal, &minimunLoc, &maximunLoc, maskMat);
   maxLoc->x = maximunLoc.x; maxLoc->y = maximunLoc.y;
   minLoc->x = minimunLoc.x; minLoc->y = minimunLoc.y;
}




void cudaMeanStdDev(const cv::cuda::GpuMat* mtx, CvScalar* mean, CvScalar* stddev, cv::cuda::GpuMat* buffer)
{
   cv::Scalar meanVal, stdDevVal;
   if (buffer)
      cv::cuda::meanStdDev(*mtx, meanVal, stdDevVal, *buffer);
   else
      cv::cuda::meanStdDev(*mtx, meanVal, stdDevVal);

   memcpy(mean->val, meanVal.val, sizeof(double)*4);
   memcpy(stddev->val, stdDevVal.val, sizeof(double) * 4);
}

double cudaNorm(const cv::cuda::GpuMat* src1, const cv::cuda::GpuMat* src2, int normType)
{
   if (src2)
      return cv::cuda::norm(*src1, *src2, normType);
   else
      return cv::cuda::norm(*src1, normType);
}

int cudaCountNonZero(const cv::cuda::GpuMat* src)
{
   return cv::cuda::countNonZero(*src);
}

void cudaReduce(const cv::cuda::GpuMat* mtx, cv::cuda::GpuMat* vec, int dim, int reduceOp, cv::cuda::Stream* stream)
{
   cv::cuda::reduce(*mtx, *vec, dim, reduceOp, vec->depth(), stream ? *stream : cv::cuda::Stream::Null());
}



void cudaBitwiseNot(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream)
{
   cv::cuda::bitwise_not(*src, *dst, mask ? *mask : cv::cuda::GpuMat(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseAnd(const cv::cuda::GpuMat* src1, const cv::cuda::GpuMat* src2, cv::cuda::GpuMat* dst, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream)
{
   cv::cuda::bitwise_and(*src1, *src2, *dst, mask ? *mask : cv::cuda::GpuMat(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseAndS(const cv::cuda::GpuMat* src1, const cv::Scalar sc, cv::cuda::GpuMat* dst, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream)
{
   cv::cuda::bitwise_and(*src1, sc, *dst, mask ? *mask : cv::cuda::GpuMat(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseOr(const cv::cuda::GpuMat* src1, const cv::cuda::GpuMat* src2, cv::cuda::GpuMat* dst, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream)
{
   cv::cuda::bitwise_or(*src1, *src2, *dst, mask ? *mask : cv::cuda::GpuMat(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseOrS(const cv::cuda::GpuMat* src1, const cv::Scalar sc, cv::cuda::GpuMat* dst,  const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream)
{
   cv::cuda::bitwise_or(*src1, sc, *dst, mask ? *mask : cv::cuda::GpuMat(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseXor(const cv::cuda::GpuMat* src1, const cv::cuda::GpuMat* src2, cv::cuda::GpuMat* dst, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream)
{
   cv::cuda::bitwise_xor(*src1, *src2, *dst, mask ? *mask : cv::cuda::GpuMat(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseXorS(const cv::cuda::GpuMat* src1, const cv::Scalar sc, cv::cuda::GpuMat* dst, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream)
{
   cv::cuda::bitwise_xor(*src1, sc, *dst,  mask ? *mask : cv::cuda::GpuMat(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMin(const cv::cuda::GpuMat* src1, const cv::cuda::GpuMat* src2, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream)
{
   cv::cuda::min(*src1, *src2, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMinS(const cv::cuda::GpuMat* src1, double src2, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream)
{
   cv::cuda::min(*src1, src2, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMax(const cv::cuda::GpuMat* src1, const cv::cuda::GpuMat* src2, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream)
{
   cv::cuda::max(*src1, *src2, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMaxS(const cv::cuda::GpuMat* src1, double src2, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream)
{
   cv::cuda::max(*src1, src2, *dst, stream ? *stream : cv::cuda::Stream::Null());
}



void cudaGemm(const cv::cuda::GpuMat* src1, const cv::cuda::GpuMat* src2, double alpha, 
                const cv::cuda::GpuMat* src3, double beta, cv::cuda::GpuMat* dst, int flags, cv::cuda::Stream* stream)
{
   cv::cuda::GpuMat src3Mat = src3 ? *src3 : cv::cuda::GpuMat();
   cv::cuda::gemm(*src1, *src2, alpha, src3Mat, beta, *dst, flags, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaLShift(const cv::cuda::GpuMat* a, CvScalar scale, cv::cuda::GpuMat* c, cv::cuda::Stream* stream)
{
	cv::cuda::lshift(*a, scale, *c, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaRShift(const cv::cuda::GpuMat* a, CvScalar scale, cv::cuda::GpuMat* c, cv::cuda::Stream* stream)
{
	cv::cuda::rshift(*a, scale, *c, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaAdd(const cv::cuda::GpuMat* a, const cv::cuda::GpuMat* b, cv::cuda::GpuMat* c, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream)
{
   cv::cuda::add(*a, *b, *c, mask ? *mask : cv::cuda::GpuMat(), c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaAddS(const cv::cuda::GpuMat* a, const CvScalar scale, cv::cuda::GpuMat* c, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream)
{
   cv::Scalar s = scale;
   cv::cuda::add(*a, s, *c, mask ? *mask : cv::cuda::GpuMat(), c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaSubtract(const cv::cuda::GpuMat* a, const cv::cuda::GpuMat* b, cv::cuda::GpuMat* c, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream)
{
   cv::cuda::subtract(*a, *b, *c, mask ? *mask : cv::cuda::GpuMat(), c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaSubtractS(const cv::cuda::GpuMat* a, const CvScalar scale, cv::cuda::GpuMat* c, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream)
{
   cv::Scalar s = scale;
   cv::cuda::subtract(*a, s, *c, mask ? *mask : cv::cuda::GpuMat(), c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMultiply(const cv::cuda::GpuMat* a, const cv::cuda::GpuMat* b, cv::cuda::GpuMat* c, double scale, cv::cuda::Stream* stream)
{
   cv::cuda::multiply(*a, *b, *c, scale, c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMultiplyS(const cv::cuda::GpuMat* a, const CvScalar s, cv::cuda::GpuMat* c, cv::cuda::Stream* stream)
{
   cv::Scalar scalar = s;
   cv::cuda::multiply(*a, scalar, *c, 1, c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaDivide(const cv::cuda::GpuMat* a, const cv::cuda::GpuMat* b, cv::cuda::GpuMat* c, double scale, cv::cuda::Stream* stream)
{
   cv::cuda::divide(*a, *b, *c, scale, c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaDivideSR(const cv::cuda::GpuMat* a, const CvScalar s, cv::cuda::GpuMat* c, cv::cuda::Stream* stream)
{
   cv::Scalar scalar = s;
   cv::cuda::divide(*a, scalar, *c, 1, c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaDivideSL(const double s, const cv::cuda::GpuMat* b, cv::cuda::GpuMat* c, cv::cuda::Stream* stream)
{
   cv::cuda::divide(s, *b, *c, c->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaAddWeighted(const cv::cuda::GpuMat* src1, double alpha, const cv::cuda::GpuMat* src2, double beta, double gamma, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream)
{
   cv::cuda::addWeighted(*src1, alpha, *src2, beta, gamma, *dst, dst->depth(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaAbsdiff(const cv::cuda::GpuMat* a, const cv::cuda::GpuMat* b, cv::cuda::GpuMat* c, cv::cuda::Stream* stream)
{
   cv::cuda::absdiff(*a, *b, *c, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaAbs(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream)
{
   cv::cuda::abs(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaSqr(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream)
{
   cv::cuda::sqr(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaSqrt(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream)
{
   cv::cuda::sqrt(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaAbsdiffS(const cv::cuda::GpuMat* a, const CvScalar s, cv::cuda::GpuMat* c, cv::cuda::Stream* stream)
{
   cv::Scalar scalar = s;
   cv::cuda::absdiff(*a, scalar, *c, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaCompare(const cv::cuda::GpuMat* a, const cv::cuda::GpuMat* b, cv::cuda::GpuMat* c, int cmpop, cv::cuda::Stream* stream)
{
   cv::cuda::compare(*a, *b, *c, cmpop, stream ? *stream : cv::cuda::Stream::Null());
}

double cudaThreshold(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, double thresh, double maxval, int type, cv::cuda::Stream* stream)
{
   return cv::cuda::threshold(*src, *dst, thresh, maxval, type, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaCopyMakeBorder(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int top, int bottom, int left, int right, int gpuBorderType, const CvScalar value, cv::cuda::Stream* stream)
{
   cv::cuda::copyMakeBorder(*src, *dst, top, bottom, left, right, gpuBorderType, value, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaIntegral(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* sum, cv::cuda::GpuMat* sqrSum, cv::cuda::Stream* stream)
{
   if (sum && sqrSum)
      cv::cuda::integralBuffered(*src, *sum, *sqrSum, stream ? *stream : cv::cuda::Stream::Null());
   else if (sum)
   {
      CV_Assert(stream == 0);
      cv::cuda::integral(*src, *sum, cv::cuda::Stream::Null());
   } else if (sqrSum)
   {
      cv::cuda::sqrIntegral(*src, *sqrSum, stream ? *stream : cv::cuda::Stream::Null());
   }
}


void cudaDft(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int flags, cv::cuda::Stream* stream)
{
   cv::cuda::dft(*src, *dst, dst->size(), flags | (dst->channels() == 1 ? cv::DFT_REAL_OUTPUT : 0), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaFlip(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int flipcode, cv::cuda::Stream* stream)
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
void cudaLookUpTableTransform(cv::cuda::LookUpTable* lut, cv::cuda::GpuMat* image, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream)
{
   lut->transform(*image, *dst, stream ? *stream : cv::cuda::Stream::Null());
}
void cudaLookUpTableRelease(cv::cuda::LookUpTable** lut)
{
   delete *lut;
   *lut=0;
}