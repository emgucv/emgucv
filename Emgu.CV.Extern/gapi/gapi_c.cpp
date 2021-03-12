//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gapi_c.h"

cv::GMat* cveGMatCreate()
{
	return new cv::GMat();
}
void cveGMatRelease(cv::GMat** gmat)
{
	delete* gmat;
	*gmat = 0;
}

cv::GMat* cveGapiAdd(cv::GMat* src1, cv::GMat* src2, int ddepth)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::add(*src1, *src2, ddepth);
	return result;
}

cv::GMat* cveGapiAddC(cv::GMat* src1, cv::GScalar* c, int ddepth)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::addC(*src1, *c, ddepth);
	return result;
}

cv::GMat* cveGapiSub(cv::GMat* src1, cv::GMat* src2, int ddepth)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::sub(*src1, *src2, ddepth);
	return result;
}

cv::GMat* cveGapiSubC(cv::GMat* src1, cv::GScalar* c, int ddepth)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::subC(*src1, *c, ddepth);
	return result;
}

cv::GMat* cveGapiSubRC(cv::GScalar* c, cv::GMat* src1, int ddepth)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::subRC(*c, *src1, ddepth);
	return result;
}

cv::GMat* cveGapiMul(cv::GMat* src1, cv::GMat* src2, double scale, int ddepth)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::mul(*src1, *src2, scale, ddepth);
	return result;
}

cv::GMat* cveGapiMulC(cv::GMat* src, cv::GScalar* scale, int ddepth)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::mulC(*src, *scale, ddepth);
	return result;
}

cv::GMat* cveGapiDiv(cv::GMat* src1, cv::GMat* src2, double scale, int ddepth)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::div(*src1, *src2, scale, ddepth);
	return result;
}
cv::GMat* cveGapiDivC(cv::GMat* src, cv::GScalar* divisor, double scale, int ddepth)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::divC(*src, *divisor, scale, ddepth);
	return result;
}
cv::GMat* cveGapiDivRC(cv::GScalar* divident, cv::GMat* src, double scale, int ddepth)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::divRC(*divident, *src, scale, ddepth);
	return result;
}

cv::GScalar* cveGapiMean(cv::GMat* src)
{
	cv::GScalar* result = new cv::GScalar();
	*result = cv::gapi::mean(*src);
	return result;
}

void cveGapiPolarToCart(
	cv::GMat* magnitude,
	cv::GMat* angle,
	bool angleInDegrees,
	cv::GMat* outX,
	cv::GMat* outY)
{
	std::tie(*outX, *outY) = cv::gapi::polarToCart(
		*magnitude,
		*angle,
		angleInDegrees);
}
void cveGapiCartToPolar(
	cv::GMat* x,
	cv::GMat* y,
	bool angleInDegrees,
	cv::GMat* outMagnitude,
	cv::GMat* outAngle)
{
	std::tie(*outMagnitude, *outAngle) = cv::gapi::cartToPolar(
		*x,
		*y,
		angleInDegrees);
}
cv::GMat* cveGapiPhase(cv::GMat* x, cv::GMat* y, bool angleInDegrees)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::phase(*x, *y, angleInDegrees);
	return result;
}
cv::GMat* cveGapiSqrt(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::sqrt(*src);
	return result;
}
cv::GMat* cveGapiCmpGT(cv::GMat* src1, cv::GMat* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::cmpGT(*src1, *src2);
	return result;
}
cv::GMat* cveGapiCmpGTS(cv::GMat* src1, cv::GMat* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::cmpGT(*src1, *src2);
	return result;
}
cv::GMat* cveGapiCmpLT(cv::GMat* src1, cv::GMat* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::cmpLT(*src1, *src2);
	return result;
}
cv::GMat* cveGapiCmpLTS(cv::GMat* src1, cv::GScalar* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::cmpLT(*src1, *src2);
	return result;
}
cv::GMat* cveGapiCmpGE(cv::GMat* src1, cv::GMat* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::cmpGE(*src1, *src2);
	return result;
}
cv::GMat* cveGapiCmpGES(cv::GMat* src1, cv::GScalar* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::cmpGE(*src1, *src2);
	return result;
}
cv::GMat* cveGapiCmpLE(cv::GMat* src1, cv::GMat* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::cmpLE(*src1, *src2);
	return result;
}
cv::GMat* cveGapiCmpLES(cv::GMat* src1, cv::GScalar* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::cmpLE(*src1, *src2);
	return result;
}
cv::GMat* cveGapiCmpEQ(cv::GMat* src1, cv::GMat* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::cmpEQ(*src1, *src2);
	return result;
}
cv::GMat* cveGapiCmpEQS(cv::GMat* src1, cv::GScalar* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::cmpEQ(*src1, *src2);
	return result;
}
cv::GMat* cveGapiCmpNE(cv::GMat* src1, cv::GMat* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::cmpNE(*src1, *src2);
	return result;
}
cv::GMat* cveGapiCmpNES(cv::GMat* src1, cv::GScalar* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::cmpNE(*src1, *src2);
	return result;
}
cv::GMat* cveGapiBitwiseAnd(cv::GMat* src1, cv::GMat* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::bitwise_and(*src1, *src2);
	return result;
}
cv::GMat* cveGapiBitwiseAndS(cv::GMat* src1, cv::GScalar* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::bitwise_and(*src1, *src2);
	return result;
}
cv::GMat* cveGapiBitwiseOr(cv::GMat* src1, cv::GMat* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::bitwise_or(*src1, *src2);
	return result;
}
cv::GMat* cveGapiBitwiseOrS(cv::GMat* src1, cv::GScalar* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::bitwise_or(*src1, *src2);
	return result;
}
cv::GMat* cveGapiBitwiseXor(cv::GMat* src1, cv::GMat* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::bitwise_xor(*src1, *src2);
	return result;
}

