//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CUDA_C_H
#define EMGU_CUDA_C_H

#include "opencv2/cuda.hpp"
#include "opencv2/core/cuda.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "emgu_c.h"

//----------------------------------------------------------------------------
//
//  Gpu Device Info
//
//----------------------------------------------------------------------------

CVAPI(int) cudaGetCudaEnabledDeviceCount();

CVAPI(void) cudaSetDevice(int deviceId);

CVAPI(int) cudaGetDevice();

CVAPI(cv::cuda::DeviceInfo*) cudaDeviceInfoCreate(int* deviceId);

CVAPI(void) cudaDeviceInfoRelease(cv::cuda::DeviceInfo** di);

CVAPI(void) cudaDeviceInfoDeviceName(cv::cuda::DeviceInfo* device, char* name, int maxSizeInBytes);

CVAPI(void) cudaDeviceInfoComputeCapability(cv::cuda::DeviceInfo* device, int* major, int* minor);

CVAPI(int) cudaDeviceInfoMultiProcessorCount(cv::cuda::DeviceInfo* device);

CVAPI(void) cudaDeviceInfoFreeMemInfo(cv::cuda::DeviceInfo* info, size_t* free);

CVAPI(void) cudaDeviceInfoTotalMemInfo(cv::cuda::DeviceInfo* info, size_t* total);

CVAPI(bool) cudaDeviceInfoSupports(cv::cuda::DeviceInfo* device, cv::cuda::FeatureSet feature);

CVAPI(bool) cudaDeviceInfoIsCompatible(cv::cuda::DeviceInfo* device);

//----------------------------------------------------------------------------
//
//  Gpu Module Info
//
//----------------------------------------------------------------------------

CVAPI(bool) targetArchsBuildWith(cv::cuda::FeatureSet featureSet);

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

CVAPI(cv::cuda::GpuMat*) gpuMatCreateDefault();

CVAPI(cv::cuda::GpuMat*) gpuMatCreate(int rows, int cols, int type);

CVAPI(cv::cuda::GpuMat*) gpuMatCreateContinuous(int rows, int cols, int type);

CVAPI(bool) gpuMatIsContinuous(cv::cuda::GpuMat* gpuMat);

CVAPI(cv::cuda::GpuMat*) gpuMatGetRegion(cv::cuda::GpuMat* other, CvSlice rowRange, CvSlice colRange);

CVAPI(void) gpuMatRelease(cv::cuda::GpuMat** mat);

CVAPI(cv::cuda::GpuMat*) gpuMatCreateFromArr(CvArr* arr);

CVAPI(emgu::size) gpuMatGetSize(cv::cuda::GpuMat* gpuMat);

CVAPI(bool) gpuMatIsEmpty(cv::cuda::GpuMat* gpuMat);

CVAPI(int) gpuMatGetChannels(cv::cuda::GpuMat* gpuMat);

CVAPI(int) gpuMatGetType(cv::cuda::GpuMat* gpuMat);

CVAPI(void) gpuMatUpload(cv::cuda::GpuMat* gpuMat, CvArr* arr);

CVAPI(void) gpuMatDownload(cv::cuda::GpuMat* gpuMat, CvArr* arr);

CVAPI(void) gpuMatConvertTo(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, double alpha, double beta, cv::cuda::Stream* stream);

CVAPI(void) gpuMatCopy(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream);

CVAPI(void) gpuMatSetTo(cv::cuda::GpuMat* mat, const CvScalar s, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream);


CVAPI(void) gpuMatReshape(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int cn, int rows);


CVAPI(cv::cuda::GpuMat*) gpuMatGetSubRect(const cv::cuda::GpuMat* arr, CvRect rect);


//----------------------------------------------------------------------------
//
//  CudaCascadeClassifier
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::CascadeClassifier_CUDA*) cudaCascadeClassifierCreate(const char* filename);

CVAPI(void) cudaCascadeClassifierRelease(cv::cuda::CascadeClassifier_CUDA** classifier);

CVAPI(int) cudaCascadeClassifierDetectMultiScale(cv::cuda::CascadeClassifier_CUDA* classifier, const cv::cuda::GpuMat* image, cv::cuda::GpuMat* objectsBuf, double scaleFactor, int minNeighbors, CvSize minSize, CvSeq* results);

//----------------------------------------------------------------------------
//
//  CudaHOGDescriptor
//
//----------------------------------------------------------------------------
CVAPI(void) cudaHOGDescriptorGetPeopleDetector64x128(std::vector<float>* vector);

CVAPI(void) cudaHOGDescriptorGetPeopleDetector48x96(std::vector<float>* vector);

CVAPI(cv::cuda::HOGDescriptor*) cudaHOGDescriptorCreateDefault();

CVAPI(cv::cuda::HOGDescriptor*) cudaHOGDescriptorCreate(
   cv::Size* _winSize, 
   cv::Size* _blockSize, 
   cv::Size* _blockStride,
   cv::Size* _cellSize, 
   int _nbins, 
   double _winSigma,
   double _L2HysThreshold, 
   bool _gammaCorrection, 
   int _nlevels);

CVAPI(void) cudaHOGSetSVMDetector(cv::cuda::HOGDescriptor* descriptor, std::vector<float>* vector);

CVAPI(void) cudaHOGDescriptorRelease(cv::cuda::HOGDescriptor** descriptor);

CVAPI(void) cudaHOGDescriptorDetectMultiScale(
   cv::cuda::HOGDescriptor* descriptor, 
   cv::cuda::GpuMat* img, 
   CvSeq* foundLocations,
   double hitThreshold, 
   CvSize winStride,
   CvSize padding, 
   double scale,
   int groupThreshold);

//----------------------------------------------------------------------------
//
//  Stream
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::Stream*) streamCreate();

CVAPI(void) streamRelease(cv::cuda::Stream** stream);

CVAPI(void) streamWaitForCompletion(cv::cuda::Stream* stream);

CVAPI(bool) streamQueryIfComplete(cv::cuda::Stream* stream);

//CVAPI(void) streamEnqueueCopy(cv::cuda::Stream* stream, cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst);




#endif