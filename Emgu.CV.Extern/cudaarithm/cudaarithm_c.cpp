//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudaarithm_c.h"
#include "opencv2/core/opengl.hpp"

void cudaExp(cv::_InputArray* a, cv::_OutputArray* b, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::exp(*a, *b, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaPow(cv::_InputArray* src, double power, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::pow(*src, power, *dst, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaLog(cv::_InputArray* a, cv::_OutputArray* b, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::log(*a, *b, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaMagnitude(cv::_InputArray* x, cv::_InputArray* y, cv::_OutputArray* magnitude, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::magnitude(*x, *y, *magnitude, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaMagnitudeSqr(cv::_InputArray* x, cv::_InputArray* y, cv::_OutputArray* magnitude, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::magnitudeSqr(*x, *y, *magnitude, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaPhase(cv::_InputArray* x, cv::_InputArray* y, cv::_OutputArray* angle, bool angleInDegrees, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::phase(*x, *y, *angle, angleInDegrees, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaCartToPolar(cv::_InputArray* x, cv::_InputArray* y, cv::_OutputArray* magnitude, cv::_OutputArray* angle, bool angleInDegrees, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::cartToPolar(*x, *y, *magnitude, *angle, angleInDegrees, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaPolarToCart(cv::_InputArray* magnitude, cv::_InputArray* angle, cv::_OutputArray* x, cv::_OutputArray* y, bool angleInDegrees, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::polarToCart(*magnitude, *angle, *x, *y, angleInDegrees, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaMerge(std::vector< cv::cuda::GpuMat >* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::merge(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//only support single channel gpuMat
void cudaMinMaxLoc(cv::_InputArray* src, double* minVal, double* maxVal, cv::Point* minLoc, cv::Point* maxLoc, cv::_InputArray* mask)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::Point minimunLoc, maximunLoc;
		cv::_InputArray maskMat = mask ? *mask : (cv::_InputArray) cv::noArray();
		cv::cuda::minMaxLoc(*src, minVal, maxVal, &minimunLoc, &maximunLoc, maskMat);
		maxLoc->x = maximunLoc.x; maxLoc->y = maximunLoc.y;
		minLoc->x = minimunLoc.x; minLoc->y = minimunLoc.y;
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaFindMinMaxLoc(cv::_InputArray* src, cv::_OutputArray* minMaxVals, cv::_OutputArray* loc, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::findMinMaxLoc(*src, *minMaxVals, *loc, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


void cudaMeanStdDev(cv::_InputArray* mtx, cv::Scalar* mean, cv::Scalar* stddev)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::Scalar meanVal, stdDevVal;

		cv::cuda::meanStdDev(*mtx, meanVal, stdDevVal);

		memcpy(mean->val, meanVal.val, sizeof(double) * 4);
		memcpy(stddev->val, stdDevVal.val, sizeof(double) * 4);
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

double cudaNorm1(cv::_InputArray* src1, int normType, cv::_InputArray* mask)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		return cv::cuda::norm(*src1, normType, mask ? *mask : (cv::_InputArray) cv::noArray());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
double cudaNorm2(cv::_InputArray* src1, cv::_InputArray* src2, int normType)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		return cv::cuda::norm(*src1, *src2, normType);
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cudaCalcNorm(cv::_InputArray* src, cv::_OutputArray* dst, int normType, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::calcNorm(*src, *dst, normType, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cudaCalcNormDiff(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int normType, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::calcNormDiff(*src1, *src2, *dst, normType, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaAbsSum(cv::_InputArray* src, cv::Scalar* sum, cv::_InputArray* mask)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		*sum = cv::cuda::absSum(*src, mask ? *mask : (cv::_InputArray) cv::noArray());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cudaCalcAbsSum(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::calcAbsSum(*src, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cudaSqrSum(cv::_InputArray* src, cv::Scalar* sqrSum, cv::_InputArray* mask)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		*sqrSum = cv::cuda::sqrSum(*src, mask ? *mask : (cv::_InputArray) cv::noArray());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cudaCalcSqrSum(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::calcSqrSum(*src, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

int cudaCountNonZero1(cv::_InputArray* src)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		return cv::cuda::countNonZero(*src);
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cudaCountNonZero2(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::countNonZero(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaReduce(cv::_InputArray* mtx, cv::_OutputArray* vec, int dim, int reduceOp, int dType, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::reduce(*mtx, *vec, dim, reduceOp, dType, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaBitwiseNot(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::bitwise_not(*src, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaBitwiseAnd(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::bitwise_and(*src1, *src2, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaBitwiseOr(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::bitwise_or(*src1, *src2, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaBitwiseXor(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::bitwise_xor(*src1, *src2, *dst, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaMin(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::min(*src1, *src2, *dst, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaMax(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::max(*src1, *src2, *dst, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaGemm(cv::_InputArray* src1, cv::_InputArray* src2, double alpha,
	cv::_InputArray* src3, double beta, cv::_OutputArray* dst, int flags, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::gemm(*src1, *src2, alpha, *src3, beta, *dst, flags, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaLShift(cv::_InputArray* a, cv::Scalar* scale, cv::_OutputArray* c, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::lshift(*a, static_cast<cv::Scalar>(*scale), *c, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaRShift(cv::_InputArray* a, cv::Scalar* scale, cv::_OutputArray* c, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::rshift(*a, static_cast<cv::Scalar>(*scale), *c, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaAdd(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, cv::_InputArray* mask, int dtype, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::add(*a, *b, *c, mask ? *mask : (cv::_InputArray) cv::noArray(), dtype, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaSubtract(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, cv::_InputArray* mask, int dtype, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::subtract(*a, *b, *c, mask ? *mask : (cv::_InputArray) cv::noArray(), dtype, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaMultiply(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, double scale, int dtype, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::multiply(*a, *b, *c, scale, dtype, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaDivide(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, double scale, int dtype, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::divide(*a, *b, *c, scale, dtype, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaAddWeighted(cv::_InputArray* src1, double alpha, cv::_InputArray* src2, double beta, double gamma, cv::_OutputArray* dst, int dtype, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::addWeighted(*src1, alpha, *src2, beta, gamma, *dst, dtype, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaAbsdiff(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::absdiff(*a, *b, *c, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaAbs(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::abs(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaSqr(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::sqr(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaSqrt(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::sqrt(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaCompare(cv::_InputArray* a, cv::_InputArray* b, cv::_OutputArray* c, int cmpop, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::compare(*a, *b, *c, cmpop, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

double cudaThreshold(cv::_InputArray* src, cv::_OutputArray* dst, double thresh, double maxval, int type, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		return cv::cuda::threshold(*src, *dst, thresh, maxval, type, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cudaCopyMakeBorder(cv::_InputArray* src, cv::_OutputArray* dst, int top, int bottom, int left, int right, int gpuBorderType, const cv::Scalar* value, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::copyMakeBorder(*src, *dst, top, bottom, left, right, gpuBorderType, *value, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaIntegral(cv::_InputArray* src, cv::_OutputArray* sum, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::integral(*src, *sum, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaSqrIntegral(cv::_InputArray* src, cv::_OutputArray* sqrSum, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::sqrIntegral(*src, *sqrSum, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaDft(cv::_InputArray* src, cv::_OutputArray* dst, cv::Size* dftSize, int flags, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::dft(*src, *dst, *dftSize, flags | (dst->channels() == 1 ? cv::DFT_REAL_OUTPUT : 0), stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaMulAndScaleSpectrums(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int flags, float scale, bool conjB, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::mulAndScaleSpectrums(*src1, *src2, *dst, flags, scale, conjB, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaMulSpectrums(cv::_InputArray* src1, cv::_InputArray* src2, cv::_OutputArray* dst, int flags, bool conjB, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::mulSpectrums(*src1, *src2, *dst, flags, conjB, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaFlip(cv::_InputArray* src, cv::_OutputArray* dst, int flipcode, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::flip(*src, *dst, flipcode, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaSplit(cv::_InputArray* src, std::vector< cv::cuda::GpuMat >* dst, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::split(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::cuda::LookUpTable* cudaLookUpTableCreate(cv::_InputArray* lut, cv::Ptr<cv::cuda::LookUpTable>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::Ptr<cv::cuda::LookUpTable> ptr = cv::cuda::createLookUpTable(*lut);
		*sharedPtr = new cv::Ptr<cv::cuda::LookUpTable>(ptr);
		return ptr.get();
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cudaLookUpTableTransform(cv::cuda::LookUpTable* lut, cv::_InputArray* image, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		lut->transform(*image, *dst, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cudaLookUpTableRelease(cv::Ptr<cv::cuda::LookUpTable>** lut)
{
#ifdef HAVE_OPENCV_CUDAARITHM
	delete* lut;
	*lut = 0;
#else
	throw_no_cudaarithm();
#endif
}

void cudaTranspose(cv::_InputArray* src1, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::transpose(*src1, *dst, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaNormalize(cv::_InputArray* src, cv::_OutputArray* dst, double alpha, double beta,
	int norm_type, int dtype, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::normalize(*src, *dst, alpha, beta, norm_type, dtype, mask ? *mask : (cv::_InputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaSetGlDevice(int device)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::setGlDevice(device);
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::cuda::Convolution* cudaConvolutionCreate(cv::Size* userBlockSize, cv::Ptr<cv::cuda::Convolution>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::Ptr<cv::cuda::Convolution> ptr = cv::cuda::createConvolution(*userBlockSize);
		*sharedPtr = new cv::Ptr<cv::cuda::Convolution>(ptr);
		return ptr.get();
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cudaConvolutionConvolve(cv::cuda::Convolution* convolution, cv::_InputArray* image, cv::_InputArray* templ, cv::_OutputArray* result, bool ccorr, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		convolution->convolve(*image, *templ, *result, ccorr, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cudaConvolutionRelease(cv::Ptr<cv::cuda::Convolution>** convolution)
{
#ifdef HAVE_OPENCV_CUDAARITHM
	delete* convolution;
	*convolution = 0;
#else
	throw_no_cudaarithm();
#endif
}

void cudaInRange(cv::_InputArray* src, cv::Scalar* lowerb, cv::Scalar* upperb, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDAARITHM
		cv::cuda::inRange(*src, *lowerb, *upperb, *dst, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudaarithm();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}