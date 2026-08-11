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
// CvErrorHandler already receives the full error details as parameters
// (status/func/message/file/line), so it records them directly into
// managed [ThreadStatic] fields and CheckError() reads them back with no
// extra native call. That path only fires for errors that went through
// cv::error(), though (i.e. cv::Exception) -- a plain std::exception (or
// anything else) thrown directly by third-party code never reaches
// cv::error(), so the callback never fires for it automatically. The catch
// macros below invoke the same registered callback themselves in that case
// -- via the pointer cveRedirectError() stashes here -- so C# still learns
// about the error either way.
extern cv::ErrorCallback emguErrorCallback;
extern void* emguErrorCallbackUserData;

void emguSetErrorCallback(cv::ErrorCallback callback, void* userdata);

#define CVAPI_CATCH_CV_ERRORS(returnValueOnError) \
	catch (const cv::Exception&) \
	{ \
		/* cv::error() already invoked emguErrorCallback with full details */ \
		/* before throwing this -- just stop the unwind here. */ \
		return returnValueOnError; \
	} \
	catch (const std::exception& e) \
	{ \
		if (emguErrorCallback) \
			emguErrorCallback((int) cv::Error::StsError, "", e.what(), "", 0, emguErrorCallbackUserData); \
		return returnValueOnError; \
	} \
	catch (...) \
	{ \
		if (emguErrorCallback) \
			emguErrorCallback((int) cv::Error::StsError, "", "Unknown exception", "", 0, emguErrorCallbackUserData); \
		return returnValueOnError; \
	}

// Same as CVAPI_CATCH_CV_ERRORS, for functions that return void.
#define CVAPI_CATCH_CV_ERRORS_VOID \
	catch (const cv::Exception&) \
	{ \
	} \
	catch (const std::exception& e) \
	{ \
		if (emguErrorCallback) \
			emguErrorCallback((int) cv::Error::StsError, "", e.what(), "", 0, emguErrorCallbackUserData); \
	} \
	catch (...) \
	{ \
		if (emguErrorCallback) \
			emguErrorCallback((int) cv::Error::StsError, "", "Unknown exception", "", 0, emguErrorCallbackUserData); \
	}

#endif
