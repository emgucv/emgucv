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
// Two independent capture paths exist, because CvInvoke's static
// constructor does not register a redirectError callback on every
// platform -- iOS, MacCatalyst, Blazor/WASM, and Unity WebGL never call
// RedirectError at all:
//
//  1. Fast path: CvErrorHandler already receives the full error details as
//     parameters, so it records them directly into managed [ThreadStatic]
//     fields, and CheckError() reads them back with zero extra P/Invoke
//     calls. cv::error() invokes the registered callback automatically for
//     cv::Exception; the catch macros below invoke it manually for
//     std::exception/unknown exceptions, which never reach cv::error() on
//     their own. This only works where a callback is actually registered.
//
//  2. Fallback path: the catch macros always ALSO record the error into
//     this thread-local EmguPendingError struct, independent of whether
//     any callback is registered, since they have direct access to the
//     caught exception's own details. CheckError() only consults this (via
//     cveHasPendingError()/cveCheckPendingError() -- a few extra P/Invoke
//     calls, only paid when there's actually an error to retrieve) on
//     platforms where it knows no callback was registered for the fast path.
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

// Fast check with no string marshaling -- call this before
// cveCheckPendingError() so the (comparatively expensive) CvString-based
// retrieval only runs when there is actually an error to report.
CVAPI(bool) cveHasPendingError();

// Retrieve and clear the pending error for the current thread, if any.
// Returns true and fills in the out params if there was one.
CVAPI(bool) cveCheckPendingError(int* status, cv::String* funcName, cv::String* errMsg, cv::String* fileName, int* line);

extern cv::ErrorCallback emguErrorCallback;
extern void* emguErrorCallbackUserData;

void emguSetErrorCallback(cv::ErrorCallback callback, void* userdata);

#define CVAPI_CATCH_CV_ERRORS(returnValueOnError) \
	catch (const cv::Exception& e) \
	{ \
		/* cv::error() already invoked emguErrorCallback (if one is registered) */ \
		/* with full details before throwing this -- always also record to the */ \
		/* thread-local fallback, for platforms where no callback is registered. */ \
		emguRecordError((int) e.code, e.func.c_str(), e.err.c_str(), e.file.c_str(), e.line); \
		return returnValueOnError; \
	} \
	catch (const std::exception& e) \
	{ \
		emguRecordError((int) cv::Error::StsError, "", e.what(), "", 0); \
		if (emguErrorCallback) \
			emguErrorCallback((int) cv::Error::StsError, "", e.what(), "", 0, emguErrorCallbackUserData); \
		return returnValueOnError; \
	} \
	catch (...) \
	{ \
		emguRecordError((int) cv::Error::StsError, "", "Unknown exception", "", 0); \
		if (emguErrorCallback) \
			emguErrorCallback((int) cv::Error::StsError, "", "Unknown exception", "", 0, emguErrorCallbackUserData); \
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
		if (emguErrorCallback) \
			emguErrorCallback((int) cv::Error::StsError, "", e.what(), "", 0, emguErrorCallbackUserData); \
	} \
	catch (...) \
	{ \
		emguRecordError((int) cv::Error::StsError, "", "Unknown exception", "", 0); \
		if (emguErrorCallback) \
			emguErrorCallback((int) cv::Error::StsError, "", "Unknown exception", "", 0, emguErrorCallbackUserData); \
	}

#endif