cv::GMat* cveGapiBitwiseXorS(cv::GMat* src1, cv::GScalar* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::bitwise_xor(*src1, *src2);
	return result;
}

cv::GMat* cveGapiMask(cv::GMat* src, cv::GMat* mask)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::mask(*src, *mask);
	return result;
}


cv::GMat* cveGapiResize(cv::GMat* src, cv::Size* dsize, double fx, double fy, int interpolation)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::resize(*src, *dsize, fx, fy, interpolation);
	return result;
}

cv::GMat* cveGapiBitwiseNot(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::bitwise_not(*src);
	return result;
}

cv::GMat* cveGapiSelect(cv::GMat* src1, cv::GMat* src2, cv::GMat* mask)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::select(*src1, *src2, *mask);
	return result;
}
cv::GMat* cveGapiMin(cv::GMat* src1, cv::GMat* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::min(*src1, *src2);
	return result;
}
cv::GMat* cveGapiMax(cv::GMat* src1, cv::GMat* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::max(*src1, *src2);
	return result;
}
cv::GMat* cveGapiAbsDiff(cv::GMat* src1, cv::GMat* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::absDiff(*src1, *src2);
	return result;
}
cv::GMat* cveGapiAbsDiffC(cv::GMat* src, cv::GScalar* c)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::absDiffC(*src, *c);
	return result;
}
cv::GScalar* cveGapiSum(cv::GMat* src)
{
	cv::GScalar* result = new cv::GScalar();
	*result = cv::gapi::sum(*src);
	return result;
}
cv::GMat* cveGapiAddWeighted(
	cv::GMat* src1,
	double alpha,
	cv::GMat* src2,
	double beta,
	double gamma,
	int ddepth)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::addWeighted(
		*src1,
		alpha,
		*src2,
		beta,
		gamma,
		ddepth);
	return result;
}
cv::GScalar* cveGapiNormL1(cv::GMat* src)
{
	cv::GScalar* result = new cv::GScalar();
	*result = cv::gapi::normL1(*src);
	return result;
}
cv::GScalar* cveGapiNormL2(cv::GMat* src)
{
	cv::GScalar* result = new cv::GScalar();
	*result = cv::gapi::normL2(*src);
	return result;
}
cv::GScalar* cveGapiNormInf(cv::GMat* src)
{
	cv::GScalar* result = new cv::GScalar();
	*result = cv::gapi::normInf(*src);
	return result;
}
void cveGapiIntegral(cv::GMat* src, int sdepth, int sqdepth, cv::GMat* dst1, cv::GMat* dst2)
{
	std::tie(*dst1, *dst2) = cv::gapi::integral(*src, sdepth, sqdepth);
}
cv::GMat* cveGapiThreshold(cv::GMat* src, cv::GScalar* thresh, cv::GScalar* maxval, int type)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::threshold(
		*src,
		*thresh,
		*maxval,
		type);
	return result;
}
cv::GMat* cveGapiInRange(cv::GMat* src, cv::GScalar* threshLow, cv::GScalar* threshUp)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::inRange(
		*src,
		*threshLow,
		*threshUp);
	return result;
}
cv::GMat* cveGapiMerge4(cv::GMat* src1, cv::GMat* src2, cv::GMat* src3, cv::GMat* src4)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::merge4(*src1, *src2, *src3, *src4);
	return result;
}
cv::GMat* cveGapiMerge3(cv::GMat* src1, cv::GMat* src2, cv::GMat* src3)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::merge3(*src1, *src2, *src3);
	return result;
}
void cveGapiSplit4(cv::GMat* src, cv::GMat* dst1, cv::GMat* dst2, cv::GMat* dst3, cv::GMat* dst4)
{
	std::tie(*dst1, *dst2, *dst3, *dst4) = cv::gapi::split4(*src);
}

