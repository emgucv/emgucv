//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gapi_c.h"

cv::GMat* cveGMatCreate()
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		return new cv::GMat();
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveGMatRelease(cv::GMat** gmat)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		delete* gmat;
		*gmat = 0;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::GMat* cveGapiAdd(cv::GMat* src1, cv::GMat* src2, int ddepth)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::add(*src1, *src2, ddepth);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GMat* cveGapiAddC(cv::GMat* src1, cv::GScalar* c, int ddepth)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::addC(*src1, *c, ddepth);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GMat* cveGapiSub(cv::GMat* src1, cv::GMat* src2, int ddepth)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::sub(*src1, *src2, ddepth);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GMat* cveGapiSubC(cv::GMat* src1, cv::GScalar* c, int ddepth)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::subC(*src1, *c, ddepth);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GMat* cveGapiSubRC(cv::GScalar* c, cv::GMat* src1, int ddepth)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::subRC(*c, *src1, ddepth);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GMat* cveGapiMul(cv::GMat* src1, cv::GMat* src2, double scale, int ddepth)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::mul(*src1, *src2, scale, ddepth);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GMat* cveGapiMulC(cv::GMat* src, cv::GScalar* scale, int ddepth)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::mulC(*src, *scale, ddepth);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GMat* cveGapiDiv(cv::GMat* src1, cv::GMat* src2, double scale, int ddepth)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::div(*src1, *src2, scale, ddepth);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiDivC(cv::GMat* src, cv::GScalar* divisor, double scale, int ddepth)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::divC(*src, *divisor, scale, ddepth);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiDivRC(cv::GScalar* divident, cv::GMat* src, double scale, int ddepth)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::divRC(*divident, *src, scale, ddepth);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GScalar* cveGapiMean(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GScalar* result = new cv::GScalar();
		*result = cv::gapi::mean(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveGapiPolarToCart(
	cv::GMat* magnitude,
	cv::GMat* angle,
	bool angleInDegrees,
	cv::GMat* outX,
	cv::GMat* outY)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		std::tie(*outX, *outY) = cv::gapi::polarToCart(
			*magnitude,
			*angle,
			angleInDegrees);
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveGapiCartToPolar(
	cv::GMat* x,
	cv::GMat* y,
	bool angleInDegrees,
	cv::GMat* outMagnitude,
	cv::GMat* outAngle)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		std::tie(*outMagnitude, *outAngle) = cv::gapi::cartToPolar(
			*x,
			*y,
			angleInDegrees);
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::GMat* cveGapiPhase(cv::GMat* x, cv::GMat* y, bool angleInDegrees)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::phase(*x, *y, angleInDegrees);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiSqrt(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::sqrt(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiCmpGT(cv::GMat* src1, cv::GMat* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::cmpGT(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiCmpGTS(cv::GMat* src1, cv::GMat* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::cmpGT(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiCmpLT(cv::GMat* src1, cv::GMat* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::cmpLT(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiCmpLTS(cv::GMat* src1, cv::GScalar* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::cmpLT(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiCmpGE(cv::GMat* src1, cv::GMat* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::cmpGE(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiCmpGES(cv::GMat* src1, cv::GScalar* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::cmpGE(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiCmpLE(cv::GMat* src1, cv::GMat* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::cmpLE(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiCmpLES(cv::GMat* src1, cv::GScalar* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::cmpLE(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiCmpEQ(cv::GMat* src1, cv::GMat* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::cmpEQ(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiCmpEQS(cv::GMat* src1, cv::GScalar* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::cmpEQ(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiCmpNE(cv::GMat* src1, cv::GMat* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::cmpNE(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiCmpNES(cv::GMat* src1, cv::GScalar* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::cmpNE(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiBitwiseAnd(cv::GMat* src1, cv::GMat* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::bitwise_and(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiBitwiseAndS(cv::GMat* src1, cv::GScalar* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::bitwise_and(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiBitwiseOr(cv::GMat* src1, cv::GMat* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::bitwise_or(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiBitwiseOrS(cv::GMat* src1, cv::GScalar* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::bitwise_or(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiBitwiseXor(cv::GMat* src1, cv::GMat* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::bitwise_xor(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GMat* cveGapiBitwiseXorS(cv::GMat* src1, cv::GScalar* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::bitwise_xor(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GMat* cveGapiMask(cv::GMat* src, cv::GMat* mask)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::mask(*src, *mask);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}


cv::GMat* cveGapiResize(cv::GMat* src, cv::Size* dsize, double fx, double fy, int interpolation)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::resize(*src, *dsize, fx, fy, interpolation);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GMat* cveGapiBitwiseNot(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::bitwise_not(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GMat* cveGapiSelect(cv::GMat* src1, cv::GMat* src2, cv::GMat* mask)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::select(*src1, *src2, *mask);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiMin(cv::GMat* src1, cv::GMat* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::min(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiMax(cv::GMat* src1, cv::GMat* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::max(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiAbsDiff(cv::GMat* src1, cv::GMat* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::absDiff(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiAbsDiffC(cv::GMat* src, cv::GScalar* c)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::absDiffC(*src, *c);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GScalar* cveGapiSum(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GScalar* result = new cv::GScalar();
		*result = cv::gapi::sum(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiAddWeighted(
	cv::GMat* src1,
	double alpha,
	cv::GMat* src2,
	double beta,
	double gamma,
	int ddepth)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::addWeighted(
			*src1,
			alpha,
			*src2,
			beta,
			gamma,
			ddepth);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GScalar* cveGapiNormL1(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GScalar* result = new cv::GScalar();
		*result = cv::gapi::normL1(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GScalar* cveGapiNormL2(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GScalar* result = new cv::GScalar();
		*result = cv::gapi::normL2(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GScalar* cveGapiNormInf(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GScalar* result = new cv::GScalar();
		*result = cv::gapi::normInf(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveGapiIntegral(cv::GMat* src, int sdepth, int sqdepth, cv::GMat* dst1, cv::GMat* dst2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		std::tie(*dst1, *dst2) = cv::gapi::integral(*src, sdepth, sqdepth);
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::GMat* cveGapiThreshold(cv::GMat* src, cv::GScalar* thresh, cv::GScalar* maxval, int type)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::threshold(
			*src,
			*thresh,
			*maxval,
			type);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiInRange(cv::GMat* src, cv::GScalar* threshLow, cv::GScalar* threshUp)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::inRange(
			*src,
			*threshLow,
			*threshUp);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiMerge4(cv::GMat* src1, cv::GMat* src2, cv::GMat* src3, cv::GMat* src4)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::merge4(*src1, *src2, *src3, *src4);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiMerge3(cv::GMat* src1, cv::GMat* src2, cv::GMat* src3)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::merge3(*src1, *src2, *src3);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveGapiSplit4(cv::GMat* src, cv::GMat* dst1, cv::GMat* dst2, cv::GMat* dst3, cv::GMat* dst4)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		std::tie(*dst1, *dst2, *dst3, *dst4) = cv::gapi::split4(*src);
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveGapiSplit3(cv::GMat* src, cv::GMat* dst1, cv::GMat* dst2, cv::GMat* dst3)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		std::tie(*dst1, *dst2, *dst3) = cv::gapi::split3(*src);
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::GMat* cveGapiRemap(
	cv::GMat* src,
	cv::Mat* map1,
	cv::Mat* map2,
	int interpolation,
	int borderMode,
	cv::Scalar* borderValue)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::remap(*src, *map1, *map2, interpolation, borderMode, *borderValue);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiFlip(cv::GMat* src, int flipCode)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::flip(*src, flipCode);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiCrop(cv::GMat* src, cv::Rect* rect)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::crop(*src, *rect);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiConcatHor(cv::GMat* src1, cv::GMat* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::concatHor(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiConcatHorV(std::vector< cv::GMat >* v)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::concatHor(*v);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiConcatVert(cv::GMat* src1, cv::GMat* src2)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::concatVert(*src1, *src2);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiConcatVertV(std::vector< cv::GMat >* v)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::concatHor(*v);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiLUT(cv::GMat* src, cv::Mat* lut)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::LUT(*src, *lut);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiConvertTo(
	cv::GMat* src,
	int rdepth,
	double alpha,
	double beta)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::convertTo(*src, rdepth, alpha, beta);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiNormalize(
	cv::GMat* src,
	double alpha,
	double beta,
	int normType,
	int ddepth)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::normalize(*src, alpha, beta, normType, ddepth);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiWarpPerspective(
	cv::GMat* src,
	cv::Mat* M,
	cv::Size* dsize,
	int flags,
	int borderMode,
	cv::Scalar* borderValue)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::warpPerspective(*src, *M, *dsize, flags, borderMode, *borderValue);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiWarpAffine(
	cv::GMat* src,
	cv::Mat* M,
	cv::Size* dsize,
	int flags,
	int borderMode,
	cv::Scalar* borderValue)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::warpAffine(*src, *M, *dsize, flags, borderMode, *borderValue);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GMat* cveGapiTranspose(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::transpose(*src);
		return result;
	#else
		throw_no_gapi();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GComputation* cveGComputationCreate1(cv::GMat* in, cv::GMat* out)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		return new cv::GComputation(*in, *out);
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GComputation* cveGComputationCreate2(cv::GMat* input, cv::GScalar* output)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		return new cv::GComputation(*input, *output);
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GComputation* cveGComputationCreate3(cv::GMat* input1, cv::GMat* input2, cv::GMat* output)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		return new cv::GComputation(*input1, *input2, *output);
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GComputation* cveGComputationCreate4(cv::GMat* input1, cv::GMat* input2, cv::GScalar* output)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		return new cv::GComputation(*input1, *input2, *output);
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GComputation* cveGComputationCreate5(std::vector< cv::GMat >* ins, std::vector< cv::GMat >* outs)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		return new cv::GComputation(*ins, *outs);
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveGComputationRelease(cv::GComputation** computation)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		delete* computation;
		*computation = 0;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveGComputationApply1(cv::GComputation* computation, cv::Mat* input, cv::Mat* output)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		computation->apply(*input, *output);
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveGComputationApply2(cv::GComputation* computation, cv::Mat* input, cv::Scalar* output)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::Scalar o;
		computation->apply(*input, o);
		*output = o;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveGComputationApply3(cv::GComputation* computation, cv::Mat* input1, cv::Mat* input2, cv::Mat* output)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		computation->apply(*input1, *input2, *output);
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveGComputationApply4(cv::GComputation* computation, cv::Mat* input1, cv::Mat* input2, cv::Scalar* output)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::Scalar o;
		computation->apply(*input1, *input2, o);
		*output = o;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveGComputationApply5(cv::GComputation* computation, std::vector< cv::Mat >* inputs, std::vector< cv::Mat >* outputs)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		computation->apply(*inputs, *outputs);
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::GScalar* cveGScalarCreate(cv::Scalar* value)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		return new cv::GScalar(*value);
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveGScalarRelease(cv::GScalar** gscalar)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		delete* gscalar;
		*gscalar = 0;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::GMat* cveGapiSepFilter(
	cv::GMat* src,
	int ddepth,
	cv::Mat* kernelX,
	cv::Mat* kernelY,
	cv::Point* anchor,
	cv::Scalar* delta,
	int borderType,
	cv::Scalar* borderValue)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::sepFilter(
			*src,
			ddepth,
			*kernelX,
			*kernelY,
			*anchor,
			*delta,
			borderType,
			*borderValue
		);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiFilter2D(
	cv::GMat* src,
	int ddepth,
	cv::Mat* kernel,
	cv::Point* anchor,
	cv::Scalar* delta,
	int borderType,
	cv::Scalar* borderValue)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::filter2D(
			*src,
			ddepth,
			*kernel,
			*anchor,
			*delta,
			borderType,
			*borderValue);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiBoxFilter(
	cv::GMat* src,
	int dtype,
	cv::Size* ksize,
	cv::Point* anchor,
	bool normalize,
	int borderType,
	cv::Scalar* borderValue)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::boxFilter(
			*src,
			dtype,
			*ksize,
			*anchor,
			normalize,
			borderType,
			*borderValue);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GMat* cveGapiBlur(
	cv::GMat* src,
	cv::Size* ksize,
	cv::Point* anchor,
	int borderType,
	cv::Scalar* borderValue)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::blur(
			*src,
			*ksize,
			*anchor,
			borderType,
			*borderValue
		);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiGaussianBlur(
	cv::GMat* src,
	cv::Size* ksize,
	double sigmaX,
	double sigmaY,
	int borderType,
	cv::Scalar* borderValue)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::gaussianBlur(
			*src,
			*ksize,
			sigmaX,
			sigmaY,
			borderType,
			*borderValue
		);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiMedianBlur(
	cv::GMat* src,
	int ksize)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::medianBlur(
			*src,
			ksize
		);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiErode(
	cv::GMat* src,
	cv::Mat* kernel,
	cv::Point* anchor,
	int iterations,
	int borderType,
	cv::Scalar* borderValue)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::erode(
			*src,
			*kernel,
			*anchor,
			iterations,
			borderType,
			*borderValue
		);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiErode3x3(
	cv::GMat* src,
	int iterations,
	int borderType,
	cv::Scalar* borderValue)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::erode3x3(
			*src,
			iterations,
			borderType,
			*borderValue
		);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiDilate(
	cv::GMat* src,
	cv::Mat* kernel,
	cv::Point* anchor,
	int iterations,
	int borderType,
	cv::Scalar* borderValue)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::dilate(
			*src,
			*kernel,
			*anchor,
			iterations,
			borderType,
			*borderValue
		);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiDilate3x3(
	cv::GMat* src,
	int iterations,
	int borderType,
	cv::Scalar* borderValue)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::dilate3x3(
			*src,
			iterations,
			borderType,
			*borderValue
		);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiMorphologyEx(
	cv::GMat* src,
	int op,
	cv::Mat* kernel,
	cv::Point* anchor,
	int  iterations,
	int  borderType,
	cv::Scalar* borderValue)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::morphologyEx(
			*src,
			static_cast<cv::MorphTypes>(op),
			*kernel,
			*anchor,
			iterations,
			static_cast<cv::BorderTypes>(borderType),
			*borderValue
		);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::GMat* cveGapiSobel(
	cv::GMat* src,
	int ddepth,
	int dx,
	int dy,
	int ksize,
	double scale,
	double delta,
	int borderType,
	cv::Scalar* borderValue)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::Sobel(
			*src,
			ddepth,
			dx,
			dy,
			ksize,
			scale,
			delta,
			borderType,
			*borderValue
		);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveGapiSobelXY(
	cv::GMat* src,
	int ddepth,
	int order,
	int ksize,
	double scale,
	double delta,
	int borderType,
	cv::Scalar* borderValue,
	cv::GMat* sobelX,
	cv::GMat* sobelY)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		std::tie(*sobelX, *sobelY) = cv::gapi::SobelXY(
			*src,
			ddepth,
			order,
			ksize,
			scale,
			delta,
			borderType,
			*borderValue
		);
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
cv::GMat* cveGapiLaplacian(
	cv::GMat* src,
	int ddepth,
	int ksize,
	double scale,
	double delta,
	int borderType)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::Laplacian(
			*src,
			ddepth,
			ksize,
			scale,
			delta,
			borderType);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiBilateralFilter(
	cv::GMat* src,
	int d,
	double sigmaColor,
	double sigmaSpace,
	int borderType)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::bilateralFilter(
			*src,
			d,
			sigmaColor,
			sigmaSpace,
			borderType);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiCanny(
	cv::GMat* image,
	double threshold1,
	double threshold2,
	int apertureSize,
	bool L2gradient)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::Canny(
			*image,
			threshold1,
			threshold2,
			apertureSize,
			L2gradient);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiEqualizeHist(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::equalizeHist(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiBGR2RGB(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::BGR2RGB(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiRGB2Gray1(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::RGB2Gray(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiRGB2Gray2(cv::GMat* src, float rY, float gY, float bY)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::RGB2Gray(*src, rY, gY, bY);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiBGR2Gray(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::BGR2Gray(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiRGB2YUV(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::RGB2YUV(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiBGR2I420(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::BGR2I420(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiRGB2I420(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::RGB2I420(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiI4202BGR(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::I4202BGR(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiI4202RGB(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::I4202RGB(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiBGR2LUV(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::BGR2LUV(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiLUV2BGR(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::LUV2BGR(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiYUV2BGR(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::YUV2BGR(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiBGR2YUV(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::BGR2YUV(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiRGB2Lab(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::RGB2Lab(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiYUV2RGB(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::YUV2RGB(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiNV12toRGB(cv::GMat* srcY, cv::GMat* srcUV)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::NV12toRGB(*srcY, *srcUV);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiNV12toGray(cv::GMat* srcY, cv::GMat* srcUV)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::NV12toGray(*srcY, *srcUV);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiNV12toBGR(cv::GMat* srcY, cv::GMat* srcUV)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::NV12toBGR(*srcY, *srcUV);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiBayerGR2RGB(cv::GMat* srcGR)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::BayerGR2RGB(*srcGR);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiRGB2HSV(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::RGB2HSV(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::GMat* cveGapiRGB2YUV422(cv::GMat* src)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::RGB2YUV422(*src);
		return result;
	#else
		throw_no_gapi();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}


cv::GMat* cveGapiStereo(cv::GMat* left, cv::GMat* right, int of)
{
	try
	{
	#ifdef HAVE_OPENCV_GAPI
		cv::GMat* result = new cv::GMat();
		*result = cv::gapi::stereo(*left, *right, static_cast<cv::gapi::StereoOutputFormat>(of));
		return result;
	#else
		throw_no_gapi();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS(0)
}