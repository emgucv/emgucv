//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "core_cuda_c.h"

/*
#if !defined (HAVE_CUDA) || defined (CUDA_DISABLER)
void cv::cuda::matchTemplate(const GpuMat&, const GpuMat&, GpuMat&, int, MatchTemplateBuf&, Stream&)
{
CV_Error(CV_GpuNotSupported, "The library is compiled without GPU support");
}
#endif
*/

//----------------------------------------------------------------------------
//
//  Cuda Device Info
//
//----------------------------------------------------------------------------

int cudaGetCudaEnabledDeviceCount()
{
	return cv::cuda::getCudaEnabledDeviceCount();
}

void cudaSetDevice(int deviceId)
{
	cv::cuda::setDevice(deviceId);
}

int cudaGetDevice()
{
	return cv::cuda::getDevice();
}

void cudaResetDevice()
{
	cv::cuda::resetDevice();
}

cv::cuda::DeviceInfo* cudaDeviceInfoCreate(int* deviceId)
{
	if (*deviceId < 0)
		*deviceId = cv::cuda::getDevice();

	return new cv::cuda::DeviceInfo(*deviceId);
}

void cudaDeviceInfoRelease(cv::cuda::DeviceInfo** di)
{
	delete* di;
	*di = 0;
}

void cudaDeviceInfoDeviceName(cv::cuda::DeviceInfo* device, char* name, int maxSizeInBytes)
{
	std::string dName = device->name();
	strncpy(name, dName.c_str(), maxSizeInBytes);
}

void cudaDeviceInfoComputeCapability(cv::cuda::DeviceInfo* device, int* major, int* minor)
{
	*major = device->majorVersion();
	*minor = device->minorVersion();
}

int cudaDeviceInfoMultiProcessorCount(cv::cuda::DeviceInfo* device)
{
	return device->multiProcessorCount();
}

void cudaDeviceInfoFreeMemInfo(cv::cuda::DeviceInfo* info, size_t* free)
{
	*free = info->freeMemory();
}

void cudaDeviceInfoTotalMemInfo(cv::cuda::DeviceInfo* info, size_t* total)
{
	*total = info->totalMemory();
}

bool cudaDeviceInfoSupports(cv::cuda::DeviceInfo* device, cv::cuda::FeatureSet feature)
{
	return device->supports(feature);
}

bool cudaDeviceInfoIsCompatible(cv::cuda::DeviceInfo* device)
{
	return device->isCompatible();
}

void cudaPrintCudaDeviceInfo(int device)
{
	cv::cuda::printCudaDeviceInfo(device);
}

void cudaPrintShortCudaDeviceInfo(int device)
{
	cv::cuda::printShortCudaDeviceInfo(device);
}

void cudaConvertFp16(cv::_InputArray* src, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDEV
	cv::cuda::convertFp16(*src, *dst, *stream ? *stream : cv::cuda::Stream::Null());
#else
	CV_Error(CV_GpuNotSupported, "The library is compiled without CUDEV support");
#endif
}

//----------------------------------------------------------------------------
//
//  Gpu Module Info
//
//----------------------------------------------------------------------------

bool targetArchsBuildWith(cv::cuda::FeatureSet featureSet)
{
	return cv::cuda::TargetArchs::builtWith(featureSet);
}

bool targetArchsHas(int major, int minor)
{
	return cv::cuda::TargetArchs::has(major, minor);
}

bool targetArchsHasPtx(int major, int minor)
{
	return cv::cuda::TargetArchs::hasPtx(major, minor);
}

bool targetArchsHasBin(int major, int minor)
{
	return cv::cuda::TargetArchs::hasBin(major, minor);
}

bool targetArchsHasEqualOrLessPtx(int major, int minor)
{
	return cv::cuda::TargetArchs::hasBin(major, minor);
}

bool targetArchsHasEqualOrGreater(int major, int minor)
{
	return cv::cuda::TargetArchs::hasEqualOrGreater(major, minor);
}

bool targetArchsHasEqualOrGreaterPtx(int major, int minor)
{
	return cv::cuda::TargetArchs::hasEqualOrGreaterPtx(major, minor);
}

