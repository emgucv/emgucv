//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cuda_c.h"

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

cv::cuda::DeviceInfo* cudaDeviceInfoCreate(int* deviceId)
{
   if (deviceId < 0)
      *deviceId = cv::cuda::getDevice();

   return new cv::cuda::DeviceInfo(*deviceId);
}

void cudaDeviceInfoRelease(cv::cuda::DeviceInfo** di)
{
   delete *di;
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

cv::cuda::GpuMat* gpuMatCreateDefault() { return new cv::cuda::GpuMat() ; }

cv::cuda::GpuMat* gpuMatCreate(int rows, int cols, int type)
{
   return new cv::cuda::GpuMat(rows, cols, type);
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

cv::cuda::GpuMat* gpuMatGetRegion(cv::cuda::GpuMat* other, CvSlice rowRange, CvSlice colRange)
{
   return new cv::cuda::GpuMat(*other, cv::Range(rowRange), cv::Range(colRange));
}

void gpuMatRelease(cv::cuda::GpuMat** mat)
{
   delete *mat;
   *mat = 0;
}

cv::cuda::GpuMat* gpuMatCreateFromArr(CvArr* arr)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   return new cv::cuda::GpuMat(mat);
}

emgu::size gpuMatGetSize(cv::cuda::GpuMat* gpuMat)
{
   cv::Size s = gpuMat->size();
   emgu::size result;
   result.width = s.width;
   result.height = s.height;
   return result;
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

void gpuMatUpload(cv::cuda::GpuMat* gpuMat, CvArr* arr)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   gpuMat->upload(mat);
}

void gpuMatDownload(cv::cuda::GpuMat* gpuMat, CvArr* arr)
{
   cv::Mat mat = cv::cvarrToMat(arr);
   gpuMat->download(mat);
}


void gpuMatConvertTo(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, double alpha, double beta, cv::cuda::Stream* stream)
{
   src->convertTo(*dst, dst->type(), alpha, beta, stream ? *stream : cv::cuda::Stream::Null());
}

void gpuMatCopy(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream)
{
   if (mask)
      src->copyTo(*dst, *mask, stream ? *stream : cv::cuda::Stream::Null());
   else
      src->copyTo(*dst, stream ? *stream : cv::cuda::Stream::Null());
}

void gpuMatSetTo(cv::cuda::GpuMat* mat, const CvScalar s, const cv::cuda::GpuMat* mask, cv::cuda::Stream* stream)
{
      (*mat).setTo(s, mask ? *mask : cv::cuda::GpuMat(), stream ? *stream : cv::cuda::Stream::Null());
}

CVAPI(void) gpuMatReshape(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int cn, int rows)
{
   cv::cuda::GpuMat tmp = src->reshape(cn, rows);
   dst->swap(tmp);
}

cv::cuda::GpuMat* gpuMatGetSubRect(const cv::cuda::GpuMat* arr, CvRect rect) 
{ 
   return new cv::cuda::GpuMat(*arr, rect);
}