void cveGapiSplit3(cv::GMat* src, cv::GMat* dst1, cv::GMat* dst2, cv::GMat* dst3)
{
	std::tie(*dst1, *dst2, *dst3) = cv::gapi::split3(*src);
}
cv::GMat* cveGapiRemap(
	cv::GMat* src,
	cv::Mat* map1,
	cv::Mat* map2,
	int interpolation,
	int borderMode,
	CvScalar* borderValue)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::remap(*src, *map1, *map2, interpolation, borderMode, *borderValue);
	return result;
}
cv::GMat* cveGapiFlip(cv::GMat* src, int flipCode)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::flip(*src, flipCode);
	return result;
}
cv::GMat* cveGapiCrop(cv::GMat* src, CvRect* rect)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::crop(*src, *rect);
	return result;
}
cv::GMat* cveGapiConcatHor(cv::GMat* src1, cv::GMat* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::concatHor(*src1, *src2);
	return result;
}
cv::GMat* cveGapiConcatHorV(std::vector< cv::GMat >* v)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::concatHor(*v);
	return result;
}
cv::GMat* cveGapiConcatVert(cv::GMat* src1, cv::GMat* src2)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::concatVert(*src1, *src2);
	return result;
}
cv::GMat* cveGapiConcatVertV(std::vector< cv::GMat >* v)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::concatHor(*v);
	return result;
}
cv::GMat* cveGapiLUT(cv::GMat* src, cv::Mat* lut)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::LUT(*src, *lut);
	return result;
}
cv::GMat* cveGapiConvertTo(
	cv::GMat* src,
	int rdepth,
	double alpha,
	double beta)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::convertTo(*src, rdepth, alpha, beta);
	return result;
}
cv::GMat* cveGapiNormalize(
	cv::GMat* src,
	double alpha,
	double beta,
	int normType,
	int ddepth)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::normalize(*src, alpha, beta, normType, ddepth);
	return result;
}
cv::GMat* cveGapiWarpPerspective(
	cv::GMat* src,
	cv::Mat* M,
	CvSize* dsize,
	int flags,
	int borderMode,
	CvScalar* borderValue)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::warpPerspective(*src, *M, *dsize, flags, borderMode, *borderValue);
	return result;
}
cv::GMat* cveGapiWarpAffine(
	cv::GMat* src,
	cv::Mat* M,
	CvSize* dsize,
	int flags,
	int borderMode,
	CvScalar* borderValue)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::warpAffine(*src, *M, *dsize, flags, borderMode, *borderValue);
	return result;
}

cv::GComputation* cveGComputationCreate1(cv::GMat* in, cv::GMat* out)
{
	return new cv::GComputation(*in, *out);
}

cv::GComputation* cveGComputationCreate2(cv::GMat* input, cv::GScalar* output)
{
	return new cv::GComputation(*input, *output);
}
cv::GComputation* cveGComputationCreate3(cv::GMat* input1, cv::GMat* input2, cv::GMat* output)
{
	return new cv::GComputation(*input1, *input2, *output);
}

