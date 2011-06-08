//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_GPU_C_H
#define EMGU_GPU_C_H

#include "opencv2/gpu/gpu.hpp"

//----------------------------------------------------------------------------
//
//  GpuMat
//
//----------------------------------------------------------------------------
CVAPI(int) gpuGetCudaEnabledDeviceCount();

CVAPI(int) gpuGetDevice();

CVAPI(cv::gpu::DeviceInfo*) gpuDeviceInfoCreate(int* deviceId);

CVAPI(void) gpuDeviceInfoRelease(cv::gpu::DeviceInfo** di);

CVAPI(void) gpuDeviceInfoDeviceName(cv::gpu::DeviceInfo* device, char* name, int maxSizeInBytes);

CVAPI(void) gpuDeviceInfoComputeCapability(cv::gpu::DeviceInfo* device, int* major, int* minor);

CVAPI(int) gpuDeviceInfoMultiProcessorCount(cv::gpu::DeviceInfo* device);

CVAPI(void) gpuDeviceInfoFreeMemInfo(cv::gpu::DeviceInfo* info, size_t* free);

CVAPI(void) gpuDeviceInfoTotalMemInfo(cv::gpu::DeviceInfo* info, size_t* total);

CVAPI(bool) gpuDeviceInfoSupports(cv::gpu::DeviceInfo* device, cv::gpu::FeatureSet feature);

CVAPI(cv::gpu::GpuMat*) gpuMatCreateDefault();

CVAPI(cv::gpu::GpuMat*) gpuMatCreate(int rows, int cols, int type);

CVAPI(cv::gpu::GpuMat*) gpuMatCreateContinuous(int rows, int cols, int type);

CVAPI(bool) gpuMatIsContinuous(cv::gpu::GpuMat* gpuMat);

CVAPI(cv::gpu::GpuMat*) gpuMatGetRegion(cv::gpu::GpuMat* other, CvSlice rowRange, CvSlice colRange);

CVAPI(void) gpuMatRelease(cv::gpu::GpuMat** mat);

CVAPI(cv::gpu::GpuMat*) gpuMatCreateFromArr(CvArr* arr);

CVAPI(CvSize) gpuMatGetSize(cv::gpu::GpuMat* gpuMat, cv::Size* size);

CVAPI(bool) gpuMatIsEmpty(cv::gpu::GpuMat* gpuMat);

CVAPI(int) gpuMatGetChannels(cv::gpu::GpuMat* gpuMat);

CVAPI(void) gpuMatUpload(cv::gpu::GpuMat* gpuMat, CvArr* arr);

CVAPI(void) gpuMatDownload(cv::gpu::GpuMat* gpuMat, CvArr* arr);

