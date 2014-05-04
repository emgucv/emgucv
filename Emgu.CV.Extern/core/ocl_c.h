//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_OCL_C_H
#define EMGU_OCL_C_H

#include "opencv2/core/types_c.h"
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

CVAPI(void) oclDeviceGetProperty(
   cv::ocl::Device* oclDeviceInfo, 
   int* type, 
   
   const char** version,
   const char** name, 
   const char** vendor, 
   int* vendorId,
   const char** driverVersion, 
   const char** extensions,
   
   int* maxWorkGroupSize,
   int* maxComputeUnits,
   int* globalMemorySize,
   int* localMemorySize,
   int* maxMemAllocSize,
   int* image2DMaxWidth,
   int* image2DMaxHeight,
   int* deviceVersionMajor,
   int* deviceVersionMinor,
   int* doubleFPConfig,
   int* hostUnifiedMemory,
   const char** openCLVersion
   );

#endif