bool targetArchsHasEqualOrGreaterBin(int major, int minor)
{
	return cv::cuda::TargetArchs::hasEqualOrGreaterBin(major, minor);
}

//----------------------------------------------------------------------------
//
//  GpuMat
//
//----------------------------------------------------------------------------

cv::cuda::GpuMat* gpuMatCreateDefault() { return new cv::cuda::GpuMat(); }

void gpuMatCreate(cv::cuda::GpuMat* m, int rows, int cols, int type)
{
	m->create(rows, cols, type);
}

cv::cuda::GpuMat* gpuMatCreateContinuous(int rows, int cols, int type)
{
	cv::cuda::GpuMat* result = new cv::cuda::GpuMat();
	cv::cuda::createContinuous(rows, cols, type, *result);
	return result;
}

bool gpuMatIsContinuous(cv::cuda::GpuMat* gpuMat)
{
	return gpuMat->isContinuous();
}

cv::cuda::GpuMat* gpuMatGetRegion(cv::cuda::GpuMat* other, cv::Range* rowRange, cv::Range* colRange)
{
	return new cv::cuda::GpuMat(*other, *rowRange, *colRange);
}

void gpuMatRelease(cv::cuda::GpuMat** mat)
{
	delete* mat;
	*mat = 0;
}

cv::cuda::GpuMat* gpuMatCreateFromInputArray(cv::_InputArray* arr)
{
	return new cv::cuda::GpuMat(*arr);
}

void gpuMatGetSize(cv::cuda::GpuMat* gpuMat, CvSize* size)
{
	cv::Size s = gpuMat->size();
	size->width = s.width;
	size->height = s.height;
}

bool gpuMatIsEmpty(cv::cuda::GpuMat* gpuMat)
{
	return gpuMat->empty();
}

int gpuMatGetChannels(cv::cuda::GpuMat* gpuMat)
{
	return gpuMat->channels();
}

int gpuMatGetType(cv::cuda::GpuMat* gpuMat)
{
	return gpuMat->type();
}

int gpuMatGetDepth(cv::cuda::GpuMat* gpuMat)
{
	return gpuMat->depth();
}

void gpuMatUpload(cv::cuda::GpuMat* gpuMat, cv::_InputArray* arr, cv::cuda::Stream* stream)
{
	if (stream)
		gpuMat->upload(*arr, *stream);
	else
		gpuMat->upload(*arr);
}

void gpuMatDownload(cv::cuda::GpuMat* gpuMat, cv::_OutputArray* arr, cv::cuda::Stream* stream)
{
	if (stream)
		gpuMat->download(*arr, *stream);
	else
		gpuMat->download(*arr);
}


void gpuMatConvertTo(const cv::cuda::GpuMat* src, cv::_OutputArray* dst, int rtype, double alpha, double beta, cv::cuda::Stream* stream)
{
	src->convertTo(*dst, rtype, alpha, beta, stream ? *stream : cv::cuda::Stream::Null());
}

void gpuMatCopyTo(const cv::cuda::GpuMat* src, cv::_OutputArray* dst, const cv::_InputArray* mask, cv::cuda::Stream* stream)
{
	if (mask)
		src->copyTo(*dst, *mask, stream ? *stream : cv::cuda::Stream::Null());
	else
		src->copyTo(*dst, stream ? *stream : cv::cuda::Stream::Null());
}

void gpuMatSetTo(cv::cuda::GpuMat* mat, const CvScalar* s, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
	if (mask)
		mat->setTo(*s, *mask, stream ? *stream : cv::cuda::Stream::Null());
	else
		mat->setTo(*s, stream ? *stream : cv::cuda::Stream::Null());
}

CVAPI(void) gpuMatReshape(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int cn, int rows)
{
	cv::cuda::GpuMat tmp = src->reshape(cn, rows);
	dst->swap(tmp);
}

cv::cuda::GpuMat* gpuMatGetSubRect(const cv::cuda::GpuMat* arr, CvRect* rect)
{
	return new cv::cuda::GpuMat(*arr, *rect);
}
