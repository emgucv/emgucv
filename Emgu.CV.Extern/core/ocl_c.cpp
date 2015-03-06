//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "ocl_c.h"


void oclGetPlatformsInfo(std::vector<cv::ocl::PlatformInfo>* oclPlatforms)
{
   cv::ocl::getPlatfomsInfo(*oclPlatforms);
}


bool cveHaveOpenCL()
{
   return cv::ocl::haveOpenCL();
}
bool cveUseOpenCL()
{
   return cv::ocl::useOpenCL();
}
void cveSetUseOpenCL(bool flag)
{
   return cv::ocl::setUseOpenCL(flag);
}
void cveOclFinish()
{
   cv::ocl::finish();
}

//----------------------------------------------------------------------------
//
//  OclPlatformInfo
//
//----------------------------------------------------------------------------

void oclPlatformInfoGetVersion(cv::ocl::PlatformInfo* oclPlatformInfo, cv::String* platformVersion)
{
   *platformVersion = oclPlatformInfo->version();
}

void oclPlatformInfoGetName(cv::ocl::PlatformInfo* oclPlatformInfo, cv::String* platformName)
{
   *platformName = oclPlatformInfo->name();
}

void oclPlatformInfoGetVender(cv::ocl::PlatformInfo* oclPlatformInfo, cv::String* platformVender)
{
   *platformVender = oclPlatformInfo->vendor();
}

int oclPlatformInfoDeviceNumber(cv::ocl::PlatformInfo* platformInfo)
{
   return platformInfo->deviceNumber();
}

void oclPlatformInfoGetDevice(cv::ocl::PlatformInfo* platformInfo, cv::ocl::Device* device, int d)
{
   platformInfo->getDevice(*device, d);
}
void oclPlatformInfoRelease(cv::ocl::PlatformInfo** platformInfo)
{
   delete *platformInfo;
}

//----------------------------------------------------------------------------
//
//  OclDeviceInfo
//
//----------------------------------------------------------------------------
cv::ocl::Device* oclDeviceCreate()
{
   return new cv::ocl::Device();
}

void oclDeviceRelease(cv::ocl::Device** device)
{
   delete *device;
   *device = 0;
}

void oclDeviceGetVersion(cv::ocl::Device* oclDeviceInfo, cv::String* version)
{
   *version = oclDeviceInfo->version();
}
void oclDeviceGetName(cv::ocl::Device* oclDeviceInfo, cv::String* name)
{
   *name = oclDeviceInfo->name();
}
void oclDeviceGetVenderName(cv::ocl::Device* oclDeviceInfo, cv::String* vender)
{
   *vender = oclDeviceInfo->vendorName();
}

void oclDeviceGetDriverVersion(cv::ocl::Device* oclDeviceInfo, cv::String* driverVersion)
{
   *driverVersion = oclDeviceInfo->driverVersion();
}
void oclDeviceGetExtensions(cv::ocl::Device* oclDeviceInfo, cv::String* extensions)
{
   *extensions = oclDeviceInfo->extensions();
}
void oclDeviceGetOpenCLVersion(cv::ocl::Device* oclDeviceInfo, cv::String* extensions)
{
   *extensions = oclDeviceInfo->extensions();
}



