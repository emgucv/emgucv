//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CORE_CUDA_C_H
#define EMGU_CORE_CUDA_C_H

//#include "opencv2/cuda.hpp"
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

CVAPI(void) gpuMatCreate(cv::cuda::GpuMat* m, int rows, int cols, int type);

CVAPI(cv::cuda::GpuMat*) gpuMatCreateContinuous(int rows, int cols, int type);

CVAPI(bool) gpuMatIsContinuous(cv::cuda::GpuMat* gpuMat);

CVAPI(cv::cuda::GpuMat*) gpuMatGetRegion(cv::cuda::GpuMat* other, cv::Range* rowRange, cv::Range* colRange);

CVAPI(void) gpuMatRelease(cv::cuda::GpuMat** mat);

CVAPI(cv::cuda::GpuMat*) gpuMatCreateFromInputArray(cv::_InputArray* arr);

CVAPI(void) gpuMatGetSize(cv::cuda::GpuMat* gpuMat, CvSize* size);

CVAPI(bool) gpuMatIsEmpty(cv::cuda::GpuMat* gpuMat);

CVAPI(int) gpuMatGetChannels(cv::cuda::GpuMat* gpuMat);

CVAPI(int) gpuMatGetType(cv::cuda::GpuMat* gpuMat);

CVAPI(int) gpuMatGetDepth(cv::cuda::GpuMat* gpuMat);

CVAPI(void) gpuMatUpload(cv::cuda::GpuMat* gpuMat, cv::_InputArray* arr, cv::cuda::Stream* stream);

CVAPI(void) gpuMatDownload(cv::cuda::GpuMat* gpuMat, cv::_OutputArray* arr, cv::cuda::Stream* stream);

CVAPI(void) gpuMatConvertTo(const cv::cuda::GpuMat* src, cv::_OutputArray* dst, int rtype, double alpha, double beta, cv::cuda::Stream* stream);

CVAPI(void) gpuMatCopyTo(const cv::cuda::GpuMat* src, cv::_OutputArray* dst, const cv::_InputArray* mask, cv::cuda::Stream* stream);

CVAPI(void) gpuMatSetTo(cv::cuda::GpuMat* mat, const CvScalar* s, cv::_InputArray* mask, cv::cuda::Stream* stream);

CVAPI(void) gpuMatReshape(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int cn, int rows);

CVAPI(cv::cuda::GpuMat*) gpuMatGetSubRect(const cv::cuda::GpuMat* arr, CvRect* rect);

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