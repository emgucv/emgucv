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


#endif