cv::GComputation* cveGComputationCreate4(cv::GMat* input1, cv::GMat* input2, cv::GScalar* output)
{
	return new cv::GComputation(*input1, *input2, *output);
}
cv::GComputation* cveGComputationCreate5(std::vector< cv::GMat >* ins, std::vector< cv::GMat >* outs)
{
	return new cv::GComputation(*ins, *outs);
}

void cveGComputationRelease(cv::GComputation** computation)
{
	delete* computation;
	*computation = 0;
}

void cveGComputationApply1(cv::GComputation* computation, cv::Mat* input, cv::Mat* output)
{
	computation->apply(*input, *output);
}

void cveGComputationApply2(cv::GComputation* computation, cv::Mat* input, CvScalar* output)
{
	cv::Scalar o;
	computation->apply(*input, o);
	*output = cvScalar(o);
}
void cveGComputationApply3(cv::GComputation* computation, cv::Mat* input1, cv::Mat* input2, cv::Mat* output)
{
	computation->apply(*input1, *input2, *output);
}
void cveGComputationApply4(cv::GComputation* computation, cv::Mat* input1, cv::Mat* input2, CvScalar* output)
{
	cv::Scalar o;
	computation->apply(*input1, *input2, o);
	*output = cvScalar(o);
}
void cveGComputationApply5(cv::GComputation* computation, std::vector< cv::Mat >* inputs, std::vector< cv::Mat >* outputs)
{
	computation->apply(*inputs, *outputs);
}

cv::GScalar* cveGScalarCreate(CvScalar* value)
{
	return new cv::GScalar(*value);
}
void cveGScalarRelease(cv::GScalar** gscalar)
{
	delete* gscalar;
	*gscalar = 0;
}

cv::GMat* cveGapiSepFilter(
	cv::GMat* src,
	int ddepth,
	cv::Mat* kernelX,
	cv::Mat* kernelY,
	CvPoint* anchor,
	CvScalar* delta,
	int borderType,
	CvScalar* borderValue)
{
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
}
cv::GMat* cveGapiFilter2D(
	cv::GMat* src,
	int ddepth,
	cv::Mat* kernel,
	CvPoint* anchor,
	CvScalar* delta,
	int borderType,
	CvScalar* borderValue)
{
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
}
cv::GMat* cveGapiBoxFilter(
	cv::GMat* src,
	int dtype,
	CvSize* ksize,
	CvPoint* anchor,
	bool normalize,
	int borderType,
	CvScalar* borderValue)
{
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
}

