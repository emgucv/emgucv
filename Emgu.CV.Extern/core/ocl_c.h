//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
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

CVAPI(void) oclSetDevice(const cv::ocl::Device* oclInfo);

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

CVAPI(void) oclDeviceRelease(cv::ocl::Device** device);

CVAPI(int) oclDeviceGetType(cv::ocl::Device* oclDeviceInfo);
CVAPI(void) oclDeviceGetVersion(cv::ocl::Device* oclDeviceInfo, cv::String* version);
CVAPI(void) oclDeviceGetName(cv::ocl::Device* oclDeviceInfo, cv::String* name);
CVAPI(void) oclDeviceGetVenderName(cv::ocl::Device* oclDeviceInfo, cv::String* vender);
CVAPI(int) oclDeviceGetVenderId(cv::ocl::Device* oclDeviceInfo, int venderId);
CVAPI(void) oclDeviceGetDriverVersion(cv::ocl::Device* oclDeviceInfo, cv::String* driverVersion);
CVAPI(void) oclDeviceGetExtensions(cv::ocl::Device* oclDeviceInfo, cv::String* extensions);
CVAPI(int) oclDeviceGetMaxWorkGroupSize(cv::ocl::Device* oclDeviceInfo);
CVAPI(int) oclDeviceGetMaxComputeUnits(cv::ocl::Device* oclDeviceInfo);
CVAPI(int) oclDeviceGetGlobalMemorySize(cv::ocl::Device* oclDeviceInfo);
CVAPI(int) oclDeviceGetLocalMemorySize(cv::ocl::Device* oclDeviceInfo);
CVAPI(int) oclDeviceGetMaxMemAllocSize(cv::ocl::Device* oclDeviceInfo);
CVAPI(int) oclDeviceGetImage2DMaxWidth(cv::ocl::Device* oclDeviceInfo);
CVAPI(int) oclDeviceGetImage2DMaxHeight(cv::ocl::Device* oclDeviceInfo);
CVAPI(int) oclDeviceGetDeviceVersionMajor(cv::ocl::Device* oclDeviceInfo);
CVAPI(int) oclDeviceGetDeviceVersionMinor(cv::ocl::Device* oclDeviceInfo);
CVAPI(int) oclDeviceGetDoubleFPConfig(cv::ocl::Device* oclDeviceInfo);
CVAPI(int) oclDeviceGetHostUnifiedMemory(cv::ocl::Device* oclDeviceInfo);
CVAPI(void) oclDeviceGetOpenCLVersion(cv::ocl::Device* oclDeviceInfo, cv::String* extensions);

#endif