//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_ERROR_H
#define EMGU_ERROR_H

#include "opencv2/core.hpp"
#include "cvapi_compat.h"

// cv::error() (see opencv/modules/core/src/system.cpp) always throws a
// cv::Exception after invoking the callback registered via
// cv::redirectError -- it ignores that callback's return value entirely.
// The callback (CvInvoke.CvErrorHandler on the C# side) must never throw a
// managed exception, since a .NET exception escaping a reverse P/Invoke
// call is treated as fatal by the CLR. So every native entry point that can
// reach a CV_Error(...)/throw call must catch the resulting C++ exception
// locally -- via the CVAPI_CATCH_CV_ERRORS macro below -- so it never
// unwinds across the P/Invoke return boundary into managed code.
//
// The caught exception's details are recorded here, in thread-local
// storage, so the managed caller can retrieve and rethrow it (as a
// catchable CvException, entirely in managed code) after the native call
// returns normally. See CvInvoke.CheckError() on the C# side.
struct EmguPendingError
{
	bool hasError;
	int status;
	char funcName[256];
	char errMsg[1024];
	char fileName[256];
	int line;
};

#if defined(_MSC_VER)
#define EMGU_THREAD_LOCAL __declspec(thread)
#else
#define EMGU_THREAD_LOCAL __thread
#endif

extern EMGU_THREAD_LOCAL EmguPendingError emguPendingError;

void emguRecordError(int status, const char* funcName, const char* errMsg, const char* fileName, int line);

// Retrieve and clear the pending error for the current thread, if any.
// Returns true and fills in the out params if there was one.
CVAPI(bool) cveCheckPendingError(int* status, cv::String* funcName, cv::String* errMsg, cv::String* fileName, int* line);

#define CVAPI_CATCH_CV_ERRORS(returnValueOnError) \
	catch (const cv::Exception& e) \
	{ \
		emguRecordError((int) e.code, e.func.c_str(), e.err.c_str(), e.file.c_str(), e.line); \
		return returnValueOnError; \
	} \
	catch (const std::exception& e) \
	{ \
		emguRecordError((int) cv::Error::StsError, "", e.what(), "", 0); \
		return returnValueOnError; \
	} \
	catch (...) \
	{ \
		emguRecordError((int) cv::Error::StsError, "", "Unknown exception", "", 0); \
		return returnValueOnError; \
	}

// Same as CVAPI_CATCH_CV_ERRORS, for functions that return void.
#define CVAPI_CATCH_CV_ERRORS_VOID \
	catch (const cv::Exception& e) \
	{ \
		emguRecordError((int) e.code, e.func.c_str(), e.err.c_str(), e.file.c_str(), e.line); \
	} \
	catch (const std::exception& e) \
	{ \
		emguRecordError((int) cv::Error::StsError, "", e.what(), "", 0); \
	} \
	catch (...) \
	{ \
		emguRecordError((int) cv::Error::StsError, "", "Unknown exception", "", 0); \
	}

#endif