cv::GMat* cveGapiBlur(
	cv::GMat* src,
	CvSize* ksize,
	CvPoint* anchor,
	int borderType,
	CvScalar* borderValue)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::blur(
		*src,
		*ksize,
		*anchor,
		borderType,
		*borderValue
	);
	return result;
}
cv::GMat* cveGapiGaussianBlur(
	cv::GMat* src,
	CvSize* ksize,
	double sigmaX,
	double sigmaY,
	int borderType,
	CvScalar* borderValue)
{
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
}
cv::GMat* cveGapiMedianBlur(
	cv::GMat* src,
	int ksize)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::medianBlur(
		*src,
		ksize
	);
	return result;
}
cv::GMat* cveGapiErode(
	cv::GMat* src,
	cv::Mat* kernel,
	CvPoint* anchor,
	int iterations,
	int borderType,
	CvScalar* borderValue)
{
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
}
cv::GMat* cveGapiErode3x3(
	cv::GMat* src,
	int iterations,
	int borderType,
	CvScalar* borderValue)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::erode3x3(
		*src,
		iterations,
		borderType,
		*borderValue
	);
	return result;
}
cv::GMat* cveGapiDilate(
	cv::GMat* src,
	cv::Mat* kernel,
	CvPoint* anchor,
	int iterations,
	int borderType,
	CvScalar* borderValue)
{
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
}
cv::GMat* cveGapiDilate3x3(
	cv::GMat* src,
	int iterations,
	int borderType,
	CvScalar* borderValue)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::dilate3x3(
		*src,
		iterations,
		borderType,
		*borderValue
	);
	return result;
}
cv::GMat* cveGapiMorphologyEx(
	cv::GMat* src,
	int op,
	cv::Mat* kernel,
	CvPoint* anchor,
	int  iterations,
	int  borderType,
	CvScalar* borderValue)
{
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
	CvScalar* borderValue)
{
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
}
void cveGapiSobelXY(
	cv::GMat* src,
	int ddepth,
	int order,
	int ksize,
	double scale,
	double delta,
	int borderType,
	CvScalar* borderValue,
	cv::GMat* sobelX,
	cv::GMat* sobelY)
{
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
}
cv::GMat* cveGapiLaplacian(
	cv::GMat* src,
	int ddepth,
	int ksize,
	double scale,
	double delta,
	int borderType)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::Laplacian(
		*src,
		ddepth,
		ksize,
		scale,
		delta,
		borderType);
	return result;
}
cv::GMat* cveGapiBilateralFilter(
	cv::GMat* src,
	int d,
	double sigmaColor,
	double sigmaSpace,
	int borderType)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::bilateralFilter(
		*src,
		d,
		sigmaColor,
		sigmaSpace,
		borderType);
	return result;
}
cv::GMat* cveGapiCanny(
	cv::GMat* image,
	double threshold1,
	double threshold2,
	int apertureSize,
	bool L2gradient)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::Canny(
		*image,
		threshold1,
		threshold2,
		apertureSize,
		L2gradient);
	return result;
}
cv::GMat* cveGapiEqualizeHist(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::equalizeHist(*src);
	return result;
}
cv::GMat* cveGapiBGR2RGB(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::BGR2RGB(*src);
	return result;
}
cv::GMat* cveGapiRGB2Gray1(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::RGB2Gray(*src);
	return result;
}
cv::GMat* cveGapiRGB2Gray2(cv::GMat* src, float rY, float gY, float bY)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::RGB2Gray(*src, rY, gY, bY);
	return result;
}
cv::GMat* cveGapiBGR2Gray(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::BGR2Gray(*src);
	return result;
}
cv::GMat* cveGapiRGB2YUV(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::RGB2YUV(*src);
	return result;
}
cv::GMat* cveGapiBGR2I420(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::BGR2I420(*src);
	return result;
}
cv::GMat* cveGapiRGB2I420(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::RGB2I420(*src);
	return result;
}
cv::GMat* cveGapiI4202BGR(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::I4202BGR(*src);
	return result;
}
cv::GMat* cveGapiI4202RGB(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::I4202RGB(*src);
	return result;
}
cv::GMat* cveGapiBGR2LUV(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::BGR2LUV(*src);
	return result;
}
cv::GMat* cveGapiLUV2BGR(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::LUV2BGR(*src);
	return result;
}
cv::GMat* cveGapiYUV2BGR(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::YUV2BGR(*src);
	return result;
}
cv::GMat* cveGapiBGR2YUV(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::BGR2YUV(*src);
	return result;
}
cv::GMat* cveGapiRGB2Lab(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::RGB2Lab(*src);
	return result;
}
cv::GMat* cveGapiYUV2RGB(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::YUV2RGB(*src);
	return result;
}
cv::GMat* cveGapiNV12toRGB(cv::GMat* srcY, cv::GMat* srcUV)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::NV12toRGB(*srcY, *srcUV);
	return result;
}
cv::GMat* cveGapiNV12toGray(cv::GMat* srcY, cv::GMat* srcUV)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::NV12toGray(*srcY, *srcUV);
	return result;
}
cv::GMat* cveGapiNV12toBGR(cv::GMat* srcY, cv::GMat* srcUV)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::NV12toBGR(*srcY, *srcUV);
	return result;
}
cv::GMat* cveGapiBayerGR2RGB(cv::GMat* srcGR)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::BayerGR2RGB(*srcGR);
	return result;
}
cv::GMat* cveGapiRGB2HSV(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::RGB2HSV(*src);
	return result;
}
cv::GMat* cveGapiRGB2YUV422(cv::GMat* src)
{
	cv::GMat* result = new cv::GMat();
	*result = cv::gapi::RGB2YUV422(*src);
	return result;
}