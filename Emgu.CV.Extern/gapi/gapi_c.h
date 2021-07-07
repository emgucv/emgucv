//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_GAPI_C_H
#define EMGU_GAPI_C_H

#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_GAPI
#include "opencv2/gapi.hpp"
#include "opencv2/gapi/core.hpp"
#include "opencv2/gapi/imgproc.hpp"
#include "opencv2/gapi/stereo.hpp"
#else
static inline CV_NORETURN void throw_no_gapi() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without gapi support"); }
namespace cv {
	class GMat {};
	class GScalar {};
	class GComputation {};
	namespace gapi {

	}
}
#endif

CVAPI(cv::GMat*) cveGMatCreate();
CVAPI(void) cveGMatRelease(cv::GMat** gmat);

CVAPI(cv::GMat*) cveGapiAdd(cv::GMat* src1, cv::GMat* src2, int ddepth);
CVAPI(cv::GMat*) cveGapiAddC(cv::GMat* src1, cv::GScalar* c, int ddepth);

CVAPI(cv::GMat*) cveGapiSub(cv::GMat* src1, cv::GMat* src2, int ddepth);
CVAPI(cv::GMat*) cveGapiSubC(cv::GMat* src1, cv::GScalar* c, int ddepth);
CVAPI(cv::GMat*) cveGapiSubRC(cv::GScalar* c, cv::GMat* src1, int ddepth);

CVAPI(cv::GMat*) cveGapiMul(cv::GMat* src1, cv::GMat* src2, double scale, int ddepth);
CVAPI(cv::GMat*) cveGapiMulC(cv::GMat* src, cv::GScalar* scale, int ddepth);

CVAPI(cv::GMat*) cveGapiDiv(cv::GMat* src1, cv::GMat* src2, double scale, int ddepth);
CVAPI(cv::GMat*) cveGapiDivC(cv::GMat* src, cv::GScalar* divisor, double scale, int ddepth);
CVAPI(cv::GMat*) cveGapiDivRC(cv::GScalar* divident, cv::GMat* src, double scale, int ddepth);

CVAPI(cv::GScalar*) cveGapiMean(cv::GMat* src);

CVAPI(void) cveGapiPolarToCart(
	cv::GMat* magnitude,
	cv::GMat* angle,
	bool angleInDegrees,
	cv::GMat* outX,
	cv::GMat* outY);
CVAPI(void) cveGapiCartToPolar(
	cv::GMat* x,
	cv::GMat* y,
	bool angleInDegrees,
	cv::GMat* outMagnitude,
	cv::GMat* outAngle);
