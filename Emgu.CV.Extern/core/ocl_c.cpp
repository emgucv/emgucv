//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
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
	try
	{
		delete* platformInfo;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
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
const cv::ocl::Device* oclDeviceGetDefault()
{
	return  &cv::ocl::Device::getDefault();
}
void oclDeviceRelease(cv::ocl::Device** device)
{
	try
	{
		delete* device;
		*device = 0;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//----------------------------------------------------------------------------
//
//  OclContext
//
//----------------------------------------------------------------------------
cv::ocl::Context* oclContextCreate()
{
	return new cv::ocl::Context();
}
cv::ocl::Context* oclContextGetDefault()
{
	return &cv::ocl::Context::getDefault();
}
void oclContextRelease(cv::ocl::Context** context)
{
	try
	{
		delete* context;
		*context = 0;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
const cv::ocl::Program* oclContextGetProg(
	cv::ocl::Context* context,
	cv::ocl::ProgramSource* prog,
	cv::String* buildopt,
	cv::String* errmsg)
{
	cv::String tmp;
	errmsg = errmsg ? errmsg : &tmp;
	cv::ocl::Program program = context->getProg(*prog, *buildopt, *errmsg);
	cv::ocl::Program* p = new cv::ocl::Program(program);
	return p;
}

//----------------------------------------------------------------------------
//
//  OclProgram
//
//----------------------------------------------------------------------------
cv::ocl::Program* oclProgramCreate()
{
	return new cv::ocl::Program();
}
void oclProgramRelease(cv::ocl::Program** program)
{
	try
	{
		delete* program;
		*program = 0;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void oclProgramGetBinary(cv::ocl::Program* program, std::vector<char>* binary)
{
	program->getBinary(*binary);
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
	try
	{
		delete* programSource;
		*programSource = 0;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
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
	try
	{
		delete* kernel;
		*kernel = 0;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int oclKernelSetImage2D(cv::ocl::Kernel* kernel, int i, cv::ocl::Image2D* image2D)
{
	return kernel->set(i, *image2D);
}
int oclKernelSetUMat(cv::ocl::Kernel* kernel, int i, cv::UMat* umat)
{
	return kernel->set(i, *umat);
}
int oclKernelSet(cv::ocl::Kernel* kernel, int i, void* value, int size)
{
	return kernel->set(i, value, static_cast<size_t>(size));
}
int oclKernelSetKernelArg(cv::ocl::Kernel* kernel, int i, cv::ocl::KernelArg* kernelArg)
{
	return kernel->set(i, *kernelArg);
}
bool oclKernelRun(cv::ocl::Kernel* kernel, int dims, size_t* globalsize, size_t* localsize, bool sync, cv::ocl::Queue* q)
{
	return kernel->run(dims, globalsize, localsize, sync, q ? *q : cv::ocl::Queue());
}
//----------------------------------------------------------------------------
//
//  OclImage2D
//
//----------------------------------------------------------------------------
cv::ocl::Image2D* oclImage2DFromUMat(cv::UMat* src, bool norm, bool alias)
{
	cv::ocl::Image2D* img2d = new cv::ocl::Image2D(*src, norm, alias);
	return img2d;
}
void oclImage2DRelease(cv::ocl::Image2D** image2D)
{
	try
	{
		delete* image2D;
		*image2D = 0;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//----------------------------------------------------------------------------
//
//  OclKernelArg
//
//----------------------------------------------------------------------------
cv::ocl::KernelArg* oclKernelArgCreate(int flags, cv::UMat* m, int wscale, int iwscale, const void* obj, size_t sz)
{
	return new cv::ocl::KernelArg(flags, m, wscale, iwscale, obj, sz);
}
void oclKernelArgRelease(cv::ocl::KernelArg** k)
{
	try
	{
		delete* k;
		*k = 0;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//----------------------------------------------------------------------------
//
//  OclQueue
//
//----------------------------------------------------------------------------
cv::ocl::Queue* oclQueueCreate()
{
	return new cv::ocl::Queue();
}
void oclQueueFinish(cv::ocl::Queue* queue)
{
	queue->finish();
}
void oclQueueRelease(cv::ocl::Queue** queue)
{
	try
	{
		delete* queue;
		*queue = 0;
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


void oclTypeToString(int type, cv::String* str)
{
	const char* s = cv::ocl::typeToStr(type);
	*str = s;
}