CVAPI(void) gpuMatAdd(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatAddS(const cv::gpu::GpuMat* a, const CvScalar scale, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatSubtract(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatSubtractS(const cv::gpu::GpuMat* a, const CvScalar scale, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMultiply(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMultiplyS(const cv::gpu::GpuMat* a, const CvScalar s, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatDivide(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatDivideS(const cv::gpu::GpuMat* a, const CvScalar s, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatAbsdiff(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatAbsdiffS(const cv::gpu::GpuMat* a, const CvScalar s, cv::gpu::GpuMat* c, cv::gpu::Stream* stream);

CVAPI(void) gpuMatCompare(const cv::gpu::GpuMat* a, const cv::gpu::GpuMat* b, cv::gpu::GpuMat* c, int cmpop, cv::gpu::Stream* stream);

CVAPI(double) gpuMatThreshold(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, double thresh, double maxval, int type, cv::gpu::Stream* stream);

CVAPI(void) gpuMatCvtColor(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int code, const cv::gpu::Stream* stream);

CVAPI(void) gpuMatConvertTo(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, double alpha, double beta, cv::gpu::Stream* stream);

CVAPI(void) gpuMatCopy(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask);

CVAPI(void) gpuMatSetTo(cv::gpu::GpuMat* mat, const CvScalar s, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream);

CVAPI(void) gpuMatResize(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int interpolation, cv::gpu::Stream* stream);

CVAPI(cv::gpu::GpuMat*) gpuMatReshape(const cv::gpu::GpuMat* src, int cn, int rows);

CVAPI(void) gpuMatFlip(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int flipcode, cv::gpu::Stream* stream);

CVAPI(void) gpuMatSplit(const cv::gpu::GpuMat* src, cv::gpu::GpuMat** dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatExp(const cv::gpu::GpuMat* a, cv::gpu::GpuMat* b, cv::gpu::Stream* stream);

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

CVAPI(void) gpuMatMatchTemplate(const cv::gpu::GpuMat* image, const cv::gpu::GpuMat* templ, cv::gpu::GpuMat* result, int method);

CVAPI(void) gpuMatMeanStdDev(const cv::gpu::GpuMat* mtx, CvScalar* mean, CvScalar* stddev);

CVAPI(double) gpuMatNorm(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, int normType);

CVAPI(int) gpuMatCountNonZero(const cv::gpu::GpuMat* src);

CVAPI(void) gpuMatLUT(const cv::gpu::GpuMat* src, const CvArr* lut, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatFilter2D(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const CvArr* kernel, CvPoint anchor);

CVAPI(void) gpuMatBitwiseNot(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream);

CVAPI(void) gpuMatBitwiseAnd(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream);

CVAPI(void) gpuMatBitwiseOr(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream);

CVAPI(void) gpuMatBitwiseXor(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* mask, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMin(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMinS(const cv::gpu::GpuMat* src1, double src2, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMax(const cv::gpu::GpuMat* src1, const cv::gpu::GpuMat* src2, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatMaxS(const cv::gpu::GpuMat* src1, double src2, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream);

CVAPI(void) gpuMatSobel(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int dx, int dy, int ksize, double scale);

CVAPI(void) gpuMatGaussianBlur(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, CvSize ksize, double sigma1, double sigma2);

CVAPI(void) gpuMatLaplacian(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int ksize, double scale);

CVAPI(void) gpuMatErode( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, CvArr* kernel, CvPoint anchor, int iterations, cv::gpu::Stream* stream);

CVAPI(void) gpuMatDilate( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, CvArr* kernel, CvPoint anchor, int iterations, cv::gpu::Stream* stream);

CVAPI(void) gpuMatWarpAffine( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst,  const CvArr* M, int flags, cv::gpu::Stream* stream);

CVAPI(void) gpuMatWarpPerspective( const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst,  const CvArr* M, int flags, cv::gpu::Stream* stream);

CVAPI(void) gpuMatRemap(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, const cv::gpu::GpuMat* xmap, const cv::gpu::GpuMat* ymap);

CVAPI(void) gpuMatMeanShiftFiltering(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int sp, int sr,
            CvTermCriteria criteria);

CVAPI(void) gpuMatMeanShiftProc(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dstr, cv::gpu::GpuMat* dstsp, int sp, int sr,
            CvTermCriteria criteria);

CVAPI(void) gpuMatMeanShiftSegmentation(const cv::gpu::GpuMat* src, cv::Mat* dst, int sp, int sr, int minsize,
            CvTermCriteria criteria);

CVAPI(cv::gpu::GpuMat*) gpuMatHistEven(const cv::gpu::GpuMat* src, int histSize, int lowerLevel, int upperLevel);

CVAPI(cv::gpu::GpuMat*) gpuMatGetSubRect(const cv::gpu::GpuMat* arr, CvRect rect);

CVAPI(void) gpuMatIntegral(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* sum, cv::gpu::GpuMat* sqsum, cv::gpu::Stream* stream);

CVAPI(void) gpuMatCornerHarris(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int blockSize, int ksize, double k, int borderType);

CVAPI(void) gpuMatDft(const cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst, int flags);
/*
CVAPI(void) gpuMatCanny(const cv::gpu::GpuMat* image, cv::gpu::GpuMat* edges, double threshold1, double threshold2, int apertureSize)
{
   cv::gpu::Canny(*image, *edges, threshold1, threshold2, apertureSize);
}*/

//----------------------------------------------------------------------------
//
//  GpuBruteForceMatcher
//
//----------------------------------------------------------------------------
CVAPI(cv::gpu::BruteForceMatcher_GPU_base*) gpuBruteForceMatcherCreate(cv::gpu::BruteForceMatcher_GPU_base::DistType distType);

CVAPI(void) gpuBruteForceMatcherRelease(cv::gpu::BruteForceMatcher_GPU_base** matcher);

CVAPI(void) gpuBruteForceMatcherAdd(cv::gpu::BruteForceMatcher_GPU_base* matcher, const cv::gpu::GpuMat* trainDescs);

CVAPI(void) gpuBruteForceMatcherKnnMatch(
   cv::gpu::BruteForceMatcher_GPU_base* matcher,
   const cv::gpu::GpuMat* queryDescs, const cv::gpu::GpuMat* trainDescs,
   cv::gpu::GpuMat* trainIdx, cv::gpu::GpuMat* distance, 
   int k, const cv::gpu::GpuMat* mask);

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
CVAPI(void) gpuHOGDescriptorPeopleDetectorCreate(CvSeq* seq);

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

CVAPI(void) gpuHOGDescriptorRelease(cv::gpu::HOGDescriptor* descriptor);

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
//  GpuSURFDetector
//
//----------------------------------------------------------------------------

CVAPI(cv::gpu::SURF_GPU*) gpuSURFDetectorCreate(double _hessianThreshold, int _nOctaves, int _nOctaveLayers, bool _extended, float _keypointsRatio, bool _upright);

CVAPI(void) gpuSURFDetectorRelease(cv::gpu::SURF_GPU** detector);

CVAPI(void) gpuSURFDetectorDetectKeyPoints(cv::gpu::SURF_GPU* detector, const cv::gpu::GpuMat* img, const cv::gpu::GpuMat* mask, cv::gpu::GpuMat* keypoints);

CVAPI(void) gpuDownloadKeypoints(cv::gpu::SURF_GPU* detector, const cv::gpu::GpuMat* keypointsGPU, std::vector<cv::KeyPoint>* keypoints);

CVAPI(void) gpuUploadKeypoints(cv::gpu::SURF_GPU* detector, const std::vector<cv::KeyPoint>* keypoints, cv::gpu::GpuMat* keypointsGPU);

CVAPI(void) gpuSURFDetectorCompute(
   cv::gpu::SURF_GPU* detector, 
   const cv::gpu::GpuMat* img, 
   const cv::gpu::GpuMat* mask, 
   cv::gpu::GpuMat* keypoints, 
   cv::gpu::GpuMat* descriptors, 
   bool useProvidedKeypoints);

CVAPI(int) gpuSURFDetectorGetDescriptorSize(cv::gpu::SURF_GPU* detector);

#endif