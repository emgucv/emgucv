//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_OCL_C_H
#define EMGU_OCL_C_H

//#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "opencv2/core/ocl.hpp"
#include "emgu_c.h"

CVAPI(void) oclGetPlatformsInfo(std::vector<cv::ocl::PlatformInfo>* oclPlatforms);

//CVAPI(void) oclSetDevice(const cv::ocl::Device* oclInfo);

CVAPI(bool) cveHaveOpenCL();
CVAPI(bool) cveUseOpenCL();
CVAPI(void) cveSetUseOpenCL(bool flag);
CVAPI(void) cveOclFinish();

//----------------------------------------------------------------------------
//
//  OclPlatformInfo
//
//----------------------------------------------------------------------------
CVAPI(void) oclPlatformInfoGetProperties(
   cv::ocl::PlatformInfo* oclPlatformInfo,
   const char** platformVersion,
   const char** platformName,
   const char** platformVendor
   );

CVAPI(void) oclPlatformInfoGetVersion(cv::ocl::PlatformInfo* oclPlatformInfo, cv::String* platformVersion);

CVAPI(void) oclPlatformInfoGetName(cv::ocl::PlatformInfo* oclPlatformInfo, cv::String* platformName);

CVAPI(void) oclPlatformInfoGetVender(cv::ocl::PlatformInfo* oclPlatformInfo, cv::String* platformVender);

CVAPI(int) oclPlatformInfoDeviceNumber(cv::ocl::PlatformInfo* platformInfo);

CVAPI(void) oclPlatformInfoGetDevice(cv::ocl::PlatformInfo* platformInfo, cv::ocl::Device* device, int d);

CVAPI(void) oclPlatformInfoRelease(cv::ocl::PlatformInfo** platformInfo);


//----------------------------------------------------------------------------
//
//  OclDevice
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::Device*) oclDeviceCreate();
CVAPI(void) oclDeviceSet(cv::ocl::Device* device, void* p);
CVAPI(const cv::ocl::Device*) oclDeviceGetDefault();
CVAPI(void) oclDeviceRelease(cv::ocl::Device** device);
CVAPI(void*) oclDeviceGetPtr(cv::ocl::Device* device);

//----------------------------------------------------------------------------
//
//  OclProgramSource
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::ProgramSource*) oclProgramSourceCreate(cv::String* source);
CVAPI(void) oclProgramSourceRelease(cv::ocl::ProgramSource** programSource);
CVAPI(const cv::String*) oclProgramSourceGetSource(cv::ocl::ProgramSource* programSource);

//----------------------------------------------------------------------------
//
//  OclKernel
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::Kernel*) oclKernelCreateDefault();
CVAPI(bool) oclKernelCreate(cv::ocl::Kernel* kernel, cv::String* kname, cv::ocl::ProgramSource* source, cv::String* buildOpts, cv::String* errmsg);
CVAPI(void) oclKernelRelease(cv::ocl::Kernel** kernel);
CVAPI(int) oclKernelSetImage2D(cv::ocl::Kernel* kernel, int i, cv::ocl::Image2D* image2D);
CVAPI(int) oclKernelSetUMat(cv::ocl::Kernel* kernel, int i, cv::UMat* umat);
CVAPI(int) oclKernelSet(cv::ocl::Kernel* kernel, int i, void* value, int size);
CVAPI(int) oclKernelSetKernelArg(cv::ocl::Kernel* kernel, int i, cv::ocl::KernelArg* kernelArg);
CVAPI(bool) oclKernelRun(cv::ocl::Kernel* kernel, int dims, size_t* globalsize, size_t* localsize, bool sync, cv::ocl::Queue* q);

//----------------------------------------------------------------------------
//
//  OclImage2D
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::Image2D*) oclImage2DFromUMat(cv::UMat* src, bool norm, bool alias);
CVAPI(void) oclImage2DRelease(cv::ocl::Image2D** image2D);

//----------------------------------------------------------------------------
//
//  OclKernelArg
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::KernelArg*) oclKernelArgCreate(int flags, cv::UMat* m, int wscale, int iwscale, const void* obj, size_t sz);
CVAPI(void) oclKernelArgRelease(cv::ocl::KernelArg** k);

//----------------------------------------------------------------------------
//
//  OclQueue
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::Queue*) oclQueueCreate();
CVAPI(void) oclQueueFinish(cv::ocl::Queue* queue);
CVAPI(void) oclQueueRelease(cv::ocl::Queue** queue);

CVAPI(void) oclTypeToString(int type, cv::String* str);
#endif