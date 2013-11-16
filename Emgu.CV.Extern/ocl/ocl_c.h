//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_OCL_C_H
#define EMGU_OCL_C_H

#include "opencv2/ocl/ocl.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "emgu_c.h"

CVAPI(int) oclGetPlatforms(std::vector<const cv::ocl::PlatformInfo*>* oclPlatforms);
CVAPI(int) oclGetDevices(std::vector<const cv::ocl::DeviceInfo*>* oclDevices, int deviceType, const cv::ocl::PlatformInfo* platform);
CVAPI(void) oclSetDevice(const cv::ocl::DeviceInfo* oclInfo);

CVAPI(void) oclFinish();

CVAPI(void) oclSetBinaryDiskCache(int mode, const char* path);
CVAPI(void) oclSetBinaryPath(const char *path);

//----------------------------------------------------------------------------
//
//  OclMat
//
//----------------------------------------------------------------------------

CVAPI(cv::ocl::oclMat*) oclMatCreateDefault();

CVAPI(cv::ocl::oclMat*) oclMatCreate(int rows, int cols, int type);

//CVAPI(cv::ocl::oclMat*) oclMatCreateContinuous(int rows, int cols, int type);

CVAPI(cv::ocl::oclMat*) oclMatCreateFromArr(CvArr* arr);

CVAPI(int) oclMatGetType(cv::ocl::oclMat* oclMat);

CVAPI(emgu::size) oclMatGetSize(cv::ocl::oclMat* oclMat);

CVAPI(emgu::size) oclMatGetWholeSize(cv::ocl::oclMat* oclMat);

CVAPI(bool) oclMatIsEmpty(cv::ocl::oclMat* oclMat);

CVAPI(bool) oclMatIsContinuous(cv::ocl::oclMat* oclMat);

CVAPI(int) oclMatGetChannels(cv::ocl::oclMat* oclMat);

CVAPI(void) oclMatRelease(cv::ocl::oclMat** mat);

//Pefroms blocking upload data to oclMat.
CVAPI(void) oclMatUpload(cv::ocl::oclMat* oclMat, CvArr* arr);

//Downloads data from device to host memory. Blocking calls.
CVAPI(void) oclMatDownload(cv::ocl::oclMat* oclMat, CvArr* arr);

CVAPI(int) oclCountNonZero(cv::ocl::oclMat* oclMat);

CVAPI(void) oclAdd(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask);

CVAPI(void) oclAddS(const cv::ocl::oclMat* a, const CvScalar scale, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask);

CVAPI(void) oclSubtract(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask);

CVAPI(void) oclSubtractS(const cv::ocl::oclMat* a, const CvScalar scale, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask);

CVAPI(void) oclMultiply(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, double scale);

CVAPI(void) oclMultiplyS(const cv::ocl::oclMat* a, const double s, cv::ocl::oclMat* c);

CVAPI(void) oclDivide(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, double scale);

//CVAPI(void) oclMatDivideSR(const cv::ocl::oclMat* a, const CvScalar s, cv::ocl::oclMat* c);

CVAPI(void) oclDivideSL(const double s, const cv::ocl::oclMat* b, cv::ocl::oclMat* c);

CVAPI(void) oclAddWeighted(const cv::ocl::oclMat* src1, double alpha, const cv::ocl::oclMat* src2, double beta, double gamma, cv::ocl::oclMat* dst);

CVAPI(void) oclAbsdiff(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c);

CVAPI(void) oclAbsdiffS(const cv::ocl::oclMat* a, const CvScalar s, cv::ocl::oclMat* c);

CVAPI(void) oclFlip(const cv::ocl::oclMat* a, cv::ocl::oclMat* b, int flipCode);

CVAPI(void) oclBitwiseNot(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst);

