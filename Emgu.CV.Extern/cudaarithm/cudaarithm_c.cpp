//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
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

void cudaMerge(std::vector< cv::cuda::GpuMat >* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   cv::cuda::merge(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
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

void cudaFindMinMaxLoc(cv::_InputArray* src, cv::_OutputArray* minMaxVals, cv::_OutputArray* loc, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::cuda::findMinMaxLoc(*src, *minMaxVals, *loc, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}


void cudaMeanStdDev(cv::_InputArray* mtx, CvScalar* mean, CvScalar* stddev)
{
   cv::Scalar meanVal, stdDevVal;
   
   cv::cuda::meanStdDev(*mtx, meanVal, stdDevVal);
   
   memcpy(mean->val, meanVal.val, sizeof(double)*4);
   memcpy(stddev->val, stdDevVal.val, sizeof(double)*4);
}

double cudaNorm1(cv::_InputArray* src1, int normType, cv::_InputArray* mask)
{
   return cv::cuda::norm(*src1, normType, mask ? *mask : (cv::_InputArray) cv::noArray());
}
double cudaNorm2(cv::_InputArray* src1, cv::_InputArray* src2, int normType)
{
   return cv::cuda::norm(*src1, *src2, normType);
}
void cudaCalcNorm(cv::_InputArray* src, cv::_OutputArray* dst, int normType, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::cuda::calcNorm(*src, *dst, normType, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}
void cudaCalcNormDiff(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int normType, cv::cuda::Stream* stream)
{
   cv::cuda::calcNormDiff(*src1, *src2, *dst, normType, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaAbsSum(cv::_InputArray* src, CvScalar* sum, cv::_InputArray* mask)
{
   *sum = cv::cuda::absSum(*src, mask ? *mask : (cv::_InputArray) cv::noArray());
}
void cudaCalcAbsSum(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::cuda::calcAbsSum(*src, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}
void cudaSqrSum(cv::_InputArray* src, CvScalar* sqrSum, cv::_InputArray* mask)
{
   *sqrSum = cv::cuda::sqrSum(*src, mask ? *mask : (cv::_InputArray) cv::noArray());
}
void cudaCalcSqrSum(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::cuda::calcSqrSum(*src, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}

int cudaCountNonZero1(cv::_InputArray* src)
{
   return cv::cuda::countNonZero(*src);
}

void cudaCountNonZero2(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   cv::cuda::countNonZero(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaReduce(cv::_InputArray* mtx, cv::_OutputArray* vec, int dim, int reduceOp, int dType, cv::cuda::Stream* stream)
{
   cv::cuda::reduce(*mtx, *vec, dim, reduceOp, dType, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseNot(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::cuda::bitwise_not(*src, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseAnd(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::cuda::bitwise_and(*src1, *src2, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseOr(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::cuda::bitwise_or(*src1, *src2, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBitwiseXor(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::cuda::bitwise_xor(*src1, *src2, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMin(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   cv::cuda::min(*src1, *src2, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMax(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   cv::cuda::max(*src1, *src2, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaGemm(cv::_InputArray* src1, cv::_InputArray* src2, double alpha,
	cv::_InputArray* src3, double beta, cv::_OutputArray* dst, int flags, cv::cuda::Stream* stream)
{
   cv::cuda::gemm(*src1, *src2, alpha, *src3, beta, *dst, flags, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaLShift(cv::_InputArray* a, CvScalar* scale, cv::_OutputArray* c, cv::cuda::Stream* stream)
{
	cv::cuda::lshift(*a, *scale, *c, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaRShift(cv::_InputArray* a, CvScalar* scale, cv::_OutputArray* c, cv::cuda::Stream* stream)
{
	cv::cuda::rshift(*a, *scale, *c, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaAdd(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, cv::_InputArray* mask, int dtype, cv::cuda::Stream* stream)
{
   cv::cuda::add(*a, *b, *c, mask ? *mask : (cv::_InputArray) cv::noArray(), dtype, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaSubtract(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, cv::_InputArray* mask, int dtype, cv::cuda::Stream* stream)
{
   cv::cuda::subtract(*a, *b, *c, mask ? *mask : (cv::_InputArray) cv::noArray(), dtype, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMultiply(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, double scale, int dtype, cv::cuda::Stream* stream)
{
   cv::cuda::multiply(*a, *b, *c, scale, dtype, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaDivide(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, double scale, int dtype, cv::cuda::Stream* stream)
{
   cv::cuda::divide(*a, *b, *c, scale, dtype, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaAddWeighted(cv::_InputArray* src1, double alpha, cv::_InputArray* src2, double beta, double gamma, cv::_OutputArray* dst, int dtype, cv::cuda::Stream* stream)
{
   cv::cuda::addWeighted(*src1, alpha, *src2, beta, gamma, *dst, dtype, stream ? *stream : cv::cuda::Stream::Null());
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

void cudaIntegral(cv::_InputArray* src, cv::_OutputArray* sum, cv::cuda::Stream* stream)
{
   cv::cuda::integral(*src, *sum, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaSqrIntegral(cv::_InputArray* src, cv::_OutputArray* sqrSum,  cv::cuda::Stream* stream)
{
   cv::cuda::sqrIntegral(*src, *sqrSum, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaDft(cv::_InputArray* src, cv::_OutputArray* dst, CvSize* dftSize, int flags, cv::cuda::Stream* stream)
{
   cv::cuda::dft(*src, *dst, *dftSize, flags | (dst->channels() == 1 ? cv::DFT_REAL_OUTPUT : 0), stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMulAndScaleSpectrums(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int flags, float scale, bool conjB, cv::cuda::Stream* stream)
{
   cv::cuda::mulAndScaleSpectrums(*src1, *src2, *dst, flags, scale, conjB, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaMulSpectrums(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int flags, bool conjB, cv::cuda::Stream* stream)
{
   cv::cuda::mulSpectrums(*src1, *src2, *dst, flags, conjB, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaFlip(cv::_InputArray* src, cv::_OutputArray* dst, int flipcode, cv::cuda::Stream* stream)
{
   cv::cuda::flip(*src, *dst, flipcode, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaSplit(cv::_InputArray* src, std::vector< cv::cuda::GpuMat >* dst, cv::cuda::Stream* stream)
{
   cv::cuda::split(*src, *dst, stream? *stream : cv::cuda::Stream::Null());
}

cv::cuda::LookUpTable* cudaLookUpTableCreate( cv::_InputArray* lut )
{
   cv::Ptr<cv::cuda::LookUpTable> ptr = cv::cuda::createLookUpTable(*lut);
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

void cudaTranspose(cv::_InputArray* src1, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   cv::cuda::transpose(*src1, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaNormalize(cv::_InputArray* src, cv::_OutputArray* dst, double alpha, double beta,
   int norm_type, int dtype, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
   cv::cuda::normalize(*src, *dst, alpha, beta, norm_type, dtype, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null()); 
}