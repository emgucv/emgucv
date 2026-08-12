//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
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
	try
	{
		return cv::cuda::getCudaEnabledDeviceCount();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cudaSetDevice(int deviceId)
{
	try
	{
		cv::cuda::setDevice(deviceId);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

int cudaGetDevice()
{
	try
	{
		return cv::cuda::getDevice();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cudaResetDevice()
{
	try
	{
		cv::cuda::resetDevice();
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::cuda::DeviceInfo* cudaDeviceInfoCreate(int* deviceId)
{
	try
	{
		if (*deviceId < 0)
			*deviceId = cv::cuda::getDevice();

		return new cv::cuda::DeviceInfo(*deviceId);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cudaDeviceInfoRelease(cv::cuda::DeviceInfo** di)
{
	delete* di;
	*di = 0;
}

void cudaDeviceInfoDeviceName(cv::cuda::DeviceInfo* device, char* name, int maxSizeInBytes)
{
	try
	{
		std::string dName = device->name();
		strncpy(name, dName.c_str(), maxSizeInBytes);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaDeviceInfoComputeCapability(cv::cuda::DeviceInfo* device, int* major, int* minor)
{
	try
	{
		*major = device->majorVersion();
		*minor = device->minorVersion();
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

int cudaDeviceInfoMultiProcessorCount(cv::cuda::DeviceInfo* device)
{
	try
	{
		return device->multiProcessorCount();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cudaDeviceInfoFreeMemInfo(cv::cuda::DeviceInfo* info, size_t* free)
{
	try
	{
		*free = info->freeMemory();
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaDeviceInfoTotalMemInfo(cv::cuda::DeviceInfo* info, size_t* total)
{
	try
	{
		*total = info->totalMemory();
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

bool cudaDeviceInfoSupports(cv::cuda::DeviceInfo* device, cv::cuda::FeatureSet feature)
{
	try
	{
		return device->supports(feature);
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cudaDeviceInfoIsCompatible(cv::cuda::DeviceInfo* device)
{
	try
	{
		return device->isCompatible();
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

void cudaPrintCudaDeviceInfo(int device)
{
	try
	{
		cv::cuda::printCudaDeviceInfo(device);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cudaPrintShortCudaDeviceInfo(int device)
{
	try
	{
		cv::cuda::printShortCudaDeviceInfo(device);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//----------------------------------------------------------------------------
//
//  Gpu Module Info
//
//----------------------------------------------------------------------------

bool targetArchsBuildWith(cv::cuda::FeatureSet featureSet)
{
	try
	{
		return cv::cuda::TargetArchs::builtWith(featureSet);
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool targetArchsHas(int major, int minor)
{
	try
	{
		return cv::cuda::TargetArchs::has(major, minor);
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool targetArchsHasPtx(int major, int minor)
{
	try
	{
		return cv::cuda::TargetArchs::hasPtx(major, minor);
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool targetArchsHasBin(int major, int minor)
{
	try
	{
		return cv::cuda::TargetArchs::hasBin(major, minor);
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool targetArchsHasEqualOrLessPtx(int major, int minor)
{
	try
	{
		return cv::cuda::TargetArchs::hasBin(major, minor);
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool targetArchsHasEqualOrGreater(int major, int minor)
{
	try
	{
		return cv::cuda::TargetArchs::hasEqualOrGreater(major, minor);
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool targetArchsHasEqualOrGreaterPtx(int major, int minor)
{
	try
	{
		return cv::cuda::TargetArchs::hasEqualOrGreaterPtx(major, minor);
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool targetArchsHasEqualOrGreaterBin(int major, int minor)
{
	try
	{
		return cv::cuda::TargetArchs::hasEqualOrGreaterBin(major, minor);
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

//----------------------------------------------------------------------------
//
//  GpuMat
//
//----------------------------------------------------------------------------

cv::cuda::GpuMat* gpuMatCreateDefault()
{
	try
	{
		return new cv::cuda::GpuMat();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::cuda::GpuMat* gpuMatCreateFromData(int rows, int cols, int type, void* data, int step)
{
	try
	{
		return new cv::cuda::GpuMat(rows, cols, type, data, step);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void gpuMatCreate(cv::cuda::GpuMat* m, int rows, int cols, int type)
{
	try
	{
		m->create(rows, cols, type);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}



cv::cuda::GpuMat* gpuMatCreateContinuous(int rows, int cols, int type)
{
	try
	{
		cv::cuda::GpuMat* result = new cv::cuda::GpuMat();
		cv::cuda::createContinuous(rows, cols, type, *result);
		return result;
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

bool gpuMatIsContinuous(cv::cuda::GpuMat* gpuMat)
{
	try
	{
		return gpuMat->isContinuous();
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

cv::cuda::GpuMat* gpuMatGetRegion(cv::cuda::GpuMat* other, cv::Range* rowRange, cv::Range* colRange)
{
	try
	{
		return new cv::cuda::GpuMat(*other, *rowRange, *colRange);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void gpuMatRelease(cv::cuda::GpuMat** mat)
{
	delete* mat;
	*mat = 0;
}

cv::cuda::GpuMat* gpuMatCreateFromInputArray(cv::_InputArray* arr)
{
	try
	{
		return new cv::cuda::GpuMat(*arr);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void gpuMatGetSize(cv::cuda::GpuMat* gpuMat, cv::Size* size)
{
	try
	{
		cv::Size s = gpuMat->size();
		size->width = s.width;
		size->height = s.height;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

bool gpuMatIsEmpty(cv::cuda::GpuMat* gpuMat)
{
	try
	{
		return gpuMat->empty();
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

int gpuMatGetChannels(cv::cuda::GpuMat* gpuMat)
{
	try
	{
		return gpuMat->channels();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

int gpuMatGetType(cv::cuda::GpuMat* gpuMat)
{
	try
	{
		return gpuMat->type();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

int gpuMatGetDepth(cv::cuda::GpuMat* gpuMat)
{
	try
	{
		return gpuMat->depth();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void gpuMatUpload(cv::cuda::GpuMat* gpuMat, cv::_InputArray* arr, cv::cuda::Stream* stream)
{
	try
	{
		if (stream)
			gpuMat->upload(*arr, *stream);
		else
			gpuMat->upload(*arr);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void gpuMatDownload(cv::cuda::GpuMat* gpuMat, cv::_OutputArray* arr, cv::cuda::Stream* stream)
{
	try
	{
		if (stream)
			gpuMat->download(*arr, *stream);
		else
			gpuMat->download(*arr);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


void gpuMatConvertTo(const cv::cuda::GpuMat* src, cv::_OutputArray* dst, int rtype, double alpha, double beta, cv::cuda::Stream* stream)
{
	try
	{
		src->convertTo(*dst, rtype, alpha, beta, stream ? *stream : cv::cuda::Stream::Null());
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void gpuMatCopyTo(const cv::cuda::GpuMat* src, cv::_OutputArray* dst, const cv::_InputArray* mask, cv::cuda::Stream* stream)
{
	try
	{
		if (mask)
			src->copyTo(*dst, *mask, stream ? *stream : cv::cuda::Stream::Null());
		else
			src->copyTo(*dst, stream ? *stream : cv::cuda::Stream::Null());
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void gpuMatSetTo(cv::cuda::GpuMat* mat, const cv::Scalar* s, cv::_InputArray* mask, cv::cuda::Stream* stream)
{
	try
	{
		if (mask)
			mat->setTo(*s, *mask, stream ? *stream : cv::cuda::Stream::Null());
		else
			mat->setTo(*s, stream ? *stream : cv::cuda::Stream::Null());
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void gpuMatReshape(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int cn, int rows)
{
	try
	{
		cv::cuda::GpuMat tmp = src->reshape(cn, rows);
		dst->swap(tmp);
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

cv::cuda::GpuMat* gpuMatGetSubRect(const cv::cuda::GpuMat* arr, cv::Rect* rect)
{
	try
	{
		return new cv::cuda::GpuMat(*arr, *rect);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