CVAPI(void) oclBitwiseAnd(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclBitwiseAndS(const cv::ocl::oclMat* src1, const cv::Scalar sc, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclBitwiseOr(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclBitwiseOrS(const cv::ocl::oclMat* src1, const cv::Scalar sc, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclBitwiseXor(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclBitwiseXorS(const cv::ocl::oclMat* src1, const cv::Scalar sc, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclErode( const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const CvArr* kernel, CvPoint anchor, int iterations, int borderType, CvScalar borderValue);

CVAPI(void) oclDilate( const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const CvArr* kernel, CvPoint anchor, int iterations, int borderType, CvScalar borderValue);

CVAPI(void) oclMorphologyEx( const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int op, const CvArr* kernel, CvPoint anchor, int iterations, int borderType, CvScalar borderValue);

CVAPI(void) oclCompare(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, int cmpop);

CVAPI(void) oclCvtColor(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int code);

CVAPI(void) oclMatCopy(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatSetTo(cv::ocl::oclMat* mat, const CvScalar s, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatResize(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, double fx, double fy, int interpolation);

//only support single channel oclMat
CVAPI(void) oclMinMaxLoc(const cv::ocl::oclMat* src, 
   double* minVal, double* maxVal, 
   CvPoint* minLoc, CvPoint* maxLoc, 
   const cv::ocl::oclMat* mask);

CVAPI(void) oclMatchTemplate(const cv::ocl::oclMat* image, const cv::ocl::oclMat* templ, cv::ocl::oclMat* result, int method, cv::ocl::MatchTemplateBuf* buffer);

CVAPI(void) oclPyrDown(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst);

CVAPI(void) oclPyrUp(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst);

CVAPI(void) oclSplit(const cv::ocl::oclMat* src, cv::ocl::oclMat** dst);

CVAPI(void) oclMerge(const cv::ocl::oclMat** src, cv::ocl::oclMat* dst);

CVAPI(void) oclConvertTo(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, double alpha, double beta);

CVAPI(void) oclFilter2D(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const CvArr* kernel, CvPoint anchor, int borderType);

CVAPI(void) oclReshape(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int cn, int rows);

CVAPI(void) oclSobel(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int dx, int dy, int ksize, double scale, int borderType);

CVAPI(void) oclScharr(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int dx, int dy, double scale, double delta, int borderType);

CVAPI(void) oclGaussianBlur(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, CvSize ksize, double sigma1, double sigma2, int borderType);

CVAPI(void) oclLaplacian(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int ksize, double scale);

CVAPI(void) oclGemm(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, double alpha, 
   const cv::ocl::oclMat* src3, double beta, cv::ocl::oclMat* dst, int flags);

CVAPI(void) oclCanny(const cv::ocl::oclMat* image, cv::ocl::oclMat* edges, double lowThreshold, double highThreshold, int apertureSize, bool L2gradient);

CVAPI(void) oclMeanStdDev(const cv::ocl::oclMat* mtx, CvScalar* mean, CvScalar* stddev);

CVAPI(double) oclNorm(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, int normType);

CVAPI(void) oclLUT(const cv::ocl::oclMat* src, const cv::ocl::oclMat* lut, cv::ocl::oclMat* dst);

CVAPI(void) oclCopyMakeBorder(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int top, int bottom, int left, int right, int borderType, const CvScalar value);

CVAPI(void) oclMedianFilter(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int m);

CVAPI(void) oclIntegral(const cv::ocl::oclMat* src, cv::ocl::oclMat* sum, cv::ocl::oclMat* sqrSum);

CVAPI(void) oclCornerHarris(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int blockSize, int ksize, double k, int borderType);

CVAPI(void) oclBilateralFilter(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int d, double sigmaColor, double sigmaSpave, int borderType);

CVAPI(void) oclAdaptiveBilateralFilter(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, CvSize* ksize, double sigmaSpace, CvPoint* anchor, int borderType);

CVAPI(void) oclPow(const cv::ocl::oclMat* x, double p, cv::ocl::oclMat *y);

CVAPI(void) oclExp(const cv::ocl::oclMat* a, cv::ocl::oclMat* b);

CVAPI(void) oclLog(const cv::ocl::oclMat* a, cv::ocl::oclMat* b);

CVAPI(void) oclCartToPolar(const cv::ocl::oclMat* x, const cv::ocl::oclMat* y, cv::ocl::oclMat* magnitude, cv::ocl::oclMat* angle, bool angleInDegrees);

CVAPI(void) oclPolarToCart(const cv::ocl::oclMat* magnitude, const cv::ocl::oclMat* angle, cv::ocl::oclMat* x, cv::ocl::oclMat* y, bool angleInDegrees);

CVAPI(void) oclCalcHist(const cv::ocl::oclMat* mat_src, cv::ocl::oclMat* mat_hist);

CVAPI(void) oclEqualizeHist(const cv::ocl::oclMat* mat_src, cv::ocl::oclMat* mat_dst);

CVAPI(void) oclHoughCircles(const cv::ocl::oclMat* src, cv::ocl::oclMat* circles, int method, float dp, float minDist, int cannyThreshold, int votesThreshold, int minRadius, int maxRadius, int maxCircles);

CVAPI(void) oclHoughCirclesDownload(const cv::ocl::oclMat* d_circles, cv::Mat* h_circles);

CVAPI(void) oclMeanShiftFiltering(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int sp, int sr,
   CvTermCriteria* criteria);

CVAPI(void) oclMeanShiftProc(const cv::ocl::oclMat* src, cv::ocl::oclMat* dstr, cv::ocl::oclMat* dstsp, int sp, int sr,
   CvTermCriteria* criteria);

CVAPI(void) oclMeanShiftSegmentation(const cv::ocl::oclMat* src, IplImage* dst, int sp, int sr, int minsize,
   CvTermCriteria* criteria);

CVAPI(void) oclWarpAffine(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const CvMat* M, int flags);

CVAPI(void) oclWarpPerspective(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const CvMat* M, int flags);

CVAPI(cv::ocl::oclMat*) oclMatGetSubRect(const cv::ocl::oclMat* arr, CvRect* rect);

CVAPI(cv::ocl::oclMat*) oclMatGetRegion(cv::ocl::oclMat* other, CvSlice* rowRange, CvSlice* colRange);

CVAPI(void) oclCLAHE(cv::ocl::oclMat* src, cv::ocl::oclMat* dst, double clipLimit, emgu::size* tileGridSize);

//----------------------------------------------------------------------------
//
//  OclHOGDescriptor
//
//----------------------------------------------------------------------------
CVAPI(void) oclHOGDescriptorGetPeopleDetector64x128(std::vector<float>* vector);

CVAPI(void) oclHOGDescriptorGetPeopleDetector48x96(std::vector<float>* vector);

CVAPI(cv::ocl::HOGDescriptor*) oclHOGDescriptorCreateDefault();

CVAPI(cv::ocl::HOGDescriptor*) oclHOGDescriptorCreate(
   cv::Size* _winSize, 
   cv::Size* _blockSize, 
   cv::Size* _blockStride,
   cv::Size* _cellSize, 
   int _nbins, 
   double _winSigma,
   double _L2HysThreshold, 
   bool _gammaCorrection, 
   int _nlevels);

CVAPI(void) oclHOGSetSVMDetector(cv::ocl::HOGDescriptor* descriptor, std::vector<float>* vector);

CVAPI(void) oclHOGDescriptorRelease(cv::ocl::HOGDescriptor** descriptor);

CVAPI(void) oclHOGDescriptorDetectMultiScale(
   cv::ocl::HOGDescriptor* descriptor, 
   cv::ocl::oclMat* img, 
   CvSeq* foundLocations,
   double hitThreshold, 
   CvSize winStride,
   CvSize padding, 
   double scale,
   int groupThreshold);

//----------------------------------------------------------------------------
//
//  GpuMatchTemplateBuf
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::MatchTemplateBuf*) oclMatchTemplateBufCreate();
CVAPI(void) oclMatchTemplateBufRelease(cv::ocl::MatchTemplateBuf** buffer);

//----------------------------------------------------------------------------
//
//  GpuCascadeClassifier
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::OclCascadeClassifier*) oclCascadeClassifierCreate(const char* filename);

CVAPI(void) oclCascadeClassifierRelease(cv::ocl::OclCascadeClassifier** classifier);

//----------------------------------------------------------------------------
//
//  oclPyrLKOpticalFlow
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::PyrLKOpticalFlow*) oclPyrLKOpticalFlowCreate(emgu::size winSize, int maxLevel, int iters, bool useInitialFlow);
CVAPI(void) oclPyrLKOpticalFlowSparse(
   cv::ocl::PyrLKOpticalFlow* flow, 
   const cv::ocl::oclMat* prevImg, 
   const cv::ocl::oclMat* nextImg, 
   const cv::ocl::oclMat* prevPts, 
   cv::ocl::oclMat* nextPts,
   cv::ocl::oclMat* status, 
   cv::ocl::oclMat* err);
CVAPI(void) oclPyrLKOpticalFlowDense(
   cv::ocl::PyrLKOpticalFlow* flow, 
   const cv::ocl::oclMat* prevImg, 
   const cv::ocl::oclMat* nextImg,
   cv::ocl::oclMat* u, 
   cv::ocl::oclMat* v, 
   cv::ocl::oclMat* err);
CVAPI(void) oclPyrLKOpticalFlowRelease(cv::ocl::PyrLKOpticalFlow** flow);

//----------------------------------------------------------------------------
//
//  OpticalFlowDual_TVL1_OCL
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::OpticalFlowDual_TVL1_OCL*)  oclOpticalFlowDualTVL1Create();
CVAPI(void) oclOpticalFlowDualTVL1Compute(cv::ocl::OpticalFlowDual_TVL1_OCL* flow, cv::ocl::oclMat* i0, cv::ocl::oclMat* i1, cv::ocl::oclMat* flowx, cv::ocl::oclMat* flowy);
CVAPI(void) oclOpticalFlowDualTVL1Release(cv::ocl::OpticalFlowDual_TVL1_OCL** flow);

//----------------------------------------------------------------------------
//
//  Ocl Stereo
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::StereoBM_OCL*) oclStereoBMCreate(int preset, int ndisparities, int winSize);

CVAPI(void) oclStereoBMFindStereoCorrespondence(cv::ocl::StereoBM_OCL* stereo, const cv::ocl::oclMat* left, const cv::ocl::oclMat* right, cv::ocl::oclMat* disparity);

CVAPI(void) oclStereoBMRelease(cv::ocl::StereoBM_OCL** stereoBM);

CVAPI(cv::ocl::StereoConstantSpaceBP*) oclStereoConstantSpaceBPCreate(int ndisp, int iters, int levels, int nr_plane);

CVAPI(void) oclStereoConstantSpaceBPFindStereoCorrespondence(cv::ocl::StereoConstantSpaceBP* stereo, const cv::ocl::oclMat* left, const cv::ocl::oclMat* right, cv::ocl::oclMat* disparity);

CVAPI(void) oclStereoConstantSpaceBPRelease(cv::ocl::StereoConstantSpaceBP** stereoBM);

//----------------------------------------------------------------------------
//
//  OclBruteForceMatcher
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::BFMatcher_OCL*) oclBruteForceMatcherCreate(int distType);

CVAPI(void) oclBruteForceMatcherRelease(cv::ocl::BFMatcher_OCL** matcher);

CVAPI(void) oclBruteForceMatcherAdd(cv::ocl::BFMatcher_OCL* matcher, const cv::ocl::oclMat* trainDescs);

CVAPI(void) oclBruteForceMatcherKnnMatchSingle(
   cv::ocl::BFMatcher_OCL* matcher,
   const cv::ocl::oclMat* queryDescs, const cv::ocl::oclMat* trainDescs,
   cv::ocl::oclMat* trainIdx, cv::ocl::oclMat* distance, 
   int k, const cv::ocl::oclMat* mask);

//----------------------------------------------------------------------------
//
//  Vector of VectorOfOclPlatformInfo
//
//----------------------------------------------------------------------------
CVAPI(std::vector<const cv::ocl::PlatformInfo*>*) VectorOfOclPlatformInfoCreate();

CVAPI(std::vector<const cv::ocl::PlatformInfo*>*) VectorOfOclPlatformInfoCreateSize(int size);

CVAPI(int) VectorOfOclPlatformInfoGetSize(std::vector<const cv::ocl::PlatformInfo*>* v);

CVAPI(void) VectorOfOclPlatformInfoClear(std::vector<const cv::ocl::PlatformInfo*>* v);

CVAPI(void) VectorOfOclPlatformInfoRelease(std::vector<const cv::ocl::PlatformInfo*>* v);

CVAPI(const cv::ocl::PlatformInfo*) VectorOfOclPlatformInfoGetStartAddress(std::vector<const cv::ocl::PlatformInfo*>* v);

CVAPI(const cv::ocl::PlatformInfo*) VectorOfOclPlatformInfoGetItem(std::vector<const cv::ocl::PlatformInfo*>* v, int index);

//----------------------------------------------------------------------------
//
//  Vector of VectorOfOclDeviceInfo
//
//----------------------------------------------------------------------------
CVAPI(std::vector<const cv::ocl::DeviceInfo*>*) VectorOfOclDeviceInfoCreate();

CVAPI(std::vector<const cv::ocl::DeviceInfo*>*) VectorOfOclDeviceInfoCreateSize(int size);

CVAPI(int) VectorOfOclDeviceInfoGetSize(std::vector<const cv::ocl::DeviceInfo*>* v);

CVAPI(void) VectorOfOclDeviceInfoClear(std::vector<const cv::ocl::DeviceInfo*>* v);

CVAPI(void) VectorOfOclDeviceInfoRelease(std::vector<const cv::ocl::DeviceInfo*>* v);

CVAPI(const cv::ocl::DeviceInfo*) VectorOfOclDeviceInfoGetStartAddress(std::vector<const cv::ocl::DeviceInfo*>* v);

CVAPI(const cv::ocl::DeviceInfo*) VectorOfOclDeviceInfoGetItem(std::vector<const cv::ocl::DeviceInfo*>* v, int index);

//----------------------------------------------------------------------------
//
//  OclPlatformInfo
//
//----------------------------------------------------------------------------
CVAPI(void) oclPlatformInfoGetProperties(
   cv::ocl::PlatformInfo* oclPlatformInfo,
   const char** platformProfile,
   const char** platformVersion,
   const char** platformName,
   const char** platformVendor,
   const char** platformExtensions,

   int* platformVersionMajor,
   int* platformVersionMinor
   );

CVAPI(std::vector<const cv::ocl::DeviceInfo*>* ) oclPlatformInfoGetDevices(cv::ocl::PlatformInfo* oclPlatformInfo);

//----------------------------------------------------------------------------
//
//  OclDeviceInfo
//
//----------------------------------------------------------------------------


CVAPI(void) oclDeviceInfoGetProperty(cv::ocl::DeviceInfo* oclDeviceInfo, 
   int* type, 
   const char** profile, 
   const char** version,
   const char** name, 
   const char** vendor, 
   int* vendorId,
   const char** driverVersion, 
   const char** extensions,
   
   int* maxWorkGroupSize,
   int* maxComputeUnits,
   int* localMemorySize,
   int* maxMemAllocSize,
   int* deviceVersionMajor,
   int* deviceVersionMinor,
   int* haveDoubleSupport,
   int* isUnifiedMemory,
   const char** compilationExtraOptions
   );

CVAPI(const cv::ocl::PlatformInfo*) oclDeviceInfoGetPlatform(cv::ocl::DeviceInfo* oclDeviceInfo);
#endif