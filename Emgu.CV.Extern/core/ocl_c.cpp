//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
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

void oclPlatformInfoGetProperties(
   cv::ocl::PlatformInfo* oclPlatformInfo,
   const char** platformVersion,
   const char** platformName,
   const char** platformVendor
   )
{
   *platformVersion = oclPlatformInfo->version().c_str();
   *platformName = oclPlatformInfo->name().c_str();
   *platformVendor = oclPlatformInfo->vendor().c_str();
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

void oclDeviceGetProperty(
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
   )
{
   *type = oclDeviceInfo->type();
   
   *version = oclDeviceInfo->version().c_str();
   *name = oclDeviceInfo->name().c_str();
   *vendor = oclDeviceInfo->vendorName().c_str();
   *vendorId = oclDeviceInfo->vendorID();
   *driverVersion = oclDeviceInfo->driverVersion().c_str();
   *extensions = oclDeviceInfo->extensions().c_str();

   *maxWorkGroupSize = static_cast<int>( oclDeviceInfo->maxWorkGroupSize() );
   *maxComputeUnits = static_cast<int>( oclDeviceInfo->maxComputeUnits() );
   *globalMemorySize = static_cast<int>( oclDeviceInfo->globalMemSize() );
   *localMemorySize = static_cast<int> (oclDeviceInfo->localMemSize() );
   *maxMemAllocSize = static_cast<int> (oclDeviceInfo->maxMemAllocSize() );
   *image2DMaxWidth = static_cast<int> (oclDeviceInfo->image2DMaxWidth());
   *image2DMaxHeight = static_cast<int> (oclDeviceInfo->image2DMaxHeight());
   *deviceVersionMajor = oclDeviceInfo->deviceVersionMajor();
   *deviceVersionMinor = oclDeviceInfo->deviceVersionMinor();
   *doubleFPConfig = oclDeviceInfo->doubleFPConfig();
   *hostUnifiedMemory = oclDeviceInfo->hostUnifiedMemory();

   *openCLVersion = oclDeviceInfo->OpenCLVersion().c_str();
}



