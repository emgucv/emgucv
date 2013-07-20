//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_GPU_C_H
#define EMGU_GPU_C_H

#include "opencv2/gpu/gpu.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"

//----------------------------------------------------------------------------
//
//  Gpu Device Info
//
//----------------------------------------------------------------------------

CVAPI(int) gpuGetCudaEnabledDeviceCount();

CVAPI(void) gpuSetDevice(int deviceId);

CVAPI(int) gpuGetDevice();

CVAPI(cv::gpu::DeviceInfo*) gpuDeviceInfoCreate(int* deviceId);

CVAPI(void) gpuDeviceInfoRelease(cv::gpu::DeviceInfo** di);

CVAPI(void) gpuDeviceInfoDeviceName(cv::gpu::DeviceInfo* device, char* name, int maxSizeInBytes);

CVAPI(void) gpuDeviceInfoComputeCapability(cv::gpu::DeviceInfo* device, int* major, int* minor);

CVAPI(int) gpuDeviceInfoMultiProcessorCount(cv::gpu::DeviceInfo* device);

CVAPI(void) gpuDeviceInfoFreeMemInfo(cv::gpu::DeviceInfo* info, size_t* free);

CVAPI(void) gpuDeviceInfoTotalMemInfo(cv::gpu::DeviceInfo* info, size_t* total);

CVAPI(bool) gpuDeviceInfoSupports(cv::gpu::DeviceInfo* device, cv::gpu::FeatureSet feature);

CVAPI(bool) gpuDeviceInfoIsCompatible(cv::gpu::DeviceInfo* device);

//----------------------------------------------------------------------------
//
//  Gpu Module Info
//
//----------------------------------------------------------------------------

CVAPI(bool) targetArchsBuildWith(cv::gpu::FeatureSet featureSet);

CVAPI(bool) targetArchsHas(int major, int minor);

CVAPI(bool) targetArchsHasPtx(int major, int minor);

CVAPI(bool) targetArchsHasBin(int major, int minor);

CVAPI(bool) targetArchsHasEqualOrLessPtx(int major, int minor);

CVAPI(bool) targetArchsHasEqualOrGreater(int major, int minor);

CVAPI(bool) targetArchsHasEqualOrGreaterPtx(int major, int minor);

CVAPI(bool) targetArchsHasEqualOrGreaterBin(int major, int minor);

//----------------------------------------------------------------------------
//
//  GpuMat
//
//----------------------------------------------------------------------------

CVAPI(cv::gpu::GpuMat*) gpuMatCreateDefault();

CVAPI(cv::gpu::GpuMat*) gpuMatCreate(int rows, int cols, int type);

CVAPI(cv::gpu::GpuMat*) gpuMatCreateContinuous(int rows, int cols, int type);

CVAPI(bool) gpuMatIsContinuous(cv::gpu::GpuMat* gpuMat);

CVAPI(cv::gpu::GpuMat*) gpuMatGetRegion(cv::gpu::GpuMat* other, CvSlice rowRange, CvSlice colRange);

CVAPI(void) gpuMatRelease(cv::gpu::GpuMat** mat);

CVAPI(cv::gpu::GpuMat*) gpuMatCreateFromArr(CvArr* arr);

CVAPI(CvSize) gpuMatGetSize(cv::gpu::GpuMat* gpuMat);

CVAPI(bool) gpuMatIsEmpty(cv::gpu::GpuMat* gpuMat);

CVAPI(int) gpuMatGetChannels(cv::gpu::GpuMat* gpuMat);

CVAPI(void) gpuMatUpload(cv::gpu::GpuMat* gpuMat, CvArr* arr);

CVAPI(void) gpuMatDownload(cv::gpu::GpuMat* gpuMat, CvArr* arr);