CVAPI(cv::GMat*) cveGapiPhase(cv::GMat* x, cv::GMat* y, bool angleInDegrees);
CVAPI(cv::GMat*) cveGapiSqrt(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiCmpGT(cv::GMat* src1, cv::GMat* src2);
CVAPI(cv::GMat*) cveGapiCmpGTS(cv::GMat* src1, cv::GMat* src2);
CVAPI(cv::GMat*) cveGapiCmpLT(cv::GMat* src1, cv::GMat* src2);
CVAPI(cv::GMat*) cveGapiCmpLTS(cv::GMat* src1, cv::GScalar* src2);
CVAPI(cv::GMat*) cveGapiCmpGE(cv::GMat* src1, cv::GMat* src2);
CVAPI(cv::GMat*) cveGapiCmpGES(cv::GMat* src1, cv::GScalar* src2);
CVAPI(cv::GMat*) cveGapiCmpLE(cv::GMat* src1, cv::GMat* src2);
CVAPI(cv::GMat*) cveGapiCmpLES(cv::GMat* src1, cv::GScalar* src2);
CVAPI(cv::GMat*) cveGapiCmpEQ(cv::GMat* src1, cv::GMat* src2);
CVAPI(cv::GMat*) cveGapiCmpEQS(cv::GMat* src1, cv::GScalar* src2);
CVAPI(cv::GMat*) cveGapiCmpNE(cv::GMat* src1, cv::GMat* src2);
CVAPI(cv::GMat*) cveGapiCmpNES(cv::GMat* src1, cv::GScalar* src2);
CVAPI(cv::GMat*) cveGapiBitwiseAnd(cv::GMat* src1, cv::GMat* src2);
CVAPI(cv::GMat*) cveGapiBitwiseAndS(cv::GMat* src1, cv::GScalar* src2);
CVAPI(cv::GMat*) cveGapiBitwiseOr(cv::GMat* src1, cv::GMat* src2);
CVAPI(cv::GMat*) cveGapiBitwiseOrS(cv::GMat* src1, cv::GScalar* src2);
CVAPI(cv::GMat*) cveGapiBitwiseXor(cv::GMat* src1, cv::GMat* src2);
CVAPI(cv::GMat*) cveGapiBitwiseXorS(cv::GMat* src1, cv::GScalar* src2);

CVAPI(cv::GMat*) cveGapiMask(cv::GMat* src, cv::GMat* mask);

CVAPI(cv::GMat*) cveGapiResize(cv::GMat* src, cv::Size* dsize, double fx, double fy, int interpolation);
CVAPI(cv::GMat*) cveGapiBitwiseNot(cv::GMat* src);


CVAPI(cv::GMat*) cveGapiSelect(cv::GMat* src1, cv::GMat* src2, cv::GMat* mask);
CVAPI(cv::GMat*) cveGapiMin(cv::GMat* src1, cv::GMat* src2);
CVAPI(cv::GMat*) cveGapiMax(cv::GMat* src1, cv::GMat* src2);
CVAPI(cv::GMat*) cveGapiAbsDiff(cv::GMat* src1, cv::GMat* src2);
CVAPI(cv::GMat*) cveGapiAbsDiffC(cv::GMat* src, cv::GScalar* c);
CVAPI(cv::GScalar*) cveGapiSum(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiAddWeighted(
	cv::GMat* src1,
	double alpha,
	cv::GMat* src2,
	double beta,
	double gamma,
	int ddepth);
CVAPI(cv::GScalar*) cveGapiNormL1(cv::GMat* src);
CVAPI(cv::GScalar*) cveGapiNormL2(cv::GMat* src);
CVAPI(cv::GScalar*) cveGapiNormInf(cv::GMat* src);
CVAPI(void) cveGapiIntegral(cv::GMat* src, int sdepth, int sqdepth, cv::GMat* dst1, cv::GMat* dst2);
CVAPI(cv::GMat*) cveGapiThreshold(cv::GMat* src, cv::GScalar* thresh, cv::GScalar* maxval, int type);
CVAPI(cv::GMat*) cveGapiInRange(cv::GMat* src, cv::GScalar* threshLow, cv::GScalar* threshUp);
CVAPI(cv::GMat*) cveGapiMerge4(cv::GMat* src1, cv::GMat* src2, cv::GMat* src3, cv::GMat* src4);
CVAPI(cv::GMat*) cveGapiMerge3(cv::GMat* src1, cv::GMat* src2, cv::GMat* src3);
CVAPI(void) cveGapiSplit4(cv::GMat* src, cv::GMat* dst1, cv::GMat* dst2, cv::GMat* dst3, cv::GMat* dst4);
CVAPI(void) cveGapiSplit3(cv::GMat* src, cv::GMat* dst1, cv::GMat* dst2, cv::GMat* dst3);
CVAPI(cv::GMat*) cveGapiRemap(
	cv::GMat* src,
	cv::Mat* map1,
	cv::Mat* map2,
	int interpolation,
	int borderMode,
	CvScalar* borderValue);
CVAPI(cv::GMat*) cveGapiFlip(cv::GMat* src, int flipCode);
CVAPI(cv::GMat*) cveGapiCrop(cv::GMat* src, CvRect* rect);
CVAPI(cv::GMat*) cveGapiConcatHor(cv::GMat* src1, cv::GMat* src2);
CVAPI(cv::GMat*) cveGapiConcatHorV(std::vector< cv::GMat >* v);
CVAPI(cv::GMat*) cveGapiConcatVert(cv::GMat* src1, cv::GMat* src2);
CVAPI(cv::GMat*) cveGapiConcatVertV(std::vector< cv::GMat >* v);
CVAPI(cv::GMat*) cveGapiLUT(cv::GMat* src, cv::Mat* lut);
CVAPI(cv::GMat*) cveGapiConvertTo(
	cv::GMat* src,
	int rdepth,
	double alpha,
	double beta);
CVAPI(cv::GMat*) cveGapiNormalize(
	cv::GMat* src,
	double alpha,
	double beta,
	int normType,
	int ddepth);
CVAPI(cv::GMat*) cveGapiWarpPerspective(
	cv::GMat* src,
	cv::Mat* M,
	CvSize* dsize,
	int flags,
	int borderMode,
	CvScalar* borderValue);
CVAPI(cv::GMat*) cveGapiWarpAffine(
	cv::GMat* src,
	cv::Mat* M,
	CvSize* dsize,
	int flags,
	int borderMode,
	CvScalar* borderValue);

CVAPI(cv::GMat*) cveGapiTranspose(cv::GMat* src);

CVAPI(cv::GComputation*) cveGComputationCreate1(cv::GMat* input, cv::GMat* output);
CVAPI(cv::GComputation*) cveGComputationCreate2(cv::GMat* input, cv::GScalar* output);
CVAPI(cv::GComputation*) cveGComputationCreate3(cv::GMat* input1, cv::GMat* input2, cv::GMat* output);
CVAPI(cv::GComputation*) cveGComputationCreate4(cv::GMat* input1, cv::GMat* input2, cv::GScalar* output);
CVAPI(cv::GComputation*) cveGComputationCreate5(std::vector< cv::GMat >* ins, std::vector< cv::GMat >* outs);


CVAPI(void) cveGComputationRelease(cv::GComputation** computation);
CVAPI(void) cveGComputationApply1(cv::GComputation* computation, cv::Mat* input, cv::Mat* output);
CVAPI(void) cveGComputationApply2(cv::GComputation* computation, cv::Mat* input, CvScalar* output);
CVAPI(void) cveGComputationApply3(cv::GComputation* computation, cv::Mat* input1, cv::Mat* input2, cv::Mat* output);
CVAPI(void) cveGComputationApply4(cv::GComputation* computation, cv::Mat* input1, cv::Mat* input2, CvScalar* output);
CVAPI(void) cveGComputationApply5(cv::GComputation* computation, std::vector< cv::Mat >* inputs, std::vector< cv::Mat >* outputs);

CVAPI(cv::GScalar*) cveGScalarCreate(CvScalar* value);
CVAPI(void) cveGScalarRelease(cv::GScalar** gscalar);

CVAPI(cv::GMat*) cveGapiSepFilter(
	cv::GMat* src,
	int ddepth,
	cv::Mat* kernelX,
	cv::Mat* kernelY,
	CvPoint* anchor,
	CvScalar* delta,
	int borderType,
	CvScalar* borderValue);
CVAPI(cv::GMat*) cveGapiFilter2D(
	cv::GMat* src,
	int ddepth,
	cv::Mat* kernel,
	CvPoint* anchor,
	CvScalar* delta,
	int borderType,
	CvScalar* borderValue);
CVAPI(cv::GMat*) cveGapiBoxFilter(
	cv::GMat* src,
	int dtype,
	CvSize* ksize,
	CvPoint* anchor,
	bool normalize,
	int borderType,
	CvScalar* borderValue);

CVAPI(cv::GMat*) cveGapiBlur(
	cv::GMat* src,
	CvSize* ksize,
	CvPoint* anchor,
	int borderType,
	CvScalar* borderValue);
CVAPI(cv::GMat*) cveGapiGaussianBlur(
	cv::GMat* src,
	CvSize* ksize,
	double sigmaX,
	double sigmaY,
	int borderType,
	CvScalar* borderValue);
CVAPI(cv::GMat*) cveGapiMedianBlur(
	cv::GMat* src,
	int ksize);
CVAPI(cv::GMat*) cveGapiErode(
	cv::GMat* src,
	cv::Mat* kernel,
	CvPoint* anchor,
	int iterations,
	int borderType,
	CvScalar* borderValue);
CVAPI(cv::GMat*) cveGapiErode3x3(
	cv::GMat* src,
	int iterations,
	int borderType,
	CvScalar* borderValue);
CVAPI(cv::GMat*) cveGapiDilate(
	cv::GMat* src,
	cv::Mat* kernel,
	CvPoint* anchor,
	int iterations,
	int borderType,
	CvScalar* borderValue);
CVAPI(cv::GMat*) cveGapiDilate3x3(
	cv::GMat* src,
	int iterations,
	int borderType,
	CvScalar* borderValue);
CVAPI(cv::GMat*) cveGapiMorphologyEx(
	cv::GMat* src,
	int op,
	cv::Mat* kernel,
	CvPoint* anchor,
	int  iterations,
	int  borderType,
	CvScalar* borderValue);


CVAPI(cv::GMat*) cveGapiSobel(
	cv::GMat* src,
	int ddepth,
	int dx,
	int dy,
	int ksize,
	double scale,
	double delta,
	int borderType,
	CvScalar* borderValue);
CVAPI(void) cveGapiSobelXY(
	cv::GMat* src,
	int ddepth,
	int order,
	int ksize,
	double scale,
	double delta,
	int borderType,
	CvScalar* borderValue,
	cv::GMat* sobelX,
	cv::GMat* sobelY);
CVAPI(cv::GMat*) cveGapiLaplacian(
	cv::GMat* src,
	int ddepth,
	int ksize,
	double scale,
	double delta,
	int borderType);
CVAPI(cv::GMat*) cveGapiBilateralFilter(
	cv::GMat* src,
	int d,
	double sigmaColor,
	double sigmaSpace,
	int borderType);
CVAPI(cv::GMat*) cveGapiCanny(
	cv::GMat* image,
	double threshold1,
	double threshold2,
	int apertureSize,
	bool L2gradient);
CVAPI(cv::GMat*) cveGapiEqualizeHist(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiBGR2RGB(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiRGB2Gray1(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiRGB2Gray2(cv::GMat* src, float rY, float gY, float bY);
CVAPI(cv::GMat*) cveGapiBGR2Gray(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiRGB2YUV(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiBGR2I420(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiRGB2I420(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiI4202BGR(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiI4202RGB(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiBGR2LUV(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiLUV2BGR(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiYUV2BGR(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiBGR2YUV(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiRGB2Lab(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiYUV2RGB(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiNV12toRGB(cv::GMat* srcY, cv::GMat* srcUV);
CVAPI(cv::GMat*) cveGapiNV12toGray(cv::GMat* srcY, cv::GMat* srcUV);
CVAPI(cv::GMat*) cveGapiNV12toBGR(cv::GMat* srcY, cv::GMat* srcUV);
CVAPI(cv::GMat*) cveGapiBayerGR2RGB(cv::GMat* srcGR);
CVAPI(cv::GMat*) cveGapiRGB2HSV(cv::GMat* src);
CVAPI(cv::GMat*) cveGapiRGB2YUV422(cv::GMat* src);


CVAPI(cv::GMat*) cveGapiStereo(cv::GMat* left, cv::GMat* right, int of);
#endif