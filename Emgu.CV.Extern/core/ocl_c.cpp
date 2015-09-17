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
void oclDeviceSet(cv::ocl::Device* device, void* p)
{
   device->set(p);
}
const cv::ocl::Device* oclDeviceGetDefault()
{
   return  &cv::ocl::Device::getDefault();
}
void oclDeviceRelease(cv::ocl::Device** device)
{
   delete *device;
   *device = 0;
}
void* oclDeviceGetPtr(cv::ocl::Device* device)
{
   return device->ptr();
}



//----------------------------------------------------------------------------
//
//  OclProgramSource
//
//----------------------------------------------------------------------------
cv::ocl::ProgramSource* oclProgramSourceCreate(cv::String* source)
{
   return new cv::ocl::ProgramSource(*source);
}
void oclProgramSourceRelease(cv::ocl::ProgramSource** programSource)
{
   delete *programSource;
   *programSource = 0;
}
const cv::String* oclProgramSourceGetSource(cv::ocl::ProgramSource* programSource)
{
   return &programSource->source();
}

//----------------------------------------------------------------------------
//
//  OclKernel
//
//----------------------------------------------------------------------------
cv::ocl::Kernel* oclKernelCreateDefault()
{
   return new cv::ocl::Kernel();
}
bool oclKernelCreate(cv::ocl::Kernel* kernel, cv::String* kname, cv::ocl::ProgramSource* source, cv::String* buildOpts, cv::String* errmsg)
{
   return kernel->create(kname->c_str(), *source, *buildOpts, errmsg);
}
void oclKernelRelease(cv::ocl::Kernel** kernel)
{
   delete *kernel;
   *kernel = 0;
}