CVAPI(void) gpuMatLShift(const cv::gpu::GpuMat* a, CvScalar scale, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatRShift(const cv::gpu::GpuMat* a, CvScalar scale, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatAdd(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream);

CVAPI(void) gpuMatAddS(const cv::gpu::GpuMat* a, const CvScalar scale, cv::gpu::GpuMat* c, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream);

CVAPI(void) gpuMatSubtract(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream);

CVAPI(void) gpuMatSubtractS(const cv::gpu::GpuMat* a, const CvScalar scale, cv::gpu::GpuMat* c, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMultiply(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, double scale, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMultiplyS(const cv::gpu::GpuMat* a, const CvScalar s, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatDivide(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, double scale, cv::gpu::Stream* stream);

CVAPI(void) gpuMatDivideSR(const cv::gpu::GpuMat* a, const CvScalar s, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatDivideSL(const double s, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatAddWeighted(const cv::gpu::GpuMat* src1, double alpha, const cv::gpu::GpuMat* src2, double beta, double gamma, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatAbsdiff(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatAbs(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatSqr(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatSqrt(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatAbsdiffS(const cv::gpu::GpuMat* a, const CvScalar s, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatCompare(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, int cmpop, cv::gpu::Stream* stream);

CVAPI(double) gpuMatThreshold(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, double thresh, double maxval, int type, cv::gpu::Stream* stream);

CVAPI(void) gpuMatCvtColor(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int code, cv::gpu::Stream* stream);

CVAPI(void) gpuMatSwapChannels(cv::gpu::GpuMat* image, const int* dstOrder, cv::gpu::Stream* stream);

CVAPI(void) gpuMatConvertTo(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, double alpha, double beta, cv::gpu::Stream* stream);

CVAPI(void) gpuMatCopy(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask);

CVAPI(void) gpuMatSetTo(cv::gpu::GpuMat* mat, const CvScalar s, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream);

CVAPI(void) gpuMatResize(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int interpolation, cv::gpu::Stream* stream);

CVAPI(void) gpuMatReshape(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int cn, int rows);

CVAPI(void) gpuMatFlip(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int flipcode, cv::gpu::Stream* stream);

CVAPI(void) gpuMatSplit(const cv::gpu::GpuMat* src, cv::gpu::GpuMat** dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatExp(const cv::gpu::GpuMat* a, cv::gpu::GpuMat* b, cv::gpu::Stream* stream);

CVAPI(void) gpuMatPow(const cv::gpu::GpuMat* src, double power, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatLog(const cv::gpu::GpuMat* a, cv::gpu::GpuMat* b, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMagnitude(const cv::gpu::GpuMat* x, const cv::gpu::GpuMat* y, cv::gpu::GpuMat* magnitude, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMagnitudeSqr(const cv::gpu::GpuMat* x, const cv::gpu::GpuMat* y, cv::gpu::GpuMat* magnitude, cv::gpu::Stream* stream);

CVAPI(void) gpuMatPhase(const cv::gpu::GpuMat* x, const cv::gpu::GpuMat* y, cv::gpu::GpuMat* angle, bool angleInDegrees, cv::gpu::Stream* stream);

CVAPI(void) gpuMatCartToPolar(const cv::gpu::GpuMat* x, const cv::gpu::GpuMat* y, cv::gpu::GpuMat* magnitude, cv::gpu::GpuMat* angle, bool angleInDegrees, cv::gpu::Stream* stream);

CVAPI(void) gpuMatPolarToCart(const cv::gpu::GpuMat* magnitude, const cv::gpu::GpuMat* angle, cv::gpu::GpuMat* x, cv::gpu::GpuMat* y, bool angleInDegrees, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMerge(const cv::gpu::GpuMat** src, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

//only support single channel gpuMat
CVAPI(void) gpuMatMinMaxLoc(const cv::gpu::GpuMat* src, 
   double* minVal, double* maxVal, 
   CvPoint* minLoc, CvPoint* maxLoc, 
   const cv::gpu::GpuMat* mask);

CVAPI(void) gpuMatMatchTemplate(const cv::gpu::GpuMat* image, const cv::gpu::GpuMat* templ, cv::gpu::GpuMat* result, int method, cv::gpu::MatchTemplateBuf* buffer, cv::gpu::Stream* stream);

CVAPI(void) gpuMatPyrDown(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatPyrUp(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatBlendLinear(
   const cv::gpu::GpuMat* img1, const cv::gpu::GpuMat* img2, 
   const cv::gpu::GpuMat* weights1, const cv::gpu::GpuMat* weights2, 
   cv::gpu::GpuMat* result, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMeanStdDev(const cv::gpu::GpuMat* mtx, CvScalar* mean, CvScalar* stddev, cv::gpu::GpuMat* buffer);

CVAPI(double) gpuMatNorm(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, int normType);

CVAPI(int) gpuMatCountNonZero(const cv::gpu::GpuMat* src);

CVAPI(void) gpuMatReduce(const cv::gpu::GpuMat* mtx, cv::gpu::GpuMat* vec, int dim, int reduceOp, cv::gpu::Stream* stream);

CVAPI(void) gpuMatLUT(const cv::gpu::GpuMat* src, const CvArr* lut, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatFilter2D(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const CvArr* kernel, CvPoint anchor, int borderType, cv::gpu::Stream* stream);

CVAPI(void) gpuMatBitwiseNot(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream);

CVAPI(void) gpuMatBitwiseAnd(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream);

CVAPI(void) gpuMatBitwiseAndS(const cv::gpu::GpuMat* src1, const cv::Scalar sc, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatBitwiseOr(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream);

CVAPI(void) gpuMatBitwiseOrS(const cv::gpu::GpuMat* src1, const cv::Scalar sc, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatBitwiseXor(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream);

CVAPI(void) gpuMatBitwiseXorS(const cv::gpu::GpuMat* src1, const cv::Scalar sc, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMin(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMinS(const cv::gpu::GpuMat* src1, double src2, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMax(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMaxS(const cv::gpu::GpuMat* src1, double src2, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatSobel(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int dx, int dy, cv::gpu::GpuMat* buffer, int ksize, double scale, int rowBorderType, int columnBorderType, cv::gpu::Stream* stream);

CVAPI(void) gpuMatGaussianBlur(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, CvSize ksize, cv::gpu::GpuMat* buffer, double sigma1, double sigma2, int rowBorderType, int columnBorderType, cv::gpu::Stream* stream);

CVAPI(void) gpuMatLaplacian(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int ksize, double scale, int borderType, cv::gpu::Stream* stream);

CVAPI(void) gpuMatGemm(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, double alpha, 
   const cv::gpu::GpuMat* src3, double beta, cv::gpu::GpuMat* dst, int flags, cv::gpu::Stream* stream);

CVAPI(void) gpuMatErode( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const CvArr* kernel, cv::gpu::GpuMat* buffer, CvPoint anchor, int iterations, cv::gpu::Stream* stream);

CVAPI(void) gpuMatDilate( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const CvArr* kernel, cv::gpu::GpuMat* buffer, CvPoint anchor, int iterations, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMorphologyEx( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int op, const CvArr* kernel, cv::gpu::GpuMat* buffer1, cv::gpu::GpuMat* buffer2, CvPoint anchor, int iterations, cv::gpu::Stream* stream);

CVAPI(void) gpuMatWarpAffine( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const CvArr* M, int flags, int borderMode, cv::Scalar borderValue, cv::gpu::Stream* stream);

CVAPI(void) gpuMatWarpPerspective( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst,  const CvArr* M, int flags, int borderMode, cv::Scalar borderValue, cv::gpu::Stream* stream);

CVAPI(void) gpuMatRemap(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* xmap, const cv::gpu::GpuMat* ymap, int interpolation, int borderMode, CvScalar borderValue, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMeanShiftFiltering(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int sp, int sr,
   CvTermCriteria criteria, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMeanShiftProc(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dstr, cv::gpu::GpuMat* dstsp, int sp, int sr,
   CvTermCriteria criteria, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMeanShiftSegmentation(const cv::gpu::GpuMat* src, cv::Mat* dst, int sp, int sr, int minsize,
   CvTermCriteria criteria);

CVAPI(void) gpuMatHistEven(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* hist, cv::gpu::GpuMat* buffer, int histSize, int lowerLevel, int upperLevel, cv::gpu::Stream* stream);

CVAPI(cv::gpu::GpuMat*) gpuMatGetSubRect(const cv::gpu::GpuMat* arr, CvRect rect);

CVAPI(void) gpuMatRotate(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, double angle, double xShift, double yShift, int interpolation, cv::gpu::Stream* s);

CVAPI(void) gpuMatCopyMakeBorder(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int top, int bottom, int left, int right, int gpuBorderType, const CvScalar value, cv::gpu::Stream* stream);

CVAPI(void) gpuMatIntegral(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* sum, cv::gpu::GpuMat* buffer, cv::gpu::Stream* stream);

CVAPI(void) gpuMatCornerHarris(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int blockSize, int ksize, double k, int borderType);

CVAPI(void) gpuMatDft(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int flags, cv::gpu::Stream* stream);

CVAPI(void) gpuMatCanny(const cv::gpu::GpuMat* image, cv::gpu::GpuMat* edges, double lowThreshold, double highThreshold, int apertureSize, bool L2gradient);

//----------------------------------------------------------------------------
//
//  GpuBruteForceMatcher
//
//----------------------------------------------------------------------------
CVAPI(cv::gpu::BFMatcher_GPU*) gpuBruteForceMatcherCreate(int distType);

CVAPI(void) gpuBruteForceMatcherRelease(cv::gpu::BFMatcher_GPU** matcher);

CVAPI(void) gpuBruteForceMatcherAdd(cv::gpu::BFMatcher_GPU* matcher, const cv::gpu::GpuMat* trainDescs);

CVAPI(void) gpuBruteForceMatcherKnnMatchSingle(
   cv::gpu::BFMatcher_GPU* matcher,
   const cv::gpu::GpuMat* queryDescs, const cv::gpu::GpuMat* trainDescs,
   cv::gpu::GpuMat* trainIdx, cv::gpu::GpuMat* distance, 
   int k, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream);

//----------------------------------------------------------------------------
//
//  GpuCascadeClassifier
//
//----------------------------------------------------------------------------
CVAPI(cv::gpu::CascadeClassifier_GPU*) gpuCascadeClassifierCreate(const char* filename);

CVAPI(void) gpuCascadeClassifierRelease(cv::gpu::CascadeClassifier_GPU** classifier);

CVAPI(int) gpuCascadeClassifierDetectMultiScale(cv::gpu::CascadeClassifier_GPU* classifier, const cv::gpu::GpuMat* image, cv::gpu::GpuMat* objectsBuf, double scaleFactor, int minNeighbors, CvSize minSize, CvSeq* results);

//----------------------------------------------------------------------------
//
//  GpuHOGDescriptor
//
//----------------------------------------------------------------------------
CVAPI(void) gpuHOGDescriptorGetPeopleDetector64x128(std::vector<float>* vector);

CVAPI(void) gpuHOGDescriptorGetPeopleDetector48x96(std::vector<float>* vector);

CVAPI(cv::gpu::HOGDescriptor*) gpuHOGDescriptorCreateDefault();

CVAPI(cv::gpu::HOGDescriptor*) gpuHOGDescriptorCreate(
   cv::Size* _winSize, 
   cv::Size* _blockSize, 
   cv::Size* _blockStride,
   cv::Size* _cellSize, 
   int _nbins, 
   double _winSigma,
   double _L2HysThreshold, 
   bool _gammaCorrection, 
   int _nlevels);

CVAPI(void) gpuHOGSetSVMDetector(cv::gpu::HOGDescriptor* descriptor, std::vector<float>* vector);

CVAPI(void) gpuHOGDescriptorRelease(cv::gpu::HOGDescriptor** descriptor);

CVAPI(void) gpuHOGDescriptorDetectMultiScale(
   cv::gpu::HOGDescriptor* descriptor, 
   cv::gpu::GpuMat* img, 
   CvSeq* foundLocations,
   double hitThreshold, 
   CvSize winStride,
   CvSize padding, 
   double scale,
   int groupThreshold);

//----------------------------------------------------------------------------
//
//  Gpu Stereo
//
//----------------------------------------------------------------------------
CVAPI(cv::gpu::StereoBM_GPU*) GpuStereoBMCreate(int preset, int ndisparities, int winSize);

CVAPI(void) GpuStereoBMFindStereoCorrespondence(cv::gpu::StereoBM_GPU* stereo, const cv::gpu::GpuMat* left, const cv::gpu::GpuMat* right, cv::gpu::GpuMat* disparity, cv::gpu::Stream* stream);

CVAPI(void) GpuStereoBMRelease(cv::gpu::StereoBM_GPU** stereoBM);

CVAPI(cv::gpu::StereoConstantSpaceBP*) GpuStereoConstantSpaceBPCreate(int ndisp, int iters, int levels, int nr_plane);

CVAPI(void) GpuStereoConstantSpaceBPFindStereoCorrespondence(cv::gpu::StereoConstantSpaceBP* stereo, const cv::gpu::GpuMat* left, const cv::gpu::GpuMat* right, cv::gpu::GpuMat* disparity, cv::gpu::Stream* stream);

CVAPI(void) GpuStereoConstantSpaceBPRelease(cv::gpu::StereoConstantSpaceBP** stereoBM);

CVAPI(cv::gpu::DisparityBilateralFilter*) GpuDisparityBilateralFilterCreate(int ndisp, int radius, int iters, float edge_threshold, float max_disc_threshold, float sigma_range);

CVAPI(void) GpuDisparityBilateralFilterApply(cv::gpu::DisparityBilateralFilter* filter, const cv::gpu::GpuMat* disparity, const cv::gpu::GpuMat* image, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) GpuDisparityBilateralFilterRelease(cv::gpu::DisparityBilateralFilter** filter);

//----------------------------------------------------------------------------
//
//  GpuStream
//
//----------------------------------------------------------------------------
CVAPI(cv::gpu::Stream*) streamCreate();

CVAPI(void) streamRelease(cv::gpu::Stream** stream);

CVAPI(void) streamWaitForCompletion(cv::gpu::Stream* stream);

CVAPI(bool) streamQueryIfComplete(cv::gpu::Stream* stream);

CVAPI(void) streamEnqueueCopy(cv::gpu::Stream* stream, cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst);

//----------------------------------------------------------------------------
//
//  GpuBroxOpticalFlow 
//
//----------------------------------------------------------------------------

CVAPI(cv::gpu::BroxOpticalFlow*) gpuBroxOpticalFlowCreate(float alpha, float gamma, float scaleFactor, int innerIterations, int outerIterations, int solverIterations);

CVAPI(void) gpuBroxOpticalFlowCompute(cv::gpu::BroxOpticalFlow* flow, cv::gpu::GpuMat* frame0, const cv::gpu::GpuMat* frame1, cv::gpu::GpuMat* u, cv::gpu::GpuMat* v, cv::gpu::Stream* stream);

CVAPI(void) gpuBroxOpticalFlowRelease(cv::gpu::BroxOpticalFlow** flow);

//----------------------------------------------------------------------------
//
//  GpuPyrLKOpticalFlow
//
//----------------------------------------------------------------------------
CVAPI(cv::gpu::PyrLKOpticalFlow*) gpuPyrLKOpticalFlowCreate(cv::Size winSize, int maxLevel, int iters, bool useInitialFlow);
CVAPI(void) gpuPryLKOpticalFlowSparse(
   cv::gpu::PyrLKOpticalFlow* flow, 
   const cv::gpu::GpuMat* prevImg, 
   const cv::gpu::GpuMat* nextImg, 
   const cv::gpu::GpuMat* prevPts, 
   cv::gpu::GpuMat* nextPts,
   cv::gpu::GpuMat* status, 
   cv::gpu::GpuMat* err);
CVAPI(void) gpuPyrLKOpticalFlowDense(
   cv::gpu::PyrLKOpticalFlow* flow, 
   const cv::gpu::GpuMat* prevImg, 
   const cv::gpu::GpuMat* nextImg,
   cv::gpu::GpuMat* u, 
   cv::gpu::GpuMat* v, 
   cv::gpu::GpuMat* err);
CVAPI(void) gpuPyrLKOpticalFlowRelease(cv::gpu::PyrLKOpticalFlow** flow);

//----------------------------------------------------------------------------
//
//  GpuFarnebackOpticalFlow
//
//----------------------------------------------------------------------------
CVAPI(cv::gpu::FarnebackOpticalFlow*) gpuFarnebackOpticalFlowCreate(    
   int numLevels,
   double pyrScale,
   bool fastPyramids,
   int winSize,
   int numIters,
   int polyN,
   double polySigma,
   int flags);

CVAPI(void) gpuFarnebackOpticalFlowCompute(cv::gpu::FarnebackOpticalFlow* flow, const cv::gpu::GpuMat* frame0, const cv::gpu::GpuMat* frame1, cv::gpu::GpuMat* u, cv::gpu::GpuMat* v, cv::gpu::Stream* stream);

CVAPI(void) gpuFarnebackOpticalFlowRelease(cv::gpu::FarnebackOpticalFlow** flow);

//----------------------------------------------------------------------------
//
//  GpuGoodFeaturesToTrackDetector
//
//----------------------------------------------------------------------------
CVAPI(cv::gpu::GoodFeaturesToTrackDetector_GPU*) gpuGoodFeaturesToTrackDetectorCreate(int maxCorners, double qualityLevel, double minDistance);
CVAPI(void) gpuGoodFeaturesToTrackDetectorDetect(cv::gpu::GoodFeaturesToTrackDetector_GPU* detector, const cv::gpu::GpuMat* image, cv::gpu::GpuMat* corners, const cv::gpu::GpuMat* mask);
CVAPI(void) gpuGoodFeaturesToTrackDetectorRelease(cv::gpu::GoodFeaturesToTrackDetector_GPU** detector);

//----------------------------------------------------------------------------
//
//  GpuFASTDetector
//
//----------------------------------------------------------------------------

CVAPI(cv::gpu::FAST_GPU*) gpuFASTDetectorCreate(int threshold, bool nonmaxSupression, double keypointsRatio);

CVAPI(void) gpuFASTDetectorRelease(cv::gpu::FAST_GPU** detector);

CVAPI(void) gpuFASTDetectorDetectKeyPoints(cv::gpu::FAST_GPU* detector, const cv::gpu::GpuMat* img, const cv::gpu::GpuMat* mask, cv::gpu::GpuMat* keypoints);

CVAPI(void) gpuFASTDownloadKeypoints(cv::gpu::FAST_GPU* detector, cv::gpu::GpuMat* keypointsGPU, std::vector<cv::KeyPoint>* keypoints);

//----------------------------------------------------------------------------
//
//  GpuORBDetector
//
//----------------------------------------------------------------------------

CVAPI(cv::gpu::ORB_GPU*) gpuORBDetectorCreate(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTA_K, int scoreType, int patchSize);

CVAPI(void) gpuORBDetectorRelease(cv::gpu::ORB_GPU** detector);

CVAPI(void) gpuORBDetectorDetectKeyPoints(cv::gpu::ORB_GPU* detector, const cv::gpu::GpuMat* img, const cv::gpu::GpuMat* mask, cv::gpu::GpuMat* keypoints);

CVAPI(void) gpuORBDownloadKeypoints(cv::gpu::ORB_GPU* detector, cv::gpu::GpuMat* keypointsGPU, std::vector<cv::KeyPoint>* keypoints);

CVAPI(void) gpuORBDetectorCompute(
   cv::gpu::ORB_GPU* detector, 
   const cv::gpu::GpuMat* img, 
   const cv::gpu::GpuMat* mask, 
   cv::gpu::GpuMat* keypoints, 
   cv::gpu::GpuMat* descriptors);

CVAPI(int) gpuORBDetectorGetDescriptorSize(cv::gpu::ORB_GPU* detector);

//----------------------------------------------------------------------------
//
//  Utilities
//
//----------------------------------------------------------------------------
CVAPI(void) gpuCreateOpticalFlowNeedleMap(const cv::gpu::GpuMat* u, const cv::gpu::GpuMat* v, cv::gpu::GpuMat* vertex, cv::gpu::GpuMat* colors);

//----------------------------------------------------------------------------
//
//  GpuMatchTemplateBuf
//
//----------------------------------------------------------------------------
CVAPI(cv::gpu::MatchTemplateBuf*) gpuMatchTemplateBufCreate();
CVAPI(void) gpuMatchTemplateBufRelease(cv::gpu::MatchTemplateBuf** buffer);

//----------------------------------------------------------------------------
//
//  MOG2 GPU
//
//----------------------------------------------------------------------------
CVAPI(cv::gpu::MOG2_GPU*) gpuMog2Create(int nMixtures);
CVAPI(void) gpuMog2Compute(cv::gpu::MOG2_GPU* mog, cv::gpu::GpuMat* frame, float learningRate, cv::gpu::GpuMat* fgMask, cv::gpu::Stream* stream);
CVAPI(void) gpuMog2Release(cv::gpu::MOG2_GPU** mog);
#endif